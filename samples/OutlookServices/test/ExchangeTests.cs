using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Office365.OutlookServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAuth;
using DayOfWeek = Microsoft.Office365.OutlookServices.DayOfWeek;

namespace ExchangeManagedOMTests
{
    [TestClass]
    public class ExchangeTests
    {
        private OutlookServicesClient client;
        

        [TestInitialize]
        public void Init()
        {
            var settings = Settings.ExchangePrd;

            Its.Configuration.Settings.SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config");

            var authSettings = Its.Configuration.Settings.Get<AuthSettings>();

            if(String.IsNullOrWhiteSpace(authSettings.ClientId))
                throw new Exception(String.Format("Create a file called AuthSettings.json in {0} and provide your O365 credentials via this JSON object:\r\n {1}", Its.Configuration.Settings.SettingsDirectory, AuthSettings.GetJsonTemplate()));
            var xauth =
                new XAuth.Auth(authSettings);

            client = new OutlookServicesClient(settings.Environment.EndpointUri, () => xauth.GetAccessToken(settings.Environment.ResourceId));
        }

        [TestMethod]
        public async Task GetInboxMessages()
        {
            var messageRequest = await (from i in client.Me.Messages
                                        orderby i.DateTimeSent descending
                                        select i).Take(10).ExecuteAsync();
            var messages = messageRequest.CurrentPage.ToArray();

            Assert.IsTrue(messages.Length <= 10, "We have more than 10 messages");
        }

        [TestMethod]
        public async Task GetInboxMessagesBatch()
        {
            var messagesRequest = (from i in client.Me.Messages
                                   orderby i.DateTimeSent descending
                                   select i).Take(10);
            var messagesBatchResponse = await client.Context.ExecuteBatchAsync(messagesRequest);
            Assert.IsTrue(messagesBatchResponse.Any(), "We have no response");
            var messagesResult = messagesBatchResponse[0].SuccessResult;
            Assert.IsNotNull(messagesResult, "No success result in batch response");
            var messages = messagesResult.CurrentPage.ToArray();
            Assert.IsTrue(messages.Length <= 10, "We have more than 10 messages");
        }

        [TestMethod]
        public async Task GetAttachments()
        {
            var messagesRequest = await (from i in client.Me.Messages select i).Take(1).ExecuteAsync();
            var message = messagesRequest.CurrentPage.First();

            Assert.IsTrue(message.Attachments.CurrentPage.Count == 0, "We have attachments without an Expand?");
        }

        [TestMethod]
        public async Task GetAttachments2Queries()
        {
            var messageRequest = await client.Me.Messages.Where(i => i.HasAttachments.Value).Take(1).Expand(i => i.Attachments).ExecuteAsync();
            var message = messageRequest.CurrentPage.First();

            var attachments = await ((IMessageFetcher)message).Attachments.ExecuteAsync();
            var allAttachments = attachments.CurrentPage.ToArray();
            Assert.IsTrue(allAttachments.Length != 0, "We have no attachments");
        }

        /*
         * // service bug
        [TestMethod]
        public async Task GetAttachmentsExpand()
        {
            var contactRequest = await (from i in client.Me.Contacts select i).Take(1).Expand(i => i.Attachments).ExecuteAsync();
            var contact = contactRequest.CurrentPage.First();

            Assert.IsTrue(contact.Attachments != null, "We do not have attachments even with an Expand?");
        }
        */

        [TestMethod]
        public async Task GetMessageAttachmentsExpand()
        {
            var messages = await client.Me.Messages.Expand(i => i.Attachments).ExecuteAsync();
                        
            Assert.IsTrue(messages.CurrentPage.Count > 0, "We have no attachments");
        }


        /*
        [TestMethod]
        public async Task GetByMeetingType()
        {
            Task.Run(async () =>
            {
                var messageRequest = await (from i in client.Me.Inbox.Messages
                                            where i.MeetingMessageType == MeetingMessageType.MeetingRequest
                                            select i).Take(10).ExecuteAsync();
                var messages = messageRequest.CurrentPage.ToArray();


                Assert.IsTrue(messages.Length <= 10, "We have more than 10 messages");
            }).Wait();
        }
        */

        [TestMethod]
        public async Task GetEvents()
        {

            var eventQuery = from k in client.Me.Events
                             where k.End > DateTimeOffset.UtcNow.AddDays(-30)
                             select new
                             {
                                 End = k.End.Value.LocalDateTime,
                                 Start = k.Start.Value.LocalDateTime,
                                 Subject = k.Subject,
                                 Location = k.Location,
                                 Body = k.Body

                             };
            var events = await eventQuery.ExecuteAsync();

            var allEvents = events.CurrentPage.ToArray();

            Assert.IsTrue(allEvents.Length <= 10, "We have more than 10 messages");

        }

        [TestMethod]
        public async Task GetCalendarView()
        {
            var events = (await client.Me.Calendar.GetCalendarView(DateTimeOffset.Now, DateTimeOffset.Now).ExecuteAsync());
        }

        [TestMethod]
        public async Task GetInstances()
        {
            var @event = new Event
            {
                Subject = "subject",
                Body = new ItemBody
                {
                    Content = "bodyContent"
                },
                Recurrence = new PatternedRecurrence
                {
                    Pattern =
                        new RecurrencePattern
                        {
                            Type = RecurrencePatternType.Weekly,
                            Interval = 1,
                            Month = 0,
                            Index = WeekIndex.First,
                            FirstDayOfWeek = DayOfWeek.Sunday,
                            DayOfMonth = 0,
                            DaysOfWeek = new[] {DayOfWeek.Tuesday}
                        },
                    Range = new RecurrenceRange { Type = RecurrenceRangeType.NoEnd, StartDate = DateTimeOffset.Now, EndDate = DateTimeOffset.MinValue, NumberOfOccurrences = 0 }
                },
                Attendees =
                    new[]
                    {
                        new Attendee
                        {
                            EmailAddress = new EmailAddress {Address = "piotrp@contoso.com", Name = "piotrp"},
                            Type = AttendeeType.Required
                        }
                    },
            };
            
            await client.Me.Events.AddEventAsync(@event);

            var instances = await client.Me.Calendar.Events.GetById(@event.Id).GetInstances(DateTimeOffset.Now, DateTimeOffset.Now).ExecuteAsync();
        }

        [TestMethod]
        public async Task GetEventsRaw()
        {

            var eventQuery = from k in client.Me.Events
                             where k.End > DateTimeOffset.UtcNow.AddDays(-1000)
                             select k;

            var events = await eventQuery.ExecuteAsync();

            var allEvents = events.CurrentPage.ToArray();
        }

        [TestMethod]
        public async Task GetMessagesWithAttachments()
        {
            var messageRequest = (from i in client.Me.Messages
                                  where i.HasAttachments.Value.Equals(true)
                                  orderby i.DateTimeSent descending
                                  select i).Take(10);

            var messages = await messageRequest.ExecuteAsync();

            var allMessages = messages.CurrentPage.ToArray();

            Assert.IsTrue(allMessages.Length <= 10, "We have more than 10 messages");
        }

        [TestMethod]
        public async Task SendMail()
        {
            var message = new Message
            {
                Subject = "Automated email from client library",
                Body = new ItemBody
                {
                    ContentType = BodyType.HTML,
                    Content = "This is the body"
                },
                ToRecipients = {
                         new Recipient
                         {
                            EmailAddress = new EmailAddress
                            {
                                Address = "vslstest@vscptls2.onmicrosoft.com",
                                Name = "Test User"
                            }
                         }
                    }
            };

            Assert.IsNull(message.Id, "Message has an Id before creation");

            await client.Me.SendMailAsync(message, true);
            
            Assert.IsNotNull(message.Id, "Message does not have after creation");
        }

        [TestMethod]
        public void UpdateMessage()
        {
            Message message = new Message();
            message.Subject = "Test Email " + DateTime.Now.ToString();
            client.Me.Messages.AddMessageAsync(message).Wait();
            var messages = client.Me.Folders["Drafts"].Messages.ExecuteAsync();
            messages.Wait();

            Message updateMessage = message;
            updateMessage.Id = messages.Result.CurrentPage[0].Id;
            updateMessage.BccRecipients = new List<Recipient>();
            updateMessage.BccRecipients.Add(new Recipient() { EmailAddress = new EmailAddress() { Name = "BCCUser1", Address = "bcc1@dsfsfsdf.com" } });
            updateMessage.BccRecipients.Add(new Recipient() { EmailAddress = new EmailAddress() { Name = "BCCUser2", Address = "bcc2@dsfsfsdf.com" } });
            updateMessage.Body = new ItemBody() { Content = "Text content", ContentType = BodyType.HTML };
            updateMessage.BodyPreview = "Should be ignored.";
            updateMessage.Categories.Add("blue");
            updateMessage.Categories.Add("green");
            updateMessage.CcRecipients = new List<Recipient>();
            updateMessage.CcRecipients.Add(new Recipient() { EmailAddress = new EmailAddress() { Name = "CCUser1", Address = "cc1@dsfsfsdf.com" } });
            updateMessage.CcRecipients.Add(new Recipient() { EmailAddress = new EmailAddress() { Name = "CCUser2", Address = "cc2@dsfsfsdf.com" } });
            updateMessage.ChangeKey = "Should be ignored";
            updateMessage.ConversationId = "ConversationId1";
            updateMessage.DateTimeCreated = new DateTimeOffset(DateTime.Now);
            updateMessage.DateTimeLastModified = new DateTimeOffset(DateTime.Now);
            updateMessage.DateTimeReceived = new DateTimeOffset(DateTime.Now);
            updateMessage.DateTimeSent = new DateTimeOffset(DateTime.Now);
            updateMessage.From = new Recipient() { EmailAddress = new EmailAddress() { Name = "joe", Address = "joe@bob.com"} };
            updateMessage.HasAttachments = true; //should be ignored.
            updateMessage.Importance = Importance.High;
            updateMessage.IsDeliveryReceiptRequested = true;
            updateMessage.IsDraft = false; // should be ignored.
            updateMessage.IsRead = false;
            updateMessage.IsReadReceiptRequested = true;
            updateMessage.ParentFolderId = "Inbox"; //should be ignored.
            updateMessage.Sender = new Recipient() { EmailAddress = new EmailAddress() { Name = "joe", Address = "joe@bob.com" } };
            updateMessage.Subject = "Updated Test Subject " + DateTime.Now.ToString();
            updateMessage.ToRecipients = new List<Recipient>();
            updateMessage.ToRecipients.Add(new Recipient() { EmailAddress = new EmailAddress() { Name = "TOUser1", Address = "to1@dsfsfsdf.com" } });
            updateMessage.ToRecipients.Add(new Recipient() { EmailAddress = new EmailAddress() { Name = "TOUser2", Address = "to2@dsfsfsdf.com" } });
            updateMessage.UniqueBody = new ItemBody() { Content = "Unique Body content.", ContentType = BodyType.HTML };
            updateMessage.UpdateAsync().Wait();
        }

        [TestMethod]
        public void SendMessageWithAttachments()
        {
            Message message = new Message();
            message.Subject = "Test Email " + DateTime.Now.ToString();
            message.Categories.Add("blue");
            message.Categories.Add("green");
            message.CcRecipients = new List<Recipient>();
            message.CcRecipients.Add(new Recipient() { EmailAddress = new EmailAddress() { Name = "CCUser1", Address = "cc1@dsfsfsdf.com" } });
            message.CcRecipients.Add(new Recipient() { EmailAddress = new EmailAddress() { Name = "CCUser2", Address = "cc2@dsfsfsdf.com" } });

            FileAttachment attachment = new FileAttachment();
            attachment.ContentBytes = Convert.FromBase64String("VXNlciBOYW1lLEZpcnN0IE5hbWUsTGFzdCBOYW1lLERpc3BsYXkgTmFtZSxKb2IgVGl0bGUsRGVwYXJ0bWVudCxPZmZpY2UgTnVtYmVyLE9mZmljZSBQaG9uZSxNb2JpbGUgUGhvbmUsRmF4LEFkZHJlc3MsQ2l0eSxTdGF0ZSBvciBQcm92aW5jZSxaSVAgb3IgUG9zdGFsIENvZGUsQ291bnRyeSBvciBSZWdpb24NCmF0dWxnb0BleG9sa2Rldi5jY3NjdHAubmV0LEF0dWwsR295YWwsQXR1bCBHb3lhbCwsLCwsLCwsLCwsDQpqZWZ3aWdodEBleG9sa2Rldi5jY3NjdHAubmV0LEplZmYsV2lnaHQsSmVmZiBXaWdodCwsLCwsLCwsLCwsDQpzYWJyYWhhQGV4b2xrZGV2LmNjc2N0cC5uZXQsU2FiaXRoYSxBYnJhaGFtLFNhYml0aGEgQWJyYWhhbSwsLCwsLCwsLCwsDQpnYXJlZ2luQGV4b2xrZGV2LmNjc2N0cC5uZXQsR2FyZWdpbixBdmFneWFuLEdhcmVnaW4gQXZhZ3lhbiwsLCwsLCwsLCwsDQpkYW5hYkBleG9sa2Rldi5jY3NjdHAubmV0LERhbmEsQmlya2J5LERhbmEgQmlya2J5LCwsLCwsLCwsLCwNCmRhdnN0ZXJAZXhvbGtkZXYuY2NzY3RwLm5ldCxEYXZpZCxTdGVybGluZyxEYXZpZCBTdGVybGluZywsLCwsLCwsLCwsDQpmZXJuYW5ndkBleG9sa2Rldi5jY3NjdHAubmV0LEZlcm5hbmRvLEdhcmNpYSxGZXJuYW5kbyBHYXJjaWEsLCwsLCwsLCwsLA0KYWxpbmVAZXhvbGtkZXYuY2NzY3RwLm5ldCxBbGluZSxOYWthc2hpYW4sQWxpbmUgTmFrYXNoaWFuLCwsLCwsLCwsLCwNCnB0b3VzaWdAZXhvbGtkZXYuY2NzY3RwLm5ldCxQYXRyaWNrLFRvdXNpZ25hbnQsUGF0cmljayBUb3VzaWduYW50LCwsLCwsLCwsLCwNCm1lbmNpbmFAZXhvbGtkZXYuY2NzY3RwLm5ldCxNYXVyaWNpbyxFbmNpbmEsTWF1cmljaW8gRW5jaW5hLCwsLCwsLCwsLCwNCnJvaGl0YWdAZXhvbGtkZXYuY2NzY3RwLm5ldCxSb2hpdCxOYWdhcm1hbCxSb2hpdCBOYWdhcm1hbCwsLCwsLCwsLCwsDQp2ZW5rYXRheUBleG9sa2Rldi5jY3NjdHAubmV0LFZlbmthdCxBeXlhZGV2YXJhLFZlbmthdCBBeXlhZGV2YXJhLCwsLCwsLCwsLCwNCmRlc2luZ2hAZXhvbGtkZXYuY2NzY3RwLm5ldCxEZWVwYWssU2luZ2gsRGVlcGFrIFNpbmdoLCwsLCwsLCwsLCwNCnNoYXduYnJAZXhvbGtkZXYuY2NzY3RwLm5ldCxTaGF3bixCcmFjZXdlbGwsU2hhd24gQnJhY2V3ZWxsLCwsLCwsLCwsLCwNCmphc29uaGVuQGV4b2xrZGV2LmNjc2N0cC5uZXQsSmFzb24sSGVuZGVyc29uLEphc29uIEhlbmRlcnNvbiwsLCwsLCwsLCwsDQpzaHJlZWRwQGV4b2xrZGV2LmNjc2N0cC5uZXQsU2hyZWVkZXZpLFBhZG1hc2luaSxTaHJlZWRldmkgUGFkbWFzaW5pLCwsLCwsLCwsLCwNCmFuc2NvZ2dpQGV4b2xrZGV2LmNjc2N0cC5uZXQsRHJldyxTY29nZ2lucyxEcmV3IFNjb2dnaW5zLCwsLCwsLCwsLCwNCnBpb3RycEBleG9sa2Rldi5jY3NjdHAubmV0LFBldGVyLFB1c3praWV3aWN6LFBldGVyIFB1c3praWV3aWN6LCwsLCwsLCwsLCwNCmpvc2hnYXZAZXhvbGtkZXYuY2NzY3RwLm5ldCxKb3NoLEdhdmFudCxKb3NoIEdhdmFudCwsLCwsLCwsLCwsDQp2YXJ1bmdAZXhvbGtkZXYuY2NzY3RwLm5ldCxWYXJ1bixHdXB0YSxWYXJ1biBHdXB0YSwsLCwsLCwsLCwsDQo=");
            attachment.Name = "ExolkdevUsers.csv";
            attachment.ContentType = "application/vnd.ms-excel";

            message.Attachments.Add(attachment);

            client.Me.Messages.AddMessageAsync(message).Wait();
            var messages = client.Me.Folders["Drafts"].Messages.ExecuteAsync();
            messages.Wait();

            var attachments = client.Me.Messages.GetById(messages.Result.CurrentPage[0].Id).Attachments.ExecuteAsync();
            attachments.Wait();
        }
    }
}
