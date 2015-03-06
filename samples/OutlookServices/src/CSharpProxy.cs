namespace Microsoft.Office365.OutlookServices

{

    using global::Microsoft.OData.Client;

    using global::Microsoft.OData.Edm;

    using System;

    using System.Collections.Generic;

    using System.ComponentModel;

    using System.Linq;

    using System.Reflection;

    using System.Threading.Tasks;

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

    

    public partial class ItemBody:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.Office365.OutlookServices.BodyType _ContentType;

        private System.String _Content;

        public Microsoft.Office365.OutlookServices.BodyType ContentType

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

    public partial class Recipient:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.Office365.OutlookServices.EmailAddress _EmailAddress;

        public Microsoft.Office365.OutlookServices.EmailAddress EmailAddress

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

    public partial class Attendee:Microsoft.Office365.OutlookServices.Recipient

    {

        private Microsoft.Office365.OutlookServices.ResponseStatus _Status;

        private Microsoft.Office365.OutlookServices.AttendeeType _Type;

        public Microsoft.Office365.OutlookServices.ResponseStatus Status

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

        public Microsoft.Office365.OutlookServices.AttendeeType Type

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

    public partial class ResponseStatus:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.Office365.OutlookServices.ResponseType _Response;

        private System.Nullable<System.DateTimeOffset> _Time;

        public Microsoft.Office365.OutlookServices.ResponseType Response

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

    public partial class PatternedRecurrence:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.Office365.OutlookServices.RecurrencePattern _Pattern;

        private Microsoft.Office365.OutlookServices.RecurrenceRange _Range;

        public Microsoft.Office365.OutlookServices.RecurrencePattern Pattern

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

        public Microsoft.Office365.OutlookServices.RecurrenceRange Range

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

    public partial class RecurrencePattern:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.Office365.OutlookServices.RecurrencePatternType _Type;

        private System.Int32 _Interval;

        private System.Int32 _Month;

        private System.Int32 _DayOfMonth;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.DayOfWeek> _DaysOfWeek;

        private Microsoft.Office365.OutlookServices.DayOfWeek _FirstDayOfWeek;

        private Microsoft.Office365.OutlookServices.WeekIndex _Index;

        public Microsoft.Office365.OutlookServices.RecurrencePatternType Type

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

        public System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.DayOfWeek> DaysOfWeek

        {

            get

            {

                if (this._DaysOfWeek == default(System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.DayOfWeek>))

                {

                    this._DaysOfWeek = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.DayOfWeek>();

                    this._DaysOfWeek.SetContainer(() => GetContainingEntity("DaysOfWeek"));

                }

                return this._DaysOfWeek;

            }

            set

            {

                DaysOfWeek.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        DaysOfWeek.Add(i);

                    }

                }

            }

        }

        public Microsoft.Office365.OutlookServices.DayOfWeek FirstDayOfWeek

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

        public Microsoft.Office365.OutlookServices.WeekIndex Index

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

        private Microsoft.Office365.OutlookServices.RecurrenceRangeType _Type;

        private System.Nullable<System.DateTimeOffset> _StartDate;

        private System.Nullable<System.DateTimeOffset> _EndDate;

        private System.Int32 _NumberOfOccurrences;

        public Microsoft.Office365.OutlookServices.RecurrenceRangeType Type

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

    [global::Microsoft.OData.Client.Key("Id")]

    public abstract partial class Entity:Microsoft.OData.ProxyExtensions.EntityBase, Microsoft.Office365.OutlookServices.IEntity, Microsoft.Office365.OutlookServices.IEntityFetcher

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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IEntity> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Entity, Microsoft.Office365.OutlookServices.IEntity>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IEntity> _query;

    }

    internal partial class EntityFetcher:Microsoft.OData.ProxyExtensions.RestShallowObjectFetcher, Microsoft.Office365.OutlookServices.IEntityFetcher

    {

        public EntityFetcher(): base()

        {

        }

    }

    internal partial class EntityCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IEntity>, Microsoft.Office365.OutlookServices.IEntityCollection

    {

        internal EntityCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IEntityFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEntity>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddEntityAsync(Microsoft.Office365.OutlookServices.IEntity item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IEntityFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.Entity>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.EntityFetcher();

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

    public abstract partial class Attachment:Microsoft.Office365.OutlookServices.Entity, Microsoft.Office365.OutlookServices.IAttachment, Microsoft.Office365.OutlookServices.IAttachmentFetcher

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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IAttachment> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Attachment, Microsoft.Office365.OutlookServices.IAttachment>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IAttachment> _query;

    }

    internal partial class AttachmentFetcher:Microsoft.Office365.OutlookServices.EntityFetcher, Microsoft.Office365.OutlookServices.IAttachmentFetcher

    {

        public AttachmentFetcher()

        {

        }

    }

    internal partial class AttachmentCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IAttachment>, Microsoft.Office365.OutlookServices.IAttachmentCollection

    {

        internal AttachmentCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IAttachmentFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IAttachment>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddAttachmentAsync(Microsoft.Office365.OutlookServices.IAttachment item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IAttachmentFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.Attachment>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.AttachmentFetcher();

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

    public abstract partial class Item:Microsoft.Office365.OutlookServices.Entity, Microsoft.Office365.OutlookServices.IItem, Microsoft.Office365.OutlookServices.IItemFetcher

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

                Categories.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Categories.Add(i);

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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IItem> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Item, Microsoft.Office365.OutlookServices.IItem>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IItem> _query;

    }

    internal partial class ItemFetcher:Microsoft.Office365.OutlookServices.EntityFetcher, Microsoft.Office365.OutlookServices.IItemFetcher

    {

        public ItemFetcher()

        {

        }

    }

    internal partial class ItemCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IItem>, Microsoft.Office365.OutlookServices.IItemCollection

    {

        internal ItemCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IItemFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IItem>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddItemAsync(Microsoft.Office365.OutlookServices.IItem item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IItemFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.Item>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.ItemFetcher();

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

    public partial class User:Microsoft.Office365.OutlookServices.Entity, Microsoft.Office365.OutlookServices.IUser, Microsoft.Office365.OutlookServices.IUserFetcher

    {

        private Microsoft.Office365.OutlookServices.Folder _RootFolder;

        private Microsoft.Office365.OutlookServices.Calendar _Calendar;

        private Microsoft.Office365.OutlookServices.FolderFetcher _RootFolderFetcher;

        private Microsoft.Office365.OutlookServices.CalendarFetcher _CalendarFetcher;

        private Microsoft.Office365.OutlookServices.FolderCollection _FoldersCollection;

        private Microsoft.Office365.OutlookServices.MessageCollection _MessagesCollection;

        private Microsoft.Office365.OutlookServices.CalendarCollection _CalendarsCollection;

        private Microsoft.Office365.OutlookServices.CalendarGroupCollection _CalendarGroupsCollection;

        private Microsoft.Office365.OutlookServices.EventCollection _EventsCollection;

        private Microsoft.Office365.OutlookServices.EventCollection _CalendarViewCollection;

        private Microsoft.Office365.OutlookServices.ContactCollection _ContactsCollection;

        private Microsoft.Office365.OutlookServices.ContactFolderCollection _ContactFoldersCollection;

        private System.String _DisplayName;

        private System.String _Alias;

        private System.Nullable<System.Guid> _MailboxGuid;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Folder> _FoldersConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Message> _MessagesConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Calendar> _CalendarsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.CalendarGroup> _CalendarGroupsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Event> _EventsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Event> _CalendarViewConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Contact> _ContactsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.ContactFolder> _ContactFoldersConcrete;

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

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IFolder> Microsoft.Office365.OutlookServices.IUser.Folders

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IFolder, Microsoft.Office365.OutlookServices.Folder>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Folder>) Folders);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IMessage> Microsoft.Office365.OutlookServices.IUser.Messages

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IMessage, Microsoft.Office365.OutlookServices.Message>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Message>) Messages);

            }

        }

        Microsoft.Office365.OutlookServices.IFolder Microsoft.Office365.OutlookServices.IUser.RootFolder

        {

            get

            {

                return this._RootFolder;

            }

            set

            {

                if (this.RootFolder != value)

                {

                    this.RootFolder = (Microsoft.Office365.OutlookServices.Folder)value;

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.ICalendar> Microsoft.Office365.OutlookServices.IUser.Calendars

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.ICalendar, Microsoft.Office365.OutlookServices.Calendar>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Calendar>) Calendars);

            }

        }

        Microsoft.Office365.OutlookServices.ICalendar Microsoft.Office365.OutlookServices.IUser.Calendar

        {

            get

            {

                return this._Calendar;

            }

            set

            {

                if (this.Calendar != value)

                {

                    this.Calendar = (Microsoft.Office365.OutlookServices.Calendar)value;

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.ICalendarGroup> Microsoft.Office365.OutlookServices.IUser.CalendarGroups

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.ICalendarGroup, Microsoft.Office365.OutlookServices.CalendarGroup>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.CalendarGroup>) CalendarGroups);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent> Microsoft.Office365.OutlookServices.IUser.Events

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IEvent, Microsoft.Office365.OutlookServices.Event>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Event>) Events);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent> Microsoft.Office365.OutlookServices.IUser.CalendarView

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IEvent, Microsoft.Office365.OutlookServices.Event>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Event>) CalendarView);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContact> Microsoft.Office365.OutlookServices.IUser.Contacts

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IContact, Microsoft.Office365.OutlookServices.Contact>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Contact>) Contacts);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContactFolder> Microsoft.Office365.OutlookServices.IUser.ContactFolders

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IContactFolder, Microsoft.Office365.OutlookServices.ContactFolder>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.ContactFolder>) ContactFolders);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Folder> Folders

        {

            get

            {

                if (this._FoldersConcrete == null)

                {

                    this._FoldersConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Folder>();

                    this._FoldersConcrete.SetContainer(() => GetContainingEntity("Folders"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Folder>)this._FoldersConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Folders.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Folders.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Message> Messages

        {

            get

            {

                if (this._MessagesConcrete == null)

                {

                    this._MessagesConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Message>();

                    this._MessagesConcrete.SetContainer(() => GetContainingEntity("Messages"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Message>)this._MessagesConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Messages.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Messages.Add(i);

                    }

                }

            }

        }

        public Microsoft.Office365.OutlookServices.Folder RootFolder

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

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Calendar> Calendars

        {

            get

            {

                if (this._CalendarsConcrete == null)

                {

                    this._CalendarsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Calendar>();

                    this._CalendarsConcrete.SetContainer(() => GetContainingEntity("Calendars"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Calendar>)this._CalendarsConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Calendars.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Calendars.Add(i);

                    }

                }

            }

        }

        public Microsoft.Office365.OutlookServices.Calendar Calendar

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

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.CalendarGroup> CalendarGroups

        {

            get

            {

                if (this._CalendarGroupsConcrete == null)

                {

                    this._CalendarGroupsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.CalendarGroup>();

                    this._CalendarGroupsConcrete.SetContainer(() => GetContainingEntity("CalendarGroups"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.CalendarGroup>)this._CalendarGroupsConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                CalendarGroups.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        CalendarGroups.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Event> Events

        {

            get

            {

                if (this._EventsConcrete == null)

                {

                    this._EventsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Event>();

                    this._EventsConcrete.SetContainer(() => GetContainingEntity("Events"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Event>)this._EventsConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Events.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Events.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Event> CalendarView

        {

            get

            {

                if (this._CalendarViewConcrete == null)

                {

                    this._CalendarViewConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Event>();

                    this._CalendarViewConcrete.SetContainer(() => GetContainingEntity("CalendarView"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Event>)this._CalendarViewConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                CalendarView.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        CalendarView.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Contact> Contacts

        {

            get

            {

                if (this._ContactsConcrete == null)

                {

                    this._ContactsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Contact>();

                    this._ContactsConcrete.SetContainer(() => GetContainingEntity("Contacts"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Contact>)this._ContactsConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Contacts.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Contacts.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.ContactFolder> ContactFolders

        {

            get

            {

                if (this._ContactFoldersConcrete == null)

                {

                    this._ContactFoldersConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.ContactFolder>();

                    this._ContactFoldersConcrete.SetContainer(() => GetContainingEntity("ContactFolders"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.ContactFolder>)this._ContactFoldersConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                ContactFolders.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        ContactFolders.Add(i);

                    }

                }

            }

        }

        Microsoft.Office365.OutlookServices.IFolderCollection Microsoft.Office365.OutlookServices.IUserFetcher.Folders

        {

            get

            {

                if (this._FoldersCollection == null)

                {

                    this._FoldersCollection = new Microsoft.Office365.OutlookServices.FolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Folder>(GetPath("Folders")) : null,
                       Context,
                       this,
                       GetPath("Folders"));

                }

                

                return this._FoldersCollection;

            }

        }

        Microsoft.Office365.OutlookServices.IMessageCollection Microsoft.Office365.OutlookServices.IUserFetcher.Messages

        {

            get

            {

                if (this._MessagesCollection == null)

                {

                    this._MessagesCollection = new Microsoft.Office365.OutlookServices.MessageCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Message>(GetPath("Messages")) : null,
                       Context,
                       this,
                       GetPath("Messages"));

                }

                

                return this._MessagesCollection;

            }

        }

        Microsoft.Office365.OutlookServices.IFolderFetcher Microsoft.Office365.OutlookServices.IUserFetcher.RootFolder

        {

            get

            {

                if (this._RootFolderFetcher == null)

                {

                    this._RootFolderFetcher = new Microsoft.Office365.OutlookServices.FolderFetcher();

                    this._RootFolderFetcher.Initialize(this.Context, GetPath("RootFolder"));

                }

                

                return this._RootFolderFetcher;

            }

        }

        Microsoft.Office365.OutlookServices.ICalendarCollection Microsoft.Office365.OutlookServices.IUserFetcher.Calendars

        {

            get

            {

                if (this._CalendarsCollection == null)

                {

                    this._CalendarsCollection = new Microsoft.Office365.OutlookServices.CalendarCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Calendar>(GetPath("Calendars")) : null,
                       Context,
                       this,
                       GetPath("Calendars"));

                }

                

                return this._CalendarsCollection;

            }

        }

        Microsoft.Office365.OutlookServices.ICalendarFetcher Microsoft.Office365.OutlookServices.IUserFetcher.Calendar

        {

            get

            {

                if (this._CalendarFetcher == null)

                {

                    this._CalendarFetcher = new Microsoft.Office365.OutlookServices.CalendarFetcher();

                    this._CalendarFetcher.Initialize(this.Context, GetPath("Calendar"));

                }

                

                return this._CalendarFetcher;

            }

        }

        Microsoft.Office365.OutlookServices.ICalendarGroupCollection Microsoft.Office365.OutlookServices.IUserFetcher.CalendarGroups

        {

            get

            {

                if (this._CalendarGroupsCollection == null)

                {

                    this._CalendarGroupsCollection = new Microsoft.Office365.OutlookServices.CalendarGroupCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.CalendarGroup>(GetPath("CalendarGroups")) : null,
                       Context,
                       this,
                       GetPath("CalendarGroups"));

                }

                

                return this._CalendarGroupsCollection;

            }

        }

        Microsoft.Office365.OutlookServices.IEventCollection Microsoft.Office365.OutlookServices.IUserFetcher.Events

        {

            get

            {

                if (this._EventsCollection == null)

                {

                    this._EventsCollection = new Microsoft.Office365.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Event>(GetPath("Events")) : null,
                       Context,
                       this,
                       GetPath("Events"));

                }

                

                return this._EventsCollection;

            }

        }

        Microsoft.Office365.OutlookServices.IEventCollection Microsoft.Office365.OutlookServices.IUserFetcher.CalendarView

        {

            get

            {

                if (this._CalendarViewCollection == null)

                {

                    this._CalendarViewCollection = new Microsoft.Office365.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Event>(GetPath("CalendarView")) : null,
                       Context,
                       this,
                       GetPath("CalendarView"));

                }

                

                return this._CalendarViewCollection;

            }

        }

        Microsoft.Office365.OutlookServices.IContactCollection Microsoft.Office365.OutlookServices.IUserFetcher.Contacts

        {

            get

            {

                if (this._ContactsCollection == null)

                {

                    this._ContactsCollection = new Microsoft.Office365.OutlookServices.ContactCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Contact>(GetPath("Contacts")) : null,
                       Context,
                       this,
                       GetPath("Contacts"));

                }

                

                return this._ContactsCollection;

            }

        }

        Microsoft.Office365.OutlookServices.IContactFolderCollection Microsoft.Office365.OutlookServices.IUserFetcher.ContactFolders

        {

            get

            {

                if (this._ContactFoldersCollection == null)

                {

                    this._ContactFoldersCollection = new Microsoft.Office365.OutlookServices.ContactFolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.ContactFolder>(GetPath("ContactFolders")) : null,
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

        public async System.Threading.Tasks.Task SendMailAsync(Microsoft.Office365.OutlookServices.IMessage Message, System.Nullable<System.Boolean> SaveToSentItems)

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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IUser> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.User, Microsoft.Office365.OutlookServices.IUser>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IUser> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IUser> Microsoft.Office365.OutlookServices.IUserFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.OutlookServices.IUser>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.OutlookServices.IUserFetcher Microsoft.Office365.OutlookServices.IUserFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IUser, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IUserFetcher) this;

        }

    }

    internal partial class UserFetcher:Microsoft.Office365.OutlookServices.EntityFetcher, Microsoft.Office365.OutlookServices.IUserFetcher

    {

        private Microsoft.Office365.OutlookServices.FolderFetcher _RootFolderFetcher;

        private Microsoft.Office365.OutlookServices.CalendarFetcher _CalendarFetcher;

        private Microsoft.Office365.OutlookServices.FolderCollection _FoldersCollection;

        private Microsoft.Office365.OutlookServices.MessageCollection _MessagesCollection;

        private Microsoft.Office365.OutlookServices.CalendarCollection _CalendarsCollection;

        private Microsoft.Office365.OutlookServices.CalendarGroupCollection _CalendarGroupsCollection;

        private Microsoft.Office365.OutlookServices.EventCollection _EventsCollection;

        private Microsoft.Office365.OutlookServices.EventCollection _CalendarViewCollection;

        private Microsoft.Office365.OutlookServices.ContactCollection _ContactsCollection;

        private Microsoft.Office365.OutlookServices.ContactFolderCollection _ContactFoldersCollection;

        public Microsoft.Office365.OutlookServices.IFolderCollection Folders

        {

            get

            {

                if (this._FoldersCollection == null)

                {

                    this._FoldersCollection = new Microsoft.Office365.OutlookServices.FolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Folder>(GetPath("Folders")) : null,
                       Context,
                       this,
                       GetPath("Folders"));

                }

                

                return this._FoldersCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.IMessageCollection Messages

        {

            get

            {

                if (this._MessagesCollection == null)

                {

                    this._MessagesCollection = new Microsoft.Office365.OutlookServices.MessageCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Message>(GetPath("Messages")) : null,
                       Context,
                       this,
                       GetPath("Messages"));

                }

                

                return this._MessagesCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.IFolderFetcher RootFolder

        {

            get

            {

                if (this._RootFolderFetcher == null)

                {

                    this._RootFolderFetcher = new Microsoft.Office365.OutlookServices.FolderFetcher();

                    this._RootFolderFetcher.Initialize(this.Context, GetPath("RootFolder"));

                }

                

                return this._RootFolderFetcher;

            }

        }

        public Microsoft.Office365.OutlookServices.ICalendarCollection Calendars

        {

            get

            {

                if (this._CalendarsCollection == null)

                {

                    this._CalendarsCollection = new Microsoft.Office365.OutlookServices.CalendarCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Calendar>(GetPath("Calendars")) : null,
                       Context,
                       this,
                       GetPath("Calendars"));

                }

                

                return this._CalendarsCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.ICalendarFetcher Calendar

        {

            get

            {

                if (this._CalendarFetcher == null)

                {

                    this._CalendarFetcher = new Microsoft.Office365.OutlookServices.CalendarFetcher();

                    this._CalendarFetcher.Initialize(this.Context, GetPath("Calendar"));

                }

                

                return this._CalendarFetcher;

            }

        }

        public Microsoft.Office365.OutlookServices.ICalendarGroupCollection CalendarGroups

        {

            get

            {

                if (this._CalendarGroupsCollection == null)

                {

                    this._CalendarGroupsCollection = new Microsoft.Office365.OutlookServices.CalendarGroupCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.CalendarGroup>(GetPath("CalendarGroups")) : null,
                       Context,
                       this,
                       GetPath("CalendarGroups"));

                }

                

                return this._CalendarGroupsCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.IEventCollection Events

        {

            get

            {

                if (this._EventsCollection == null)

                {

                    this._EventsCollection = new Microsoft.Office365.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Event>(GetPath("Events")) : null,
                       Context,
                       this,
                       GetPath("Events"));

                }

                

                return this._EventsCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.IEventCollection CalendarView

        {

            get

            {

                if (this._CalendarViewCollection == null)

                {

                    this._CalendarViewCollection = new Microsoft.Office365.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Event>(GetPath("CalendarView")) : null,
                       Context,
                       this,
                       GetPath("CalendarView"));

                }

                

                return this._CalendarViewCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.IContactCollection Contacts

        {

            get

            {

                if (this._ContactsCollection == null)

                {

                    this._ContactsCollection = new Microsoft.Office365.OutlookServices.ContactCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Contact>(GetPath("Contacts")) : null,
                       Context,
                       this,
                       GetPath("Contacts"));

                }

                

                return this._ContactsCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.IContactFolderCollection ContactFolders

        {

            get

            {

                if (this._ContactFoldersCollection == null)

                {

                    this._ContactFoldersCollection = new Microsoft.Office365.OutlookServices.ContactFolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.ContactFolder>(GetPath("ContactFolders")) : null,
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

        public async System.Threading.Tasks.Task SendMailAsync(Microsoft.Office365.OutlookServices.IMessage Message, System.Nullable<System.Boolean> SaveToSentItems)

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

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IUser> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.OutlookServices.IUserFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IUser, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IUserFetcher) new Microsoft.Office365.OutlookServices.UserFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IUser> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.User, Microsoft.Office365.OutlookServices.IUser>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IUser> _query;

    }

    internal partial class UserCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IUser>, Microsoft.Office365.OutlookServices.IUserCollection

    {

        internal UserCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IUserFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IUser>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddUserAsync(Microsoft.Office365.OutlookServices.IUser item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IUserFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.User>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.UserFetcher();

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

    public partial class Folder:Microsoft.Office365.OutlookServices.Entity, Microsoft.Office365.OutlookServices.IFolder, Microsoft.Office365.OutlookServices.IFolderFetcher

    {

        private Microsoft.Office365.OutlookServices.FolderCollection _ChildFoldersCollection;

        private Microsoft.Office365.OutlookServices.MessageCollection _MessagesCollection;

        private System.String _ParentFolderId;

        private System.String _DisplayName;

        private System.Nullable<System.Int32> _ChildFolderCount;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Folder> _ChildFoldersConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Message> _MessagesConcrete;

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

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IFolder> Microsoft.Office365.OutlookServices.IFolder.ChildFolders

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IFolder, Microsoft.Office365.OutlookServices.Folder>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Folder>) ChildFolders);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IMessage> Microsoft.Office365.OutlookServices.IFolder.Messages

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IMessage, Microsoft.Office365.OutlookServices.Message>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Message>) Messages);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Folder> ChildFolders

        {

            get

            {

                if (this._ChildFoldersConcrete == null)

                {

                    this._ChildFoldersConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Folder>();

                    this._ChildFoldersConcrete.SetContainer(() => GetContainingEntity("ChildFolders"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Folder>)this._ChildFoldersConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                ChildFolders.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        ChildFolders.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Message> Messages

        {

            get

            {

                if (this._MessagesConcrete == null)

                {

                    this._MessagesConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Message>();

                    this._MessagesConcrete.SetContainer(() => GetContainingEntity("Messages"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Message>)this._MessagesConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Messages.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Messages.Add(i);

                    }

                }

            }

        }

        Microsoft.Office365.OutlookServices.IFolderCollection Microsoft.Office365.OutlookServices.IFolderFetcher.ChildFolders

        {

            get

            {

                if (this._ChildFoldersCollection == null)

                {

                    this._ChildFoldersCollection = new Microsoft.Office365.OutlookServices.FolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Folder>(GetPath("ChildFolders")) : null,
                       Context,
                       this,
                       GetPath("ChildFolders"));

                }

                

                return this._ChildFoldersCollection;

            }

        }

        Microsoft.Office365.OutlookServices.IMessageCollection Microsoft.Office365.OutlookServices.IFolderFetcher.Messages

        {

            get

            {

                if (this._MessagesCollection == null)

                {

                    this._MessagesCollection = new Microsoft.Office365.OutlookServices.MessageCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Message>(GetPath("Messages")) : null,
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

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> CopyAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Copy");

            return (Microsoft.Office365.OutlookServices.IFolder) Enumerable.Single<Microsoft.Office365.OutlookServices.Folder>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Folder>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> MoveAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Move");

            return (Microsoft.Office365.OutlookServices.IFolder) Enumerable.Single<Microsoft.Office365.OutlookServices.Folder>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Folder>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IFolder> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Folder, Microsoft.Office365.OutlookServices.IFolder>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IFolder> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> Microsoft.Office365.OutlookServices.IFolderFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.OutlookServices.IFolder>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.OutlookServices.IFolderFetcher Microsoft.Office365.OutlookServices.IFolderFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IFolder, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IFolderFetcher) this;

        }

    }

    internal partial class FolderFetcher:Microsoft.Office365.OutlookServices.EntityFetcher, Microsoft.Office365.OutlookServices.IFolderFetcher

    {

        private Microsoft.Office365.OutlookServices.FolderCollection _ChildFoldersCollection;

        private Microsoft.Office365.OutlookServices.MessageCollection _MessagesCollection;

        public Microsoft.Office365.OutlookServices.IFolderCollection ChildFolders

        {

            get

            {

                if (this._ChildFoldersCollection == null)

                {

                    this._ChildFoldersCollection = new Microsoft.Office365.OutlookServices.FolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Folder>(GetPath("ChildFolders")) : null,
                       Context,
                       this,
                       GetPath("ChildFolders"));

                }

                

                return this._ChildFoldersCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.IMessageCollection Messages

        {

            get

            {

                if (this._MessagesCollection == null)

                {

                    this._MessagesCollection = new Microsoft.Office365.OutlookServices.MessageCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Message>(GetPath("Messages")) : null,
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

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> CopyAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Copy");

            return (Microsoft.Office365.OutlookServices.IFolder) Enumerable.Single<Microsoft.Office365.OutlookServices.Folder>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Folder>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> MoveAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Move");

            return (Microsoft.Office365.OutlookServices.IFolder) Enumerable.Single<Microsoft.Office365.OutlookServices.Folder>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Folder>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.OutlookServices.IFolderFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IFolder, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IFolderFetcher) new Microsoft.Office365.OutlookServices.FolderFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IFolder> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Folder, Microsoft.Office365.OutlookServices.IFolder>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IFolder> _query;

    }

    internal partial class FolderCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IFolder>, Microsoft.Office365.OutlookServices.IFolderCollection

    {

        internal FolderCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IFolderFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IFolder>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddFolderAsync(Microsoft.Office365.OutlookServices.IFolder item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IFolderFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.Folder>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.FolderFetcher();

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

    public partial class Message:Microsoft.Office365.OutlookServices.Item, Microsoft.Office365.OutlookServices.IMessage, Microsoft.Office365.OutlookServices.IMessageFetcher

    {

        private Microsoft.Office365.OutlookServices.AttachmentCollection _AttachmentsCollection;

        private System.String _Subject;

        private Microsoft.Office365.OutlookServices.ItemBody _Body;

        private System.String _BodyPreview;

        private Microsoft.Office365.OutlookServices.Importance _Importance;

        private System.Nullable<System.Boolean> _HasAttachments;

        private System.String _ParentFolderId;

        private Microsoft.Office365.OutlookServices.Recipient _From;

        private Microsoft.Office365.OutlookServices.Recipient _Sender;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.Recipient> _ToRecipients;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.Recipient> _CcRecipients;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.Recipient> _BccRecipients;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.Recipient> _ReplyTo;

        private System.String _ConversationId;

        private Microsoft.Office365.OutlookServices.ItemBody _UniqueBody;

        private System.Nullable<System.DateTimeOffset> _DateTimeReceived;

        private System.Nullable<System.DateTimeOffset> _DateTimeSent;

        private System.Nullable<System.Boolean> _IsDeliveryReceiptRequested;

        private System.Nullable<System.Boolean> _IsReadReceiptRequested;

        private System.Nullable<System.Boolean> _IsDraft;

        private System.Nullable<System.Boolean> _IsRead;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Attachment> _AttachmentsConcrete;

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

        public Microsoft.Office365.OutlookServices.ItemBody Body

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

        public Microsoft.Office365.OutlookServices.Importance Importance

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

        public Microsoft.Office365.OutlookServices.Recipient From

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

        public Microsoft.Office365.OutlookServices.Recipient Sender

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

        public System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient> ToRecipients

        {

            get

            {

                if (this._ToRecipients == default(System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient>))

                {

                    this._ToRecipients = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.Recipient>();

                    this._ToRecipients.SetContainer(() => GetContainingEntity("ToRecipients"));

                }

                return this._ToRecipients;

            }

            set

            {

                ToRecipients.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        ToRecipients.Add(i);

                    }

                }

            }

        }

        public System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient> CcRecipients

        {

            get

            {

                if (this._CcRecipients == default(System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient>))

                {

                    this._CcRecipients = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.Recipient>();

                    this._CcRecipients.SetContainer(() => GetContainingEntity("CcRecipients"));

                }

                return this._CcRecipients;

            }

            set

            {

                CcRecipients.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        CcRecipients.Add(i);

                    }

                }

            }

        }

        public System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient> BccRecipients

        {

            get

            {

                if (this._BccRecipients == default(System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient>))

                {

                    this._BccRecipients = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.Recipient>();

                    this._BccRecipients.SetContainer(() => GetContainingEntity("BccRecipients"));

                }

                return this._BccRecipients;

            }

            set

            {

                BccRecipients.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        BccRecipients.Add(i);

                    }

                }

            }

        }

        public System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient> ReplyTo

        {

            get

            {

                if (this._ReplyTo == default(System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient>))

                {

                    this._ReplyTo = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.Recipient>();

                    this._ReplyTo.SetContainer(() => GetContainingEntity("ReplyTo"));

                }

                return this._ReplyTo;

            }

            set

            {

                ReplyTo.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        ReplyTo.Add(i);

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

        public Microsoft.Office365.OutlookServices.ItemBody UniqueBody

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

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IAttachment> Microsoft.Office365.OutlookServices.IMessage.Attachments

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IAttachment, Microsoft.Office365.OutlookServices.Attachment>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Attachment>) Attachments);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Attachment> Attachments

        {

            get

            {

                if (this._AttachmentsConcrete == null)

                {

                    this._AttachmentsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Attachment>();

                    this._AttachmentsConcrete.SetContainer(() => GetContainingEntity("Attachments"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Attachment>)this._AttachmentsConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Attachments.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Attachments.Add(i);

                    }

                }

            }

        }

        Microsoft.Office365.OutlookServices.IAttachmentCollection Microsoft.Office365.OutlookServices.IMessageFetcher.Attachments

        {

            get

            {

                if (this._AttachmentsCollection == null)

                {

                    this._AttachmentsCollection = new Microsoft.Office365.OutlookServices.AttachmentCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Attachment>(GetPath("Attachments")) : null,
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

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CopyAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Copy");

            return (Microsoft.Office365.OutlookServices.IMessage) Enumerable.Single<Microsoft.Office365.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> MoveAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Move");

            return (Microsoft.Office365.OutlookServices.IMessage) Enumerable.Single<Microsoft.Office365.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateReplyAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateReply");

            return (Microsoft.Office365.OutlookServices.IMessage) Enumerable.Single<Microsoft.Office365.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateReplyAllAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateReplyAll");

            return (Microsoft.Office365.OutlookServices.IMessage) Enumerable.Single<Microsoft.Office365.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateForwardAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateForward");

            return (Microsoft.Office365.OutlookServices.IMessage) Enumerable.Single<Microsoft.Office365.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

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

        public async System.Threading.Tasks.Task ForwardAsync(System.String Comment, System.Collections.Generic.ICollection<Microsoft.Office365.OutlookServices.Recipient> ToRecipients)

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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IMessage> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Message, Microsoft.Office365.OutlookServices.IMessage>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IMessage> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> Microsoft.Office365.OutlookServices.IMessageFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.OutlookServices.IMessage>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.OutlookServices.IMessageFetcher Microsoft.Office365.OutlookServices.IMessageFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IMessage, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IMessageFetcher) this;

        }

    }

    internal partial class MessageFetcher:Microsoft.Office365.OutlookServices.ItemFetcher, Microsoft.Office365.OutlookServices.IMessageFetcher

    {

        private Microsoft.Office365.OutlookServices.AttachmentCollection _AttachmentsCollection;

        public Microsoft.Office365.OutlookServices.IAttachmentCollection Attachments

        {

            get

            {

                if (this._AttachmentsCollection == null)

                {

                    this._AttachmentsCollection = new Microsoft.Office365.OutlookServices.AttachmentCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Attachment>(GetPath("Attachments")) : null,
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

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CopyAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Copy");

            return (Microsoft.Office365.OutlookServices.IMessage) Enumerable.Single<Microsoft.Office365.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> MoveAsync(System.String DestinationId)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/Move");

            return (Microsoft.Office365.OutlookServices.IMessage) Enumerable.Single<Microsoft.Office365.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("DestinationId", (object) DestinationId),

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateReplyAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateReply");

            return (Microsoft.Office365.OutlookServices.IMessage) Enumerable.Single<Microsoft.Office365.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateReplyAllAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateReplyAll");

            return (Microsoft.Office365.OutlookServices.IMessage) Enumerable.Single<Microsoft.Office365.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

            {

            }

            ));

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateForwardAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/CreateForward");

            return (Microsoft.Office365.OutlookServices.IMessage) Enumerable.Single<Microsoft.Office365.OutlookServices.Message>(await this.Context.ExecuteAsync<Microsoft.Office365.OutlookServices.Message>(requestUri, "POST", true, new OperationParameter[]

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

        public async System.Threading.Tasks.Task ForwardAsync(System.String Comment, System.Collections.Generic.ICollection<Microsoft.Office365.OutlookServices.Recipient> ToRecipients)

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

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.OutlookServices.IMessageFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IMessage, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IMessageFetcher) new Microsoft.Office365.OutlookServices.MessageFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IMessage> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Message, Microsoft.Office365.OutlookServices.IMessage>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IMessage> _query;

    }

    internal partial class MessageCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IMessage>, Microsoft.Office365.OutlookServices.IMessageCollection

    {

        internal MessageCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IMessageFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IMessage>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddMessageAsync(Microsoft.Office365.OutlookServices.IMessage item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IMessageFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.Message>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.MessageFetcher();

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

    public partial class FileAttachment:Microsoft.Office365.OutlookServices.Attachment, Microsoft.Office365.OutlookServices.IFileAttachment, Microsoft.Office365.OutlookServices.IFileAttachmentFetcher

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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IFileAttachment> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.FileAttachment, Microsoft.Office365.OutlookServices.IFileAttachment>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IFileAttachment> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFileAttachment> Microsoft.Office365.OutlookServices.IFileAttachmentFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.OutlookServices.IFileAttachment>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.OutlookServices.IFileAttachmentFetcher Microsoft.Office365.OutlookServices.IFileAttachmentFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IFileAttachment, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IFileAttachmentFetcher) this;

        }

    }

    internal partial class FileAttachmentFetcher:Microsoft.Office365.OutlookServices.AttachmentFetcher, Microsoft.Office365.OutlookServices.IFileAttachmentFetcher

    {

        public FileAttachmentFetcher()

        {

        }

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFileAttachment> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.OutlookServices.IFileAttachmentFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IFileAttachment, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IFileAttachmentFetcher) new Microsoft.Office365.OutlookServices.FileAttachmentFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IFileAttachment> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.FileAttachment, Microsoft.Office365.OutlookServices.IFileAttachment>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IFileAttachment> _query;

    }

    internal partial class FileAttachmentCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IFileAttachment>, Microsoft.Office365.OutlookServices.IFileAttachmentCollection

    {

        internal FileAttachmentCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IFileAttachmentFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IFileAttachment>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddFileAttachmentAsync(Microsoft.Office365.OutlookServices.IFileAttachment item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IFileAttachmentFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.FileAttachment>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.FileAttachmentFetcher();

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

    public partial class ItemAttachment:Microsoft.Office365.OutlookServices.Attachment, Microsoft.Office365.OutlookServices.IItemAttachment, Microsoft.Office365.OutlookServices.IItemAttachmentFetcher

    {

        private Microsoft.Office365.OutlookServices.Item _Item;

        private Microsoft.Office365.OutlookServices.ItemFetcher _ItemFetcher;

        Microsoft.Office365.OutlookServices.IItem Microsoft.Office365.OutlookServices.IItemAttachment.Item

        {

            get

            {

                return this._Item;

            }

            set

            {

                if (this.Item != value)

                {

                    this.Item = (Microsoft.Office365.OutlookServices.Item)value;

                }

            }

        }

        public Microsoft.Office365.OutlookServices.Item Item

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

        Microsoft.Office365.OutlookServices.IItemFetcher Microsoft.Office365.OutlookServices.IItemAttachmentFetcher.Item

        {

            get

            {

                if (this._ItemFetcher == null)

                {

                    this._ItemFetcher = new Microsoft.Office365.OutlookServices.ItemFetcher();

                    this._ItemFetcher.Initialize(this.Context, GetPath("Item"));

                }

                

                return this._ItemFetcher;

            }

        }

        public ItemAttachment()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IItemAttachment> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.ItemAttachment, Microsoft.Office365.OutlookServices.IItemAttachment>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IItemAttachment> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IItemAttachment> Microsoft.Office365.OutlookServices.IItemAttachmentFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.OutlookServices.IItemAttachment>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.OutlookServices.IItemAttachmentFetcher Microsoft.Office365.OutlookServices.IItemAttachmentFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IItemAttachment, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IItemAttachmentFetcher) this;

        }

    }

    internal partial class ItemAttachmentFetcher:Microsoft.Office365.OutlookServices.AttachmentFetcher, Microsoft.Office365.OutlookServices.IItemAttachmentFetcher

    {

        private Microsoft.Office365.OutlookServices.ItemFetcher _ItemFetcher;

        public Microsoft.Office365.OutlookServices.IItemFetcher Item

        {

            get

            {

                if (this._ItemFetcher == null)

                {

                    this._ItemFetcher = new Microsoft.Office365.OutlookServices.ItemFetcher();

                    this._ItemFetcher.Initialize(this.Context, GetPath("Item"));

                }

                

                return this._ItemFetcher;

            }

        }

        public ItemAttachmentFetcher()

        {

        }

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IItemAttachment> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.OutlookServices.IItemAttachmentFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IItemAttachment, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IItemAttachmentFetcher) new Microsoft.Office365.OutlookServices.ItemAttachmentFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IItemAttachment> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.ItemAttachment, Microsoft.Office365.OutlookServices.IItemAttachment>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IItemAttachment> _query;

    }

    internal partial class ItemAttachmentCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IItemAttachment>, Microsoft.Office365.OutlookServices.IItemAttachmentCollection

    {

        internal ItemAttachmentCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IItemAttachmentFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IItemAttachment>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddItemAttachmentAsync(Microsoft.Office365.OutlookServices.IItemAttachment item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IItemAttachmentFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.ItemAttachment>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.ItemAttachmentFetcher();

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

    public partial class Calendar:Microsoft.Office365.OutlookServices.Entity, Microsoft.Office365.OutlookServices.ICalendar, Microsoft.Office365.OutlookServices.ICalendarFetcher

    {

        private Microsoft.Office365.OutlookServices.EventCollection _CalendarViewCollection;

        private Microsoft.Office365.OutlookServices.EventCollection _EventsCollection;

        private System.String _Name;

        private System.String _ChangeKey;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Event> _CalendarViewConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Event> _EventsConcrete;

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

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent> Microsoft.Office365.OutlookServices.ICalendar.CalendarView

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IEvent, Microsoft.Office365.OutlookServices.Event>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Event>) CalendarView);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent> Microsoft.Office365.OutlookServices.ICalendar.Events

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IEvent, Microsoft.Office365.OutlookServices.Event>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Event>) Events);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Event> CalendarView

        {

            get

            {

                if (this._CalendarViewConcrete == null)

                {

                    this._CalendarViewConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Event>();

                    this._CalendarViewConcrete.SetContainer(() => GetContainingEntity("CalendarView"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Event>)this._CalendarViewConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                CalendarView.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        CalendarView.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Event> Events

        {

            get

            {

                if (this._EventsConcrete == null)

                {

                    this._EventsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Event>();

                    this._EventsConcrete.SetContainer(() => GetContainingEntity("Events"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Event>)this._EventsConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Events.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Events.Add(i);

                    }

                }

            }

        }

        Microsoft.Office365.OutlookServices.IEventCollection Microsoft.Office365.OutlookServices.ICalendarFetcher.CalendarView

        {

            get

            {

                if (this._CalendarViewCollection == null)

                {

                    this._CalendarViewCollection = new Microsoft.Office365.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Event>(GetPath("CalendarView")) : null,
                       Context,
                       this,
                       GetPath("CalendarView"));

                }

                

                return this._CalendarViewCollection;

            }

        }

        Microsoft.Office365.OutlookServices.IEventCollection Microsoft.Office365.OutlookServices.ICalendarFetcher.Events

        {

            get

            {

                if (this._EventsCollection == null)

                {

                    this._EventsCollection = new Microsoft.Office365.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Event>(GetPath("Events")) : null,
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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.ICalendar> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Calendar, Microsoft.Office365.OutlookServices.ICalendar>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.ICalendar> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.ICalendar> Microsoft.Office365.OutlookServices.ICalendarFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.OutlookServices.ICalendar>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.OutlookServices.ICalendarFetcher Microsoft.Office365.OutlookServices.ICalendarFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.ICalendar, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.ICalendarFetcher) this;

        }

    }

    internal partial class CalendarFetcher:Microsoft.Office365.OutlookServices.EntityFetcher, Microsoft.Office365.OutlookServices.ICalendarFetcher

    {

        private Microsoft.Office365.OutlookServices.EventCollection _CalendarViewCollection;

        private Microsoft.Office365.OutlookServices.EventCollection _EventsCollection;

        public Microsoft.Office365.OutlookServices.IEventCollection CalendarView

        {

            get

            {

                if (this._CalendarViewCollection == null)

                {

                    this._CalendarViewCollection = new Microsoft.Office365.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Event>(GetPath("CalendarView")) : null,
                       Context,
                       this,
                       GetPath("CalendarView"));

                }

                

                return this._CalendarViewCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.IEventCollection Events

        {

            get

            {

                if (this._EventsCollection == null)

                {

                    this._EventsCollection = new Microsoft.Office365.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Event>(GetPath("Events")) : null,
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

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.ICalendar> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.OutlookServices.ICalendarFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.ICalendar, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.ICalendarFetcher) new Microsoft.Office365.OutlookServices.CalendarFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.ICalendar> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Calendar, Microsoft.Office365.OutlookServices.ICalendar>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.ICalendar> _query;

    }

    internal partial class CalendarCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.ICalendar>, Microsoft.Office365.OutlookServices.ICalendarCollection

    {

        internal CalendarCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.ICalendarFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.ICalendar>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddCalendarAsync(Microsoft.Office365.OutlookServices.ICalendar item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.ICalendarFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.Calendar>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.CalendarFetcher();

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

    public partial class CalendarGroup:Microsoft.Office365.OutlookServices.Entity, Microsoft.Office365.OutlookServices.ICalendarGroup, Microsoft.Office365.OutlookServices.ICalendarGroupFetcher

    {

        private Microsoft.Office365.OutlookServices.CalendarCollection _CalendarsCollection;

        private System.String _Name;

        private System.String _ChangeKey;

        private System.Nullable<System.Guid> _ClassId;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Calendar> _CalendarsConcrete;

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

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.ICalendar> Microsoft.Office365.OutlookServices.ICalendarGroup.Calendars

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.ICalendar, Microsoft.Office365.OutlookServices.Calendar>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Calendar>) Calendars);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Calendar> Calendars

        {

            get

            {

                if (this._CalendarsConcrete == null)

                {

                    this._CalendarsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Calendar>();

                    this._CalendarsConcrete.SetContainer(() => GetContainingEntity("Calendars"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Calendar>)this._CalendarsConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Calendars.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Calendars.Add(i);

                    }

                }

            }

        }

        Microsoft.Office365.OutlookServices.ICalendarCollection Microsoft.Office365.OutlookServices.ICalendarGroupFetcher.Calendars

        {

            get

            {

                if (this._CalendarsCollection == null)

                {

                    this._CalendarsCollection = new Microsoft.Office365.OutlookServices.CalendarCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Calendar>(GetPath("Calendars")) : null,
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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.ICalendarGroup> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.CalendarGroup, Microsoft.Office365.OutlookServices.ICalendarGroup>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.ICalendarGroup> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.ICalendarGroup> Microsoft.Office365.OutlookServices.ICalendarGroupFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.OutlookServices.ICalendarGroup>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.OutlookServices.ICalendarGroupFetcher Microsoft.Office365.OutlookServices.ICalendarGroupFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.ICalendarGroup, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.ICalendarGroupFetcher) this;

        }

    }

    internal partial class CalendarGroupFetcher:Microsoft.Office365.OutlookServices.EntityFetcher, Microsoft.Office365.OutlookServices.ICalendarGroupFetcher

    {

        private Microsoft.Office365.OutlookServices.CalendarCollection _CalendarsCollection;

        public Microsoft.Office365.OutlookServices.ICalendarCollection Calendars

        {

            get

            {

                if (this._CalendarsCollection == null)

                {

                    this._CalendarsCollection = new Microsoft.Office365.OutlookServices.CalendarCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Calendar>(GetPath("Calendars")) : null,
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

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.ICalendarGroup> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.OutlookServices.ICalendarGroupFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.ICalendarGroup, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.ICalendarGroupFetcher) new Microsoft.Office365.OutlookServices.CalendarGroupFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.ICalendarGroup> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.CalendarGroup, Microsoft.Office365.OutlookServices.ICalendarGroup>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.ICalendarGroup> _query;

    }

    internal partial class CalendarGroupCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.ICalendarGroup>, Microsoft.Office365.OutlookServices.ICalendarGroupCollection

    {

        internal CalendarGroupCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.ICalendarGroupFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.ICalendarGroup>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddCalendarGroupAsync(Microsoft.Office365.OutlookServices.ICalendarGroup item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.ICalendarGroupFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.CalendarGroup>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.CalendarGroupFetcher();

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

    public partial class Event:Microsoft.Office365.OutlookServices.Item, Microsoft.Office365.OutlookServices.IEvent, Microsoft.Office365.OutlookServices.IEventFetcher

    {

        private Microsoft.Office365.OutlookServices.Calendar _Calendar;

        private Microsoft.Office365.OutlookServices.CalendarFetcher _CalendarFetcher;

        private Microsoft.Office365.OutlookServices.AttachmentCollection _AttachmentsCollection;

        private Microsoft.Office365.OutlookServices.EventCollection _InstancesCollection;

        private System.String _Subject;

        private Microsoft.Office365.OutlookServices.ItemBody _Body;

        private System.String _BodyPreview;

        private Microsoft.Office365.OutlookServices.Importance _Importance;

        private System.Nullable<System.Boolean> _HasAttachments;

        private System.Nullable<System.DateTimeOffset> _Start;

        private System.Nullable<System.DateTimeOffset> _End;

        private Microsoft.Office365.OutlookServices.Location _Location;

        private Microsoft.Office365.OutlookServices.FreeBusyStatus _ShowAs;

        private System.Nullable<System.Boolean> _IsAllDay;

        private System.Nullable<System.Boolean> _IsCancelled;

        private System.Nullable<System.Boolean> _IsOrganizer;

        private System.Nullable<System.Boolean> _ResponseRequested;

        private Microsoft.Office365.OutlookServices.EventType _Type;

        private System.String _SeriesMasterId;

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.Attendee> _Attendees;

        private Microsoft.Office365.OutlookServices.PatternedRecurrence _Recurrence;

        private Microsoft.Office365.OutlookServices.Recipient _Organizer;

        private System.String _StartTimeZone;

        private System.String _EndTimeZone;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Attachment> _AttachmentsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Event> _InstancesConcrete;

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

        public Microsoft.Office365.OutlookServices.ItemBody Body

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

        public Microsoft.Office365.OutlookServices.Importance Importance

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

        public Microsoft.Office365.OutlookServices.Location Location

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

        public Microsoft.Office365.OutlookServices.FreeBusyStatus ShowAs

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

        public Microsoft.Office365.OutlookServices.EventType Type

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

        public System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Attendee> Attendees

        {

            get

            {

                if (this._Attendees == default(System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Attendee>))

                {

                    this._Attendees = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.Attendee>();

                    this._Attendees.SetContainer(() => GetContainingEntity("Attendees"));

                }

                return this._Attendees;

            }

            set

            {

                Attendees.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Attendees.Add(i);

                    }

                }

            }

        }

        public Microsoft.Office365.OutlookServices.PatternedRecurrence Recurrence

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

        public Microsoft.Office365.OutlookServices.Recipient Organizer

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

        public System.String StartTimeZone

        {

            get

            {

                return _StartTimeZone;

            }

            set

            {

                if (value != _StartTimeZone)

                {

                    _StartTimeZone = value;

                    OnPropertyChanged("StartTimeZone");

                }

            }

        }

        public System.String EndTimeZone

        {

            get

            {

                return _EndTimeZone;

            }

            set

            {

                if (value != _EndTimeZone)

                {

                    _EndTimeZone = value;

                    OnPropertyChanged("EndTimeZone");

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IAttachment> Microsoft.Office365.OutlookServices.IEvent.Attachments

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IAttachment, Microsoft.Office365.OutlookServices.Attachment>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Attachment>) Attachments);

            }

        }

        Microsoft.Office365.OutlookServices.ICalendar Microsoft.Office365.OutlookServices.IEvent.Calendar

        {

            get

            {

                return this._Calendar;

            }

            set

            {

                if (this.Calendar != value)

                {

                    this.Calendar = (Microsoft.Office365.OutlookServices.Calendar)value;

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent> Microsoft.Office365.OutlookServices.IEvent.Instances

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IEvent, Microsoft.Office365.OutlookServices.Event>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Event>) Instances);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Attachment> Attachments

        {

            get

            {

                if (this._AttachmentsConcrete == null)

                {

                    this._AttachmentsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Attachment>();

                    this._AttachmentsConcrete.SetContainer(() => GetContainingEntity("Attachments"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Attachment>)this._AttachmentsConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Attachments.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Attachments.Add(i);

                    }

                }

            }

        }

        public Microsoft.Office365.OutlookServices.Calendar Calendar

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

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Event> Instances

        {

            get

            {

                if (this._InstancesConcrete == null)

                {

                    this._InstancesConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Event>();

                    this._InstancesConcrete.SetContainer(() => GetContainingEntity("Instances"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Event>)this._InstancesConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Instances.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Instances.Add(i);

                    }

                }

            }

        }

        Microsoft.Office365.OutlookServices.IAttachmentCollection Microsoft.Office365.OutlookServices.IEventFetcher.Attachments

        {

            get

            {

                if (this._AttachmentsCollection == null)

                {

                    this._AttachmentsCollection = new Microsoft.Office365.OutlookServices.AttachmentCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Attachment>(GetPath("Attachments")) : null,
                       Context,
                       this,
                       GetPath("Attachments"));

                }

                

                return this._AttachmentsCollection;

            }

        }

        Microsoft.Office365.OutlookServices.ICalendarFetcher Microsoft.Office365.OutlookServices.IEventFetcher.Calendar

        {

            get

            {

                if (this._CalendarFetcher == null)

                {

                    this._CalendarFetcher = new Microsoft.Office365.OutlookServices.CalendarFetcher();

                    this._CalendarFetcher.Initialize(this.Context, GetPath("Calendar"));

                }

                

                return this._CalendarFetcher;

            }

        }

        Microsoft.Office365.OutlookServices.IEventCollection Microsoft.Office365.OutlookServices.IEventFetcher.Instances

        {

            get

            {

                if (this._InstancesCollection == null)

                {

                    this._InstancesCollection = new Microsoft.Office365.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Event>(GetPath("Instances")) : null,
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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IEvent> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Event, Microsoft.Office365.OutlookServices.IEvent>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IEvent> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IEvent> Microsoft.Office365.OutlookServices.IEventFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.OutlookServices.IEvent>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.OutlookServices.IEventFetcher Microsoft.Office365.OutlookServices.IEventFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IEvent, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IEventFetcher) this;

        }

    }

    internal partial class EventFetcher:Microsoft.Office365.OutlookServices.ItemFetcher, Microsoft.Office365.OutlookServices.IEventFetcher

    {

        private Microsoft.Office365.OutlookServices.CalendarFetcher _CalendarFetcher;

        private Microsoft.Office365.OutlookServices.AttachmentCollection _AttachmentsCollection;

        private Microsoft.Office365.OutlookServices.EventCollection _InstancesCollection;

        public Microsoft.Office365.OutlookServices.IAttachmentCollection Attachments

        {

            get

            {

                if (this._AttachmentsCollection == null)

                {

                    this._AttachmentsCollection = new Microsoft.Office365.OutlookServices.AttachmentCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Attachment>(GetPath("Attachments")) : null,
                       Context,
                       this,
                       GetPath("Attachments"));

                }

                

                return this._AttachmentsCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.ICalendarFetcher Calendar

        {

            get

            {

                if (this._CalendarFetcher == null)

                {

                    this._CalendarFetcher = new Microsoft.Office365.OutlookServices.CalendarFetcher();

                    this._CalendarFetcher.Initialize(this.Context, GetPath("Calendar"));

                }

                

                return this._CalendarFetcher;

            }

        }

        public Microsoft.Office365.OutlookServices.IEventCollection Instances

        {

            get

            {

                if (this._InstancesCollection == null)

                {

                    this._InstancesCollection = new Microsoft.Office365.OutlookServices.EventCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Event>(GetPath("Instances")) : null,
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

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IEvent> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.OutlookServices.IEventFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IEvent, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IEventFetcher) new Microsoft.Office365.OutlookServices.EventFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IEvent> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Event, Microsoft.Office365.OutlookServices.IEvent>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IEvent> _query;

    }

    internal partial class EventCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IEvent>, Microsoft.Office365.OutlookServices.IEventCollection

    {

        internal EventCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IEventFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddEventAsync(Microsoft.Office365.OutlookServices.IEvent item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IEventFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.Event>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.EventFetcher();

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

    public partial class Contact:Microsoft.Office365.OutlookServices.Item, Microsoft.Office365.OutlookServices.IContact, Microsoft.Office365.OutlookServices.IContactFetcher

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

        private Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.EmailAddress> _EmailAddresses;

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

        private Microsoft.Office365.OutlookServices.PhysicalAddress _HomeAddress;

        private Microsoft.Office365.OutlookServices.PhysicalAddress _BusinessAddress;

        private Microsoft.Office365.OutlookServices.PhysicalAddress _OtherAddress;

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

        public System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.EmailAddress> EmailAddresses

        {

            get

            {

                if (this._EmailAddresses == default(System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.EmailAddress>))

                {

                    this._EmailAddresses = new Microsoft.OData.ProxyExtensions.NonEntityTypeCollectionImpl<Microsoft.Office365.OutlookServices.EmailAddress>();

                    this._EmailAddresses.SetContainer(() => GetContainingEntity("EmailAddresses"));

                }

                return this._EmailAddresses;

            }

            set

            {

                EmailAddresses.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        EmailAddresses.Add(i);

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

                ImAddresses.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        ImAddresses.Add(i);

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

                HomePhones.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        HomePhones.Add(i);

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

                BusinessPhones.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        BusinessPhones.Add(i);

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

        public Microsoft.Office365.OutlookServices.PhysicalAddress HomeAddress

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

        public Microsoft.Office365.OutlookServices.PhysicalAddress BusinessAddress

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

        public Microsoft.Office365.OutlookServices.PhysicalAddress OtherAddress

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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IContact> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Contact, Microsoft.Office365.OutlookServices.IContact>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IContact> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IContact> Microsoft.Office365.OutlookServices.IContactFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.OutlookServices.IContact>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.OutlookServices.IContactFetcher Microsoft.Office365.OutlookServices.IContactFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IContact, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IContactFetcher) this;

        }

    }

    internal partial class ContactFetcher:Microsoft.Office365.OutlookServices.ItemFetcher, Microsoft.Office365.OutlookServices.IContactFetcher

    {

        public ContactFetcher()

        {

        }

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IContact> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.OutlookServices.IContactFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IContact, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IContactFetcher) new Microsoft.Office365.OutlookServices.ContactFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IContact> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.Contact, Microsoft.Office365.OutlookServices.IContact>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IContact> _query;

    }

    internal partial class ContactCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IContact>, Microsoft.Office365.OutlookServices.IContactCollection

    {

        internal ContactCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IContactFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContact>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddContactAsync(Microsoft.Office365.OutlookServices.IContact item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IContactFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.Contact>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.ContactFetcher();

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

    public partial class ContactFolder:Microsoft.Office365.OutlookServices.Entity, Microsoft.Office365.OutlookServices.IContactFolder, Microsoft.Office365.OutlookServices.IContactFolderFetcher

    {

        private Microsoft.Office365.OutlookServices.ContactCollection _ContactsCollection;

        private Microsoft.Office365.OutlookServices.ContactFolderCollection _ChildFoldersCollection;

        private System.String _ParentFolderId;

        private System.String _DisplayName;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Contact> _ContactsConcrete;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.ContactFolder> _ChildFoldersConcrete;

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

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContact> Microsoft.Office365.OutlookServices.IContactFolder.Contacts

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IContact, Microsoft.Office365.OutlookServices.Contact>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.Contact>) Contacts);

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContactFolder> Microsoft.Office365.OutlookServices.IContactFolder.ChildFolders

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.OutlookServices.IContactFolder, Microsoft.Office365.OutlookServices.ContactFolder>(Context, (DataServiceCollection<Microsoft.Office365.OutlookServices.ContactFolder>) ChildFolders);

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Contact> Contacts

        {

            get

            {

                if (this._ContactsConcrete == null)

                {

                    this._ContactsConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.Contact>();

                    this._ContactsConcrete.SetContainer(() => GetContainingEntity("Contacts"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Contact>)this._ContactsConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Contacts.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Contacts.Add(i);

                    }

                }

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.ContactFolder> ChildFolders

        {

            get

            {

                if (this._ChildFoldersConcrete == null)

                {

                    this._ChildFoldersConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.OutlookServices.ContactFolder>();

                    this._ChildFoldersConcrete.SetContainer(() => GetContainingEntity("ChildFolders"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.ContactFolder>)this._ChildFoldersConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                ChildFolders.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        ChildFolders.Add(i);

                    }

                }

            }

        }

        Microsoft.Office365.OutlookServices.IContactCollection Microsoft.Office365.OutlookServices.IContactFolderFetcher.Contacts

        {

            get

            {

                if (this._ContactsCollection == null)

                {

                    this._ContactsCollection = new Microsoft.Office365.OutlookServices.ContactCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Contact>(GetPath("Contacts")) : null,
                       Context,
                       this,
                       GetPath("Contacts"));

                }

                

                return this._ContactsCollection;

            }

        }

        Microsoft.Office365.OutlookServices.IContactFolderCollection Microsoft.Office365.OutlookServices.IContactFolderFetcher.ChildFolders

        {

            get

            {

                if (this._ChildFoldersCollection == null)

                {

                    this._ChildFoldersCollection = new Microsoft.Office365.OutlookServices.ContactFolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.ContactFolder>(GetPath("ChildFolders")) : null,
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

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IContactFolder> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.ContactFolder, Microsoft.Office365.OutlookServices.IContactFolder>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IContactFolder> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IContactFolder> Microsoft.Office365.OutlookServices.IContactFolderFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.OutlookServices.IContactFolder>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.OutlookServices.IContactFolderFetcher Microsoft.Office365.OutlookServices.IContactFolderFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IContactFolder, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IContactFolderFetcher) this;

        }

    }

    internal partial class ContactFolderFetcher:Microsoft.Office365.OutlookServices.EntityFetcher, Microsoft.Office365.OutlookServices.IContactFolderFetcher

    {

        private Microsoft.Office365.OutlookServices.ContactCollection _ContactsCollection;

        private Microsoft.Office365.OutlookServices.ContactFolderCollection _ChildFoldersCollection;

        public Microsoft.Office365.OutlookServices.IContactCollection Contacts

        {

            get

            {

                if (this._ContactsCollection == null)

                {

                    this._ContactsCollection = new Microsoft.Office365.OutlookServices.ContactCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.Contact>(GetPath("Contacts")) : null,
                       Context,
                       this,
                       GetPath("Contacts"));

                }

                

                return this._ContactsCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.IContactFolderCollection ChildFolders

        {

            get

            {

                if (this._ChildFoldersCollection == null)

                {

                    this._ChildFoldersCollection = new Microsoft.Office365.OutlookServices.ContactFolderCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.ContactFolder>(GetPath("ChildFolders")) : null,
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

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IContactFolder> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.OutlookServices.IContactFolderFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IContactFolder, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.OutlookServices.IContactFolderFetcher) new Microsoft.Office365.OutlookServices.ContactFolderFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IContactFolder> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.OutlookServices.ContactFolder, Microsoft.Office365.OutlookServices.IContactFolder>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.OutlookServices.IContactFolder> _query;

    }

    internal partial class ContactFolderCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.OutlookServices.IContactFolder>, Microsoft.Office365.OutlookServices.IContactFolderCollection

    {

        internal ContactFolderCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.OutlookServices.IContactFolderFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContactFolder>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddContactFolderAsync(Microsoft.Office365.OutlookServices.IContactFolder item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.OutlookServices.IContactFolderFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.OutlookServices.ContactFolder>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.OutlookServices.ContactFolderFetcher();

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

    public partial class OutlookServicesClient:Microsoft.Office365.OutlookServices.IOutlookServicesClient

    {

        private const System.String _path = "";

        private Microsoft.Office365.OutlookServices.UserFetcher _MeFetcher;

        private Microsoft.Office365.OutlookServices.UserCollection _UsersCollection;

        public Microsoft.Office365.OutlookServices.IUserCollection Users

        {

            get

            {

                if (this._UsersCollection == null)

                {

                    this._UsersCollection = new Microsoft.Office365.OutlookServices.UserCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.OutlookServices.User>(GetPath("Users")) : null,
                       Context,
                       this,
                       GetPath("Users"));

                }

                

                return this._UsersCollection;

            }

        }

        public Microsoft.Office365.OutlookServices.IUserFetcher Me

        {

            get

            {

                if (this._MeFetcher == null)

                {

                    this._MeFetcher = new Microsoft.Office365.OutlookServices.UserFetcher();

                    this._MeFetcher.Initialize(this.Context, GetPath("Me"));

                }

                

                return this._MeFetcher;

            }

            private set

            {

                this._MeFetcher = (Microsoft.Office365.OutlookServices.UserFetcher)value;

            }

        }

        public Microsoft.OData.ProxyExtensions.DataServiceContextWrapper Context

        {get; private set;}

        public OutlookServicesClient(global::System.Uri serviceRoot, global::System.Func<global::System.Threading.Tasks.Task<string>> accessTokenGetter)

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

        public void AddToUsers(Microsoft.Office365.OutlookServices.IUser users)

        {

            this.Context.AddObject("Users", (object) users);

        }

        private global::System.Type ResolveTypeFromName(System.String typeName)

        {

            global::System.Type resolvedType;

            resolvedType = Context.DefaultResolveTypeInternal(typeName,  "Microsoft.Office365.OutlookServices", "Microsoft.OutlookServices");

            if (resolvedType != null)

            {

                return resolvedType;

            }

            return null;

        }

        private System.String ResolveNameFromType(global::System.Type clientType)

        {

            string resolvedType;

            resolvedType = Context.DefaultResolveNameInternal(clientType,  "Microsoft.OutlookServices", "Microsoft.Office365.OutlookServices");

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
            
                  <EnumType Name=""MeetingMessageType"">
            
                    <Member Name=""None"" Value=""0"" />
            
                    <Member Name=""MeetingRequest"" Value=""1"" />
            
                    <Member Name=""MeetingCancelled"" Value=""2"" />
            
                    <Member Name=""MeetingAccepted"" Value=""3"" />
            
                    <Member Name=""MeetingTenativelyAccepted"" Value=""4"" />
            
                    <Member Name=""MeetingDeclined"" Value=""5"" />
            
                  </EnumType>
            
                  <EntityType Abstract=""true"" Name=""Entity"">
            
                    <Key>
            
                      <PropertyRef Name=""Id"" />
            
                    </Key>
            
                    <Property Name=""Id"" Type=""Edm.String"" />
            
                  </EntityType>
            
                  <EntityType Abstract=""true"" BaseType=""Microsoft.OutlookServices.Entity"" Name=""Attachment"">
            
                    <Property Name=""Name"" Type=""Edm.String"" />
            
                    <Property Name=""ContentType"" Type=""Edm.String"" />
            
                    <Property Name=""Size"" Nullable=""false"" Type=""Edm.Int32"" />
            
                    <Property Name=""IsInline"" Nullable=""false"" Type=""Edm.Boolean"" />
            
                    <Property Name=""DateTimeLastModified"" Type=""Edm.DateTimeOffset"" />
            
                  </EntityType>
            
                  <EntityType Abstract=""true"" BaseType=""Microsoft.OutlookServices.Entity"" Name=""Item"">
            
                    <Property Name=""ChangeKey"" Type=""Edm.String"" />
            
                    <Property Name=""Categories"" Type=""Collection(Edm.String)"" />
            
                    <Property Name=""DateTimeCreated"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""DateTimeLastModified"" Type=""Edm.DateTimeOffset"" />
            
                  </EntityType>
            
                  <EntityType BaseType=""Microsoft.OutlookServices.Entity"" Name=""User"">
            
                    <Property Name=""DisplayName"" Type=""Edm.String"" />
            
                    <Property Name=""Alias"" Type=""Edm.String"" />
            
                    <Property Name=""MailboxGuid"" Type=""Edm.Guid"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Folders"" Type=""Collection(Microsoft.OutlookServices.Folder)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Messages"" Type=""Collection(Microsoft.OutlookServices.Message)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""RootFolder"" Type=""Microsoft.OutlookServices.Folder"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Calendars"" Type=""Collection(Microsoft.OutlookServices.Calendar)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Calendar"" Type=""Microsoft.OutlookServices.Calendar"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""CalendarGroups"" Type=""Collection(Microsoft.OutlookServices.CalendarGroup)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Events"" Type=""Collection(Microsoft.OutlookServices.Event)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""CalendarView"" Type=""Collection(Microsoft.OutlookServices.Event)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Contacts"" Type=""Collection(Microsoft.OutlookServices.Contact)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""ContactFolders"" Type=""Collection(Microsoft.OutlookServices.ContactFolder)"" />
            
                  </EntityType>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""SendMail"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.User"" />
            
                    <Parameter Name=""Message"" Nullable=""false"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""SaveToSentItems"" Type=""Edm.Boolean"" />
            
                  </Action>
            
                  <EntityType BaseType=""Microsoft.OutlookServices.Entity"" Name=""Folder"">
            
                    <Property Name=""ParentFolderId"" Type=""Edm.String"" />
            
                    <Property Name=""DisplayName"" Type=""Edm.String"" />
            
                    <Property Name=""ChildFolderCount"" Type=""Edm.Int32"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""ChildFolders"" Type=""Collection(Microsoft.OutlookServices.Folder)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Messages"" Type=""Collection(Microsoft.OutlookServices.Message)"" />
            
                  </EntityType>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""Copy"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Folder"" />
            
                    <Parameter Name=""DestinationId"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Folder"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""Move"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Folder"" />
            
                    <Parameter Name=""DestinationId"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Folder"" />
            
                  </Action>
            
                  <EntityType BaseType=""Microsoft.OutlookServices.Item"" Name=""Message"">
            
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
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Attachments"" Type=""Collection(Microsoft.OutlookServices.Attachment)"" />
            
                  </EntityType>
            
                  <ComplexType Name=""ItemBody"">
            
                    <Property Name=""ContentType"" Type=""Microsoft.OutlookServices.BodyType"" />
            
                    <Property Name=""Content"" Type=""Edm.String"" />
            
                  </ComplexType>
            
                  <EnumType Name=""BodyType"">
            
                    <Member Name=""Text"" Value=""0"" />
            
                    <Member Name=""HTML"" Value=""1"" />
            
                  </EnumType>
            
                  <EnumType Name=""Importance"">
            
                    <Member Name=""Low"" Value=""0"" />
            
                    <Member Name=""Normal"" Value=""1"" />
            
                    <Member Name=""High"" Value=""2"" />
            
                  </EnumType>
            
                  <ComplexType Name=""Recipient"">
            
                    <Property Name=""EmailAddress"" Type=""Microsoft.OutlookServices.EmailAddress"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""EmailAddress"">
            
                    <Property Name=""Name"" Type=""Edm.String"" />
            
                    <Property Name=""Address"" Type=""Edm.String"" />
            
                  </ComplexType>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""Copy"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""DestinationId"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""Move"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""DestinationId"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""CreateReply"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""CreateReplyAll"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""CreateForward"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <ReturnType Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""Reply"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""ReplyAll"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""Forward"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                    <Parameter Name=""ToRecipients"" Type=""Collection(Microsoft.OutlookServices.Recipient)"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""Send"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Message"" />
            
                  </Action>
            
                  <EntityType BaseType=""Microsoft.OutlookServices.Attachment"" Name=""FileAttachment"">
            
                    <Property Name=""ContentId"" Type=""Edm.String"" />
            
                    <Property Name=""ContentLocation"" Type=""Edm.String"" />
            
                    <Property Name=""IsContactPhoto"" Nullable=""false"" Type=""Edm.Boolean"" />
            
                    <Property Name=""ContentBytes"" Type=""Edm.Binary"" />
            
                  </EntityType>
            
                  <EntityType BaseType=""Microsoft.OutlookServices.Attachment"" Name=""ItemAttachment"">
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Item"" Type=""Microsoft.OutlookServices.Item"" />
            
                  </EntityType>
            
                  <EntityType BaseType=""Microsoft.OutlookServices.Entity"" Name=""Calendar"">
            
                    <Property Name=""Name"" Type=""Edm.String"" />
            
                    <Property Name=""ChangeKey"" Type=""Edm.String"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""CalendarView"" Type=""Collection(Microsoft.OutlookServices.Event)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Events"" Type=""Collection(Microsoft.OutlookServices.Event)"" />
            
                  </EntityType>
            
                  <EntityType BaseType=""Microsoft.OutlookServices.Entity"" Name=""CalendarGroup"">
            
                    <Property Name=""Name"" Type=""Edm.String"" />
            
                    <Property Name=""ChangeKey"" Type=""Edm.String"" />
            
                    <Property Name=""ClassId"" Type=""Edm.Guid"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Calendars"" Type=""Collection(Microsoft.OutlookServices.Calendar)"" />
            
                  </EntityType>
            
                  <EntityType BaseType=""Microsoft.OutlookServices.Item"" Name=""Event"">
            
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
            
                    <Property Name=""StartTimeZone"" Type=""Edm.String"" />
            
                    <Property Name=""EndTimeZone"" Type=""Edm.String"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Attachments"" Type=""Collection(Microsoft.OutlookServices.Attachment)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Calendar"" Type=""Microsoft.OutlookServices.Calendar"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Instances"" Type=""Collection(Microsoft.OutlookServices.Event)"" />
            
                  </EntityType>
            
                  <ComplexType Name=""Location"">
            
                    <Property Name=""DisplayName"" Type=""Edm.String"" />
            
                  </ComplexType>
            
                  <EnumType Name=""FreeBusyStatus"">
            
                    <Member Name=""Free"" Value=""0"" />
            
                    <Member Name=""Tentative"" Value=""1"" />
            
                    <Member Name=""Busy"" Value=""2"" />
            
                    <Member Name=""Oof"" Value=""3"" />
            
                    <Member Name=""WorkingElsewhere"" Value=""4"" />
            
                    <Member Name=""Unknown"" Value=""-1"" />
            
                  </EnumType>
            
                  <EnumType Name=""EventType"">
            
                    <Member Name=""SingleInstance"" Value=""0"" />
            
                    <Member Name=""Occurrence"" Value=""1"" />
            
                    <Member Name=""Exception"" Value=""2"" />
            
                    <Member Name=""SeriesMaster"" Value=""3"" />
            
                  </EnumType>
            
                  <ComplexType BaseType=""Microsoft.OutlookServices.Recipient"" Name=""Attendee"">
            
                    <Property Name=""Status"" Type=""Microsoft.OutlookServices.ResponseStatus"" />
            
                    <Property Name=""Type"" Type=""Microsoft.OutlookServices.AttendeeType"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""ResponseStatus"">
            
                    <Property Name=""Response"" Type=""Microsoft.OutlookServices.ResponseType"" />
            
                    <Property Name=""Time"" Type=""Edm.DateTimeOffset"" />
            
                  </ComplexType>
            
                  <EnumType Name=""ResponseType"">
            
                    <Member Name=""None"" Value=""0"" />
            
                    <Member Name=""Organizer"" Value=""1"" />
            
                    <Member Name=""TentativelyAccepted"" Value=""2"" />
            
                    <Member Name=""Accepted"" Value=""3"" />
            
                    <Member Name=""Declined"" Value=""4"" />
            
                    <Member Name=""NotResponded"" Value=""5"" />
            
                  </EnumType>
            
                  <EnumType Name=""AttendeeType"">
            
                    <Member Name=""Required"" Value=""0"" />
            
                    <Member Name=""Optional"" Value=""1"" />
            
                    <Member Name=""Resource"" Value=""2"" />
            
                  </EnumType>
            
                  <ComplexType Name=""PatternedRecurrence"">
            
                    <Property Name=""Pattern"" Type=""Microsoft.OutlookServices.RecurrencePattern"" />
            
                    <Property Name=""Range"" Type=""Microsoft.OutlookServices.RecurrenceRange"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""RecurrencePattern"">
            
                    <Property Name=""Type"" Type=""Microsoft.OutlookServices.RecurrencePatternType"" />
            
                    <Property Name=""Interval"" Nullable=""false"" Type=""Edm.Int32"" />
            
                    <Property Name=""Month"" Nullable=""false"" Type=""Edm.Int32"" />
            
                    <Property Name=""DayOfMonth"" Nullable=""false"" Type=""Edm.Int32"" />
            
                    <Property Name=""DaysOfWeek"" Type=""Collection(Microsoft.OutlookServices.DayOfWeek)"" />
            
                    <Property Name=""FirstDayOfWeek"" Type=""Microsoft.OutlookServices.DayOfWeek"" />
            
                    <Property Name=""Index"" Type=""Microsoft.OutlookServices.WeekIndex"" />
            
                  </ComplexType>
            
                  <EnumType Name=""RecurrencePatternType"">
            
                    <Member Name=""Daily"" Value=""0"" />
            
                    <Member Name=""Weekly"" Value=""1"" />
            
                    <Member Name=""AbsoluteMonthly"" Value=""2"" />
            
                    <Member Name=""RelativeMonthly"" Value=""3"" />
            
                    <Member Name=""AbsoluteYearly"" Value=""4"" />
            
                    <Member Name=""RelativeYearly"" Value=""5"" />
            
                  </EnumType>
            
                  <EnumType Name=""DayOfWeek"">
            
                    <Member Name=""Sunday"" Value=""0"" />
            
                    <Member Name=""Monday"" Value=""1"" />
            
                    <Member Name=""Tuesday"" Value=""2"" />
            
                    <Member Name=""Wednesday"" Value=""3"" />
            
                    <Member Name=""Thursday"" Value=""4"" />
            
                    <Member Name=""Friday"" Value=""5"" />
            
                    <Member Name=""Saturday"" Value=""6"" />
            
                  </EnumType>
            
                  <EnumType Name=""WeekIndex"">
            
                    <Member Name=""First"" Value=""0"" />
            
                    <Member Name=""Second"" Value=""1"" />
            
                    <Member Name=""Third"" Value=""2"" />
            
                    <Member Name=""Fourth"" Value=""3"" />
            
                    <Member Name=""Last"" Value=""4"" />
            
                  </EnumType>
            
                  <ComplexType Name=""RecurrenceRange"">
            
                    <Property Name=""Type"" Type=""Microsoft.OutlookServices.RecurrenceRangeType"" />
            
                    <Property Name=""StartDate"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""EndDate"" Type=""Edm.DateTimeOffset"" />
            
                    <Property Name=""NumberOfOccurrences"" Nullable=""false"" Type=""Edm.Int32"" />
            
                  </ComplexType>
            
                  <EnumType Name=""RecurrenceRangeType"">
            
                    <Member Name=""EndDate"" Value=""0"" />
            
                    <Member Name=""NoEnd"" Value=""1"" />
            
                    <Member Name=""Numbered"" Value=""2"" />
            
                  </EnumType>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""Accept"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Event"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""Decline"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Event"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                  </Action>
            
                  <Action EntitySetPath=""bindingParameter"" IsBound=""true"" Name=""TentativelyAccept"">
            
                    <Parameter Name=""bindingParameter"" Type=""Microsoft.OutlookServices.Event"" />
            
                    <Parameter Name=""Comment"" Type=""Edm.String"" />
            
                  </Action>
            
                  <EntityType BaseType=""Microsoft.OutlookServices.Item"" Name=""Contact"">
            
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
            
                  <ComplexType Name=""PhysicalAddress"">
            
                    <Property Name=""Street"" Type=""Edm.String"" />
            
                    <Property Name=""City"" Type=""Edm.String"" />
            
                    <Property Name=""State"" Type=""Edm.String"" />
            
                    <Property Name=""CountryOrRegion"" Type=""Edm.String"" />
            
                    <Property Name=""PostalCode"" Type=""Edm.String"" />
            
                  </ComplexType>
            
                  <EntityType BaseType=""Microsoft.OutlookServices.Entity"" Name=""ContactFolder"">
            
                    <Property Name=""ParentFolderId"" Type=""Edm.String"" />
            
                    <Property Name=""DisplayName"" Type=""Edm.String"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""Contacts"" Type=""Collection(Microsoft.OutlookServices.Contact)"" />
            
                    <NavigationProperty ContainsTarget=""true"" Name=""ChildFolders"" Type=""Collection(Microsoft.OutlookServices.ContactFolder)"" />
            
                  </EntityType>
            
                  <EntityContainer Name=""EntityContainer"">
            
                    <EntitySet EntityType=""Microsoft.OutlookServices.User"" Name=""Users"" />
            
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

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IEntityCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IEntity>

    {

        Microsoft.Office365.OutlookServices.IEntityFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEntity>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddEntityAsync(Microsoft.Office365.OutlookServices.IEntity item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IEntityFetcher this[System.String id]

        {get;}

    }

    public partial interface IEntityCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IAttachment:Microsoft.Office365.OutlookServices.IEntity

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

    public partial interface IAttachmentFetcher:Microsoft.Office365.OutlookServices.IEntityFetcher

    {

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IAttachmentCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IAttachment>

    {

        Microsoft.Office365.OutlookServices.IAttachmentFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IAttachment>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddAttachmentAsync(Microsoft.Office365.OutlookServices.IAttachment item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IAttachmentFetcher this[System.String id]

        {get;}

    }

    public partial interface IAttachmentCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItem:Microsoft.Office365.OutlookServices.IEntity

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

    public partial interface IItemFetcher:Microsoft.Office365.OutlookServices.IEntityFetcher

    {

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IItem>

    {

        Microsoft.Office365.OutlookServices.IItemFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IItem>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddItemAsync(Microsoft.Office365.OutlookServices.IItem item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IItemFetcher this[System.String id]

        {get;}

    }

    public partial interface IItemCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IUser:Microsoft.Office365.OutlookServices.IEntity

    {

        System.String DisplayName
        {get;set;}

        System.String Alias
        {get;set;}

        System.Nullable<System.Guid> MailboxGuid
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IFolder> Folders
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IMessage> Messages
        {get;}

        Microsoft.Office365.OutlookServices.IFolder RootFolder
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.ICalendar> Calendars
        {get;}

        Microsoft.Office365.OutlookServices.ICalendar Calendar
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.ICalendarGroup> CalendarGroups
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent> Events
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent> CalendarView
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContact> Contacts
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContactFolder> ContactFolders
        {get;}

        System.Threading.Tasks.Task SendMailAsync(Microsoft.Office365.OutlookServices.IMessage Message, System.Nullable<System.Boolean> SaveToSentItems);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IUserFetcher:Microsoft.Office365.OutlookServices.IEntityFetcher

    {

        Microsoft.Office365.OutlookServices.IFolderCollection Folders
        {get;}

        Microsoft.Office365.OutlookServices.IMessageCollection Messages
        {get;}

        Microsoft.Office365.OutlookServices.IFolderFetcher RootFolder
        {get;}

        Microsoft.Office365.OutlookServices.ICalendarCollection Calendars
        {get;}

        Microsoft.Office365.OutlookServices.ICalendarFetcher Calendar
        {get;}

        Microsoft.Office365.OutlookServices.ICalendarGroupCollection CalendarGroups
        {get;}

        Microsoft.Office365.OutlookServices.IEventCollection Events
        {get;}

        Microsoft.Office365.OutlookServices.IEventCollection CalendarView
        {get;}

        Microsoft.Office365.OutlookServices.IContactCollection Contacts
        {get;}

        Microsoft.Office365.OutlookServices.IContactFolderCollection ContactFolders
        {get;}

        System.Threading.Tasks.Task SendMailAsync(Microsoft.Office365.OutlookServices.IMessage Message, System.Nullable<System.Boolean> SaveToSentItems);

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IUser> ExecuteAsync();

        Microsoft.Office365.OutlookServices.IUserFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IUser, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IUserCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IUser>

    {

        Microsoft.Office365.OutlookServices.IUserFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IUser>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddUserAsync(Microsoft.Office365.OutlookServices.IUser item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IUserFetcher this[System.String id]

        {get;}

    }

    public partial interface IUserCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFolder:Microsoft.Office365.OutlookServices.IEntity

    {

        System.String ParentFolderId
        {get;set;}

        System.String DisplayName
        {get;set;}

        System.Nullable<System.Int32> ChildFolderCount
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IFolder> ChildFolders
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IMessage> Messages
        {get;}

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> CopyAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> MoveAsync(System.String DestinationId);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFolderFetcher:Microsoft.Office365.OutlookServices.IEntityFetcher

    {

        Microsoft.Office365.OutlookServices.IFolderCollection ChildFolders
        {get;}

        Microsoft.Office365.OutlookServices.IMessageCollection Messages
        {get;}

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> CopyAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> MoveAsync(System.String DestinationId);

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFolder> ExecuteAsync();

        Microsoft.Office365.OutlookServices.IFolderFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IFolder, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFolderCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IFolder>

    {

        Microsoft.Office365.OutlookServices.IFolderFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IFolder>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddFolderAsync(Microsoft.Office365.OutlookServices.IFolder item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IFolderFetcher this[System.String id]

        {get;}

    }

    public partial interface IFolderCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IMessage:Microsoft.Office365.OutlookServices.IItem

    {

        System.String Subject
        {get;set;}

        Microsoft.Office365.OutlookServices.ItemBody Body
        {get;set;}

        System.String BodyPreview
        {get;set;}

        Microsoft.Office365.OutlookServices.Importance Importance
        {get;set;}

        System.Nullable<System.Boolean> HasAttachments
        {get;set;}

        System.String ParentFolderId
        {get;set;}

        Microsoft.Office365.OutlookServices.Recipient From
        {get;set;}

        Microsoft.Office365.OutlookServices.Recipient Sender
        {get;set;}

        System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient> ToRecipients
        {get;set;}

        System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient> CcRecipients
        {get;set;}

        System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient> BccRecipients
        {get;set;}

        System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Recipient> ReplyTo
        {get;set;}

        System.String ConversationId
        {get;set;}

        Microsoft.Office365.OutlookServices.ItemBody UniqueBody
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

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IAttachment> Attachments
        {get;}

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CopyAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> MoveAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateReplyAsync();

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateReplyAllAsync();

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateForwardAsync();

        System.Threading.Tasks.Task ReplyAsync(System.String Comment);

        System.Threading.Tasks.Task ReplyAllAsync(System.String Comment);

        System.Threading.Tasks.Task ForwardAsync(System.String Comment, System.Collections.Generic.ICollection<Microsoft.Office365.OutlookServices.Recipient> ToRecipients);

        System.Threading.Tasks.Task SendAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IMessageFetcher:Microsoft.Office365.OutlookServices.IItemFetcher

    {

        Microsoft.Office365.OutlookServices.IAttachmentCollection Attachments
        {get;}

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CopyAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> MoveAsync(System.String DestinationId);

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateReplyAsync();

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateReplyAllAsync();

        System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> CreateForwardAsync();

        System.Threading.Tasks.Task ReplyAsync(System.String Comment);

        System.Threading.Tasks.Task ReplyAllAsync(System.String Comment);

        System.Threading.Tasks.Task ForwardAsync(System.String Comment, System.Collections.Generic.ICollection<Microsoft.Office365.OutlookServices.Recipient> ToRecipients);

        System.Threading.Tasks.Task SendAsync();

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IMessage> ExecuteAsync();

        Microsoft.Office365.OutlookServices.IMessageFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IMessage, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IMessageCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IMessage>

    {

        Microsoft.Office365.OutlookServices.IMessageFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IMessage>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddMessageAsync(Microsoft.Office365.OutlookServices.IMessage item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IMessageFetcher this[System.String id]

        {get;}

    }

    public partial interface IMessageCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFileAttachment:Microsoft.Office365.OutlookServices.IAttachment

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

    public partial interface IFileAttachmentFetcher:Microsoft.Office365.OutlookServices.IAttachmentFetcher

    {

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IFileAttachment> ExecuteAsync();

        Microsoft.Office365.OutlookServices.IFileAttachmentFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IFileAttachment, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFileAttachmentCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IFileAttachment>

    {

        Microsoft.Office365.OutlookServices.IFileAttachmentFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IFileAttachment>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddFileAttachmentAsync(Microsoft.Office365.OutlookServices.IFileAttachment item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IFileAttachmentFetcher this[System.String id]

        {get;}

    }

    public partial interface IFileAttachmentCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemAttachment:Microsoft.Office365.OutlookServices.IAttachment

    {

        Microsoft.Office365.OutlookServices.IItem Item
        {get;set;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemAttachmentFetcher:Microsoft.Office365.OutlookServices.IAttachmentFetcher

    {

        Microsoft.Office365.OutlookServices.IItemFetcher Item
        {get;}

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IItemAttachment> ExecuteAsync();

        Microsoft.Office365.OutlookServices.IItemAttachmentFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IItemAttachment, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemAttachmentCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IItemAttachment>

    {

        Microsoft.Office365.OutlookServices.IItemAttachmentFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IItemAttachment>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddItemAttachmentAsync(Microsoft.Office365.OutlookServices.IItemAttachment item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IItemAttachmentFetcher this[System.String id]

        {get;}

    }

    public partial interface IItemAttachmentCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendar:Microsoft.Office365.OutlookServices.IEntity

    {

        System.String Name
        {get;set;}

        System.String ChangeKey
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent> CalendarView
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent> Events
        {get;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendarFetcher:Microsoft.Office365.OutlookServices.IEntityFetcher

    {

        Microsoft.Office365.OutlookServices.IEventCollection CalendarView
        {get;}

        Microsoft.Office365.OutlookServices.IEventCollection Events
        {get;}

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.ICalendar> ExecuteAsync();

        Microsoft.Office365.OutlookServices.ICalendarFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.ICalendar, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendarCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.ICalendar>

    {

        Microsoft.Office365.OutlookServices.ICalendarFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.ICalendar>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddCalendarAsync(Microsoft.Office365.OutlookServices.ICalendar item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.ICalendarFetcher this[System.String id]

        {get;}

    }

    public partial interface ICalendarCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendarGroup:Microsoft.Office365.OutlookServices.IEntity

    {

        System.String Name
        {get;set;}

        System.String ChangeKey
        {get;set;}

        System.Nullable<System.Guid> ClassId
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.ICalendar> Calendars
        {get;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendarGroupFetcher:Microsoft.Office365.OutlookServices.IEntityFetcher

    {

        Microsoft.Office365.OutlookServices.ICalendarCollection Calendars
        {get;}

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.ICalendarGroup> ExecuteAsync();

        Microsoft.Office365.OutlookServices.ICalendarGroupFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.ICalendarGroup, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface ICalendarGroupCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.ICalendarGroup>

    {

        Microsoft.Office365.OutlookServices.ICalendarGroupFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.ICalendarGroup>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddCalendarGroupAsync(Microsoft.Office365.OutlookServices.ICalendarGroup item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.ICalendarGroupFetcher this[System.String id]

        {get;}

    }

    public partial interface ICalendarGroupCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IEvent:Microsoft.Office365.OutlookServices.IItem

    {

        System.String Subject
        {get;set;}

        Microsoft.Office365.OutlookServices.ItemBody Body
        {get;set;}

        System.String BodyPreview
        {get;set;}

        Microsoft.Office365.OutlookServices.Importance Importance
        {get;set;}

        System.Nullable<System.Boolean> HasAttachments
        {get;set;}

        System.Nullable<System.DateTimeOffset> Start
        {get;set;}

        System.Nullable<System.DateTimeOffset> End
        {get;set;}

        Microsoft.Office365.OutlookServices.Location Location
        {get;set;}

        Microsoft.Office365.OutlookServices.FreeBusyStatus ShowAs
        {get;set;}

        System.Nullable<System.Boolean> IsAllDay
        {get;set;}

        System.Nullable<System.Boolean> IsCancelled
        {get;set;}

        System.Nullable<System.Boolean> IsOrganizer
        {get;set;}

        System.Nullable<System.Boolean> ResponseRequested
        {get;set;}

        Microsoft.Office365.OutlookServices.EventType Type
        {get;set;}

        System.String SeriesMasterId
        {get;set;}

        System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.Attendee> Attendees
        {get;set;}

        Microsoft.Office365.OutlookServices.PatternedRecurrence Recurrence
        {get;set;}

        Microsoft.Office365.OutlookServices.Recipient Organizer
        {get;set;}

        System.String StartTimeZone
        {get;set;}

        System.String EndTimeZone
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IAttachment> Attachments
        {get;}

        Microsoft.Office365.OutlookServices.ICalendar Calendar
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent> Instances
        {get;}

        System.Threading.Tasks.Task AcceptAsync(System.String Comment);

        System.Threading.Tasks.Task DeclineAsync(System.String Comment);

        System.Threading.Tasks.Task TentativelyAcceptAsync(System.String Comment);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IEventFetcher:Microsoft.Office365.OutlookServices.IItemFetcher

    {

        Microsoft.Office365.OutlookServices.IAttachmentCollection Attachments
        {get;}

        Microsoft.Office365.OutlookServices.ICalendarFetcher Calendar
        {get;}

        Microsoft.Office365.OutlookServices.IEventCollection Instances
        {get;}

        System.Threading.Tasks.Task AcceptAsync(System.String Comment);

        System.Threading.Tasks.Task DeclineAsync(System.String Comment);

        System.Threading.Tasks.Task TentativelyAcceptAsync(System.String Comment);

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IEvent> ExecuteAsync();

        Microsoft.Office365.OutlookServices.IEventFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IEvent, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IEventCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IEvent>

    {

        Microsoft.Office365.OutlookServices.IEventFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IEvent>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddEventAsync(Microsoft.Office365.OutlookServices.IEvent item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IEventFetcher this[System.String id]

        {get;}

    }

    public partial interface IEventCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContact:Microsoft.Office365.OutlookServices.IItem

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

        System.Collections.Generic.IList<Microsoft.Office365.OutlookServices.EmailAddress> EmailAddresses
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

        Microsoft.Office365.OutlookServices.PhysicalAddress HomeAddress
        {get;set;}

        Microsoft.Office365.OutlookServices.PhysicalAddress BusinessAddress
        {get;set;}

        Microsoft.Office365.OutlookServices.PhysicalAddress OtherAddress
        {get;set;}

        System.String YomiCompanyName
        {get;set;}

        System.String YomiGivenName
        {get;set;}

        System.String YomiSurname
        {get;set;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContactFetcher:Microsoft.Office365.OutlookServices.IItemFetcher

    {

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IContact> ExecuteAsync();

        Microsoft.Office365.OutlookServices.IContactFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IContact, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContactCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IContact>

    {

        Microsoft.Office365.OutlookServices.IContactFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContact>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddContactAsync(Microsoft.Office365.OutlookServices.IContact item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IContactFetcher this[System.String id]

        {get;}

    }

    public partial interface IContactCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContactFolder:Microsoft.Office365.OutlookServices.IEntity

    {

        System.String ParentFolderId
        {get;set;}

        System.String DisplayName
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContact> Contacts
        {get;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContactFolder> ChildFolders
        {get;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContactFolderFetcher:Microsoft.Office365.OutlookServices.IEntityFetcher

    {

        Microsoft.Office365.OutlookServices.IContactCollection Contacts
        {get;}

        Microsoft.Office365.OutlookServices.IContactFolderCollection ChildFolders
        {get;}

        global::System.Threading.Tasks.Task<Microsoft.Office365.OutlookServices.IContactFolder> ExecuteAsync();

        Microsoft.Office365.OutlookServices.IContactFolderFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.OutlookServices.IContactFolder, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IContactFolderCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.OutlookServices.IContactFolder>

    {

        Microsoft.Office365.OutlookServices.IContactFolderFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.OutlookServices.IContactFolder>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddContactFolderAsync(Microsoft.Office365.OutlookServices.IContactFolder item, System.Boolean dontSave = false);

         Microsoft.Office365.OutlookServices.IContactFolderFetcher this[System.String id]

        {get;}

    }

    public partial interface IContactFolderCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    public partial interface IOutlookServicesClient

    {

        Microsoft.Office365.OutlookServices.IUserCollection Users
        {get;}

        Microsoft.Office365.OutlookServices.IUserFetcher Me
        {get;}

    }

}

