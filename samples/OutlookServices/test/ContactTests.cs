using System;
using Microsoft.Office365.OutlookServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAuth;

namespace ExchangeManagedOMTests
{
    [TestClass]
    public class ContactTests
    {
        private OutlookServicesClient _client;
        private Contact _tempContact;

        [TestInitialize]
        public void Init()
        {
            var settings = Settings.ExchangePrd;
            var xauth =
                new Auth(settings.Auth);

            _client = new OutlookServicesClient(settings.Environment.EndpointUri, () => xauth.GetAccessToken(settings.Environment.ResourceId));

            _tempContact = new Contact
            {
                DisplayName = "Test User" + new Random().NextDouble(),
                GivenName = "GivenName",
                Surname = "Surname"
            };

            _client.Me.Contacts.AddContactAsync(_tempContact).Wait();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_tempContact != null)
            {
                _tempContact.DeleteAsync().Wait();
            }
        }

        //[TestMethod]
        //public async Task AddAttachment()
        //{
        //    var attachments = await ((IContactFetcher)tempContact).Attachments.ExecuteAsync();
        //    var allAttachments = attachments.CurrentPage.ToArray();

        //    Assert.IsTrue(allAttachments.Length == 0, "We have attachments already?");

        //    var contents = new byte[Resources.ProfilePhoto.Length];
        //    Resources.ProfilePhoto.Read(contents, 0, contents.Length);

        //    var newAttachment = new FileAttachment
        //    {
        //        IsContactPhoto = true,
        //        ContentBytes = contents,
        //        Name = "MyPhoto.jpg"
        //        //  ContentType = "image/jpeg"
        //    };

        //    await ((IContactFetcher)tempContact).Attachments.AddAttachmentAsync(newAttachment);
        //    await tempContact.UpdateAsync();

        //    attachments = await ((IContactFetcher)tempContact).Attachments.ExecuteAsync();
        //    allAttachments = attachments.CurrentPage.ToArray();

        //    Assert.IsTrue(allAttachments.Length == 1, "Wasn't an attachment created?");
        //}
    }
}
