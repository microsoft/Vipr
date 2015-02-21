namespace Microsoft.OutlookServices

{

    using global::Microsoft.OData.Client;

    using global::Microsoft.OData.Edm;

    using System;

    using System.Collections.Generic;

    using System.ComponentModel;

    using System.Linq;

    using System.Reflection;

    using System.Threading.Tasks;

    public enum DayOfWeek : int

    {

        Sunday
         = 0
        ,
        Monday
         = 1
        ,
        Tuesday
         = 2
        ,
        Wednesday
         = 3
        ,
        Thursday
         = 4
        ,
        Friday
         = 5
        ,
        Saturday
         = 6
        ,
    }

    

    public enum BodyType : int

    {

        Text
         = 0
        ,
        HTML
         = 1
        ,
    }

    

    public enum Importance : int

    {

        Low
         = 0
        ,
        Normal
         = 1
        ,
        High
         = 2
        ,
    }

    

    public enum AttendeeType : int

    {

        Required
         = 0
        ,
        Optional
         = 1
        ,
        Resource
         = 2
        ,
    }

    

    public enum ResponseType : int

    {

        None
         = 0
        ,
        Organizer
         = 1
        ,
        TentativelyAccepted
         = 2
        ,
        Accepted
         = 3
        ,
        Declined
         = 4
        ,
        NotResponded
         = 5
        ,
    }

    

    public enum EventType : int

    {

        SingleInstance
         = 0
        ,
        Occurrence
         = 1
        ,
        Exception
         = 2
        ,
        SeriesMaster
         = 3
        ,
    }

    

    public enum FreeBusyStatus : int

    {

        Free
         = 0
        ,
        Tentative
         = 1
        ,
        Busy
         = 2
        ,
        Oof
         = 3
        ,
        WorkingElsewhere
         = 4
        ,
        Unknown
         = -1
        ,
    }

    

    public enum MeetingMessageType : int

    {

        None
         = 0
        ,
        MeetingRequest
         = 1
        ,
        MeetingCancelled
         = 2
        ,
        MeetingAccepted
         = 3
        ,
        MeetingTenativelyAccepted
         = 4
        ,
        MeetingDeclined
         = 5
        ,
    }

    

    public enum RecurrencePatternType : int

    {

        Daily
         = 0
        ,
        Weekly
         = 1
        ,
        AbsoluteMonthly
         = 2
        ,
        RelativeMonthly
         = 3
        ,
        AbsoluteYearly
         = 4
        ,
        RelativeYearly
         = 5
        ,
    }

    

    public enum RecurrenceRangeType : int

    {

        EndDate
         = 0
        ,
        NoEnd
         = 1
        ,
        Numbered
         = 2
        ,
    }

    

    public enum WeekIndex : int

    {

        First
         = 0
        ,
        Second
         = 1
        ,
        Third
         = 2
        ,
        Fourth
         = 3
        ,
        Last
         = 4
        ,
    }

    

    public partial class EmailAddress:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private System.String _Name;

        private System.String _Address;

        public System.String Name

        {

            get

            {

                return _Name;

            }

            set

            {

                if (value != _Name)

                {

                    _Name = value;

                    OnPropertyChanged("Name");

                }

            }

        }

        public System.String Address

        {

            get

            {

                return _Address;

            }

            set

            {

                if (value != _Address)

                {

                    _Address = value;

                    OnPropertyChanged("Address");

                }

            }

        }

        public EmailAddress(): base()

        {

        }

    }

    public partial class Recipient:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.OutlookServices.EmailAddress _EmailAddress;

        public Microsoft.OutlookServices.EmailAddress EmailAddress

        {

            get

            {

                return _EmailAddress;

            }

            set

            {

                if (value != _EmailAddress)

                {

                    _EmailAddress = value;

                    OnPropertyChanged("EmailAddress");

                }

            }

        }

        public Recipient(): base()

        {

        }

    }

    public partial class Attendee:Microsoft.OutlookServices.Recipient

    {

        private Microsoft.OutlookServices.ResponseStatus _Status;

        private Microsoft.OutlookServices.AttendeeType _Type;

        public Microsoft.OutlookServices.ResponseStatus Status

        {

            get

            {

                return _Status;

            }

            set

            {

                if (value != _Status)

                {

                    _Status = value;

                    OnPropertyChanged("Status");

                }

            }

        }

        public Microsoft.OutlookServices.AttendeeType Type

        {

            get

            {

                return _Type;

            }

            set

            {

                if (value != _Type)

                {

                    _Type = value;

                    OnPropertyChanged("Type");

                }

            }

        }

        public Attendee()

        {

        }

    }

    public partial class ItemBody:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.OutlookServices.BodyType _ContentType;

        private System.String _Content;

        public Microsoft.OutlookServices.BodyType ContentType

        {

            get

            {

                return _ContentType;

            }

            set

            {

                if (value != _ContentType)

                {

                    _ContentType = value;

                    OnPropertyChanged("ContentType");

                }

            }

        }

        public System.String Content

        {

            get

            {

                return _Content;

            }

            set

            {

                if (value != _Content)

                {

                    _Content = value;

                    OnPropertyChanged("Content");

                }

            }

        }

        public ItemBody(): base()

        {

        }

    }

    public partial class Location:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private System.String _DisplayName;

        public System.String DisplayName

        {

            get

            {

                return _DisplayName;

            }

            set

            {

                if (value != _DisplayName)

                {

                    _DisplayName = value;

                    OnPropertyChanged("DisplayName");

                }

            }

        }

        public Location(): base()

        {

        }

    }

    public partial class ResponseStatus:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.OutlookServices.ResponseType _Response;

        private System.Nullable<System.DateTimeOffset> _Time;

        public Microsoft.OutlookServices.ResponseType Response

        {

            get

            {

                return _Response;

            }

            set

            {

                if (value != _Response)

                {

                    _Response = value;

                    OnPropertyChanged("Response");

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> Time

        {

            get

            {

                return _Time;

            }

            set

            {

                if (value != _Time)

                {

                    _Time = value;

                    OnPropertyChanged("Time");

                }

            }

        }

        public ResponseStatus(): base()

        {

        }

    }

    public partial class PhysicalAddress:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private System.String _Street;

        private System.String _City;

        private System.String _State;

        private System.String _CountryOrRegion;

        private System.String _PostalCode;

        public System.String Street

        {

            get

            {

                return _Street;

            }

            set

            {

                if (value != _Street)

                {

                    _Street = value;

                    OnPropertyChanged("Street");

                }

            }

        }

        public System.String City

        {

            get

            {

                return _City;

            }

            set

            {

                if (value != _City)

                {

                    _City = value;

                    OnPropertyChanged("City");

                }

            }

        }

        public System.String State

        {

            get

            {

                return _State;

            }

            set

            {

                if (value != _State)

                {

                    _State = value;

                    OnPropertyChanged("State");

                }

            }

        }

        public System.String CountryOrRegion

        {

            get

            {

                return _CountryOrRegion;

            }

            set

            {

                if (value != _CountryOrRegion)

                {

                    _CountryOrRegion = value;

                    OnPropertyChanged("CountryOrRegion");

                }

            }

        }

        public System.String PostalCode

        {

            get

            {

                return _PostalCode;

            }

            set

            {

                if (value != _PostalCode)

                {

                    _PostalCode = value;

                    OnPropertyChanged("PostalCode");

                }

            }

        }

        public PhysicalAddress(): base()

        {

        }

    }

    public partial class RecurrencePattern:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.OutlookServices.RecurrencePatternType _Type;

        private System.Int32 _Interval;

        private System.Int32 _DayOfMonth;

        private System.Int32 _Month;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.DayOfWeek> _DaysOfWeek;

        private Microsoft.OutlookServices.DayOfWeek _FirstDayOfWeek;

        private Microsoft.OutlookServices.WeekIndex _Index;

        public Microsoft.OutlookServices.RecurrencePatternType Type

        {

            get

            {

                return _Type;

            }

            set

            {

                if (value != _Type)

                {

                    _Type = value;

                    OnPropertyChanged("Type");

                }

            }

        }

        public System.Int32 Interval

        {

            get

            {

                return _Interval;

            }

            set

            {

                if (value != _Interval)

                {

                    _Interval = value;

                    OnPropertyChanged("Interval");

                }

            }

        }

        public System.Int32 DayOfMonth

        {

            get

            {

                return _DayOfMonth;

            }

            set

            {

                if (value != _DayOfMonth)

                {

                    _DayOfMonth = value;

                    OnPropertyChanged("DayOfMonth");

                }

            }

        }

        public System.Int32 Month

        {

            get

            {

                return _Month;

            }

            set

            {

                if (value != _Month)

                {

                    _Month = value;

                    OnPropertyChanged("Month");

                }

            }

        }

        public System.Collections.Generic.IList<Microsoft.OutlookServices.DayOfWeek> DaysOfWeek

        {

            get

            {

                if (this._DaysOfWeek == default(System.Collections.Generic.IList<Microsoft.OutlookServices.DayOfWeek>))

                {

                    this._DaysOfWeek = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.DayOfWeek>();

                    this._DaysOfWeek.SetContainer(() => GetContainingEntity("DaysOfWeek"));

                }

                return this._DaysOfWeek;

            }

            set

            {

                _DaysOfWeek.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _DaysOfWeek.Add(i);

                    }

                }

            }

        }

        public Microsoft.OutlookServices.DayOfWeek FirstDayOfWeek

        {

            get

            {

                return _FirstDayOfWeek;

            }

            set

            {

                if (value != _FirstDayOfWeek)

                {

                    _FirstDayOfWeek = value;

                    OnPropertyChanged("FirstDayOfWeek");

                }

            }

        }

        public Microsoft.OutlookServices.WeekIndex Index

        {

            get

            {

                return _Index;

            }

            set

            {

                if (value != _Index)

                {

                    _Index = value;

                    OnPropertyChanged("Index");

                }

            }

        }

        public RecurrencePattern(): base()

        {

        }

    }

    public partial class RecurrenceRange:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.OutlookServices.RecurrenceRangeType _Type;

        private System.Nullable<System.DateTimeOffset> _StartDate;

        private System.Nullable<System.DateTimeOffset> _EndDate;

        private System.Int32 _NumberOfOccurrences;

        public Microsoft.OutlookServices.RecurrenceRangeType Type

        {

            get

            {

                return _Type;

            }

            set

            {

                if (value != _Type)

                {

                    _Type = value;

                    OnPropertyChanged("Type");

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> StartDate

        {

            get

            {

                return _StartDate;

            }

            set

            {

                if (value != _StartDate)

                {

                    _StartDate = value;

                    OnPropertyChanged("StartDate");

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> EndDate

        {

            get

            {

                return _EndDate;

            }

            set

            {

                if (value != _EndDate)

                {

                    _EndDate = value;

                    OnPropertyChanged("EndDate");

                }

            }

        }

        public System.Int32 NumberOfOccurrences

        {

            get

            {

                return _NumberOfOccurrences;

            }

            set

            {

                if (value != _NumberOfOccurrences)

                {

                    _NumberOfOccurrences = value;

                    OnPropertyChanged("NumberOfOccurrences");

                }

            }

        }

        public RecurrenceRange(): base()

        {

        }

    }

    public partial class PatternedRecurrence:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.OutlookServices.RecurrencePattern _Pattern;

        private Microsoft.OutlookServices.RecurrenceRange _Range;

        public Microsoft.OutlookServices.RecurrencePattern Pattern

        {

            get

            {

                return _Pattern;

            }

            set

            {

                if (value != _Pattern)

                {

                    _Pattern = value;

                    OnPropertyChanged("Pattern");

                }

            }

        }

        public Microsoft.OutlookServices.RecurrenceRange Range

        {

            get

            {

                return _Range;

            }

            set

            {

                if (value != _Range)

                {

                    _Range = value;

                    OnPropertyChanged("Range");

                }

            }

        }

        public PatternedRecurrence(): base()

        {

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public abstract partial class Entity:Microsoft.OData.ProxyExtensions.EntityBase, Microsoft.OutlookServices.IEntity, Microsoft.OutlookServices.IEntityFetcher

    {

        private System.String _Id;

        public System.String Id

        {

            get

            {

                return _Id;

            }

            set

            {

                if (value != _Id)

                {

                    _Id = value;

                    OnPropertyChanged("Id");

                }

            }

        }

        public Entity(): base()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IEntity> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Entity, Microsoft.OutlookServices.IEntity>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IEntity> _query;

        Microsoft.OutlookServices.IUserFetcher Microsoft.OutlookServices.IEntityFetcher.ToUser()

        {

            return (Microsoft.OutlookServices.IUserFetcher) this;

        }

        Microsoft.OutlookServices.IFolderFetcher Microsoft.OutlookServices.IEntityFetcher.ToFolder()

        {

            return (Microsoft.OutlookServices.IFolderFetcher) this;

        }

        Microsoft.OutlookServices.IItemFetcher Microsoft.OutlookServices.IEntityFetcher.ToItem()

        {

            return (Microsoft.OutlookServices.IItemFetcher) this;

        }

        Microsoft.OutlookServices.IAttachmentFetcher Microsoft.OutlookServices.IEntityFetcher.ToAttachment()

        {

            return (Microsoft.OutlookServices.IAttachmentFetcher) this;

        }

        Microsoft.OutlookServices.ICalendarFetcher Microsoft.OutlookServices.IEntityFetcher.ToCalendar()

        {

            return (Microsoft.OutlookServices.ICalendarFetcher) this;

        }

        Microsoft.OutlookServices.ICalendarGroupFetcher Microsoft.OutlookServices.IEntityFetcher.ToCalendarGroup()

        {

            return (Microsoft.OutlookServices.ICalendarGroupFetcher) this;

        }

        Microsoft.OutlookServices.IContactFolderFetcher Microsoft.OutlookServices.IEntityFetcher.ToContactFolder()

        {

            return (Microsoft.OutlookServices.IContactFolderFetcher) this;

        }

        Microsoft.OutlookServices.IMessageFetcher Microsoft.OutlookServices.IEntityFetcher.ToMessage()

        {

            return (Microsoft.OutlookServices.IMessageFetcher) this;

        }

        Microsoft.OutlookServices.IEventFetcher Microsoft.OutlookServices.IEntityFetcher.ToEvent()

        {

            return (Microsoft.OutlookServices.IEventFetcher) this;

        }

        Microsoft.OutlookServices.IContactFetcher Microsoft.OutlookServices.IEntityFetcher.ToContact()

        {

            return (Microsoft.OutlookServices.IContactFetcher) this;

        }

        Microsoft.OutlookServices.IFileAttachmentFetcher Microsoft.OutlookServices.IEntityFetcher.ToFileAttachment()

        {

            return (Microsoft.OutlookServices.IFileAttachmentFetcher) this;

        }

        Microsoft.OutlookServices.IItemAttachmentFetcher Microsoft.OutlookServices.IEntityFetcher.ToItemAttachment()

        {

            return (Microsoft.OutlookServices.IItemAttachmentFetcher) this;

        }

    }

    internal partial class EntityFetcher:Microsoft.OData.ProxyExtensions.RestShallowObjectFetcher, Microsoft.OutlookServices.IEntityFetcher

    {

        public EntityFetcher(): base()

        {

        }

        public Microsoft.OutlookServices.IUserFetcher ToUser()

        {

            var derivedFetcher = new Microsoft.OutlookServices.UserFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IUserFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IFolderFetcher ToFolder()

        {

            var derivedFetcher = new Microsoft.OutlookServices.FolderFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IFolderFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IItemFetcher ToItem()

        {

            var derivedFetcher = new Microsoft.OutlookServices.ItemFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IItemFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IAttachmentFetcher ToAttachment()

        {

            var derivedFetcher = new Microsoft.OutlookServices.AttachmentFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IAttachmentFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.ICalendarFetcher ToCalendar()

        {

            var derivedFetcher = new Microsoft.OutlookServices.CalendarFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.ICalendarFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.ICalendarGroupFetcher ToCalendarGroup()

        {

            var derivedFetcher = new Microsoft.OutlookServices.CalendarGroupFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.ICalendarGroupFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IContactFolderFetcher ToContactFolder()

        {

            var derivedFetcher = new Microsoft.OutlookServices.ContactFolderFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IContactFolderFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IMessageFetcher ToMessage()

        {

            var derivedFetcher = new Microsoft.OutlookServices.MessageFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IMessageFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IEventFetcher ToEvent()

        {

            var derivedFetcher = new Microsoft.OutlookServices.EventFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IEventFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IContactFetcher ToContact()

        {

            var derivedFetcher = new Microsoft.OutlookServices.ContactFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IContactFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IFileAttachmentFetcher ToFileAttachment()

        {

            var derivedFetcher = new Microsoft.OutlookServices.FileAttachmentFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IFileAttachmentFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IItemAttachmentFetcher ToItemAttachment()

        {

            var derivedFetcher = new Microsoft.OutlookServices.ItemAttachmentFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IItemAttachmentFetcher) derivedFetcher;

        }

    }

    internal partial class EntityCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IEntity>, Microsoft.OutlookServices.IEntityCollection

    {

        internal EntityCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IEntityFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEntity>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddEntityAsync(Microsoft.OutlookServices.IEntity item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IEntityFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.Entity>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.EntityFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class EntityCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public partial class User:Microsoft.OutlookServices.Entity, Microsoft.OutlookServices.IUser, Microsoft.OutlookServices.IUserFetcher

    {

        private Microsoft.OutlookServices.Folder _Folders;

        private Microsoft.OutlookServices.Message _Messages;

        private Microsoft.OutlookServices.Folder _RootFolder;

        private Microsoft.OutlookServices.Calendar _Calendars;

        private Microsoft.OutlookServices.Calendar _Calendar;

        private Microsoft.OutlookServices.CalendarGroup _CalendarGroups;

        private Microsoft.OutlookServices.Event _Events;

        private Microsoft.OutlookServices.Event _CalendarView;

        private Microsoft.OutlookServices.Contact _Contacts;

        private Microsoft.OutlookServices.ContactFolder _ContactFolders;

        private Microsoft.OutlookServices.FolderCollection _FoldersFetcher;

        private Microsoft.OutlookServices.MessageCollection _MessagesFetcher;

        private Microsoft.OutlookServices.FolderFetcher _RootFolderFetcher;

        private Microsoft.OutlookServices.CalendarCollection _CalendarsFetcher;

        private Microsoft.OutlookServices.CalendarFetcher _CalendarFetcher;

        private Microsoft.OutlookServices.CalendarGroupCollection _CalendarGroupsFetcher;

        private Microsoft.OutlookServices.EventCollection _EventsFetcher;

        private Microsoft.OutlookServices.EventCollection _CalendarViewFetcher;

        private Microsoft.OutlookServices.ContactCollection _ContactsFetcher;

        private Microsoft.OutlookServices.ContactFolderCollection _ContactFoldersFetcher;

        private Microsoft.OutlookServices.FolderCollection _FoldersCollection;

        private Microsoft.OutlookServices.MessageCollection _MessagesCollection;

        private Microsoft.OutlookServices.FolderCollection _RootFolderCollection;

        private Microsoft.OutlookServices.CalendarCollection _CalendarsCollection;

        private Microsoft.OutlookServices.CalendarCollection _CalendarCollection;

        private Microsoft.OutlookServices.CalendarGroupCollection _CalendarGroupsCollection;

        private Microsoft.OutlookServices.EventCollection _EventsCollection;

        private Microsoft.OutlookServices.EventCollection _CalendarViewCollection;

        private Microsoft.OutlookServices.ContactCollection _ContactsCollection;

        private Microsoft.OutlookServices.ContactFolderCollection _ContactFoldersCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Folder> _FoldersConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Message> _MessagesConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Folder> _RootFolderConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar> _CalendarsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar> _CalendarConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.CalendarGroup> _CalendarGroupsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event> _EventsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event> _CalendarViewConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Contact> _ContactsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.ContactFolder> _ContactFoldersConcrete;

        private System.String _DisplayName;

        private System.String _Alias;

        private System.Nullable<System.Guid> _MailboxGuid;

        public System.String DisplayName

        {

            get

            {

                return _DisplayName;

            }

            set

            {

                if (value != _DisplayName)

                {

                    _DisplayName = value;

                    OnPropertyChanged("DisplayName");

                }

            }

        }

        public System.String Alias

        {

            get

            {

                return _Alias;

            }

            set

            {

                if (value != _Alias)

                {

                    _Alias = value;

                    OnPropertyChanged("Alias");

                }

            }

        }

        public System.Nullable<System.Guid> MailboxGuid

        {

            get

            {

                return _MailboxGuid;

            }

            set

            {

                if (value != _MailboxGuid)

                {

                    _MailboxGuid = value;

                    OnPropertyChanged("MailboxGuid");

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IFolder> Microsoft.OutlookServices.IUser.Folders

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IFolder, Microsoft.OutlookServices.Folder>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Folder>) _FoldersConcrete);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IMessage> Microsoft.OutlookServices.IUser.Messages

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IMessage, Microsoft.OutlookServices.Message>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Message>) _MessagesConcrete);

            }

        }

        Microsoft.OutlookServices.IFolder Microsoft.OutlookServices.IUser.RootFolder

        {

            get

            {

                return this._RootFolder;

            }

            set

            {

                if (this._RootFolder != value)

                {

                    this._RootFolder = (Microsoft.OutlookServices.Folder)value;

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.ICalendar> Microsoft.OutlookServices.IUser.Calendars

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.ICalendar, Microsoft.OutlookServices.Calendar>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar>) _CalendarsConcrete);

            }

        }

        Microsoft.OutlookServices.ICalendar Microsoft.OutlookServices.IUser.Calendar

        {

            get

            {

                return this._Calendar;

            }

            set

            {

                if (this._Calendar != value)

                {

                    this._Calendar = (Microsoft.OutlookServices.Calendar)value;

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.ICalendarGroup> Microsoft.OutlookServices.IUser.CalendarGroups

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.ICalendarGroup, Microsoft.OutlookServices.CalendarGroup>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.CalendarGroup>) _CalendarGroupsConcrete);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent> Microsoft.OutlookServices.IUser.Events

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IEvent, Microsoft.OutlookServices.Event>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event>) _EventsConcrete);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent> Microsoft.OutlookServices.IUser.CalendarView

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IEvent, Microsoft.OutlookServices.Event>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event>) _CalendarViewConcrete);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContact> Microsoft.OutlookServices.IUser.Contacts

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IContact, Microsoft.OutlookServices.Contact>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Contact>) _ContactsConcrete);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContactFolder> Microsoft.OutlookServices.IUser.ContactFolders

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IContactFolder, Microsoft.OutlookServices.ContactFolder>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.ContactFolder>) _ContactFoldersConcrete);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Folder> Folders

        {

            get

            {

                if (this._FoldersConcrete == null)

                {

                    this._FoldersConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Folder>();

                    this._FoldersConcrete.SetContainer(() => GetContainingEntity("Folders"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Folder>)this._FoldersConcrete;

            }

            set

            {

                _FoldersConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _FoldersConcrete.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Message> Messages

        {

            get

            {

                if (this._MessagesConcrete == null)

                {

                    this._MessagesConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Message>();

                    this._MessagesConcrete.SetContainer(() => GetContainingEntity("Messages"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Message>)this._MessagesConcrete;

            }

            set

            {

                _MessagesConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _MessagesConcrete.Add(i);

                    }

                }

            }

        }

        public Microsoft.OutlookServices.Folder RootFolder

        {

            get

            {

                return this._RootFolder;

            }

            set

            {

                if (this._RootFolder != value)

                {

                    this._RootFolder = value;

                    if (Context != null && Context.GetEntityDescriptor(this) != null && (value == null || Context.GetEntityDescriptor(value) != null))

                    {

                        Context.SetLink(this, "RootFolder", value);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Calendar> Calendars

        {

            get

            {

                if (this._CalendarsConcrete == null)

                {

                    this._CalendarsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar>();

                    this._CalendarsConcrete.SetContainer(() => GetContainingEntity("Calendars"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Calendar>)this._CalendarsConcrete;

            }

            set

            {

                _CalendarsConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _CalendarsConcrete.Add(i);

                    }

                }

            }

        }

        public Microsoft.OutlookServices.Calendar Calendar

        {

            get

            {

                return this._Calendar;

            }

            set

            {

                if (this._Calendar != value)

                {

                    this._Calendar = value;

                    if (Context != null && Context.GetEntityDescriptor(this) != null && (value == null || Context.GetEntityDescriptor(value) != null))

                    {

                        Context.SetLink(this, "Calendar", value);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.CalendarGroup> CalendarGroups

        {

            get

            {

                if (this._CalendarGroupsConcrete == null)

                {

                    this._CalendarGroupsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.CalendarGroup>();

                    this._CalendarGroupsConcrete.SetContainer(() => GetContainingEntity("CalendarGroups"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.CalendarGroup>)this._CalendarGroupsConcrete;

            }

            set

            {

                _CalendarGroupsConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _CalendarGroupsConcrete.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Event> Events

        {

            get

            {

                if (this._EventsConcrete == null)

                {

                    this._EventsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event>();

                    this._EventsConcrete.SetContainer(() => GetContainingEntity("Events"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Event>)this._EventsConcrete;

            }

            set

            {

                _EventsConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _EventsConcrete.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Event> CalendarView

        {

            get

            {

                if (this._CalendarViewConcrete == null)

                {

                    this._CalendarViewConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event>();

                    this._CalendarViewConcrete.SetContainer(() => GetContainingEntity("CalendarView"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Event>)this._CalendarViewConcrete;

            }

            set

            {

                _CalendarViewConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _CalendarViewConcrete.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Contact> Contacts

        {

            get

            {

                if (this._ContactsConcrete == null)

                {

                    this._ContactsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Contact>();

                    this._ContactsConcrete.SetContainer(() => GetContainingEntity("Contacts"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Contact>)this._ContactsConcrete;

            }

            set

            {

                _ContactsConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _ContactsConcrete.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.ContactFolder> ContactFolders

        {

            get

            {

                if (this._ContactFoldersConcrete == null)

                {

                    this._ContactFoldersConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.ContactFolder>();

                    this._ContactFoldersConcrete.SetContainer(() => GetContainingEntity("ContactFolders"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.ContactFolder>)this._ContactFoldersConcrete;

            }

            set

            {

                _ContactFoldersConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _ContactFoldersConcrete.Add(i);

                    }

                }

            }

        }

        Microsoft.OutlookServices.IFolderCollection Microsoft.OutlookServices.IUserFetcher.Folders

        {

            get

            {

                if (this._FoldersCollection == null)

                {

                    this._FoldersCollection = new Microsoft.OutlookServices.FolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Folder>(GetPath("Folders")) : null,
                       Context,
                       this,
                       GetPath("Folders"));

                }

                

                return this._FoldersCollection;

            }

        }

        Microsoft.OutlookServices.IMessageCollection Microsoft.OutlookServices.IUserFetcher.Messages

        {

            get

            {

                if (this._MessagesCollection == null)

                {

                    this._MessagesCollection = new Microsoft.OutlookServices.MessageCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Message>(GetPath("Messages")) : null,
                       Context,
                       this,
                       GetPath("Messages"));

                }

                

                return this._MessagesCollection;

            }

        }

        Microsoft.OutlookServices.IFolderFetcher Microsoft.OutlookServices.IUserFetcher.RootFolder

        {

            get

            {

                if (this._RootFolderFetcher == null)

                {

                    this._RootFolderFetcher = new Microsoft.OutlookServices.FolderFetcher();

                    this._RootFolderFetcher.Initialize(this.Context, GetPath("RootFolder"));

                }

                

                return this._RootFolderFetcher;

            }

        }

        Microsoft.OutlookServices.ICalendarCollection Microsoft.OutlookServices.IUserFetcher.Calendars

        {

            get

            {

                if (this._CalendarsCollection == null)

                {

                    this._CalendarsCollection = new Microsoft.OutlookServices.CalendarCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Calendar>(GetPath("Calendars")) : null,
                       Context,
                       this,
                       GetPath("Calendars"));

                }

                

                return this._CalendarsCollection;

            }

        }

        Microsoft.OutlookServices.ICalendarFetcher Microsoft.OutlookServices.IUserFetcher.Calendar

        {

            get

            {

                if (this._CalendarFetcher == null)

                {

                    this._CalendarFetcher = new Microsoft.OutlookServices.CalendarFetcher();

                    this._CalendarFetcher.Initialize(this.Context, GetPath("Calendar"));

                }

                

                return this._CalendarFetcher;

            }

        }

        Microsoft.OutlookServices.ICalendarGroupCollection Microsoft.OutlookServices.IUserFetcher.CalendarGroups

        {

            get

            {

                if (this._CalendarGroupsCollection == null)

                {

                    this._CalendarGroupsCollection = new Microsoft.OutlookServices.CalendarGroupCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.CalendarGroup>(GetPath("CalendarGroups")) : null,
                       Context,
                       this,
                       GetPath("CalendarGroups"));

                }

                

                return this._CalendarGroupsCollection;

            }

        }

        Microsoft.OutlookServices.IEventCollection Microsoft.OutlookServices.IUserFetcher.Events

        {

            get

            {

                if (this._EventsCollection == null)

                {

                    this._EventsCollection = new Microsoft.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Event>(GetPath("Events")) : null,
                       Context,
                       this,
                       GetPath("Events"));

                }

                

                return this._EventsCollection;

            }

        }

        Microsoft.OutlookServices.IEventCollection Microsoft.OutlookServices.IUserFetcher.CalendarView

        {

            get

            {

                if (this._CalendarViewCollection == null)

                {

                    this._CalendarViewCollection = new Microsoft.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Event>(GetPath("CalendarView")) : null,
                       Context,
                       this,
                       GetPath("CalendarView"));

                }

                

                return this._CalendarViewCollection;

            }

        }

        Microsoft.OutlookServices.IContactCollection Microsoft.OutlookServices.IUserFetcher.Contacts

        {

            get

            {

                if (this._ContactsCollection == null)

                {

                    this._ContactsCollection = new Microsoft.OutlookServices.ContactCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Contact>(GetPath("Contacts")) : null,
                       Context,
                       this,
                       GetPath("Contacts"));

                }

                

                return this._ContactsCollection;

            }

        }

        Microsoft.OutlookServices.IContactFolderCollection Microsoft.OutlookServices.IUserFetcher.ContactFolders

        {

            get

            {

                if (this._ContactFoldersCollection == null)

                {

                    this._ContactFoldersCollection = new Microsoft.OutlookServices.ContactFolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.ContactFolder>(GetPath("ContactFolders")) : null,
                       Context,
                       this,
                       GetPath("ContactFolders"));

                }

                

                return this._ContactFoldersCollection;

            }

        }

        public User()

        {

        }

        public async System.Threading.Tasks.Task SendMailAsync(Microsoft.OutlookServices.IMessage Message, System.Nullable<System.Boolean> SaveToSentItems)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/SendMail");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[2]

            {

                new BodyOperationParameter("Message", (object) Message),

                new BodyOperationParameter("SaveToSentItems", (object) SaveToSentItems),

            }

            );

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IUser> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.User, Microsoft.OutlookServices.IUser>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IUser> _query;

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IUser> Microsoft.OutlookServices.IUserFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.OutlookServices.IUser>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.OutlookServices.IUserFetcher Microsoft.OutlookServices.IUserFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IUser, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IUserFetcher) this;

        }

    }

    internal partial class UserFetcher:Microsoft.OutlookServices.EntityFetcher, Microsoft.OutlookServices.IUserFetcher

    {

        private Microsoft.OutlookServices.FolderCollection _FoldersFetcher;

        private Microsoft.OutlookServices.MessageCollection _MessagesFetcher;

        private Microsoft.OutlookServices.FolderFetcher _RootFolderFetcher;

        private Microsoft.OutlookServices.CalendarCollection _CalendarsFetcher;

        private Microsoft.OutlookServices.CalendarFetcher _CalendarFetcher;

        private Microsoft.OutlookServices.CalendarGroupCollection _CalendarGroupsFetcher;

        private Microsoft.OutlookServices.EventCollection _EventsFetcher;

        private Microsoft.OutlookServices.EventCollection _CalendarViewFetcher;

        private Microsoft.OutlookServices.ContactCollection _ContactsFetcher;

        private Microsoft.OutlookServices.ContactFolderCollection _ContactFoldersFetcher;

        private Microsoft.OutlookServices.FolderCollection _FoldersCollection;

        private Microsoft.OutlookServices.MessageCollection _MessagesCollection;

        private Microsoft.OutlookServices.FolderCollection _RootFolderCollection;

        private Microsoft.OutlookServices.CalendarCollection _CalendarsCollection;

        private Microsoft.OutlookServices.CalendarCollection _CalendarCollection;

        private Microsoft.OutlookServices.CalendarGroupCollection _CalendarGroupsCollection;

        private Microsoft.OutlookServices.EventCollection _EventsCollection;

        private Microsoft.OutlookServices.EventCollection _CalendarViewCollection;

        private Microsoft.OutlookServices.ContactCollection _ContactsCollection;

        private Microsoft.OutlookServices.ContactFolderCollection _ContactFoldersCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Folder> _FoldersConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Message> _MessagesConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Folder> _RootFolderConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar> _CalendarsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar> _CalendarConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.CalendarGroup> _CalendarGroupsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event> _EventsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event> _CalendarViewConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Contact> _ContactsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.ContactFolder> _ContactFoldersConcrete;

        public Microsoft.OutlookServices.IFolderCollection Folders

        {

            get

            {

                if (this._FoldersCollection == null)

                {

                    this._FoldersCollection = new Microsoft.OutlookServices.FolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Folder>(GetPath("Folders")) : null,
                       Context,
                       this,
                       GetPath("Folders"));

                }

                

                return this._FoldersCollection;

            }

        }

        public Microsoft.OutlookServices.IMessageCollection Messages

        {

            get

            {

                if (this._MessagesCollection == null)

                {

                    this._MessagesCollection = new Microsoft.OutlookServices.MessageCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Message>(GetPath("Messages")) : null,
                       Context,
                       this,
                       GetPath("Messages"));

                }

                

                return this._MessagesCollection;

            }

        }

        public Microsoft.OutlookServices.IFolderFetcher RootFolder

        {

            get

            {

                if (this._RootFolderFetcher == null)

                {

                    this._RootFolderFetcher = new Microsoft.OutlookServices.FolderFetcher();

                    this._RootFolderFetcher.Initialize(this.Context, GetPath("RootFolder"));

                }

                

                return this._RootFolderFetcher;

            }

        }

        public Microsoft.OutlookServices.ICalendarCollection Calendars

        {

            get

            {

                if (this._CalendarsCollection == null)

                {

                    this._CalendarsCollection = new Microsoft.OutlookServices.CalendarCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Calendar>(GetPath("Calendars")) : null,
                       Context,
                       this,
                       GetPath("Calendars"));

                }

                

                return this._CalendarsCollection;

            }

        }

        public Microsoft.OutlookServices.ICalendarFetcher Calendar

        {

            get

            {

                if (this._CalendarFetcher == null)

                {

                    this._CalendarFetcher = new Microsoft.OutlookServices.CalendarFetcher();

                    this._CalendarFetcher.Initialize(this.Context, GetPath("Calendar"));

                }

                

                return this._CalendarFetcher;

            }

        }

        public Microsoft.OutlookServices.ICalendarGroupCollection CalendarGroups

        {

            get

            {

                if (this._CalendarGroupsCollection == null)

                {

                    this._CalendarGroupsCollection = new Microsoft.OutlookServices.CalendarGroupCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.CalendarGroup>(GetPath("CalendarGroups")) : null,
                       Context,
                       this,
                       GetPath("CalendarGroups"));

                }

                

                return this._CalendarGroupsCollection;

            }

        }

        public Microsoft.OutlookServices.IEventCollection Events

        {

            get

            {

                if (this._EventsCollection == null)

                {

                    this._EventsCollection = new Microsoft.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Event>(GetPath("Events")) : null,
                       Context,
                       this,
                       GetPath("Events"));

                }

                

                return this._EventsCollection;

            }

        }

        public Microsoft.OutlookServices.IEventCollection CalendarView

        {

            get

            {

                if (this._CalendarViewCollection == null)

                {

                    this._CalendarViewCollection = new Microsoft.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Event>(GetPath("CalendarView")) : null,
                       Context,
                       this,
                       GetPath("CalendarView"));

                }

                

                return this._CalendarViewCollection;

            }

        }

        public Microsoft.OutlookServices.IContactCollection Contacts

        {

            get

            {

                if (this._ContactsCollection == null)

                {

                    this._ContactsCollection = new Microsoft.OutlookServices.ContactCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Contact>(GetPath("Contacts")) : null,
                       Context,
                       this,
                       GetPath("Contacts"));

                }

                

                return this._ContactsCollection;

            }

        }

        public Microsoft.OutlookServices.IContactFolderCollection ContactFolders

        {

            get

            {

                if (this._ContactFoldersCollection == null)

                {

                    this._ContactFoldersCollection = new Microsoft.OutlookServices.ContactFolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.ContactFolder>(GetPath("ContactFolders")) : null,
                       Context,
                       this,
                       GetPath("ContactFolders"));

                }

                

                return this._ContactFoldersCollection;

            }

        }

        public UserFetcher()

        {

        }

        public async System.Threading.Tasks.Task SendMailAsync(Microsoft.OutlookServices.IMessage Message, System.Nullable<System.Boolean> SaveToSentItems)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/SendMail");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[2]

            {

                new BodyOperationParameter("Message", (object) Message),

                new BodyOperationParameter("SaveToSentItems", (object) SaveToSentItems),

            }

            );

        }

        public async global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IUser> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.OutlookServices.IUserFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IUser, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IUserFetcher) new Microsoft.OutlookServices.UserFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IUser> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.User, Microsoft.OutlookServices.IUser>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IUser> _query;

    }

    internal partial class UserCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IUser>, Microsoft.OutlookServices.IUserCollection

    {

        internal UserCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IUserFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IUser>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddUserAsync(Microsoft.OutlookServices.IUser item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IUserFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.User>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.UserFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class UserCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public partial class Folder:Microsoft.OutlookServices.Entity, Microsoft.OutlookServices.IFolder, Microsoft.OutlookServices.IFolderFetcher

    {

        private Microsoft.OutlookServices.Folder _ChildFolders;

        private Microsoft.OutlookServices.Message _Messages;

        private Microsoft.OutlookServices.FolderCollection _ChildFoldersFetcher;

        private Microsoft.OutlookServices.MessageCollection _MessagesFetcher;

        private Microsoft.OutlookServices.FolderCollection _ChildFoldersCollection;

        private Microsoft.OutlookServices.MessageCollection _MessagesCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Folder> _ChildFoldersConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Message> _MessagesConcrete;

        private System.String _ParentFolderId;

        private System.String _DisplayName;

        private System.Nullable<System.Int32> _ChildFolderCount;

        public System.String ParentFolderId

        {

            get

            {

                return _ParentFolderId;

            }

            set

            {

                if (value != _ParentFolderId)

                {

                    _ParentFolderId = value;

                    OnPropertyChanged("ParentFolderId");

                }

            }

        }

        public System.String DisplayName

        {

            get

            {

                return _DisplayName;

            }

            set

            {

                if (value != _DisplayName)

                {

                    _DisplayName = value;

                    OnPropertyChanged("DisplayName");

                }

            }

        }

        public System.Nullable<System.Int32> ChildFolderCount

        {

            get

            {

                return _ChildFolderCount;

            }

            set

            {

                if (value != _ChildFolderCount)

                {

                    _ChildFolderCount = value;

                    OnPropertyChanged("ChildFolderCount");

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IFolder> Microsoft.OutlookServices.IFolder.ChildFolders

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IFolder, Microsoft.OutlookServices.Folder>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Folder>) _ChildFoldersConcrete);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IMessage> Microsoft.OutlookServices.IFolder.Messages

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IMessage, Microsoft.OutlookServices.Message>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Message>) _MessagesConcrete);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Folder> ChildFolders

        {

            get

            {

                if (this._ChildFoldersConcrete == null)

                {

                    this._ChildFoldersConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Folder>();

                    this._ChildFoldersConcrete.SetContainer(() => GetContainingEntity("ChildFolders"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Folder>)this._ChildFoldersConcrete;

            }

            set

            {

                _ChildFoldersConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _ChildFoldersConcrete.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Message> Messages

        {

            get

            {

                if (this._MessagesConcrete == null)

                {

                    this._MessagesConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Message>();

                    this._MessagesConcrete.SetContainer(() => GetContainingEntity("Messages"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Message>)this._MessagesConcrete;

            }

            set

            {

                _MessagesConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _MessagesConcrete.Add(i);

                    }

                }

            }

        }

        Microsoft.OutlookServices.IFolderCollection Microsoft.OutlookServices.IFolderFetcher.ChildFolders

        {

            get

            {

                if (this._ChildFoldersCollection == null)

                {

                    this._ChildFoldersCollection = new Microsoft.OutlookServices.FolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Folder>(GetPath("ChildFolders")) : null,
                       Context,
                       this,
                       GetPath("ChildFolders"));

                }

                

                return this._ChildFoldersCollection;

            }

        }

        Microsoft.OutlookServices.IMessageCollection Microsoft.OutlookServices.IFolderFetcher.Messages

        {

            get

            {

                if (this._MessagesCollection == null)

                {

                    this._MessagesCollection = new Microsoft.OutlookServices.MessageCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Message>(GetPath("Messages")) : null,
                       Context,
                       this,
                       GetPath("Messages"));

                }

                

                return this._MessagesCollection;

            }

        }

        public Folder()

        {

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> CopyAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Copy");

            return (Microsoft.OutlookServices.IFolder) Enumerable.Single<Microsoft.OutlookServices.Folder>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Folder>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> MoveAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Move");

            return (Microsoft.OutlookServices.IFolder) Enumerable.Single<Microsoft.OutlookServices.Folder>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Folder>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IFolder> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Folder, Microsoft.OutlookServices.IFolder>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IFolder> _query;

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> Microsoft.OutlookServices.IFolderFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.OutlookServices.IFolder>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.OutlookServices.IFolderFetcher Microsoft.OutlookServices.IFolderFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IFolder, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IFolderFetcher) this;

        }

    }

    internal partial class FolderFetcher:Microsoft.OutlookServices.EntityFetcher, Microsoft.OutlookServices.IFolderFetcher

    {

        private Microsoft.OutlookServices.FolderCollection _ChildFoldersFetcher;

        private Microsoft.OutlookServices.MessageCollection _MessagesFetcher;

        private Microsoft.OutlookServices.FolderCollection _ChildFoldersCollection;

        private Microsoft.OutlookServices.MessageCollection _MessagesCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Folder> _ChildFoldersConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Message> _MessagesConcrete;

        public Microsoft.OutlookServices.IFolderCollection ChildFolders

        {

            get

            {

                if (this._ChildFoldersCollection == null)

                {

                    this._ChildFoldersCollection = new Microsoft.OutlookServices.FolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Folder>(GetPath("ChildFolders")) : null,
                       Context,
                       this,
                       GetPath("ChildFolders"));

                }

                

                return this._ChildFoldersCollection;

            }

        }

        public Microsoft.OutlookServices.IMessageCollection Messages

        {

            get

            {

                if (this._MessagesCollection == null)

                {

                    this._MessagesCollection = new Microsoft.OutlookServices.MessageCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Message>(GetPath("Messages")) : null,
                       Context,
                       this,
                       GetPath("Messages"));

                }

                

                return this._MessagesCollection;

            }

        }

        public FolderFetcher()

        {

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> CopyAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Copy");

            return (Microsoft.OutlookServices.IFolder) Enumerable.Single<Microsoft.OutlookServices.Folder>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Folder>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> MoveAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Move");

            return (Microsoft.OutlookServices.IFolder) Enumerable.Single<Microsoft.OutlookServices.Folder>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Folder>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.OutlookServices.IFolderFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IFolder, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IFolderFetcher) new Microsoft.OutlookServices.FolderFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IFolder> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Folder, Microsoft.OutlookServices.IFolder>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IFolder> _query;

    }

    internal partial class FolderCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IFolder>, Microsoft.OutlookServices.IFolderCollection

    {

        internal FolderCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IFolderFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IFolder>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddFolderAsync(Microsoft.OutlookServices.IFolder item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IFolderFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.Folder>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.FolderFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class FolderCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public abstract partial class Item:Microsoft.OutlookServices.Entity, Microsoft.OutlookServices.IItem, Microsoft.OutlookServices.IItemFetcher

    {

        private System.String _ChangeKey;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<System.String> _Categories;

        private System.Nullable<System.DateTimeOffset> _DateTimeCreated;

        private System.Nullable<System.DateTimeOffset> _DateTimeLastModified;

        public System.String ChangeKey

        {

            get

            {

                return _ChangeKey;

            }

            set

            {

                if (value != _ChangeKey)

                {

                    _ChangeKey = value;

                    OnPropertyChanged("ChangeKey");

                }

            }

        }

        public System.Collections.Generic.IList<System.String> Categories

        {

            get

            {

                if (this._Categories == default(System.Collections.Generic.IList<System.String>))

                {

                    this._Categories = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<System.String>();

                    this._Categories.SetContainer(() => GetContainingEntity("Categories"));

                }

                return this._Categories;

            }

            set

            {

                _Categories.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _Categories.Add(i);

                    }

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> DateTimeCreated

        {

            get

            {

                return _DateTimeCreated;

            }

            set

            {

                if (value != _DateTimeCreated)

                {

                    _DateTimeCreated = value;

                    OnPropertyChanged("DateTimeCreated");

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> DateTimeLastModified

        {

            get

            {

                return _DateTimeLastModified;

            }

            set

            {

                if (value != _DateTimeLastModified)

                {

                    _DateTimeLastModified = value;

                    OnPropertyChanged("DateTimeLastModified");

                }

            }

        }

        public Item()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IItem> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Item, Microsoft.OutlookServices.IItem>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IItem> _query;

        Microsoft.OutlookServices.IMessageFetcher Microsoft.OutlookServices.IItemFetcher.ToMessage()

        {

            return (Microsoft.OutlookServices.IMessageFetcher) this;

        }

        Microsoft.OutlookServices.IEventFetcher Microsoft.OutlookServices.IItemFetcher.ToEvent()

        {

            return (Microsoft.OutlookServices.IEventFetcher) this;

        }

        Microsoft.OutlookServices.IContactFetcher Microsoft.OutlookServices.IItemFetcher.ToContact()

        {

            return (Microsoft.OutlookServices.IContactFetcher) this;

        }

    }

    internal partial class ItemFetcher:Microsoft.OutlookServices.EntityFetcher, Microsoft.OutlookServices.IItemFetcher

    {

        public ItemFetcher()

        {

        }

        public Microsoft.OutlookServices.IMessageFetcher ToMessage()

        {

            var derivedFetcher = new Microsoft.OutlookServices.MessageFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IMessageFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IEventFetcher ToEvent()

        {

            var derivedFetcher = new Microsoft.OutlookServices.EventFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IEventFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IContactFetcher ToContact()

        {

            var derivedFetcher = new Microsoft.OutlookServices.ContactFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IContactFetcher) derivedFetcher;

        }

    }

    internal partial class ItemCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IItem>, Microsoft.OutlookServices.IItemCollection

    {

        internal ItemCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IItemFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IItem>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddItemAsync(Microsoft.OutlookServices.IItem item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IItemFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.Item>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.ItemFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class ItemCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public partial class Message:Microsoft.OutlookServices.Item, Microsoft.OutlookServices.IMessage, Microsoft.OutlookServices.IMessageFetcher

    {

        private Microsoft.OutlookServices.Attachment _Attachments;

        private Microsoft.OutlookServices.AttachmentCollection _AttachmentsFetcher;

        private Microsoft.OutlookServices.AttachmentCollection _AttachmentsCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Attachment> _AttachmentsConcrete;

        private System.String _Subject;

        private Microsoft.OutlookServices.ItemBody _Body;

        private System.String _BodyPreview;

        private Microsoft.OutlookServices.Importance _Importance;

        private System.Nullable<System.Boolean> _HasAttachments;

        private System.String _ParentFolderId;

        private Microsoft.OutlookServices.Recipient _From;

        private Microsoft.OutlookServices.Recipient _Sender;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.Recipient> _ToRecipients;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.Recipient> _CcRecipients;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.Recipient> _BccRecipients;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.Recipient> _ReplyTo;

        private System.String _ConversationId;

        private Microsoft.OutlookServices.ItemBody _UniqueBody;

        private System.Nullable<System.DateTimeOffset> _DateTimeReceived;

        private System.Nullable<System.DateTimeOffset> _DateTimeSent;

        private System.Nullable<System.Boolean> _IsDeliveryReceiptRequested;

        private System.Nullable<System.Boolean> _IsReadReceiptRequested;

        private System.Nullable<System.Boolean> _IsDraft;

        private System.Nullable<System.Boolean> _IsRead;

        public System.String Subject

        {

            get

            {

                return _Subject;

            }

            set

            {

                if (value != _Subject)

                {

                    _Subject = value;

                    OnPropertyChanged("Subject");

                }

            }

        }

        public Microsoft.OutlookServices.ItemBody Body

        {

            get

            {

                return _Body;

            }

            set

            {

                if (value != _Body)

                {

                    _Body = value;

                    OnPropertyChanged("Body");

                }

            }

        }

        public System.String BodyPreview

        {

            get

            {

                return _BodyPreview;

            }

            set

            {

                if (value != _BodyPreview)

                {

                    _BodyPreview = value;

                    OnPropertyChanged("BodyPreview");

                }

            }

        }

        public Microsoft.OutlookServices.Importance Importance

        {

            get

            {

                return _Importance;

            }

            set

            {

                if (value != _Importance)

                {

                    _Importance = value;

                    OnPropertyChanged("Importance");

                }

            }

        }

        public System.Nullable<System.Boolean> HasAttachments

        {

            get

            {

                return _HasAttachments;

            }

            set

            {

                if (value != _HasAttachments)

                {

                    _HasAttachments = value;

                    OnPropertyChanged("HasAttachments");

                }

            }

        }

        public System.String ParentFolderId

        {

            get

            {

                return _ParentFolderId;

            }

            set

            {

                if (value != _ParentFolderId)

                {

                    _ParentFolderId = value;

                    OnPropertyChanged("ParentFolderId");

                }

            }

        }

        public Microsoft.OutlookServices.Recipient From

        {

            get

            {

                return _From;

            }

            set

            {

                if (value != _From)

                {

                    _From = value;

                    OnPropertyChanged("From");

                }

            }

        }

        public Microsoft.OutlookServices.Recipient Sender

        {

            get

            {

                return _Sender;

            }

            set

            {

                if (value != _Sender)

                {

                    _Sender = value;

                    OnPropertyChanged("Sender");

                }

            }

        }

        public System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient> ToRecipients

        {

            get

            {

                if (this._ToRecipients == default(System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient>))

                {

                    this._ToRecipients = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.Recipient>();

                    this._ToRecipients.SetContainer(() => GetContainingEntity("ToRecipients"));

                }

                return this._ToRecipients;

            }

            set

            {

                _ToRecipients.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _ToRecipients.Add(i);

                    }

                }

            }

        }

        public System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient> CcRecipients

        {

            get

            {

                if (this._CcRecipients == default(System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient>))

                {

                    this._CcRecipients = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.Recipient>();

                    this._CcRecipients.SetContainer(() => GetContainingEntity("CcRecipients"));

                }

                return this._CcRecipients;

            }

            set

            {

                _CcRecipients.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _CcRecipients.Add(i);

                    }

                }

            }

        }

        public System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient> BccRecipients

        {

            get

            {

                if (this._BccRecipients == default(System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient>))

                {

                    this._BccRecipients = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.Recipient>();

                    this._BccRecipients.SetContainer(() => GetContainingEntity("BccRecipients"));

                }

                return this._BccRecipients;

            }

            set

            {

                _BccRecipients.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _BccRecipients.Add(i);

                    }

                }

            }

        }

        public System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient> ReplyTo

        {

            get

            {

                if (this._ReplyTo == default(System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient>))

                {

                    this._ReplyTo = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.Recipient>();

                    this._ReplyTo.SetContainer(() => GetContainingEntity("ReplyTo"));

                }

                return this._ReplyTo;

            }

            set

            {

                _ReplyTo.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _ReplyTo.Add(i);

                    }

                }

            }

        }

        public System.String ConversationId

        {

            get

            {

                return _ConversationId;

            }

            set

            {

                if (value != _ConversationId)

                {

                    _ConversationId = value;

                    OnPropertyChanged("ConversationId");

                }

            }

        }

        public Microsoft.OutlookServices.ItemBody UniqueBody

        {

            get

            {

                return _UniqueBody;

            }

            set

            {

                if (value != _UniqueBody)

                {

                    _UniqueBody = value;

                    OnPropertyChanged("UniqueBody");

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> DateTimeReceived

        {

            get

            {

                return _DateTimeReceived;

            }

            set

            {

                if (value != _DateTimeReceived)

                {

                    _DateTimeReceived = value;

                    OnPropertyChanged("DateTimeReceived");

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> DateTimeSent

        {

            get

            {

                return _DateTimeSent;

            }

            set

            {

                if (value != _DateTimeSent)

                {

                    _DateTimeSent = value;

                    OnPropertyChanged("DateTimeSent");

                }

            }

        }

        public System.Nullable<System.Boolean> IsDeliveryReceiptRequested

        {

            get

            {

                return _IsDeliveryReceiptRequested;

            }

            set

            {

                if (value != _IsDeliveryReceiptRequested)

                {

                    _IsDeliveryReceiptRequested = value;

                    OnPropertyChanged("IsDeliveryReceiptRequested");

                }

            }

        }

        public System.Nullable<System.Boolean> IsReadReceiptRequested

        {

            get

            {

                return _IsReadReceiptRequested;

            }

            set

            {

                if (value != _IsReadReceiptRequested)

                {

                    _IsReadReceiptRequested = value;

                    OnPropertyChanged("IsReadReceiptRequested");

                }

            }

        }

        public System.Nullable<System.Boolean> IsDraft

        {

            get

            {

                return _IsDraft;

            }

            set

            {

                if (value != _IsDraft)

                {

                    _IsDraft = value;

                    OnPropertyChanged("IsDraft");

                }

            }

        }

        public System.Nullable<System.Boolean> IsRead

        {

            get

            {

                return _IsRead;

            }

            set

            {

                if (value != _IsRead)

                {

                    _IsRead = value;

                    OnPropertyChanged("IsRead");

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IAttachment> Microsoft.OutlookServices.IMessage.Attachments

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IAttachment, Microsoft.OutlookServices.Attachment>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Attachment>) _AttachmentsConcrete);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Attachment> Attachments

        {

            get

            {

                if (this._AttachmentsConcrete == null)

                {

                    this._AttachmentsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Attachment>();

                    this._AttachmentsConcrete.SetContainer(() => GetContainingEntity("Attachments"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Attachment>)this._AttachmentsConcrete;

            }

            set

            {

                _AttachmentsConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _AttachmentsConcrete.Add(i);

                    }

                }

            }

        }

        Microsoft.OutlookServices.IAttachmentCollection Microsoft.OutlookServices.IMessageFetcher.Attachments

        {

            get

            {

                if (this._AttachmentsCollection == null)

                {

                    this._AttachmentsCollection = new Microsoft.OutlookServices.AttachmentCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Attachment>(GetPath("Attachments")) : null,
                       Context,
                       this,
                       GetPath("Attachments"));

                }

                

                return this._AttachmentsCollection;

            }

        }

        public Message()

        {

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CopyAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Copy");

            return (Microsoft.OutlookServices.IMessage) Enumerable.Single<Microsoft.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> MoveAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Move");

            return (Microsoft.OutlookServices.IMessage) Enumerable.Single<Microsoft.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateReplyAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateReply");

            return (Microsoft.OutlookServices.IMessage) Enumerable.Single<Microsoft.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateReplyAllAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateReplyAll");

            return (Microsoft.OutlookServices.IMessage) Enumerable.Single<Microsoft.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateForwardAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateForward");

            return (Microsoft.OutlookServices.IMessage) Enumerable.Single<Microsoft.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

            }

            ));

        }

        public async System.Threading.Tasks.Task ReplyAsync(System.String Comment)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Reply");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("Comment", (object) Comment),

            }

            );

        }

        public async System.Threading.Tasks.Task ReplyAllAsync(System.String Comment)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/ReplyAll");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("Comment", (object) Comment),

            }

            );

        }

        public async System.Threading.Tasks.Task ForwardAsync(System.String Comment, System.Collections.Generic.ICollection<Microsoft.OutlookServices.Recipient> ToRecipients)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Forward");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[2]

            {

                new BodyOperationParameter("Comment", (object) Comment),

                new BodyOperationParameter("ToRecipients", (object) ToRecipients),

            }

            );

        }

        public async System.Threading.Tasks.Task SendAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Send");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[0]

            {

            }

            );

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IMessage> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Message, Microsoft.OutlookServices.IMessage>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IMessage> _query;

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> Microsoft.OutlookServices.IMessageFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.OutlookServices.IMessage>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.OutlookServices.IMessageFetcher Microsoft.OutlookServices.IMessageFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IMessage, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IMessageFetcher) this;

        }

    }

    internal partial class MessageFetcher:Microsoft.OutlookServices.ItemFetcher, Microsoft.OutlookServices.IMessageFetcher

    {

        private Microsoft.OutlookServices.AttachmentCollection _AttachmentsFetcher;

        private Microsoft.OutlookServices.AttachmentCollection _AttachmentsCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Attachment> _AttachmentsConcrete;

        public Microsoft.OutlookServices.IAttachmentCollection Attachments

        {

            get

            {

                if (this._AttachmentsCollection == null)

                {

                    this._AttachmentsCollection = new Microsoft.OutlookServices.AttachmentCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Attachment>(GetPath("Attachments")) : null,
                       Context,
                       this,
                       GetPath("Attachments"));

                }

                

                return this._AttachmentsCollection;

            }

        }

        public MessageFetcher()

        {

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CopyAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Copy");

            return (Microsoft.OutlookServices.IMessage) Enumerable.Single<Microsoft.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> MoveAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Move");

            return (Microsoft.OutlookServices.IMessage) Enumerable.Single<Microsoft.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateReplyAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateReply");

            return (Microsoft.OutlookServices.IMessage) Enumerable.Single<Microsoft.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateReplyAllAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateReplyAll");

            return (Microsoft.OutlookServices.IMessage) Enumerable.Single<Microsoft.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateForwardAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateForward");

            return (Microsoft.OutlookServices.IMessage) Enumerable.Single<Microsoft.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

            }

            ));

        }

        public async System.Threading.Tasks.Task ReplyAsync(System.String Comment)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Reply");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("Comment", (object) Comment),

            }

            );

        }

        public async System.Threading.Tasks.Task ReplyAllAsync(System.String Comment)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/ReplyAll");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("Comment", (object) Comment),

            }

            );

        }

        public async System.Threading.Tasks.Task ForwardAsync(System.String Comment, System.Collections.Generic.ICollection<Microsoft.OutlookServices.Recipient> ToRecipients)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Forward");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[2]

            {

                new BodyOperationParameter("Comment", (object) Comment),

                new BodyOperationParameter("ToRecipients", (object) ToRecipients),

            }

            );

        }

        public async System.Threading.Tasks.Task SendAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Send");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[0]

            {

            }

            );

        }

        public async global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.OutlookServices.IMessageFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IMessage, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IMessageFetcher) new Microsoft.OutlookServices.MessageFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IMessage> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Message, Microsoft.OutlookServices.IMessage>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IMessage> _query;

    }

    internal partial class MessageCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IMessage>, Microsoft.OutlookServices.IMessageCollection

    {

        internal MessageCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IMessageFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IMessage>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddMessageAsync(Microsoft.OutlookServices.IMessage item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IMessageFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.Message>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.MessageFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class MessageCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public abstract partial class Attachment:Microsoft.OutlookServices.Entity, Microsoft.OutlookServices.IAttachment, Microsoft.OutlookServices.IAttachmentFetcher

    {

        private System.String _Name;

        private System.String _ContentType;

        private System.Int32 _Size;

        private System.Boolean _IsInline;

        private System.Nullable<System.DateTimeOffset> _DateTimeLastModified;

        public System.String Name

        {

            get

            {

                return _Name;

            }

            set

            {

                if (value != _Name)

                {

                    _Name = value;

                    OnPropertyChanged("Name");

                }

            }

        }

        public System.String ContentType

        {

            get

            {

                return _ContentType;

            }

            set

            {

                if (value != _ContentType)

                {

                    _ContentType = value;

                    OnPropertyChanged("ContentType");

                }

            }

        }

        public System.Int32 Size

        {

            get

            {

                return _Size;

            }

            set

            {

                if (value != _Size)

                {

                    _Size = value;

                    OnPropertyChanged("Size");

                }

            }

        }

        public System.Boolean IsInline

        {

            get

            {

                return _IsInline;

            }

            set

            {

                if (value != _IsInline)

                {

                    _IsInline = value;

                    OnPropertyChanged("IsInline");

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> DateTimeLastModified

        {

            get

            {

                return _DateTimeLastModified;

            }

            set

            {

                if (value != _DateTimeLastModified)

                {

                    _DateTimeLastModified = value;

                    OnPropertyChanged("DateTimeLastModified");

                }

            }

        }

        public Attachment()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IAttachment> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Attachment, Microsoft.OutlookServices.IAttachment>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IAttachment> _query;

        Microsoft.OutlookServices.IFileAttachmentFetcher Microsoft.OutlookServices.IAttachmentFetcher.ToFileAttachment()

        {

            return (Microsoft.OutlookServices.IFileAttachmentFetcher) this;

        }

        Microsoft.OutlookServices.IItemAttachmentFetcher Microsoft.OutlookServices.IAttachmentFetcher.ToItemAttachment()

        {

            return (Microsoft.OutlookServices.IItemAttachmentFetcher) this;

        }

    }

    internal partial class AttachmentFetcher:Microsoft.OutlookServices.EntityFetcher, Microsoft.OutlookServices.IAttachmentFetcher

    {

        public AttachmentFetcher()

        {

        }

        public Microsoft.OutlookServices.IFileAttachmentFetcher ToFileAttachment()

        {

            var derivedFetcher = new Microsoft.OutlookServices.FileAttachmentFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IFileAttachmentFetcher) derivedFetcher;

        }

        public Microsoft.OutlookServices.IItemAttachmentFetcher ToItemAttachment()

        {

            var derivedFetcher = new Microsoft.OutlookServices.ItemAttachmentFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.OutlookServices.IItemAttachmentFetcher) derivedFetcher;

        }

    }

    internal partial class AttachmentCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IAttachment>, Microsoft.OutlookServices.IAttachmentCollection

    {

        internal AttachmentCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IAttachmentFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IAttachment>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddAttachmentAsync(Microsoft.OutlookServices.IAttachment item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IAttachmentFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.Attachment>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.AttachmentFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class AttachmentCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public partial class FileAttachment:Microsoft.OutlookServices.Attachment, Microsoft.OutlookServices.IFileAttachment, Microsoft.OutlookServices.IFileAttachmentFetcher

    {

        private System.String _ContentId;

        private System.String _ContentLocation;

        private System.Boolean _IsContactPhoto;

        private System.Byte[] _ContentBytes;

        public System.String ContentId

        {

            get

            {

                return _ContentId;

            }

            set

            {

                if (value != _ContentId)

                {

                    _ContentId = value;

                    OnPropertyChanged("ContentId");

                }

            }

        }

        public System.String ContentLocation

        {

            get

            {

                return _ContentLocation;

            }

            set

            {

                if (value != _ContentLocation)

                {

                    _ContentLocation = value;

                    OnPropertyChanged("ContentLocation");

                }

            }

        }

        public System.Boolean IsContactPhoto

        {

            get

            {

                return _IsContactPhoto;

            }

            set

            {

                if (value != _IsContactPhoto)

                {

                    _IsContactPhoto = value;

                    OnPropertyChanged("IsContactPhoto");

                }

            }

        }

        public System.Byte[] ContentBytes

        {

            get

            {

                return _ContentBytes;

            }

            set

            {

                if (value != _ContentBytes)

                {

                    _ContentBytes = value;

                    OnPropertyChanged("ContentBytes");

                }

            }

        }

        public FileAttachment()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IFileAttachment> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.FileAttachment, Microsoft.OutlookServices.IFileAttachment>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IFileAttachment> _query;

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IFileAttachment> Microsoft.OutlookServices.IFileAttachmentFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.OutlookServices.IFileAttachment>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.OutlookServices.IFileAttachmentFetcher Microsoft.OutlookServices.IFileAttachmentFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IFileAttachment, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IFileAttachmentFetcher) this;

        }

    }

    internal partial class FileAttachmentFetcher:Microsoft.OutlookServices.AttachmentFetcher, Microsoft.OutlookServices.IFileAttachmentFetcher

    {

        public FileAttachmentFetcher()

        {

        }

        public async global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IFileAttachment> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.OutlookServices.IFileAttachmentFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IFileAttachment, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IFileAttachmentFetcher) new Microsoft.OutlookServices.FileAttachmentFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IFileAttachment> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.FileAttachment, Microsoft.OutlookServices.IFileAttachment>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IFileAttachment> _query;

    }

    internal partial class FileAttachmentCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IFileAttachment>, Microsoft.OutlookServices.IFileAttachmentCollection

    {

        internal FileAttachmentCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IFileAttachmentFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IFileAttachment>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddFileAttachmentAsync(Microsoft.OutlookServices.IFileAttachment item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IFileAttachmentFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.FileAttachment>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.FileAttachmentFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class FileAttachmentCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public partial class ItemAttachment:Microsoft.OutlookServices.Attachment, Microsoft.OutlookServices.IItemAttachment, Microsoft.OutlookServices.IItemAttachmentFetcher

    {

        private Microsoft.OutlookServices.Item _Item;

        private Microsoft.OutlookServices.ItemFetcher _ItemFetcher;

        private Microsoft.OutlookServices.ItemCollection _ItemCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Item> _ItemConcrete;

        Microsoft.OutlookServices.IItem Microsoft.OutlookServices.IItemAttachment.Item

        {

            get

            {

                return this._Item;

            }

            set

            {

                if (this._Item != value)

                {

                    this._Item = (Microsoft.OutlookServices.Item)value;

                }

            }

        }

        public Microsoft.OutlookServices.Item Item

        {

            get

            {

                return this._Item;

            }

            set

            {

                if (this._Item != value)

                {

                    this._Item = value;

                    if (Context != null && Context.GetEntityDescriptor(this) != null && (value == null || Context.GetEntityDescriptor(value) != null))

                    {

                        Context.SetLink(this, "Item", value);

                    }

                }

            }

        }

        Microsoft.OutlookServices.IItemFetcher Microsoft.OutlookServices.IItemAttachmentFetcher.Item

        {

            get

            {

                if (this._ItemFetcher == null)

                {

                    this._ItemFetcher = new Microsoft.OutlookServices.ItemFetcher();

                    this._ItemFetcher.Initialize(this.Context, GetPath("Item"));

                }

                

                return this._ItemFetcher;

            }

        }

        public ItemAttachment()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IItemAttachment> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.ItemAttachment, Microsoft.OutlookServices.IItemAttachment>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IItemAttachment> _query;

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IItemAttachment> Microsoft.OutlookServices.IItemAttachmentFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.OutlookServices.IItemAttachment>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.OutlookServices.IItemAttachmentFetcher Microsoft.OutlookServices.IItemAttachmentFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IItemAttachment, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IItemAttachmentFetcher) this;

        }

    }

    internal partial class ItemAttachmentFetcher:Microsoft.OutlookServices.AttachmentFetcher, Microsoft.OutlookServices.IItemAttachmentFetcher

    {

        private Microsoft.OutlookServices.ItemFetcher _ItemFetcher;

        private Microsoft.OutlookServices.ItemCollection _ItemCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Item> _ItemConcrete;

        public Microsoft.OutlookServices.IItemFetcher Item

        {

            get

            {

                if (this._ItemFetcher == null)

                {

                    this._ItemFetcher = new Microsoft.OutlookServices.ItemFetcher();

                    this._ItemFetcher.Initialize(this.Context, GetPath("Item"));

                }

                

                return this._ItemFetcher;

            }

        }

        public ItemAttachmentFetcher()

        {

        }

        public async global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IItemAttachment> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.OutlookServices.IItemAttachmentFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IItemAttachment, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IItemAttachmentFetcher) new Microsoft.OutlookServices.ItemAttachmentFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IItemAttachment> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.ItemAttachment, Microsoft.OutlookServices.IItemAttachment>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IItemAttachment> _query;

    }

    internal partial class ItemAttachmentCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IItemAttachment>, Microsoft.OutlookServices.IItemAttachmentCollection

    {

        internal ItemAttachmentCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IItemAttachmentFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IItemAttachment>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddItemAttachmentAsync(Microsoft.OutlookServices.IItemAttachment item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IItemAttachmentFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.ItemAttachment>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.ItemAttachmentFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class ItemAttachmentCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public partial class Calendar:Microsoft.OutlookServices.Entity, Microsoft.OutlookServices.ICalendar, Microsoft.OutlookServices.ICalendarFetcher

    {

        private Microsoft.OutlookServices.Event _CalendarView;

        private Microsoft.OutlookServices.Event _Events;

        private Microsoft.OutlookServices.EventCollection _CalendarViewFetcher;

        private Microsoft.OutlookServices.EventCollection _EventsFetcher;

        private Microsoft.OutlookServices.EventCollection _CalendarViewCollection;

        private Microsoft.OutlookServices.EventCollection _EventsCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event> _CalendarViewConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event> _EventsConcrete;

        private System.String _Name;

        private System.String _ChangeKey;

        public System.String Name

        {

            get

            {

                return _Name;

            }

            set

            {

                if (value != _Name)

                {

                    _Name = value;

                    OnPropertyChanged("Name");

                }

            }

        }

        public System.String ChangeKey

        {

            get

            {

                return _ChangeKey;

            }

            set

            {

                if (value != _ChangeKey)

                {

                    _ChangeKey = value;

                    OnPropertyChanged("ChangeKey");

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent> Microsoft.OutlookServices.ICalendar.CalendarView

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IEvent, Microsoft.OutlookServices.Event>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event>) _CalendarViewConcrete);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent> Microsoft.OutlookServices.ICalendar.Events

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IEvent, Microsoft.OutlookServices.Event>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event>) _EventsConcrete);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Event> CalendarView

        {

            get

            {

                if (this._CalendarViewConcrete == null)

                {

                    this._CalendarViewConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event>();

                    this._CalendarViewConcrete.SetContainer(() => GetContainingEntity("CalendarView"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Event>)this._CalendarViewConcrete;

            }

            set

            {

                _CalendarViewConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _CalendarViewConcrete.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Event> Events

        {

            get

            {

                if (this._EventsConcrete == null)

                {

                    this._EventsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event>();

                    this._EventsConcrete.SetContainer(() => GetContainingEntity("Events"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Event>)this._EventsConcrete;

            }

            set

            {

                _EventsConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _EventsConcrete.Add(i);

                    }

                }

            }

        }

        Microsoft.OutlookServices.IEventCollection Microsoft.OutlookServices.ICalendarFetcher.CalendarView

        {

            get

            {

                if (this._CalendarViewCollection == null)

                {

                    this._CalendarViewCollection = new Microsoft.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Event>(GetPath("CalendarView")) : null,
                       Context,
                       this,
                       GetPath("CalendarView"));

                }

                

                return this._CalendarViewCollection;

            }

        }

        Microsoft.OutlookServices.IEventCollection Microsoft.OutlookServices.ICalendarFetcher.Events

        {

            get

            {

                if (this._EventsCollection == null)

                {

                    this._EventsCollection = new Microsoft.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Event>(GetPath("Events")) : null,
                       Context,
                       this,
                       GetPath("Events"));

                }

                

                return this._EventsCollection;

            }

        }

        public Calendar()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.ICalendar> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Calendar, Microsoft.OutlookServices.ICalendar>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.ICalendar> _query;

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.ICalendar> Microsoft.OutlookServices.ICalendarFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.OutlookServices.ICalendar>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.OutlookServices.ICalendarFetcher Microsoft.OutlookServices.ICalendarFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.ICalendar, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.ICalendarFetcher) this;

        }

    }

    internal partial class CalendarFetcher:Microsoft.OutlookServices.EntityFetcher, Microsoft.OutlookServices.ICalendarFetcher

    {

        private Microsoft.OutlookServices.EventCollection _CalendarViewFetcher;

        private Microsoft.OutlookServices.EventCollection _EventsFetcher;

        private Microsoft.OutlookServices.EventCollection _CalendarViewCollection;

        private Microsoft.OutlookServices.EventCollection _EventsCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event> _CalendarViewConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event> _EventsConcrete;

        public Microsoft.OutlookServices.IEventCollection CalendarView

        {

            get

            {

                if (this._CalendarViewCollection == null)

                {

                    this._CalendarViewCollection = new Microsoft.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Event>(GetPath("CalendarView")) : null,
                       Context,
                       this,
                       GetPath("CalendarView"));

                }

                

                return this._CalendarViewCollection;

            }

        }

        public Microsoft.OutlookServices.IEventCollection Events

        {

            get

            {

                if (this._EventsCollection == null)

                {

                    this._EventsCollection = new Microsoft.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Event>(GetPath("Events")) : null,
                       Context,
                       this,
                       GetPath("Events"));

                }

                

                return this._EventsCollection;

            }

        }

        public CalendarFetcher()

        {

        }

        public async global::System.Threading.Tasks.Task<Microsoft.OutlookServices.ICalendar> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.OutlookServices.ICalendarFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.ICalendar, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.ICalendarFetcher) new Microsoft.OutlookServices.CalendarFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.ICalendar> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Calendar, Microsoft.OutlookServices.ICalendar>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.ICalendar> _query;

    }

    internal partial class CalendarCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.ICalendar>, Microsoft.OutlookServices.ICalendarCollection

    {

        internal CalendarCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.ICalendarFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.ICalendar>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddCalendarAsync(Microsoft.OutlookServices.ICalendar item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.ICalendarFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.Calendar>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.CalendarFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class CalendarCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public partial class CalendarGroup:Microsoft.OutlookServices.Entity, Microsoft.OutlookServices.ICalendarGroup, Microsoft.OutlookServices.ICalendarGroupFetcher

    {

        private Microsoft.OutlookServices.Calendar _Calendars;

        private Microsoft.OutlookServices.CalendarCollection _CalendarsFetcher;

        private Microsoft.OutlookServices.CalendarCollection _CalendarsCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar> _CalendarsConcrete;

        private System.String _Name;

        private System.String _ChangeKey;

        private System.Nullable<System.Guid> _ClassId;

        public System.String Name

        {

            get

            {

                return _Name;

            }

            set

            {

                if (value != _Name)

                {

                    _Name = value;

                    OnPropertyChanged("Name");

                }

            }

        }

        public System.String ChangeKey

        {

            get

            {

                return _ChangeKey;

            }

            set

            {

                if (value != _ChangeKey)

                {

                    _ChangeKey = value;

                    OnPropertyChanged("ChangeKey");

                }

            }

        }

        public System.Nullable<System.Guid> ClassId

        {

            get

            {

                return _ClassId;

            }

            set

            {

                if (value != _ClassId)

                {

                    _ClassId = value;

                    OnPropertyChanged("ClassId");

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.ICalendar> Microsoft.OutlookServices.ICalendarGroup.Calendars

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.ICalendar, Microsoft.OutlookServices.Calendar>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar>) _CalendarsConcrete);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Calendar> Calendars

        {

            get

            {

                if (this._CalendarsConcrete == null)

                {

                    this._CalendarsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar>();

                    this._CalendarsConcrete.SetContainer(() => GetContainingEntity("Calendars"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Calendar>)this._CalendarsConcrete;

            }

            set

            {

                _CalendarsConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _CalendarsConcrete.Add(i);

                    }

                }

            }

        }

        Microsoft.OutlookServices.ICalendarCollection Microsoft.OutlookServices.ICalendarGroupFetcher.Calendars

        {

            get

            {

                if (this._CalendarsCollection == null)

                {

                    this._CalendarsCollection = new Microsoft.OutlookServices.CalendarCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Calendar>(GetPath("Calendars")) : null,
                       Context,
                       this,
                       GetPath("Calendars"));

                }

                

                return this._CalendarsCollection;

            }

        }

        public CalendarGroup()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.ICalendarGroup> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.CalendarGroup, Microsoft.OutlookServices.ICalendarGroup>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.ICalendarGroup> _query;

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.ICalendarGroup> Microsoft.OutlookServices.ICalendarGroupFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.OutlookServices.ICalendarGroup>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.OutlookServices.ICalendarGroupFetcher Microsoft.OutlookServices.ICalendarGroupFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.ICalendarGroup, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.ICalendarGroupFetcher) this;

        }

    }

    internal partial class CalendarGroupFetcher:Microsoft.OutlookServices.EntityFetcher, Microsoft.OutlookServices.ICalendarGroupFetcher

    {

        private Microsoft.OutlookServices.CalendarCollection _CalendarsFetcher;

        private Microsoft.OutlookServices.CalendarCollection _CalendarsCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar> _CalendarsConcrete;

        public Microsoft.OutlookServices.ICalendarCollection Calendars

        {

            get

            {

                if (this._CalendarsCollection == null)

                {

                    this._CalendarsCollection = new Microsoft.OutlookServices.CalendarCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Calendar>(GetPath("Calendars")) : null,
                       Context,
                       this,
                       GetPath("Calendars"));

                }

                

                return this._CalendarsCollection;

            }

        }

        public CalendarGroupFetcher()

        {

        }

        public async global::System.Threading.Tasks.Task<Microsoft.OutlookServices.ICalendarGroup> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.OutlookServices.ICalendarGroupFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.ICalendarGroup, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.ICalendarGroupFetcher) new Microsoft.OutlookServices.CalendarGroupFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.ICalendarGroup> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.CalendarGroup, Microsoft.OutlookServices.ICalendarGroup>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.ICalendarGroup> _query;

    }

    internal partial class CalendarGroupCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.ICalendarGroup>, Microsoft.OutlookServices.ICalendarGroupCollection

    {

        internal CalendarGroupCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.ICalendarGroupFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.ICalendarGroup>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddCalendarGroupAsync(Microsoft.OutlookServices.ICalendarGroup item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.ICalendarGroupFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.CalendarGroup>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.CalendarGroupFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class CalendarGroupCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public partial class Event:Microsoft.OutlookServices.Item, Microsoft.OutlookServices.IEvent, Microsoft.OutlookServices.IEventFetcher

    {

        private Microsoft.OutlookServices.Attachment _Attachments;

        private Microsoft.OutlookServices.Calendar _Calendar;

        private Microsoft.OutlookServices.Event _Instances;

        private Microsoft.OutlookServices.AttachmentCollection _AttachmentsFetcher;

        private Microsoft.OutlookServices.CalendarFetcher _CalendarFetcher;

        private Microsoft.OutlookServices.EventCollection _InstancesFetcher;

        private Microsoft.OutlookServices.AttachmentCollection _AttachmentsCollection;

        private Microsoft.OutlookServices.CalendarCollection _CalendarCollection;

        private Microsoft.OutlookServices.EventCollection _InstancesCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Attachment> _AttachmentsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar> _CalendarConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event> _InstancesConcrete;

        private System.String _Subject;

        private Microsoft.OutlookServices.ItemBody _Body;

        private System.String _BodyPreview;

        private Microsoft.OutlookServices.Importance _Importance;

        private System.Nullable<System.Boolean> _HasAttachments;

        private System.Nullable<System.DateTimeOffset> _Start;

        private System.Nullable<System.DateTimeOffset> _End;

        private Microsoft.OutlookServices.Location _Location;

        private Microsoft.OutlookServices.FreeBusyStatus _ShowAs;

        private System.Nullable<System.Boolean> _IsAllDay;

        private System.Nullable<System.Boolean> _IsCancelled;

        private System.Nullable<System.Boolean> _IsOrganizer;

        private System.Nullable<System.Boolean> _ResponseRequested;

        private Microsoft.OutlookServices.EventType _Type;

        private System.String _SeriesMasterId;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.Attendee> _Attendees;

        private Microsoft.OutlookServices.PatternedRecurrence _Recurrence;

        private Microsoft.OutlookServices.Recipient _Organizer;

        public System.String Subject

        {

            get

            {

                return _Subject;

            }

            set

            {

                if (value != _Subject)

                {

                    _Subject = value;

                    OnPropertyChanged("Subject");

                }

            }

        }

        public Microsoft.OutlookServices.ItemBody Body

        {

            get

            {

                return _Body;

            }

            set

            {

                if (value != _Body)

                {

                    _Body = value;

                    OnPropertyChanged("Body");

                }

            }

        }

        public System.String BodyPreview

        {

            get

            {

                return _BodyPreview;

            }

            set

            {

                if (value != _BodyPreview)

                {

                    _BodyPreview = value;

                    OnPropertyChanged("BodyPreview");

                }

            }

        }

        public Microsoft.OutlookServices.Importance Importance

        {

            get

            {

                return _Importance;

            }

            set

            {

                if (value != _Importance)

                {

                    _Importance = value;

                    OnPropertyChanged("Importance");

                }

            }

        }

        public System.Nullable<System.Boolean> HasAttachments

        {

            get

            {

                return _HasAttachments;

            }

            set

            {

                if (value != _HasAttachments)

                {

                    _HasAttachments = value;

                    OnPropertyChanged("HasAttachments");

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> Start

        {

            get

            {

                return _Start;

            }

            set

            {

                if (value != _Start)

                {

                    _Start = value;

                    OnPropertyChanged("Start");

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> End

        {

            get

            {

                return _End;

            }

            set

            {

                if (value != _End)

                {

                    _End = value;

                    OnPropertyChanged("End");

                }

            }

        }

        public Microsoft.OutlookServices.Location Location

        {

            get

            {

                return _Location;

            }

            set

            {

                if (value != _Location)

                {

                    _Location = value;

                    OnPropertyChanged("Location");

                }

            }

        }

        public Microsoft.OutlookServices.FreeBusyStatus ShowAs

        {

            get

            {

                return _ShowAs;

            }

            set

            {

                if (value != _ShowAs)

                {

                    _ShowAs = value;

                    OnPropertyChanged("ShowAs");

                }

            }

        }

        public System.Nullable<System.Boolean> IsAllDay

        {

            get

            {

                return _IsAllDay;

            }

            set

            {

                if (value != _IsAllDay)

                {

                    _IsAllDay = value;

                    OnPropertyChanged("IsAllDay");

                }

            }

        }

        public System.Nullable<System.Boolean> IsCancelled

        {

            get

            {

                return _IsCancelled;

            }

            set

            {

                if (value != _IsCancelled)

                {

                    _IsCancelled = value;

                    OnPropertyChanged("IsCancelled");

                }

            }

        }

        public System.Nullable<System.Boolean> IsOrganizer

        {

            get

            {

                return _IsOrganizer;

            }

            set

            {

                if (value != _IsOrganizer)

                {

                    _IsOrganizer = value;

                    OnPropertyChanged("IsOrganizer");

                }

            }

        }

        public System.Nullable<System.Boolean> ResponseRequested

        {

            get

            {

                return _ResponseRequested;

            }

            set

            {

                if (value != _ResponseRequested)

                {

                    _ResponseRequested = value;

                    OnPropertyChanged("ResponseRequested");

                }

            }

        }

        public Microsoft.OutlookServices.EventType Type

        {

            get

            {

                return _Type;

            }

            set

            {

                if (value != _Type)

                {

                    _Type = value;

                    OnPropertyChanged("Type");

                }

            }

        }

        public System.String SeriesMasterId

        {

            get

            {

                return _SeriesMasterId;

            }

            set

            {

                if (value != _SeriesMasterId)

                {

                    _SeriesMasterId = value;

                    OnPropertyChanged("SeriesMasterId");

                }

            }

        }

        public System.Collections.Generic.IList<Microsoft.OutlookServices.Attendee> Attendees

        {

            get

            {

                if (this._Attendees == default(System.Collections.Generic.IList<Microsoft.OutlookServices.Attendee>))

                {

                    this._Attendees = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.Attendee>();

                    this._Attendees.SetContainer(() => GetContainingEntity("Attendees"));

                }

                return this._Attendees;

            }

            set

            {

                _Attendees.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _Attendees.Add(i);

                    }

                }

            }

        }

        public Microsoft.OutlookServices.PatternedRecurrence Recurrence

        {

            get

            {

                return _Recurrence;

            }

            set

            {

                if (value != _Recurrence)

                {

                    _Recurrence = value;

                    OnPropertyChanged("Recurrence");

                }

            }

        }

        public Microsoft.OutlookServices.Recipient Organizer

        {

            get

            {

                return _Organizer;

            }

            set

            {

                if (value != _Organizer)

                {

                    _Organizer = value;

                    OnPropertyChanged("Organizer");

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IAttachment> Microsoft.OutlookServices.IEvent.Attachments

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IAttachment, Microsoft.OutlookServices.Attachment>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Attachment>) _AttachmentsConcrete);

            }

        }

        Microsoft.OutlookServices.ICalendar Microsoft.OutlookServices.IEvent.Calendar

        {

            get

            {

                return this._Calendar;

            }

            set

            {

                if (this._Calendar != value)

                {

                    this._Calendar = (Microsoft.OutlookServices.Calendar)value;

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent> Microsoft.OutlookServices.IEvent.Instances

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IEvent, Microsoft.OutlookServices.Event>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event>) _InstancesConcrete);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Attachment> Attachments

        {

            get

            {

                if (this._AttachmentsConcrete == null)

                {

                    this._AttachmentsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Attachment>();

                    this._AttachmentsConcrete.SetContainer(() => GetContainingEntity("Attachments"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Attachment>)this._AttachmentsConcrete;

            }

            set

            {

                _AttachmentsConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _AttachmentsConcrete.Add(i);

                    }

                }

            }

        }

        public Microsoft.OutlookServices.Calendar Calendar

        {

            get

            {

                return this._Calendar;

            }

            set

            {

                if (this._Calendar != value)

                {

                    this._Calendar = value;

                    if (Context != null && Context.GetEntityDescriptor(this) != null && (value == null || Context.GetEntityDescriptor(value) != null))

                    {

                        Context.SetLink(this, "Calendar", value);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Event> Instances

        {

            get

            {

                if (this._InstancesConcrete == null)

                {

                    this._InstancesConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event>();

                    this._InstancesConcrete.SetContainer(() => GetContainingEntity("Instances"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Event>)this._InstancesConcrete;

            }

            set

            {

                _InstancesConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _InstancesConcrete.Add(i);

                    }

                }

            }

        }

        Microsoft.OutlookServices.IAttachmentCollection Microsoft.OutlookServices.IEventFetcher.Attachments

        {

            get

            {

                if (this._AttachmentsCollection == null)

                {

                    this._AttachmentsCollection = new Microsoft.OutlookServices.AttachmentCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Attachment>(GetPath("Attachments")) : null,
                       Context,
                       this,
                       GetPath("Attachments"));

                }

                

                return this._AttachmentsCollection;

            }

        }

        Microsoft.OutlookServices.ICalendarFetcher Microsoft.OutlookServices.IEventFetcher.Calendar

        {

            get

            {

                if (this._CalendarFetcher == null)

                {

                    this._CalendarFetcher = new Microsoft.OutlookServices.CalendarFetcher();

                    this._CalendarFetcher.Initialize(this.Context, GetPath("Calendar"));

                }

                

                return this._CalendarFetcher;

            }

        }

        Microsoft.OutlookServices.IEventCollection Microsoft.OutlookServices.IEventFetcher.Instances

        {

            get

            {

                if (this._InstancesCollection == null)

                {

                    this._InstancesCollection = new Microsoft.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Event>(GetPath("Instances")) : null,
                       Context,
                       this,
                       GetPath("Instances"));

                }

                

                return this._InstancesCollection;

            }

        }

        public Event()

        {

        }

        public async System.Threading.Tasks.Task AcceptAsync(System.String Comment)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Accept");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("Comment", (object) Comment),

            }

            );

        }

        public async System.Threading.Tasks.Task DeclineAsync(System.String Comment)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Decline");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("Comment", (object) Comment),

            }

            );

        }

        public async System.Threading.Tasks.Task TentativelyAcceptAsync(System.String Comment)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/TentativelyAccept");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("Comment", (object) Comment),

            }

            );

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IEvent> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Event, Microsoft.OutlookServices.IEvent>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IEvent> _query;

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IEvent> Microsoft.OutlookServices.IEventFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.OutlookServices.IEvent>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.OutlookServices.IEventFetcher Microsoft.OutlookServices.IEventFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IEvent, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IEventFetcher) this;

        }

    }

    internal partial class EventFetcher:Microsoft.OutlookServices.ItemFetcher, Microsoft.OutlookServices.IEventFetcher

    {

        private Microsoft.OutlookServices.AttachmentCollection _AttachmentsFetcher;

        private Microsoft.OutlookServices.CalendarFetcher _CalendarFetcher;

        private Microsoft.OutlookServices.EventCollection _InstancesFetcher;

        private Microsoft.OutlookServices.AttachmentCollection _AttachmentsCollection;

        private Microsoft.OutlookServices.CalendarCollection _CalendarCollection;

        private Microsoft.OutlookServices.EventCollection _InstancesCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Attachment> _AttachmentsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Calendar> _CalendarConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Event> _InstancesConcrete;

        public Microsoft.OutlookServices.IAttachmentCollection Attachments

        {

            get

            {

                if (this._AttachmentsCollection == null)

                {

                    this._AttachmentsCollection = new Microsoft.OutlookServices.AttachmentCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Attachment>(GetPath("Attachments")) : null,
                       Context,
                       this,
                       GetPath("Attachments"));

                }

                

                return this._AttachmentsCollection;

            }

        }

        public Microsoft.OutlookServices.ICalendarFetcher Calendar

        {

            get

            {

                if (this._CalendarFetcher == null)

                {

                    this._CalendarFetcher = new Microsoft.OutlookServices.CalendarFetcher();

                    this._CalendarFetcher.Initialize(this.Context, GetPath("Calendar"));

                }

                

                return this._CalendarFetcher;

            }

        }

        public Microsoft.OutlookServices.IEventCollection Instances

        {

            get

            {

                if (this._InstancesCollection == null)

                {

                    this._InstancesCollection = new Microsoft.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Event>(GetPath("Instances")) : null,
                       Context,
                       this,
                       GetPath("Instances"));

                }

                

                return this._InstancesCollection;

            }

        }

        public EventFetcher()

        {

        }

        public async System.Threading.Tasks.Task AcceptAsync(System.String Comment)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Accept");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("Comment", (object) Comment),

            }

            );

        }

        public async System.Threading.Tasks.Task DeclineAsync(System.String Comment)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Decline");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("Comment", (object) Comment),

            }

            );

        }

        public async System.Threading.Tasks.Task TentativelyAcceptAsync(System.String Comment)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/TentativelyAccept");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("Comment", (object) Comment),

            }

            );

        }

        public async global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IEvent> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.OutlookServices.IEventFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IEvent, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IEventFetcher) new Microsoft.OutlookServices.EventFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IEvent> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Event, Microsoft.OutlookServices.IEvent>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IEvent> _query;

    }

    internal partial class EventCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IEvent>, Microsoft.OutlookServices.IEventCollection

    {

        internal EventCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IEventFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddEventAsync(Microsoft.OutlookServices.IEvent item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IEventFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.Event>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.EventFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class EventCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public partial class Contact:Microsoft.OutlookServices.Item, Microsoft.OutlookServices.IContact, Microsoft.OutlookServices.IContactFetcher

    {

        private System.String _ParentFolderId;

        private System.Nullable<System.DateTimeOffset> _Birthday;

        private System.String _FileAs;

        private System.String _DisplayName;

        private System.String _GivenName;

        private System.String _Initials;

        private System.String _MiddleName;

        private System.String _NickName;

        private System.String _Surname;

        private System.String _Title;

        private System.String _Generation;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.EmailAddress> _EmailAddresses;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<System.String> _ImAddresses;

        private System.String _JobTitle;

        private System.String _CompanyName;

        private System.String _Department;

        private System.String _OfficeLocation;

        private System.String _Profession;

        private System.String _BusinessHomePage;

        private System.String _AssistantName;

        private System.String _Manager;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<System.String> _HomePhones;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<System.String> _BusinessPhones;

        private System.String _MobilePhone1;

        private Microsoft.OutlookServices.PhysicalAddress _HomeAddress;

        private Microsoft.OutlookServices.PhysicalAddress _BusinessAddress;

        private Microsoft.OutlookServices.PhysicalAddress _OtherAddress;

        private System.String _YomiCompanyName;

        private System.String _YomiGivenName;

        private System.String _YomiSurname;

        public System.String ParentFolderId

        {

            get

            {

                return _ParentFolderId;

            }

            set

            {

                if (value != _ParentFolderId)

                {

                    _ParentFolderId = value;

                    OnPropertyChanged("ParentFolderId");

                }

            }

        }

        public System.Nullable<System.DateTimeOffset> Birthday

        {

            get

            {

                return _Birthday;

            }

            set

            {

                if (value != _Birthday)

                {

                    _Birthday = value;

                    OnPropertyChanged("Birthday");

                }

            }

        }

        public System.String FileAs

        {

            get

            {

                return _FileAs;

            }

            set

            {

                if (value != _FileAs)

                {

                    _FileAs = value;

                    OnPropertyChanged("FileAs");

                }

            }

        }

        public System.String DisplayName

        {

            get

            {

                return _DisplayName;

            }

            set

            {

                if (value != _DisplayName)

                {

                    _DisplayName = value;

                    OnPropertyChanged("DisplayName");

                }

            }

        }

        public System.String GivenName

        {

            get

            {

                return _GivenName;

            }

            set

            {

                if (value != _GivenName)

                {

                    _GivenName = value;

                    OnPropertyChanged("GivenName");

                }

            }

        }

        public System.String Initials

        {

            get

            {

                return _Initials;

            }

            set

            {

                if (value != _Initials)

                {

                    _Initials = value;

                    OnPropertyChanged("Initials");

                }

            }

        }

        public System.String MiddleName

        {

            get

            {

                return _MiddleName;

            }

            set

            {

                if (value != _MiddleName)

                {

                    _MiddleName = value;

                    OnPropertyChanged("MiddleName");

                }

            }

        }

        public System.String NickName

        {

            get

            {

                return _NickName;

            }

            set

            {

                if (value != _NickName)

                {

                    _NickName = value;

                    OnPropertyChanged("NickName");

                }

            }

        }

        public System.String Surname

        {

            get

            {

                return _Surname;

            }

            set

            {

                if (value != _Surname)

                {

                    _Surname = value;

                    OnPropertyChanged("Surname");

                }

            }

        }

        public System.String Title

        {

            get

            {

                return _Title;

            }

            set

            {

                if (value != _Title)

                {

                    _Title = value;

                    OnPropertyChanged("Title");

                }

            }

        }

        public System.String Generation

        {

            get

            {

                return _Generation;

            }

            set

            {

                if (value != _Generation)

                {

                    _Generation = value;

                    OnPropertyChanged("Generation");

                }

            }

        }

        public System.Collections.Generic.IList<Microsoft.OutlookServices.EmailAddress> EmailAddresses

        {

            get

            {

                if (this._EmailAddresses == default(System.Collections.Generic.IList<Microsoft.OutlookServices.EmailAddress>))

                {

                    this._EmailAddresses = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.OutlookServices.EmailAddress>();

                    this._EmailAddresses.SetContainer(() => GetContainingEntity("EmailAddresses"));

                }

                return this._EmailAddresses;

            }

            set

            {

                _EmailAddresses.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _EmailAddresses.Add(i);

                    }

                }

            }

        }

        public System.Collections.Generic.IList<System.String> ImAddresses

        {

            get

            {

                if (this._ImAddresses == default(System.Collections.Generic.IList<System.String>))

                {

                    this._ImAddresses = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<System.String>();

                    this._ImAddresses.SetContainer(() => GetContainingEntity("ImAddresses"));

                }

                return this._ImAddresses;

            }

            set

            {

                _ImAddresses.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _ImAddresses.Add(i);

                    }

                }

            }

        }

        public System.String JobTitle

        {

            get

            {

                return _JobTitle;

            }

            set

            {

                if (value != _JobTitle)

                {

                    _JobTitle = value;

                    OnPropertyChanged("JobTitle");

                }

            }

        }

        public System.String CompanyName

        {

            get

            {

                return _CompanyName;

            }

            set

            {

                if (value != _CompanyName)

                {

                    _CompanyName = value;

                    OnPropertyChanged("CompanyName");

                }

            }

        }

        public System.String Department

        {

            get

            {

                return _Department;

            }

            set

            {

                if (value != _Department)

                {

                    _Department = value;

                    OnPropertyChanged("Department");

                }

            }

        }

        public System.String OfficeLocation

        {

            get

            {

                return _OfficeLocation;

            }

            set

            {

                if (value != _OfficeLocation)

                {

                    _OfficeLocation = value;

                    OnPropertyChanged("OfficeLocation");

                }

            }

        }

        public System.String Profession

        {

            get

            {

                return _Profession;

            }

            set

            {

                if (value != _Profession)

                {

                    _Profession = value;

                    OnPropertyChanged("Profession");

                }

            }

        }

        public System.String BusinessHomePage

        {

            get

            {

                return _BusinessHomePage;

            }

            set

            {

                if (value != _BusinessHomePage)

                {

                    _BusinessHomePage = value;

                    OnPropertyChanged("BusinessHomePage");

                }

            }

        }

        public System.String AssistantName

        {

            get

            {

                return _AssistantName;

            }

            set

            {

                if (value != _AssistantName)

                {

                    _AssistantName = value;

                    OnPropertyChanged("AssistantName");

                }

            }

        }

        public System.String Manager

        {

            get

            {

                return _Manager;

            }

            set

            {

                if (value != _Manager)

                {

                    _Manager = value;

                    OnPropertyChanged("Manager");

                }

            }

        }

        public System.Collections.Generic.IList<System.String> HomePhones

        {

            get

            {

                if (this._HomePhones == default(System.Collections.Generic.IList<System.String>))

                {

                    this._HomePhones = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<System.String>();

                    this._HomePhones.SetContainer(() => GetContainingEntity("HomePhones"));

                }

                return this._HomePhones;

            }

            set

            {

                _HomePhones.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _HomePhones.Add(i);

                    }

                }

            }

        }

        public System.Collections.Generic.IList<System.String> BusinessPhones

        {

            get

            {

                if (this._BusinessPhones == default(System.Collections.Generic.IList<System.String>))

                {

                    this._BusinessPhones = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<System.String>();

                    this._BusinessPhones.SetContainer(() => GetContainingEntity("BusinessPhones"));

                }

                return this._BusinessPhones;

            }

            set

            {

                _BusinessPhones.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _BusinessPhones.Add(i);

                    }

                }

            }

        }

        public System.String MobilePhone1

        {

            get

            {

                return _MobilePhone1;

            }

            set

            {

                if (value != _MobilePhone1)

                {

                    _MobilePhone1 = value;

                    OnPropertyChanged("MobilePhone1");

                }

            }

        }

        public Microsoft.OutlookServices.PhysicalAddress HomeAddress

        {

            get

            {

                return _HomeAddress;

            }

            set

            {

                if (value != _HomeAddress)

                {

                    _HomeAddress = value;

                    OnPropertyChanged("HomeAddress");

                }

            }

        }

        public Microsoft.OutlookServices.PhysicalAddress BusinessAddress

        {

            get

            {

                return _BusinessAddress;

            }

            set

            {

                if (value != _BusinessAddress)

                {

                    _BusinessAddress = value;

                    OnPropertyChanged("BusinessAddress");

                }

            }

        }

        public Microsoft.OutlookServices.PhysicalAddress OtherAddress

        {

            get

            {

                return _OtherAddress;

            }

            set

            {

                if (value != _OtherAddress)

                {

                    _OtherAddress = value;

                    OnPropertyChanged("OtherAddress");

                }

            }

        }

        public System.String YomiCompanyName

        {

            get

            {

                return _YomiCompanyName;

            }

            set

            {

                if (value != _YomiCompanyName)

                {

                    _YomiCompanyName = value;

                    OnPropertyChanged("YomiCompanyName");

                }

            }

        }

        public System.String YomiGivenName

        {

            get

            {

                return _YomiGivenName;

            }

            set

            {

                if (value != _YomiGivenName)

                {

                    _YomiGivenName = value;

                    OnPropertyChanged("YomiGivenName");

                }

            }

        }

        public System.String YomiSurname

        {

            get

            {

                return _YomiSurname;

            }

            set

            {

                if (value != _YomiSurname)

                {

                    _YomiSurname = value;

                    OnPropertyChanged("YomiSurname");

                }

            }

        }

        public Contact()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IContact> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Contact, Microsoft.OutlookServices.IContact>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IContact> _query;

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IContact> Microsoft.OutlookServices.IContactFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.OutlookServices.IContact>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.OutlookServices.IContactFetcher Microsoft.OutlookServices.IContactFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IContact, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IContactFetcher) this;

        }

    }

    internal partial class ContactFetcher:Microsoft.OutlookServices.ItemFetcher, Microsoft.OutlookServices.IContactFetcher

    {

        public ContactFetcher()

        {

        }

        public async global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IContact> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.OutlookServices.IContactFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IContact, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IContactFetcher) new Microsoft.OutlookServices.ContactFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IContact> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.Contact, Microsoft.OutlookServices.IContact>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IContact> _query;

    }

    internal partial class ContactCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IContact>, Microsoft.OutlookServices.IContactCollection

    {

        internal ContactCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IContactFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContact>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddContactAsync(Microsoft.OutlookServices.IContact item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IContactFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.Contact>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.ContactFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class ContactCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("Id")]

    public partial class ContactFolder:Microsoft.OutlookServices.Entity, Microsoft.OutlookServices.IContactFolder, Microsoft.OutlookServices.IContactFolderFetcher

    {

        private Microsoft.OutlookServices.Contact _Contacts;

        private Microsoft.OutlookServices.ContactFolder _ChildFolders;

        private Microsoft.OutlookServices.ContactCollection _ContactsFetcher;

        private Microsoft.OutlookServices.ContactFolderCollection _ChildFoldersFetcher;

        private Microsoft.OutlookServices.ContactCollection _ContactsCollection;

        private Microsoft.OutlookServices.ContactFolderCollection _ChildFoldersCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Contact> _ContactsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.ContactFolder> _ChildFoldersConcrete;

        private System.String _ParentFolderId;

        private System.String _DisplayName;

        public System.String ParentFolderId

        {

            get

            {

                return _ParentFolderId;

            }

            set

            {

                if (value != _ParentFolderId)

                {

                    _ParentFolderId = value;

                    OnPropertyChanged("ParentFolderId");

                }

            }

        }

        public System.String DisplayName

        {

            get

            {

                return _DisplayName;

            }

            set

            {

                if (value != _DisplayName)

                {

                    _DisplayName = value;

                    OnPropertyChanged("DisplayName");

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContact> Microsoft.OutlookServices.IContactFolder.Contacts

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IContact, Microsoft.OutlookServices.Contact>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Contact>) _ContactsConcrete);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContactFolder> Microsoft.OutlookServices.IContactFolder.ChildFolders

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.OutlookServices.IContactFolder, Microsoft.OutlookServices.ContactFolder>(Context, (Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.ContactFolder>) _ChildFoldersConcrete);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.Contact> Contacts

        {

            get

            {

                if (this._ContactsConcrete == null)

                {

                    this._ContactsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Contact>();

                    this._ContactsConcrete.SetContainer(() => GetContainingEntity("Contacts"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.Contact>)this._ContactsConcrete;

            }

            set

            {

                _ContactsConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _ContactsConcrete.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.OutlookServices.ContactFolder> ChildFolders

        {

            get

            {

                if (this._ChildFoldersConcrete == null)

                {

                    this._ChildFoldersConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.ContactFolder>();

                    this._ChildFoldersConcrete.SetContainer(() => GetContainingEntity("ChildFolders"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.OutlookServices.ContactFolder>)this._ChildFoldersConcrete;

            }

            set

            {

                _ChildFoldersConcrete.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        _ChildFoldersConcrete.Add(i);

                    }

                }

            }

        }

        Microsoft.OutlookServices.IContactCollection Microsoft.OutlookServices.IContactFolderFetcher.Contacts

        {

            get

            {

                if (this._ContactsCollection == null)

                {

                    this._ContactsCollection = new Microsoft.OutlookServices.ContactCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Contact>(GetPath("Contacts")) : null,
                       Context,
                       this,
                       GetPath("Contacts"));

                }

                

                return this._ContactsCollection;

            }

        }

        Microsoft.OutlookServices.IContactFolderCollection Microsoft.OutlookServices.IContactFolderFetcher.ChildFolders

        {

            get

            {

                if (this._ChildFoldersCollection == null)

                {

                    this._ChildFoldersCollection = new Microsoft.OutlookServices.ContactFolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.ContactFolder>(GetPath("ChildFolders")) : null,
                       Context,
                       this,
                       GetPath("ChildFolders"));

                }

                

                return this._ChildFoldersCollection;

            }

        }

        public ContactFolder()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IContactFolder> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.ContactFolder, Microsoft.OutlookServices.IContactFolder>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IContactFolder> _query;

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IContactFolder> Microsoft.OutlookServices.IContactFolderFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.OutlookServices.IContactFolder>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.OutlookServices.IContactFolderFetcher Microsoft.OutlookServices.IContactFolderFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IContactFolder, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IContactFolderFetcher) this;

        }

    }

    internal partial class ContactFolderFetcher:Microsoft.OutlookServices.EntityFetcher, Microsoft.OutlookServices.IContactFolderFetcher

    {

        private Microsoft.OutlookServices.ContactCollection _ContactsFetcher;

        private Microsoft.OutlookServices.ContactFolderCollection _ChildFoldersFetcher;

        private Microsoft.OutlookServices.ContactCollection _ContactsCollection;

        private Microsoft.OutlookServices.ContactFolderCollection _ChildFoldersCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.Contact> _ContactsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.ContactFolder> _ChildFoldersConcrete;

        public Microsoft.OutlookServices.IContactCollection Contacts

        {

            get

            {

                if (this._ContactsCollection == null)

                {

                    this._ContactsCollection = new Microsoft.OutlookServices.ContactCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.Contact>(GetPath("Contacts")) : null,
                       Context,
                       this,
                       GetPath("Contacts"));

                }

                

                return this._ContactsCollection;

            }

        }

        public Microsoft.OutlookServices.IContactFolderCollection ChildFolders

        {

            get

            {

                if (this._ChildFoldersCollection == null)

                {

                    this._ChildFoldersCollection = new Microsoft.OutlookServices.ContactFolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.ContactFolder>(GetPath("ChildFolders")) : null,
                       Context,
                       this,
                       GetPath("ChildFolders"));

                }

                

                return this._ChildFoldersCollection;

            }

        }

        public ContactFolderFetcher()

        {

        }

        public async global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IContactFolder> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.OutlookServices.IContactFolderFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IContactFolder, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.OutlookServices.IContactFolderFetcher) new Microsoft.OutlookServices.ContactFolderFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IContactFolder> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.OutlookServices.ContactFolder, Microsoft.OutlookServices.IContactFolder>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.OutlookServices.IContactFolder> _query;

    }

    internal partial class ContactFolderCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.OutlookServices.IContactFolder>, Microsoft.OutlookServices.IContactFolderCollection

    {

        internal ContactFolderCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.OutlookServices.IContactFolderFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContactFolder>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddContactFolderAsync(Microsoft.OutlookServices.IContactFolder item, System.Boolean dontSave = false)

        {

            if (_entity == null)

            {

                Context.AddObject(_path, item);

            }

            else

            {

                var lastSlash = _path.LastIndexOf('/');

                var shortPath = (lastSlash >= 0 && lastSlash < _path.Length - 1) ? _path.Substring(lastSlash + 1) : _path;

                Context.AddRelatedObject(_entity, shortPath, item);

            }

            if (!dontSave)

            {

                return Context.SaveChangesAsync();

            }

            else

            {

                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();

                retVal.SetResult(null);

                return retVal.Task;

            }

        }

        public Microsoft.OutlookServices.IContactFolderFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.OutlookServices.ContactFolder>((i) => i.Id == id);

                var fetcher = new Microsoft.OutlookServices.ContactFolderFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class ContactFolderCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    public partial class EntityContainer:Microsoft.OutlookServices.IEntityContainer

    {

        private const System.String _path = "";

        private Microsoft.OutlookServices.UserCollection _UsersFetcher;

        private Microsoft.OutlookServices.UserFetcher _MeFetcher;

        private Microsoft.OutlookServices.UserCollection _UsersCollection;

        private Microsoft.OutlookServices.UserCollection _MeCollection;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.User> _UsersConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.OutlookServices.User> _MeConcrete;

        public Microsoft.OutlookServices.IUserCollection Users

        {

            get

            {

                if (this._UsersCollection == null)

                {

                    this._UsersCollection = new Microsoft.OutlookServices.UserCollection(
                       Context != null ? Context.CreateQuery<Microsoft.OutlookServices.User>(GetPath("Users")) : null,
                       Context,
                       this,
                       GetPath("Users"));

                }

                

                return this._UsersCollection;

            }

        }

        public Microsoft.OutlookServices.IUserFetcher Me

        {

            get

            {

                if (this._MeFetcher == null)

                {

                    this._MeFetcher = new Microsoft.OutlookServices.UserFetcher();

                    this._MeFetcher.Initialize(this.Context, GetPath("Me"));

                }

                

                return this._MeFetcher;

            }

            private set

            {

                this._MeFetcher = (Microsoft.OutlookServices.UserFetcher)value;

            }

        }

        public Microsoft.OData.ProxyExtensions.DataServiceContextWrapper Context

        {get; private set;}

        public EntityContainer(global::System.Uri serviceRoot, global::System.Func<global::System.Threading.Tasks.Task<string>> accessTokenGetter)

        {

            Context = new Microsoft.OData.ProxyExtensions.DataServiceContextWrapper(serviceRoot, global::Microsoft.OData.Client.ODataProtocolVersion.V4, accessTokenGetter);

            Context.MergeOption = global::Microsoft.OData.Client.MergeOption.OverwriteChanges;

            Context.ResolveName = new global::System.Func<global::System.Type, string>(this.ResolveNameFromType);

            Context.ResolveType = new global::System.Func<string, global::System.Type>(this.ResolveTypeFromName);

            this.OnContextCreated();

            Context.Format.LoadServiceModel = GeneratedEdmModel.GetInstance;

            Context.Format.UseJson();

        }

        partial void OnContextCreated();

        public void AddToUsers(Microsoft.OutlookServices.IUser users)

        {

            this.Context.AddObject("Users", (object) users);

        }

        private global::System.Type ResolveTypeFromName(System.String typeName)

        {

            global::System.Type resolvedType;

            resolvedType = Context.DefaultResolveTypeInternal(typeName,  "Microsoft.OutlookServices", "Microsoft.OutlookServices");

            if (resolvedType != null)

            {

                return resolvedType;

            }

            return null;

        }

        private System.String ResolveNameFromType(global::System.Type clientType)

        {

            string resolvedType;

            resolvedType = Context.DefaultResolveNameInternal(clientType,  "Microsoft.OutlookServices", "Microsoft.OutlookServices");

            if (!string.IsNullOrEmpty(resolvedType))

            {

                return resolvedType;

            }

            return clientType.FullName;

        }

        private System.String GetPath(System.String propertyName)

        {

            return propertyName == null ? _path : _path + "/" + propertyName;

        }

        private System.Uri GetUrl()

        {

            return this.Context.BaseUri;

        }

        private abstract partial class GeneratedEdmModel

        {

            private static global::Microsoft.OData.Edm.IEdmModel ParsedModel = LoadModelFromString();

            private const System.String Edmx = @"<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
            
              <edmx:DataServices>
            
                <Schema Namespace=""Microsoft.OutlookServices"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
            
                  <EnumType Name=""DayOfWeek"">
            
                    <Member Name=""Sunday"" Value=""0"" />
            
                    <Member Name=""Monday"" Value=""1"" />
            
                    <Member Name=""Tuesday"" Value=""2"" />
            
                    <Member Name=""Wednesday"" Value=""3"" />
            
                    <Member Name=""Thursday"" Value=""4"" />
            
                    <Member Name=""Friday"" Value=""5"" />
            
                    <Member Name=""Saturday"" Value=""6"" />
            
                  </EnumType>
            
                  <EnumType Name=""BodyType"">
            
                    <Member Name=""Text"" Value=""0"" />
            
                    <Member Name=""HTML"" Value=""1"" />
            
                  </EnumType>
            
                  <EnumType Name=""Importance"">
            
                    <Member Name=""Low"" Value=""0"" />
            
                    <Member Name=""Normal"" Value=""1"" />
            
                    <Member Name=""High"" Value=""2"" />
            
                  </EnumType>
            
                  <EnumType Name=""AttendeeType"">
            
                    <Member Name=""Required"" Value=""0"" />
            
                    <Member Name=""Optional"" Value=""1"" />
            
                    <Member Name=""Resource"" Value=""2"" />
            
                  </EnumType>
            
                  <EnumType Name=""ResponseType"">
            
                    <Member Name=""None"" Value=""0"" />
            
                    <Member Name=""Organizer"" Value=""1"" />
            
                    <Member Name=""TentativelyAccepted"" Value=""2"" />
            
                    <Member Name=""Accepted"" Value=""3"" />
            
                    <Member Name=""Declined"" Value=""4"" />
            
                    <Member Name=""NotResponded"" Value=""5"" />
            
                  </EnumType>
            
                  <EnumType Name=""EventType"">
            
                    <Member Name=""SingleInstance"" Value=""0"" />
            
                    <Member Name=""Occurrence"" Value=""1"" />
            
                    <Member Name=""Exception"" Value=""2"" />
            
                    <Member Name=""SeriesMaster"" Value=""3"" />
            
                  </EnumType>
            
                  <EnumType Name=""FreeBusyStatus"">
            
                    <Member Name=""Free"" Value=""0"" />
            
                    <Member Name=""Tentative"" Value=""1"" />
            
                    <Member Name=""Busy"" Value=""2"" />
            
                    <Member Name=""Oof"" Value=""3"" />
            
                    <Member Name=""WorkingElsewhere"" Value=""4"" />
            
                    <Member Name=""Unknown"" Value=""-1"" />
            
                  </EnumType>
            
                  <EnumType Name=""MeetingMessageType"">
            
                    <Member Name=""None"" Value=""0"" />
            
                    <Member Name=""MeetingRequest"" Value=""1"" />
            
                    <Member Name=""MeetingCancelled"" Value=""2"" />
            
                    <Member Name=""MeetingAccepted"" Value=""3"" />
            
                    <Member Name=""MeetingTenativelyAccepted"" Value=""4"" />
            
                    <Member Name=""MeetingDeclined"" Value=""5"" />
            
                  </EnumType>
            
                  <EnumType Name=""RecurrencePatternType"">
            
                    <Member Name=""Daily"" Value=""0"" />
            
                    <Member Name=""Weekly"" Value=""1"" />
            
                    <Member Name=""AbsoluteMonthly"" Value=""2"" />
            
                    <Member Name=""RelativeMonthly"" Value=""3"" />
            
                    <Member Name=""AbsoluteYearly"" Value=""4"" />
            
                    <Member Name=""RelativeYearly"" Value=""5"" />
            
                  </EnumType>
            
                  <EnumType Name=""RecurrenceRangeType"">
            
                    <Member Name=""EndDate"" Value=""0"" />
            
                    <Member Name=""NoEnd"" Value=""1"" />
            
                    <Member Name=""Numbered"" Value=""2"" />
            
                  </EnumType>
            
                  <EnumType Name=""WeekIndex"">
            
                    <Member Name=""First"" Value=""0"" />
            
                    <Member Name=""Second"" Value=""1"" />
            
                    <Member Name=""Third"" Value=""2"" />
            
                    <Member Name=""Fourth"" Value=""3"" />
            
                    <Member Name=""Last"" Value=""4"" />
            
                  </EnumType>
            
                  <ComplexType Name=""EmailAddress"">
            
                    <Property Name=""Name"" Type=""Edm.String"" />
            
                    <Property Name=""Address"" Type=""Edm.String"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""Recipient"">
            
                    <Property Name=""EmailAddress"" Type=""Microsoft.OutlookServices.EmailAddress"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""Attendee"" BaseType=""Microsoft.OutlookServices.Recipient"">
            
                    <Property Name=""Status"" Type=""Microsoft.OutlookServices.ResponseStatus"" />
            
                    <Property Name=""Type"" Type=""Microsoft.OutlookServices.AttendeeType"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""ItemBody"">
            
                    <Property Name=""ContentType"" Type=""Microsoft.OutlookServices.BodyType"" />
            
                    <Property Name=""Content"" Type=""Edm.String"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""Location"">
            
                    <Property Name=""DisplayName"" Type=""Edm.String"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""ResponseStatus"">
            
                    <Property Name=""Response"" Type=""Microsoft.OutlookServices.ResponseType"" />
            
                    <Property Name=""Time"" Type=""Edm.DateTimeOffset"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""PhysicalAddress"">
            
                    <Property Name=""Street"" Type=""Edm.String"" />
            
                    <Property Name=""City"" Type=""Edm.String"" />
            
                    <Property Name=""State"" Type=""Edm.String"" />
            
                    <Property Name=""CountryOrRegion"" Type=""Edm.String"" />
            
                    <Property Name=""PostalCode"" Type=""Edm.String"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""RecurrencePattern"">
            
                    <Property Name=""Type"" Type=""Microsoft.OutlookServices.RecurrencePatternType"" />
            
                    <Property Name=""Interval"" Type=""Edm.Int32"" Nullable=""false"" />
            
                    <Property Name=""DayOfMonth"" Type=""Edm.Int32"" Nullable=""false"" />
            
                    <Property Name=""Month"" Type=""Edm.Int32"" Nullable=""false"" />
            
                    <Property Name=""DaysOfWeek"" Type=""Collection(Microsoft.OutlookServices.DayOfWeek)"" />
            
                    <Property Name=""FirstDayOfWeek"" Type=""Microsoft.OutlookServices.DayOfWeek"" />
            
                    <Property Name=""Index"" Type=""Microsoft.OutlookServices.WeekIndex"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""RecurrenceRange"">
            
                    <Property Name=""Type"" Type=""Microsoft.OutlookServices.RecurrenceRangeType"" />
            
                    <Property Name=""StartDate"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""EndDate"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""NumberOfOccurrences"" Type=""Edm.Int32"" Nullable=""false"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""PatternedRecurrence"">
            
                    <Property Name=""Pattern"" Type=""Microsoft.OutlookServices.RecurrencePattern"" />
            
                    <Property Name=""Range"" Type=""Microsoft.OutlookServices.RecurrenceRange"" />
            
                  </ComplexType>
            
                  <EntityType Name=""Entity"" Abstract=""true"">
            
                    <Key>
            
                      <PropertyRef Name=""Id"" />
            
                    </Key>
            
                    <Property Name=""Id"" Type=""Edm.String"" />
            
                  </EntityType>
            
                  <EntityType Name=""User"" BaseType=""Microsoft.OutlookServices.Entity"">
            
                    <Property Name=""DisplayName"" Type=""Edm.String"" />
            
                    <Property Name=""Alias"" Type=""Edm.String"" />
            
                    <Property Name=""MailboxGuid"" Type=""Edm.Guid"" />
            
                    <NavigationProperty Name=""Folders"" Type=""Collection(Microsoft.OutlookServices.Folder)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""Messages"" Type=""Collection(Microsoft.OutlookServices.Message)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""RootFolder"" Type=""Microsoft.OutlookServices.Folder"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""Calendars"" Type=""Collection(Microsoft.OutlookServices.Calendar)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""Calendar"" Type=""Microsoft.OutlookServices.Calendar"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""CalendarGroups"" Type=""Collection(Microsoft.OutlookServices.CalendarGroup)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""Events"" Type=""Collection(Microsoft.OutlookServices.Event)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""CalendarView"" Type=""Collection(Microsoft.OutlookServices.Event)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""Contacts"" Type=""Collection(Microsoft.OutlookServices.Contact)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""ContactFolders"" Type=""Collection(Microsoft.OutlookServices.ContactFolder)"" ContainsTarget=""true"" />
            
                  </EntityType>
            
                  <Action Name=""SendMail"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.User"" />
            
                    <Parameter Name=""Message"" Type=""Microsoft.OutlookServices.Message"" Nullable=""false"" />
            
                    <Parameter Name=""SaveToSentItems"" Type=""Edm.Boolean"" />
            
                  </Action>
            
                  <EntityType Name=""Folder"" BaseType=""Microsoft.OutlookServices.Entity"">
            
                    <Property Name=""ParentFolderId"" Type=""Edm.String"" />
            
                    <Property Name=""DisplayName"" Type=""Edm.String"" />
            
                    <Property Name=""ChildFolderCount"" Type=""Edm.Int32"" />
            
                    <NavigationProperty Name=""ChildFolders"" Type=""Collection(Microsoft.OutlookServices.Folder)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""Messages"" Type=""Collection(Microsoft.OutlookServices.Message)"" ContainsTarget=""true"" />
            
                  </EntityType>
            
                  <Action Name=""Copy"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Folder"" />
            
                    <Parameter Name=""DestinationId"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Folder"" />
            
                  </Action>
            
                  <Action Name=""Move"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Folder"" />
            
                    <Parameter Name=""DestinationId"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Folder"" />
            
                  </Action>
            
                  <EntityType Name=""Item"" BaseType=""Microsoft.OutlookServices.Entity"" Abstract=""true"">
            
                    <Property Name=""ChangeKey"" Type=""Edm.String"" />
            
                    <Property Name=""Categories"" Type=""Collection(Edm.String)"" />
            
                    <Property Name=""DateTimeCreated"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""DateTimeLastModified"" Type=""Edm.DateTimeOffset"" />
            
                  </EntityType>
            
                  <EntityType Name=""Message"" BaseType=""Microsoft.OutlookServices.Item"">
            
                    <Property Name=""Subject"" Type=""Edm.String"" />
            
                    <Property Name=""Body"" Type=""Microsoft.OutlookServices.ItemBody"" />
            
                    <Property Name=""BodyPreview"" Type=""Edm.String"" />
            
                    <Property Name=""Importance"" Type=""Microsoft.OutlookServices.Importance"" />
            
                    <Property Name=""HasAttachments"" Type=""Edm.Boolean"" />
            
                    <Property Name=""ParentFolderId"" Type=""Edm.String"" />
            
                    <Property Name=""From"" Type=""Microsoft.OutlookServices.Recipient"" />
            
                    <Property Name=""Sender"" Type=""Microsoft.OutlookServices.Recipient"" />
            
                    <Property Name=""ToRecipients"" Type=""Collection(Microsoft.OutlookServices.Recipient)"" />
            
                    <Property Name=""CcRecipients"" Type=""Collection(Microsoft.OutlookServices.Recipient)"" />
            
                    <Property Name=""BccRecipients"" Type=""Collection(Microsoft.OutlookServices.Recipient)"" />
            
                    <Property Name=""ReplyTo"" Type=""Collection(Microsoft.OutlookServices.Recipient)"" />
            
                    <Property Name=""ConversationId"" Type=""Edm.String"" />
            
                    <Property Name=""UniqueBody"" Type=""Microsoft.OutlookServices.ItemBody"" />
            
                    <Property Name=""DateTimeReceived"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""DateTimeSent"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""IsDeliveryReceiptRequested"" Type=""Edm.Boolean"" />
            
                    <Property Name=""IsReadReceiptRequested"" Type=""Edm.Boolean"" />
            
                    <Property Name=""IsDraft"" Type=""Edm.Boolean"" />
            
                    <Property Name=""IsRead"" Type=""Edm.Boolean"" />
            
                    <NavigationProperty Name=""Attachments"" Type=""Collection(Microsoft.OutlookServices.Attachment)"" ContainsTarget=""true"" />
            
                  </EntityType>
            
                  <Action Name=""Copy"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""DestinationId"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <Action Name=""Move"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""DestinationId"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <Action Name=""CreateReply"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <Action Name=""CreateReplyAll"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <Action Name=""CreateForward"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <Action Name=""Reply"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                  </Action>
            
                  <Action Name=""ReplyAll"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                  </Action>
            
                  <Action Name=""Forward"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                    <Parameter Name=""ToRecipients"" Type=""Collection(Microsoft.OutlookServices.Recipient)"" />
            
                  </Action>
            
                  <Action Name=""Send"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <EntityType Name=""Attachment"" BaseType=""Microsoft.OutlookServices.Entity"" Abstract=""true"">
            
                    <Property Name=""Name"" Type=""Edm.String"" />
            
                    <Property Name=""ContentType"" Type=""Edm.String"" />
            
                    <Property Name=""Size"" Type=""Edm.Int32"" Nullable=""false"" />
            
                    <Property Name=""IsInline"" Type=""Edm.Boolean"" Nullable=""false"" />
            
                    <Property Name=""DateTimeLastModified"" Type=""Edm.DateTimeOffset"" />
            
                  </EntityType>
            
                  <EntityType Name=""FileAttachment"" BaseType=""Microsoft.OutlookServices.Attachment"">
            
                    <Property Name=""ContentId"" Type=""Edm.String"" />
            
                    <Property Name=""ContentLocation"" Type=""Edm.String"" />
            
                    <Property Name=""IsContactPhoto"" Type=""Edm.Boolean"" Nullable=""false"" />
            
                    <Property Name=""ContentBytes"" Type=""Edm.Binary"" />
            
                  </EntityType>
            
                  <EntityType Name=""ItemAttachment"" BaseType=""Microsoft.OutlookServices.Attachment"">
            
                    <NavigationProperty Name=""Item"" Type=""Microsoft.OutlookServices.Item"" ContainsTarget=""true"" />
            
                  </EntityType>
            
                  <EntityType Name=""Calendar"" BaseType=""Microsoft.OutlookServices.Entity"">
            
                    <Property Name=""Name"" Type=""Edm.String"" />
            
                    <Property Name=""ChangeKey"" Type=""Edm.String"" />
            
                    <NavigationProperty Name=""CalendarView"" Type=""Collection(Microsoft.OutlookServices.Event)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""Events"" Type=""Collection(Microsoft.OutlookServices.Event)"" ContainsTarget=""true"" />
            
                  </EntityType>
            
                  <EntityType Name=""CalendarGroup"" BaseType=""Microsoft.OutlookServices.Entity"">
            
                    <Property Name=""Name"" Type=""Edm.String"" />
            
                    <Property Name=""ChangeKey"" Type=""Edm.String"" />
            
                    <Property Name=""ClassId"" Type=""Edm.Guid"" />
            
                    <NavigationProperty Name=""Calendars"" Type=""Collection(Microsoft.OutlookServices.Calendar)"" ContainsTarget=""true"" />
            
                  </EntityType>
            
                  <EntityType Name=""Event"" BaseType=""Microsoft.OutlookServices.Item"">
            
                    <Property Name=""Subject"" Type=""Edm.String"" />
            
                    <Property Name=""Body"" Type=""Microsoft.OutlookServices.ItemBody"" />
            
                    <Property Name=""BodyPreview"" Type=""Edm.String"" />
            
                    <Property Name=""Importance"" Type=""Microsoft.OutlookServices.Importance"" />
            
                    <Property Name=""HasAttachments"" Type=""Edm.Boolean"" />
            
                    <Property Name=""Start"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""End"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""Location"" Type=""Microsoft.OutlookServices.Location"" />
            
                    <Property Name=""ShowAs"" Type=""Microsoft.OutlookServices.FreeBusyStatus"" />
            
                    <Property Name=""IsAllDay"" Type=""Edm.Boolean"" />
            
                    <Property Name=""IsCancelled"" Type=""Edm.Boolean"" />
            
                    <Property Name=""IsOrganizer"" Type=""Edm.Boolean"" />
            
                    <Property Name=""ResponseRequested"" Type=""Edm.Boolean"" />
            
                    <Property Name=""Type"" Type=""Microsoft.OutlookServices.EventType"" />
            
                    <Property Name=""SeriesMasterId"" Type=""Edm.String"" />
            
                    <Property Name=""Attendees"" Type=""Collection(Microsoft.OutlookServices.Attendee)"" />
            
                    <Property Name=""Recurrence"" Type=""Microsoft.OutlookServices.PatternedRecurrence"" />
            
                    <Property Name=""Organizer"" Type=""Microsoft.OutlookServices.Recipient"" />
            
                    <NavigationProperty Name=""Attachments"" Type=""Collection(Microsoft.OutlookServices.Attachment)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""Calendar"" Type=""Microsoft.OutlookServices.Calendar"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""Instances"" Type=""Collection(Microsoft.OutlookServices.Event)"" ContainsTarget=""true"" />
            
                  </EntityType>
            
                  <Action Name=""Accept"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Event"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                  </Action>
            
                  <Action Name=""Decline"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Event"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                  </Action>
            
                  <Action Name=""TentativelyAccept"" IsBound=""true"" EntitySetPath=""bindingParameter"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Event"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                  </Action>
            
                  <EntityType Name=""Contact"" BaseType=""Microsoft.OutlookServices.Item"">
            
                    <Property Name=""ParentFolderId"" Type=""Edm.String"" />
            
                    <Property Name=""Birthday"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""FileAs"" Type=""Edm.String"" />
            
                    <Property Name=""DisplayName"" Type=""Edm.String"" />
            
                    <Property Name=""GivenName"" Type=""Edm.String"" />
            
                    <Property Name=""Initials"" Type=""Edm.String"" />
            
                    <Property Name=""MiddleName"" Type=""Edm.String"" />
            
                    <Property Name=""NickName"" Type=""Edm.String"" />
            
                    <Property Name=""Surname"" Type=""Edm.String"" />
            
                    <Property Name=""Title"" Type=""Edm.String"" />
            
                    <Property Name=""Generation"" Type=""Edm.String"" />
            
                    <Property Name=""EmailAddresses"" Type=""Collection(Microsoft.OutlookServices.EmailAddress)"" />
            
                    <Property Name=""ImAddresses"" Type=""Collection(Edm.String)"" />
            
                    <Property Name=""JobTitle"" Type=""Edm.String"" />
            
                    <Property Name=""CompanyName"" Type=""Edm.String"" />
            
                    <Property Name=""Department"" Type=""Edm.String"" />
            
                    <Property Name=""OfficeLocation"" Type=""Edm.String"" />
            
                    <Property Name=""Profession"" Type=""Edm.String"" />
            
                    <Property Name=""BusinessHomePage"" Type=""Edm.String"" />
            
                    <Property Name=""AssistantName"" Type=""Edm.String"" />
            
                    <Property Name=""Manager"" Type=""Edm.String"" />
            
                    <Property Name=""HomePhones"" Type=""Collection(Edm.String)"" />
            
                    <Property Name=""BusinessPhones"" Type=""Collection(Edm.String)"" />
            
                    <Property Name=""MobilePhone1"" Type=""Edm.String"" />
            
                    <Property Name=""HomeAddress"" Type=""Microsoft.OutlookServices.PhysicalAddress"" />
            
                    <Property Name=""BusinessAddress"" Type=""Microsoft.OutlookServices.PhysicalAddress"" />
            
                    <Property Name=""OtherAddress"" Type=""Microsoft.OutlookServices.PhysicalAddress"" />
            
                    <Property Name=""YomiCompanyName"" Type=""Edm.String"" />
            
                    <Property Name=""YomiGivenName"" Type=""Edm.String"" />
            
                    <Property Name=""YomiSurname"" Type=""Edm.String"" />
            
                  </EntityType>
            
                  <EntityType Name=""ContactFolder"" BaseType=""Microsoft.OutlookServices.Entity"">
            
                    <Property Name=""ParentFolderId"" Type=""Edm.String"" />
            
                    <Property Name=""DisplayName"" Type=""Edm.String"" />
            
                    <NavigationProperty Name=""Contacts"" Type=""Collection(Microsoft.OutlookServices.Contact)"" ContainsTarget=""true"" />
            
                    <NavigationProperty Name=""ChildFolders"" Type=""Collection(Microsoft.OutlookServices.ContactFolder)"" ContainsTarget=""true"" />
            
                  </EntityType>
            
                  <EntityContainer Name=""EntityContainer"">
            
                    <EntitySet Name=""Users"" EntityType=""Microsoft.OutlookServices.User"" />
            
                    <Singleton Name=""Me"" Type=""Microsoft.OutlookServices.User"" />
            
                  </EntityContainer>
            
                </Schema>
            
              </edmx:DataServices>
            
            </edmx:Edmx>";

            public static global::Microsoft.OData.Edm.IEdmModel GetInstance()

            {

                return ParsedModel;

            }

            private static global::Microsoft.OData.Edm.IEdmModel LoadModelFromString()

            {

                global::System.Xml.XmlReader reader = CreateXmlReader(Edmx);

                try

                {

                    return global::Microsoft.OData.Edm.Csdl.EdmxReader.Parse(reader);

                }

                finally

                {

                    ((global::System.IDisposable)(reader)).Dispose();

                }

            }

            private static global::System.Xml.XmlReader CreateXmlReader(System.String edmxToParse)

            {

                return global::System.Xml.XmlReader.Create(new global::System.IO.StringReader(edmxToParse));

            }

        }

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IEntity:Microsoft.OData.ProxyExtensions.IEntityBase

    {

        System.String Id
        {get;set;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IEntityFetcher

    {

        Microsoft.OutlookServices.IUserFetcher ToUser();

        Microsoft.OutlookServices.IFolderFetcher ToFolder();

        Microsoft.OutlookServices.IItemFetcher ToItem();

        Microsoft.OutlookServices.IAttachmentFetcher ToAttachment();

        Microsoft.OutlookServices.ICalendarFetcher ToCalendar();

        Microsoft.OutlookServices.ICalendarGroupFetcher ToCalendarGroup();

        Microsoft.OutlookServices.IContactFolderFetcher ToContactFolder();

        Microsoft.OutlookServices.IMessageFetcher ToMessage();

        Microsoft.OutlookServices.IEventFetcher ToEvent();

        Microsoft.OutlookServices.IContactFetcher ToContact();

        Microsoft.OutlookServices.IFileAttachmentFetcher ToFileAttachment();

        Microsoft.OutlookServices.IItemAttachmentFetcher ToItemAttachment();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IEntityCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IEntity>

    {

        Microsoft.OutlookServices.IEntityFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEntity>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddEntityAsync(Microsoft.OutlookServices.IEntity item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IEntityFetcher this[System.String id]

        {get;}

    }

    public partial interface IEntityCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IUser:Microsoft.OutlookServices.IEntity

    {

        System.String DisplayName
        {get;set;}

        System.String Alias
        {get;set;}

        System.Nullable<System.Guid> MailboxGuid
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IFolder> Folders
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IMessage> Messages
        {get;}

        Microsoft.OutlookServices.IFolder RootFolder
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.ICalendar> Calendars
        {get;}

        Microsoft.OutlookServices.ICalendar Calendar
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.ICalendarGroup> CalendarGroups
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent> Events
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent> CalendarView
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContact> Contacts
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContactFolder> ContactFolders
        {get;}

        System.Threading.Tasks.Task SendMailAsync(Microsoft.OutlookServices.IMessage Message, System.Nullable<System.Boolean> SaveToSentItems);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IUserFetcher:Microsoft.OutlookServices.IEntityFetcher

    {

        Microsoft.OutlookServices.IFolderCollection Folders
        {get;}

        Microsoft.OutlookServices.IMessageCollection Messages
        {get;}

        Microsoft.OutlookServices.IFolderFetcher RootFolder
        {get;}

        Microsoft.OutlookServices.ICalendarCollection Calendars
        {get;}

        Microsoft.OutlookServices.ICalendarFetcher Calendar
        {get;}

        Microsoft.OutlookServices.ICalendarGroupCollection CalendarGroups
        {get;}

        Microsoft.OutlookServices.IEventCollection Events
        {get;}

        Microsoft.OutlookServices.IEventCollection CalendarView
        {get;}

        Microsoft.OutlookServices.IContactCollection Contacts
        {get;}

        Microsoft.OutlookServices.IContactFolderCollection ContactFolders
        {get;}

        System.Threading.Tasks.Task SendMailAsync(Microsoft.OutlookServices.IMessage Message, System.Nullable<System.Boolean> SaveToSentItems);

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IUser> ExecuteAsync();

        Microsoft.OutlookServices.IUserFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IUser, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IUserCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IUser>

    {

        Microsoft.OutlookServices.IUserFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IUser>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddUserAsync(Microsoft.OutlookServices.IUser item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IUserFetcher this[System.String id]

        {get;}

    }

    public partial interface IUserCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFolder:Microsoft.OutlookServices.IEntity

    {

        System.String ParentFolderId
        {get;set;}

        System.String DisplayName
        {get;set;}

        System.Nullable<System.Int32> ChildFolderCount
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IFolder> ChildFolders
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IMessage> Messages
        {get;}

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> CopyAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> MoveAsync(System.String DestinationId);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFolderFetcher:Microsoft.OutlookServices.IEntityFetcher

    {

        Microsoft.OutlookServices.IFolderCollection ChildFolders
        {get;}

        Microsoft.OutlookServices.IMessageCollection Messages
        {get;}

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> CopyAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> MoveAsync(System.String DestinationId);

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IFolder> ExecuteAsync();

        Microsoft.OutlookServices.IFolderFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IFolder, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFolderCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IFolder>

    {

        Microsoft.OutlookServices.IFolderFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IFolder>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddFolderAsync(Microsoft.OutlookServices.IFolder item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IFolderFetcher this[System.String id]

        {get;}

    }

    public partial interface IFolderCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItem:Microsoft.OutlookServices.IEntity

    {

        System.String ChangeKey
        {get;set;}

        System.Collections.Generic.IList<System.String> Categories
        {get;set;}

        System.Nullable<System.DateTimeOffset> DateTimeCreated
        {get;set;}

        System.Nullable<System.DateTimeOffset> DateTimeLastModified
        {get;set;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemFetcher:Microsoft.OutlookServices.IEntityFetcher

    {

        Microsoft.OutlookServices.IMessageFetcher ToMessage();

        Microsoft.OutlookServices.IEventFetcher ToEvent();

        Microsoft.OutlookServices.IContactFetcher ToContact();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IItem>

    {

        Microsoft.OutlookServices.IItemFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IItem>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddItemAsync(Microsoft.OutlookServices.IItem item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IItemFetcher this[System.String id]

        {get;}

    }

    public partial interface IItemCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IMessage:Microsoft.OutlookServices.IItem

    {

        System.String Subject
        {get;set;}

        Microsoft.OutlookServices.ItemBody Body
        {get;set;}

        System.String BodyPreview
        {get;set;}

        Microsoft.OutlookServices.Importance Importance
        {get;set;}

        System.Nullable<System.Boolean> HasAttachments
        {get;set;}

        System.String ParentFolderId
        {get;set;}

        Microsoft.OutlookServices.Recipient From
        {get;set;}

        Microsoft.OutlookServices.Recipient Sender
        {get;set;}

        System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient> ToRecipients
        {get;set;}

        System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient> CcRecipients
        {get;set;}

        System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient> BccRecipients
        {get;set;}

        System.Collections.Generic.IList<Microsoft.OutlookServices.Recipient> ReplyTo
        {get;set;}

        System.String ConversationId
        {get;set;}

        Microsoft.OutlookServices.ItemBody UniqueBody
        {get;set;}

        System.Nullable<System.DateTimeOffset> DateTimeReceived
        {get;set;}

        System.Nullable<System.DateTimeOffset> DateTimeSent
        {get;set;}

        System.Nullable<System.Boolean> IsDeliveryReceiptRequested
        {get;set;}

        System.Nullable<System.Boolean> IsReadReceiptRequested
        {get;set;}

        System.Nullable<System.Boolean> IsDraft
        {get;set;}

        System.Nullable<System.Boolean> IsRead
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IAttachment> Attachments
        {get;}

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CopyAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> MoveAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateReplyAsync();

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateReplyAllAsync();

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateForwardAsync();

        System.Threading.Tasks.Task ReplyAsync(System.String Comment);

        System.Threading.Tasks.Task ReplyAllAsync(System.String Comment);

        System.Threading.Tasks.Task ForwardAsync(System.String Comment, System.Collections.Generic.ICollection<Microsoft.OutlookServices.Recipient> ToRecipients);

        System.Threading.Tasks.Task SendAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IMessageFetcher:Microsoft.OutlookServices.IItemFetcher

    {

        Microsoft.OutlookServices.IAttachmentCollection Attachments
        {get;}

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CopyAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> MoveAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateReplyAsync();

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateReplyAllAsync();

        System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> CreateForwardAsync();

        System.Threading.Tasks.Task ReplyAsync(System.String Comment);

        System.Threading.Tasks.Task ReplyAllAsync(System.String Comment);

        System.Threading.Tasks.Task ForwardAsync(System.String Comment, System.Collections.Generic.ICollection<Microsoft.OutlookServices.Recipient> ToRecipients);

        System.Threading.Tasks.Task SendAsync();

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IMessage> ExecuteAsync();

        Microsoft.OutlookServices.IMessageFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IMessage, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IMessageCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IMessage>

    {

        Microsoft.OutlookServices.IMessageFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IMessage>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddMessageAsync(Microsoft.OutlookServices.IMessage item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IMessageFetcher this[System.String id]

        {get;}

    }

    public partial interface IMessageCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IAttachment:Microsoft.OutlookServices.IEntity

    {

        System.String Name
        {get;set;}

        System.String ContentType
        {get;set;}

        System.Int32 Size
        {get;set;}

        System.Boolean IsInline
        {get;set;}

        System.Nullable<System.DateTimeOffset> DateTimeLastModified
        {get;set;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IAttachmentFetcher:Microsoft.OutlookServices.IEntityFetcher

    {

        Microsoft.OutlookServices.IFileAttachmentFetcher ToFileAttachment();

        Microsoft.OutlookServices.IItemAttachmentFetcher ToItemAttachment();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IAttachmentCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IAttachment>

    {

        Microsoft.OutlookServices.IAttachmentFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IAttachment>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddAttachmentAsync(Microsoft.OutlookServices.IAttachment item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IAttachmentFetcher this[System.String id]

        {get;}

    }

    public partial interface IAttachmentCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFileAttachment:Microsoft.OutlookServices.IAttachment

    {

        System.String ContentId
        {get;set;}

        System.String ContentLocation
        {get;set;}

        System.Boolean IsContactPhoto
        {get;set;}

        System.Byte[] ContentBytes
        {get;set;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFileAttachmentFetcher:Microsoft.OutlookServices.IAttachmentFetcher

    {

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IFileAttachment> ExecuteAsync();

        Microsoft.OutlookServices.IFileAttachmentFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IFileAttachment, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFileAttachmentCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IFileAttachment>

    {

        Microsoft.OutlookServices.IFileAttachmentFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IFileAttachment>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddFileAttachmentAsync(Microsoft.OutlookServices.IFileAttachment item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IFileAttachmentFetcher this[System.String id]

        {get;}

    }

    public partial interface IFileAttachmentCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemAttachment:Microsoft.OutlookServices.IAttachment

    {

        Microsoft.OutlookServices.IItem Item
        {get;set;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemAttachmentFetcher:Microsoft.OutlookServices.IAttachmentFetcher

    {

        Microsoft.OutlookServices.IItemFetcher Item
        {get;}

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IItemAttachment> ExecuteAsync();

        Microsoft.OutlookServices.IItemAttachmentFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IItemAttachment, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemAttachmentCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IItemAttachment>

    {

        Microsoft.OutlookServices.IItemAttachmentFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IItemAttachment>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddItemAttachmentAsync(Microsoft.OutlookServices.IItemAttachment item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IItemAttachmentFetcher this[System.String id]

        {get;}

    }

    public partial interface IItemAttachmentCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendar:Microsoft.OutlookServices.IEntity

    {

        System.String Name
        {get;set;}

        System.String ChangeKey
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent> CalendarView
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent> Events
        {get;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendarFetcher:Microsoft.OutlookServices.IEntityFetcher

    {

        Microsoft.OutlookServices.IEventCollection CalendarView
        {get;}

        Microsoft.OutlookServices.IEventCollection Events
        {get;}

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.ICalendar> ExecuteAsync();

        Microsoft.OutlookServices.ICalendarFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.ICalendar, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendarCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.ICalendar>

    {

        Microsoft.OutlookServices.ICalendarFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.ICalendar>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddCalendarAsync(Microsoft.OutlookServices.ICalendar item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.ICalendarFetcher this[System.String id]

        {get;}

    }

    public partial interface ICalendarCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendarGroup:Microsoft.OutlookServices.IEntity

    {

        System.String Name
        {get;set;}

        System.String ChangeKey
        {get;set;}

        System.Nullable<System.Guid> ClassId
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.ICalendar> Calendars
        {get;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendarGroupFetcher:Microsoft.OutlookServices.IEntityFetcher

    {

        Microsoft.OutlookServices.ICalendarCollection Calendars
        {get;}

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.ICalendarGroup> ExecuteAsync();

        Microsoft.OutlookServices.ICalendarGroupFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.ICalendarGroup, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendarGroupCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.ICalendarGroup>

    {

        Microsoft.OutlookServices.ICalendarGroupFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.ICalendarGroup>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddCalendarGroupAsync(Microsoft.OutlookServices.ICalendarGroup item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.ICalendarGroupFetcher this[System.String id]

        {get;}

    }

    public partial interface ICalendarGroupCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IEvent:Microsoft.OutlookServices.IItem

    {

        System.String Subject
        {get;set;}

        Microsoft.OutlookServices.ItemBody Body
        {get;set;}

        System.String BodyPreview
        {get;set;}

        Microsoft.OutlookServices.Importance Importance
        {get;set;}

        System.Nullable<System.Boolean> HasAttachments
        {get;set;}

        System.Nullable<System.DateTimeOffset> Start
        {get;set;}

        System.Nullable<System.DateTimeOffset> End
        {get;set;}

        Microsoft.OutlookServices.Location Location
        {get;set;}

        Microsoft.OutlookServices.FreeBusyStatus ShowAs
        {get;set;}

        System.Nullable<System.Boolean> IsAllDay
        {get;set;}

        System.Nullable<System.Boolean> IsCancelled
        {get;set;}

        System.Nullable<System.Boolean> IsOrganizer
        {get;set;}

        System.Nullable<System.Boolean> ResponseRequested
        {get;set;}

        Microsoft.OutlookServices.EventType Type
        {get;set;}

        System.String SeriesMasterId
        {get;set;}

        System.Collections.Generic.IList<Microsoft.OutlookServices.Attendee> Attendees
        {get;set;}

        Microsoft.OutlookServices.PatternedRecurrence Recurrence
        {get;set;}

        Microsoft.OutlookServices.Recipient Organizer
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IAttachment> Attachments
        {get;}

        Microsoft.OutlookServices.ICalendar Calendar
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent> Instances
        {get;}

        System.Threading.Tasks.Task AcceptAsync(System.String Comment);

        System.Threading.Tasks.Task DeclineAsync(System.String Comment);

        System.Threading.Tasks.Task TentativelyAcceptAsync(System.String Comment);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IEventFetcher:Microsoft.OutlookServices.IItemFetcher

    {

        Microsoft.OutlookServices.IAttachmentCollection Attachments
        {get;}

        Microsoft.OutlookServices.ICalendarFetcher Calendar
        {get;}

        Microsoft.OutlookServices.IEventCollection Instances
        {get;}

        System.Threading.Tasks.Task AcceptAsync(System.String Comment);

        System.Threading.Tasks.Task DeclineAsync(System.String Comment);

        System.Threading.Tasks.Task TentativelyAcceptAsync(System.String Comment);

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IEvent> ExecuteAsync();

        Microsoft.OutlookServices.IEventFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IEvent, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IEventCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IEvent>

    {

        Microsoft.OutlookServices.IEventFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IEvent>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddEventAsync(Microsoft.OutlookServices.IEvent item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IEventFetcher this[System.String id]

        {get;}

    }

    public partial interface IEventCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContact:Microsoft.OutlookServices.IItem

    {

        System.String ParentFolderId
        {get;set;}

        System.Nullable<System.DateTimeOffset> Birthday
        {get;set;}

        System.String FileAs
        {get;set;}

        System.String DisplayName
        {get;set;}

        System.String GivenName
        {get;set;}

        System.String Initials
        {get;set;}

        System.String MiddleName
        {get;set;}

        System.String NickName
        {get;set;}

        System.String Surname
        {get;set;}

        System.String Title
        {get;set;}

        System.String Generation
        {get;set;}

        System.Collections.Generic.IList<Microsoft.OutlookServices.EmailAddress> EmailAddresses
        {get;set;}

        System.Collections.Generic.IList<System.String> ImAddresses
        {get;set;}

        System.String JobTitle
        {get;set;}

        System.String CompanyName
        {get;set;}

        System.String Department
        {get;set;}

        System.String OfficeLocation
        {get;set;}

        System.String Profession
        {get;set;}

        System.String BusinessHomePage
        {get;set;}

        System.String AssistantName
        {get;set;}

        System.String Manager
        {get;set;}

        System.Collections.Generic.IList<System.String> HomePhones
        {get;set;}

        System.Collections.Generic.IList<System.String> BusinessPhones
        {get;set;}

        System.String MobilePhone1
        {get;set;}

        Microsoft.OutlookServices.PhysicalAddress HomeAddress
        {get;set;}

        Microsoft.OutlookServices.PhysicalAddress BusinessAddress
        {get;set;}

        Microsoft.OutlookServices.PhysicalAddress OtherAddress
        {get;set;}

        System.String YomiCompanyName
        {get;set;}

        System.String YomiGivenName
        {get;set;}

        System.String YomiSurname
        {get;set;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContactFetcher:Microsoft.OutlookServices.IItemFetcher

    {

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IContact> ExecuteAsync();

        Microsoft.OutlookServices.IContactFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IContact, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContactCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IContact>

    {

        Microsoft.OutlookServices.IContactFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContact>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddContactAsync(Microsoft.OutlookServices.IContact item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IContactFetcher this[System.String id]

        {get;}

    }

    public partial interface IContactCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContactFolder:Microsoft.OutlookServices.IEntity

    {

        System.String ParentFolderId
        {get;set;}

        System.String DisplayName
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContact> Contacts
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContactFolder> ChildFolders
        {get;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContactFolderFetcher:Microsoft.OutlookServices.IEntityFetcher

    {

        Microsoft.OutlookServices.IContactCollection Contacts
        {get;}

        Microsoft.OutlookServices.IContactFolderCollection ChildFolders
        {get;}

        global::System.Threading.Tasks.Task<Microsoft.OutlookServices.IContactFolder> ExecuteAsync();

        Microsoft.OutlookServices.IContactFolderFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.OutlookServices.IContactFolder, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContactFolderCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.OutlookServices.IContactFolder>

    {

        Microsoft.OutlookServices.IContactFolderFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.OutlookServices.IContactFolder>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddContactFolderAsync(Microsoft.OutlookServices.IContactFolder item, System.Boolean dontSave = false);

         Microsoft.OutlookServices.IContactFolderFetcher this[System.String id]

        {get;}

    }

    public partial interface IContactFolderCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    public partial interface IEntityContainer

    {

        Microsoft.OutlookServices.IUserCollection Users
        {get;}

        Microsoft.OutlookServices.IUserFetcher Me
        {get;}

    }

}

