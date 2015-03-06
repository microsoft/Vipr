namespace Microsoft.Office365.SharePoint.CoreServices

{

    using global::Microsoft.OData.Client;

    using global::Microsoft.OData.Edm;

    using System;

    using System.Collections.Generic;

    using System.ComponentModel;

    using System.Linq;

    using System.Reflection;

    using System.Threading.Tasks;

    public partial class SharePointClient:Microsoft.Office365.SharePoint.CoreServices.ISharePointClient

    {

        private const System.String _path = "";

        private Microsoft.Office365.SharePoint.FileServices.DriveFetcher _driveFetcher;

        private Microsoft.Office365.SharePoint.FileServices.ItemCollection _filesCollection;

        public Microsoft.Office365.SharePoint.FileServices.IItemCollection Files

        {

            get

            {

                if (this._filesCollection == null)

                {

                    this._filesCollection = new Microsoft.Office365.SharePoint.FileServices.ItemCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.SharePoint.FileServices.Item>(GetPath("files")) : null,
                       Context,
                       this,
                       GetPath("files"));

                }

                

                return this._filesCollection;

            }

        }

        public Microsoft.Office365.SharePoint.FileServices.IDriveFetcher Drive

        {

            get

            {

                if (this._driveFetcher == null)

                {

                    this._driveFetcher = new Microsoft.Office365.SharePoint.FileServices.DriveFetcher();

                    this._driveFetcher.Initialize(this.Context, GetPath("drive"));

                }

                

                return this._driveFetcher;

            }

            private set

            {

                this._driveFetcher = (Microsoft.Office365.SharePoint.FileServices.DriveFetcher)value;

            }

        }

        public Microsoft.OData.ProxyExtensions.DataServiceContextWrapper Context

        {get; private set;}

        public SharePointClient(global::System.Uri serviceRoot, global::System.Func<global::System.Threading.Tasks.Task<string>> accessTokenGetter)

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

        public void AddTofiles(Microsoft.Office365.SharePoint.FileServices.IItem files)

        {

            this.Context.AddObject("files", (object) files);

        }

        private global::System.Type ResolveTypeFromName(System.String typeName)

        {

            global::System.Type resolvedType;

            resolvedType = Context.DefaultResolveTypeInternal(typeName,  "Microsoft.Office365.SharePoint.CoreServices", "Microsoft.CoreServices");

            if (resolvedType != null)

            {

                return resolvedType;

            }

            return null;

        }

        private System.String ResolveNameFromType(global::System.Type clientType)

        {

            string resolvedType;

            resolvedType = Context.DefaultResolveNameInternal(clientType,  "Microsoft.CoreServices", "Microsoft.Office365.SharePoint.CoreServices");

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
            
                <Schema Namespace=""Microsoft.CoreServices"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
            
                  <EntityContainer Name=""EntityContainer"">
            
                    <Singleton Name=""drive"" Type=""Microsoft.FileServices.Drive"" />
            
                    <EntitySet Name=""files"" EntityType=""Microsoft.FileServices.Item"" />
            
                  </EntityContainer>
            
                </Schema>
            
                <Schema Namespace=""Microsoft.FileServices"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
            
                  <ComplexType Name=""DriveQuota"">
            
                    <Property Name=""deleted"" Type=""Edm.Int64"" Nullable=""false"" />
            
                    <Property Name=""remaining"" Type=""Edm.Int64"" Nullable=""false"" />
            
                    <Property Name=""state"" Type=""Edm.String"" />
            
                    <Property Name=""total"" Type=""Edm.Int64"" Nullable=""false"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""IdentitySet"">
            
                    <Property Name=""application"" Type=""Microsoft.FileServices.Identity"" />
            
                    <Property Name=""user"" Type=""Microsoft.FileServices.Identity"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""Identity"">
            
                    <Property Name=""id"" Type=""Edm.String"" />
            
                    <Property Name=""displayName"" Type=""Edm.String"" />
            
                  </ComplexType>
            
                  <ComplexType Name=""ItemReference"">
            
                    <Property Name=""driveId"" Type=""Edm.String"" />
            
                    <Property Name=""id"" Type=""Edm.String"" />
            
                    <Property Name=""path"" Type=""Edm.String"" />
            
                  </ComplexType>
            
                  <EntityType Name=""Drive"">
            
                    <Key>
            
                      <PropertyRef Name=""id"" />
            
                    </Key>
            
                    <Property Name=""id"" Type=""Edm.String"" Nullable=""false"" />
            
                    <Property Name=""owner"" Type=""Microsoft.FileServices.Identity"" />
            
                    <Property Name=""quota"" Type=""Microsoft.FileServices.DriveQuota"" />
            
                  </EntityType>
            
                  <EntityType Name=""Item"" Abstract=""true"">
            
                    <Key>
            
                      <PropertyRef Name=""id"" />
            
                    </Key>
            
                    <Property Name=""createdBy"" Type=""Microsoft.FileServices.IdentitySet"" />
            
                    <Property Name=""eTag"" Type=""Edm.String"" />
            
                    <Property Name=""id"" Type=""Edm.String"" Nullable=""false"" />
            
                    <Property Name=""lastModifiedBy"" Type=""Microsoft.FileServices.IdentitySet"" />
            
                    <Property Name=""name"" Type=""Edm.String"" />
            
                    <Property Name=""parentReference"" Type=""Microsoft.FileServices.ItemReference"" />
            
                    <Property Name=""size"" Type=""Edm.Int64"" Nullable=""false"" />
            
                    <Property Name=""dateTimeCreated"" Type=""Edm.DateTimeOffset"" Nullable=""false"" />
            
                    <Property Name=""dateTimeLastModified"" Type=""Edm.DateTimeOffset"" Nullable=""false"" />
            
                    <Property Name=""type"" Type=""Edm.String"" />
            
                    <Property Name=""webUrl"" Type=""Edm.String"" />
            
                  </EntityType>
            
                  <EntityType Name=""File"" BaseType=""Microsoft.FileServices.Item"">
            
                    <Property Name=""contentUrl"" Type=""Edm.String"" />
            
                  </EntityType>
            
                  <EntityType Name=""Folder"" BaseType=""Microsoft.FileServices.Item"">
            
                    <Property Name=""childCount"" Type=""Edm.Int32"" Nullable=""false"" />
            
                    <NavigationProperty Name=""children"" Type=""Collection(Microsoft.FileServices.Item)"" ContainsTarget=""true"" />
            
                  </EntityType>
            
                  <Function Name=""content"" IsBound=""true"" IsComposable=""true"">
            
                    <Parameter Name=""this"" Type=""Microsoft.FileServices.File"" />
            
                    <ReturnType Type=""Edm.Stream"" />
            
                  </Function>
            
                  <Action Name=""copy"" IsBound=""true"">
            
                    <Parameter Name=""this"" Type=""Microsoft.FileServices.File"" />
            
                    <Parameter Name=""destFolderId"" Type=""Edm.String"" />
            
                    <Parameter Name=""destFolderPath"" Type=""Edm.String"" />
            
                    <Parameter Name=""newName"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.FileServices.File"" />
            
                  </Action>
            
                  <Action Name=""uploadContent"" IsBound=""true"">
            
                    <Parameter Name=""this"" Type=""Microsoft.FileServices.File"" />
            
                    <Parameter Name=""contentStream"" Type=""Edm.Stream"" />
            
                  </Action>
            
                  <Action Name=""copy"" IsBound=""true"">
            
                    <Parameter Name=""this"" Type=""Microsoft.FileServices.Folder"" />
            
                    <Parameter Name=""destFolderId"" Type=""Edm.String"" />
            
                    <Parameter Name=""destFolderPath"" Type=""Edm.String"" />
            
                    <Parameter Name=""newName"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.FileServices.Folder"" />
            
                  </Action>
            
                  <Function Name=""getByPath"" IsBound=""true"" IsComposable=""true"">
            
                    <Parameter Name=""this"" Type=""Collection(Microsoft.FileServices.Item)"" />
            
                    <Parameter Name=""path"" Type=""Edm.String"" />
            
                    <ReturnType Type=""Microsoft.FileServices.Item"" />
            
                  </Function>
            
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

    public partial interface ISharePointClient

    {

        Microsoft.Office365.SharePoint.FileServices.IItemCollection Files
        {get;}

        Microsoft.Office365.SharePoint.FileServices.IDriveFetcher Drive
        {get;}

    }

}

namespace Microsoft.Office365.SharePoint.FileServices

{

    using global::Microsoft.OData.Client;

    using global::Microsoft.OData.Edm;

    using System;

    using System.Collections.Generic;

    using System.ComponentModel;

    using System.Linq;

    using System.Reflection;

    using System.Threading.Tasks;

    public partial class DriveQuota:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private System.Int64 _deleted;

        private System.Int64 _remaining;

        private System.String _state;

        private System.Int64 _total;

        public System.Int64 Deleted

        {

            get

            {

                return _deleted;

            }

            set

            {

                if (value != _deleted)

                {

                    _deleted = value;

                    OnPropertyChanged("deleted");

                }

            }

        }

        public System.Int64 Remaining

        {

            get

            {

                return _remaining;

            }

            set

            {

                if (value != _remaining)

                {

                    _remaining = value;

                    OnPropertyChanged("remaining");

                }

            }

        }

        public System.String State

        {

            get

            {

                return _state;

            }

            set

            {

                if (value != _state)

                {

                    _state = value;

                    OnPropertyChanged("state");

                }

            }

        }

        public System.Int64 Total

        {

            get

            {

                return _total;

            }

            set

            {

                if (value != _total)

                {

                    _total = value;

                    OnPropertyChanged("total");

                }

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Deleted instead.")]

        public System.Int64 deleted

        {

            get

            {

                return Deleted;

            }

            set

            {

                Deleted = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Remaining instead.")]

        public System.Int64 remaining

        {

            get

            {

                return Remaining;

            }

            set

            {

                Remaining = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use State instead.")]

        public System.String state

        {

            get

            {

                return State;

            }

            set

            {

                State = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Total instead.")]

        public System.Int64 total

        {

            get

            {

                return Total;

            }

            set

            {

                Total = value;

            }

        }

        public DriveQuota(): base()

        {

        }

    }

    public partial class IdentitySet:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private Microsoft.Office365.SharePoint.FileServices.Identity _application;

        private Microsoft.Office365.SharePoint.FileServices.Identity _user;

        public Microsoft.Office365.SharePoint.FileServices.Identity Application

        {

            get

            {

                return _application;

            }

            set

            {

                if (value != _application)

                {

                    _application = value;

                    OnPropertyChanged("application");

                }

            }

        }

        public Microsoft.Office365.SharePoint.FileServices.Identity User

        {

            get

            {

                return _user;

            }

            set

            {

                if (value != _user)

                {

                    _user = value;

                    OnPropertyChanged("user");

                }

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Application instead.")]

        public Microsoft.Office365.SharePoint.FileServices.Identity application

        {

            get

            {

                return Application;

            }

            set

            {

                Application = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use User instead.")]

        public Microsoft.Office365.SharePoint.FileServices.Identity user

        {

            get

            {

                return User;

            }

            set

            {

                User = value;

            }

        }

        public IdentitySet(): base()

        {

        }

    }

    public partial class Identity:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private System.String _id;

        private System.String _displayName;

        public System.String Id

        {

            get

            {

                return _id;

            }

            set

            {

                if (value != _id)

                {

                    _id = value;

                    OnPropertyChanged("id");

                }

            }

        }

        public System.String DisplayName

        {

            get

            {

                return _displayName;

            }

            set

            {

                if (value != _displayName)

                {

                    _displayName = value;

                    OnPropertyChanged("displayName");

                }

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Id instead.")]

        public System.String id

        {

            get

            {

                return Id;

            }

            set

            {

                Id = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use DisplayName instead.")]

        public System.String displayName

        {

            get

            {

                return DisplayName;

            }

            set

            {

                DisplayName = value;

            }

        }

        public Identity(): base()

        {

        }

    }

    public partial class ItemReference:Microsoft.OData.ProxyExtensions.ComplexTypeBase

    {

        private System.String _driveId;

        private System.String _id;

        private System.String _path;

        public System.String DriveId

        {

            get

            {

                return _driveId;

            }

            set

            {

                if (value != _driveId)

                {

                    _driveId = value;

                    OnPropertyChanged("driveId");

                }

            }

        }

        public System.String Id

        {

            get

            {

                return _id;

            }

            set

            {

                if (value != _id)

                {

                    _id = value;

                    OnPropertyChanged("id");

                }

            }

        }

        public System.String Path

        {

            get

            {

                return _path;

            }

            set

            {

                if (value != _path)

                {

                    _path = value;

                    OnPropertyChanged("path");

                }

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use DriveId instead.")]

        public System.String driveId

        {

            get

            {

                return DriveId;

            }

            set

            {

                DriveId = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Id instead.")]

        public System.String id

        {

            get

            {

                return Id;

            }

            set

            {

                Id = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Path instead.")]

        public System.String path

        {

            get

            {

                return Path;

            }

            set

            {

                Path = value;

            }

        }

        public ItemReference(): base()

        {

        }

    }

    [global::Microsoft.OData.Client.Key("id")]

    public partial class Drive:Microsoft.OData.ProxyExtensions.EntityBase, Microsoft.Office365.SharePoint.FileServices.IDrive, Microsoft.Office365.SharePoint.FileServices.IDriveFetcher

    {

        private System.String _id;

        private Microsoft.Office365.SharePoint.FileServices.Identity _owner;

        private Microsoft.Office365.SharePoint.FileServices.DriveQuota _quota;

        public System.String Id

        {

            get

            {

                return _id;

            }

            set

            {

                if (value != _id)

                {

                    _id = value;

                    OnPropertyChanged("id");

                }

            }

        }

        public Microsoft.Office365.SharePoint.FileServices.Identity Owner

        {

            get

            {

                return _owner;

            }

            set

            {

                if (value != _owner)

                {

                    _owner = value;

                    OnPropertyChanged("owner");

                }

            }

        }

        public Microsoft.Office365.SharePoint.FileServices.DriveQuota Quota

        {

            get

            {

                return _quota;

            }

            set

            {

                if (value != _quota)

                {

                    _quota = value;

                    OnPropertyChanged("quota");

                }

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Id instead.")]

        public System.String id

        {

            get

            {

                return Id;

            }

            set

            {

                Id = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Owner instead.")]

        public Microsoft.Office365.SharePoint.FileServices.Identity owner

        {

            get

            {

                return Owner;

            }

            set

            {

                Owner = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Quota instead.")]

        public Microsoft.Office365.SharePoint.FileServices.DriveQuota quota

        {

            get

            {

                return Quota;

            }

            set

            {

                Quota = value;

            }

        }

        public Drive(): base()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IDrive> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.SharePoint.FileServices.Drive, Microsoft.Office365.SharePoint.FileServices.IDrive>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IDrive> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IDrive> Microsoft.Office365.SharePoint.FileServices.IDriveFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.SharePoint.FileServices.IDrive>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.SharePoint.FileServices.IDriveFetcher Microsoft.Office365.SharePoint.FileServices.IDriveFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.SharePoint.FileServices.IDrive, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.SharePoint.FileServices.IDriveFetcher) this;

        }

    }

    internal partial class DriveFetcher:Microsoft.OData.ProxyExtensions.RestShallowObjectFetcher, Microsoft.Office365.SharePoint.FileServices.IDriveFetcher

    {

        public DriveFetcher(): base()

        {

        }

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IDrive> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.SharePoint.FileServices.IDriveFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.SharePoint.FileServices.IDrive, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.SharePoint.FileServices.IDriveFetcher) new Microsoft.Office365.SharePoint.FileServices.DriveFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IDrive> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.SharePoint.FileServices.Drive, Microsoft.Office365.SharePoint.FileServices.IDrive>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IDrive> _query;

    }

    internal partial class DriveCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.SharePoint.FileServices.IDrive>, Microsoft.Office365.SharePoint.FileServices.IDriveCollection

    {

        internal DriveCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.SharePoint.FileServices.IDriveFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.SharePoint.FileServices.IDrive>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddDriveAsync(Microsoft.Office365.SharePoint.FileServices.IDrive item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.SharePoint.FileServices.IDriveFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.SharePoint.FileServices.Drive>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.SharePoint.FileServices.DriveFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class DriveCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    internal partial class DriveFetcher

    {

    }

    public partial class Drive

    {

    }

    [global::Microsoft.OData.Client.Key("id")]

    public abstract partial class Item:Microsoft.OData.ProxyExtensions.EntityBase, Microsoft.Office365.SharePoint.FileServices.IItem, Microsoft.Office365.SharePoint.FileServices.IItemFetcher

    {

        private Microsoft.Office365.SharePoint.FileServices.IdentitySet _createdBy;

        private System.String _eTag;

        private System.String _id;

        private Microsoft.Office365.SharePoint.FileServices.IdentitySet _lastModifiedBy;

        private System.String _name;

        private Microsoft.Office365.SharePoint.FileServices.ItemReference _parentReference;

        private System.Int64 _size;

        private System.DateTimeOffset _dateTimeCreated;

        private System.DateTimeOffset _dateTimeLastModified;

        private System.String _type;

        private System.String _webUrl;

        public Microsoft.Office365.SharePoint.FileServices.IdentitySet CreatedBy

        {

            get

            {

                return _createdBy;

            }

            set

            {

                if (value != _createdBy)

                {

                    _createdBy = value;

                    OnPropertyChanged("createdBy");

                }

            }

        }

        public System.String ETag

        {

            get

            {

                return _eTag;

            }

            set

            {

                if (value != _eTag)

                {

                    _eTag = value;

                    OnPropertyChanged("eTag");

                }

            }

        }

        public System.String Id

        {

            get

            {

                return _id;

            }

            set

            {

                if (value != _id)

                {

                    _id = value;

                    OnPropertyChanged("id");

                }

            }

        }

        public Microsoft.Office365.SharePoint.FileServices.IdentitySet LastModifiedBy

        {

            get

            {

                return _lastModifiedBy;

            }

            set

            {

                if (value != _lastModifiedBy)

                {

                    _lastModifiedBy = value;

                    OnPropertyChanged("lastModifiedBy");

                }

            }

        }

        public System.String Name

        {

            get

            {

                return _name;

            }

            set

            {

                if (value != _name)

                {

                    _name = value;

                    OnPropertyChanged("name");

                }

            }

        }

        public Microsoft.Office365.SharePoint.FileServices.ItemReference ParentReference

        {

            get

            {

                return _parentReference;

            }

            set

            {

                if (value != _parentReference)

                {

                    _parentReference = value;

                    OnPropertyChanged("parentReference");

                }

            }

        }

        public System.Int64 Size

        {

            get

            {

                return _size;

            }

            set

            {

                if (value != _size)

                {

                    _size = value;

                    OnPropertyChanged("size");

                }

            }

        }

        public System.DateTimeOffset DateTimeCreated

        {

            get

            {

                return _dateTimeCreated;

            }

            set

            {

                if (value != _dateTimeCreated)

                {

                    _dateTimeCreated = value;

                    OnPropertyChanged("dateTimeCreated");

                }

            }

        }

        public System.DateTimeOffset DateTimeLastModified

        {

            get

            {

                return _dateTimeLastModified;

            }

            set

            {

                if (value != _dateTimeLastModified)

                {

                    _dateTimeLastModified = value;

                    OnPropertyChanged("dateTimeLastModified");

                }

            }

        }

        public System.String Type

        {

            get

            {

                return _type;

            }

            set

            {

                if (value != _type)

                {

                    _type = value;

                    OnPropertyChanged("type");

                }

            }

        }

        public System.String WebUrl

        {

            get

            {

                return _webUrl;

            }

            set

            {

                if (value != _webUrl)

                {

                    _webUrl = value;

                    OnPropertyChanged("webUrl");

                }

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use CreatedBy instead.")]

        public Microsoft.Office365.SharePoint.FileServices.IdentitySet createdBy

        {

            get

            {

                return CreatedBy;

            }

            set

            {

                CreatedBy = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use ETag instead.")]

        public System.String eTag

        {

            get

            {

                return ETag;

            }

            set

            {

                ETag = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Id instead.")]

        public System.String id

        {

            get

            {

                return Id;

            }

            set

            {

                Id = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use LastModifiedBy instead.")]

        public Microsoft.Office365.SharePoint.FileServices.IdentitySet lastModifiedBy

        {

            get

            {

                return LastModifiedBy;

            }

            set

            {

                LastModifiedBy = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Name instead.")]

        public System.String name

        {

            get

            {

                return Name;

            }

            set

            {

                Name = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use ParentReference instead.")]

        public Microsoft.Office365.SharePoint.FileServices.ItemReference parentReference

        {

            get

            {

                return ParentReference;

            }

            set

            {

                ParentReference = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Size instead.")]

        public System.Int64 size

        {

            get

            {

                return Size;

            }

            set

            {

                Size = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use DateTimeCreated instead.")]

        public System.DateTimeOffset dateTimeCreated

        {

            get

            {

                return DateTimeCreated;

            }

            set

            {

                DateTimeCreated = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use DateTimeLastModified instead.")]

        public System.DateTimeOffset dateTimeLastModified

        {

            get

            {

                return DateTimeLastModified;

            }

            set

            {

                DateTimeLastModified = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Type instead.")]

        public System.String type

        {

            get

            {

                return Type;

            }

            set

            {

                Type = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use WebUrl instead.")]

        public System.String webUrl

        {

            get

            {

                return WebUrl;

            }

            set

            {

                WebUrl = value;

            }

        }

        public Item(): base()

        {

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IItem> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.SharePoint.FileServices.Item, Microsoft.Office365.SharePoint.FileServices.IItem>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IItem> _query;

    }

    internal partial class ItemFetcher:Microsoft.OData.ProxyExtensions.RestShallowObjectFetcher, Microsoft.Office365.SharePoint.FileServices.IItemFetcher

    {

        public ItemFetcher(): base()

        {

        }

    }

    internal partial class ItemCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.SharePoint.FileServices.IItem>, Microsoft.Office365.SharePoint.FileServices.IItemCollection

    {

        internal ItemCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IItem> getByPathAsync(System.String path)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/getByPath");

            return (Microsoft.Office365.SharePoint.FileServices.IItem) Enumerable.Single<Microsoft.Office365.SharePoint.FileServices.Item>(await this.Context.ExecuteAsync<Microsoft.Office365.SharePoint.FileServices.Item>(requestUri, "GET", true, new OperationParameter[]

            {

                new UriOperationParameter("path", (object) path),

            }

            ));

        }

        public Microsoft.Office365.SharePoint.FileServices.IItemFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.SharePoint.FileServices.IItem>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddItemAsync(Microsoft.Office365.SharePoint.FileServices.IItem item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.SharePoint.FileServices.IItemFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.SharePoint.FileServices.Item>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.SharePoint.FileServices.ItemFetcher();

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

    internal partial class ItemFetcher

    {

        public Microsoft.Office365.SharePoint.FileServices.IFileFetcher ToFile()

        {

            var derivedFetcher = new Microsoft.Office365.SharePoint.FileServices.FileFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.Office365.SharePoint.FileServices.IFileFetcher) derivedFetcher;

        }

        public Microsoft.Office365.SharePoint.FileServices.IFolderFetcher ToFolder()

        {

            var derivedFetcher = new Microsoft.Office365.SharePoint.FileServices.FolderFetcher();

            derivedFetcher.Initialize(this.Context, this.GetPath((string) null));

            return (Microsoft.Office365.SharePoint.FileServices.IFolderFetcher) derivedFetcher;

        }

    }

    public partial class Item

    {

        Microsoft.Office365.SharePoint.FileServices.IFileFetcher Microsoft.Office365.SharePoint.FileServices.IItemFetcher.ToFile()

        {

            return (Microsoft.Office365.SharePoint.FileServices.IFileFetcher) this;

        }

        Microsoft.Office365.SharePoint.FileServices.IFolderFetcher Microsoft.Office365.SharePoint.FileServices.IItemFetcher.ToFolder()

        {

            return (Microsoft.Office365.SharePoint.FileServices.IFolderFetcher) this;

        }

    }

    [global::Microsoft.OData.Client.Key("id")]

    public partial class File:Microsoft.Office365.SharePoint.FileServices.Item, Microsoft.Office365.SharePoint.FileServices.IFile, Microsoft.Office365.SharePoint.FileServices.IFileFetcher

    {

        private System.String _contentUrl;

        public System.String ContentUrl

        {

            get

            {

                return _contentUrl;

            }

            set

            {

                if (value != _contentUrl)

                {

                    _contentUrl = value;

                    OnPropertyChanged("contentUrl");

                }

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use ContentUrl instead.")]

        public System.String contentUrl

        {

            get

            {

                return ContentUrl;

            }

            set

            {

                ContentUrl = value;

            }

        }

        public File()

        {

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFile> copyAsync(System.String destFolderId, System.String destFolderPath, System.String newName)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/copy");

            return (Microsoft.Office365.SharePoint.FileServices.IFile) Enumerable.Single<Microsoft.Office365.SharePoint.FileServices.File>(await this.Context.ExecuteAsync<Microsoft.Office365.SharePoint.FileServices.File>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("destFolderId", (object) destFolderId),

                new BodyOperationParameter("destFolderPath", (object) destFolderPath),

                new BodyOperationParameter("newName", (object) newName),

            }

            ));

        }

        public async System.Threading.Tasks.Task uploadContentAsync(Microsoft.OData.Client.DataServiceStreamLink contentStream)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/uploadContent");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("contentStream", (object) contentStream),

            }

            );

        }

        public async System.Threading.Tasks.Task<Microsoft.OData.Client.DataServiceStreamLink> contentAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/content");

            return (Microsoft.OData.Client.DataServiceStreamLink) Enumerable.Single<Microsoft.OData.Client.DataServiceStreamLink>(await this.Context.ExecuteAsync<Microsoft.OData.Client.DataServiceStreamLink>(requestUri, "GET", true, new OperationParameter[]

            {

            }

            ));

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IFile> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.SharePoint.FileServices.File, Microsoft.Office365.SharePoint.FileServices.IFile>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IFile> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFile> Microsoft.Office365.SharePoint.FileServices.IFileFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.SharePoint.FileServices.IFile>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.SharePoint.FileServices.IFileFetcher Microsoft.Office365.SharePoint.FileServices.IFileFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.SharePoint.FileServices.IFile, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.SharePoint.FileServices.IFileFetcher) this;

        }

    }

    internal partial class FileFetcher:Microsoft.Office365.SharePoint.FileServices.ItemFetcher, Microsoft.Office365.SharePoint.FileServices.IFileFetcher

    {

        public FileFetcher()

        {

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFile> copyAsync(System.String destFolderId, System.String destFolderPath, System.String newName)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/copy");

            return (Microsoft.Office365.SharePoint.FileServices.IFile) Enumerable.Single<Microsoft.Office365.SharePoint.FileServices.File>(await this.Context.ExecuteAsync<Microsoft.Office365.SharePoint.FileServices.File>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("destFolderId", (object) destFolderId),

                new BodyOperationParameter("destFolderPath", (object) destFolderPath),

                new BodyOperationParameter("newName", (object) newName),

            }

            ));

        }

        public async System.Threading.Tasks.Task uploadContentAsync(Microsoft.OData.Client.DataServiceStreamLink contentStream)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/uploadContent");

            await this.Context.ExecuteAsync(requestUri, "POST", new OperationParameter[1]

            {

                new BodyOperationParameter("contentStream", (object) contentStream),

            }

            );

        }

        public async System.Threading.Tasks.Task<Microsoft.OData.Client.DataServiceStreamLink> contentAsync()

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/content");

            return (Microsoft.OData.Client.DataServiceStreamLink) Enumerable.Single<Microsoft.OData.Client.DataServiceStreamLink>(await this.Context.ExecuteAsync<Microsoft.OData.Client.DataServiceStreamLink>(requestUri, "GET", true, new OperationParameter[]

            {

            }

            ));

        }

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFile> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.SharePoint.FileServices.IFileFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.SharePoint.FileServices.IFile, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.SharePoint.FileServices.IFileFetcher) new Microsoft.Office365.SharePoint.FileServices.FileFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IFile> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.SharePoint.FileServices.File, Microsoft.Office365.SharePoint.FileServices.IFile>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IFile> _query;

    }

    internal partial class FileCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.SharePoint.FileServices.IFile>, Microsoft.Office365.SharePoint.FileServices.IFileCollection

    {

        internal FileCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.SharePoint.FileServices.IFileFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.SharePoint.FileServices.IFile>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddFileAsync(Microsoft.Office365.SharePoint.FileServices.IFile item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.SharePoint.FileServices.IFileFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.SharePoint.FileServices.File>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.SharePoint.FileServices.FileFetcher();

                fetcher.Initialize(Context, path);

                

                return fetcher;

            }

        }

    }

    internal partial class FileCollection

    {

        public global::System.Threading.Tasks.Task<System.Int64> CountAsync()

        {

            return new DataServiceQuerySingle<long>(Context, _path + "/$count").GetValueAsync();

        }

    }

    [global::Microsoft.OData.Client.Key("id")]

    public partial class Folder:Microsoft.Office365.SharePoint.FileServices.Item, Microsoft.Office365.SharePoint.FileServices.IFolder, Microsoft.Office365.SharePoint.FileServices.IFolderFetcher

    {

        private Microsoft.Office365.SharePoint.FileServices.ItemCollection _childrenCollection;

        private System.Int32 _childCount;

        private Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.SharePoint.FileServices.Item> _childrenConcrete;

        public System.Int32 ChildCount

        {

            get

            {

                return _childCount;

            }

            set

            {

                if (value != _childCount)

                {

                    _childCount = value;

                    OnPropertyChanged("childCount");

                }

            }

        }

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.SharePoint.FileServices.IItem> Microsoft.Office365.SharePoint.FileServices.IFolder.Children

        {

            get

            {

                return new Microsoft.OData.ProxyExtensions.PagedCollection<Microsoft.Office365.SharePoint.FileServices.IItem, Microsoft.Office365.SharePoint.FileServices.Item>(Context, (DataServiceCollection<Microsoft.Office365.SharePoint.FileServices.Item>) Children);

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use ChildCount instead.")]

        public System.Int32 childCount

        {

            get

            {

                return ChildCount;

            }

            set

            {

                ChildCount = value;

            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [Obsolete("Use Children instead.")]

        public global::System.Collections.Generic.IList<Microsoft.Office365.SharePoint.FileServices.Item> children

        {

            get

            {

                return Children;

            }

            set

            {

                Children = value;

            }

        }

        public global::System.Collections.Generic.IList<Microsoft.Office365.SharePoint.FileServices.Item> Children

        {

            get

            {

                if (this._childrenConcrete == null)

                {

                    this._childrenConcrete = new Microsoft.OData.ProxyExtensions.EntityCollectionImpl<Microsoft.Office365.SharePoint.FileServices.Item>();

                    this._childrenConcrete.SetContainer(() => GetContainingEntity("children"));

                }

                

                return (global::System.Collections.Generic.IList<Microsoft.Office365.SharePoint.FileServices.Item>)this._childrenConcrete;

            }

            set

            {

                if (this.Context == null)

                    throw new InvalidOperationException("Not Initialized");

                Children.Clear();

                if (value != null)

                {

                    foreach (var i in value)

                    {

                        Children.Add(i);

                    }

                }

            }

        }

        Microsoft.Office365.SharePoint.FileServices.IItemCollection Microsoft.Office365.SharePoint.FileServices.IFolderFetcher.Children

        {

            get

            {

                if (this._childrenCollection == null)

                {

                    this._childrenCollection = new Microsoft.Office365.SharePoint.FileServices.ItemCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.SharePoint.FileServices.Item>(GetPath("children")) : null,
                       Context,
                       this,
                       GetPath("children"));

                }

                

                return this._childrenCollection;

            }

        }

        public Folder()

        {

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFolder> copyAsync(System.String destFolderId, System.String destFolderPath, System.String newName)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/copy");

            return (Microsoft.Office365.SharePoint.FileServices.IFolder) Enumerable.Single<Microsoft.Office365.SharePoint.FileServices.Folder>(await this.Context.ExecuteAsync<Microsoft.Office365.SharePoint.FileServices.Folder>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("destFolderId", (object) destFolderId),

                new BodyOperationParameter("destFolderPath", (object) destFolderPath),

                new BodyOperationParameter("newName", (object) newName),

            }

            ));

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IFolder> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.SharePoint.FileServices.Folder, Microsoft.Office365.SharePoint.FileServices.IFolder>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IFolder> _query;

        global::System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFolder> Microsoft.Office365.SharePoint.FileServices.IFolderFetcher.ExecuteAsync()

        {

            var tsc = new global::System.Threading.Tasks.TaskCompletionSource<Microsoft.Office365.SharePoint.FileServices.IFolder>();

            tsc.SetResult(this);

            return tsc.Task;

        }

        Microsoft.Office365.SharePoint.FileServices.IFolderFetcher Microsoft.Office365.SharePoint.FileServices.IFolderFetcher.Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.SharePoint.FileServices.IFolder, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.SharePoint.FileServices.IFolderFetcher) this;

        }

    }

    internal partial class FolderFetcher:Microsoft.Office365.SharePoint.FileServices.ItemFetcher, Microsoft.Office365.SharePoint.FileServices.IFolderFetcher

    {

        private Microsoft.Office365.SharePoint.FileServices.ItemCollection _childrenCollection;

        public Microsoft.Office365.SharePoint.FileServices.IItemCollection Children

        {

            get

            {

                if (this._childrenCollection == null)

                {

                    this._childrenCollection = new Microsoft.Office365.SharePoint.FileServices.ItemCollection(
                       Context != null ? Context.CreateQuery<Microsoft.Office365.SharePoint.FileServices.Item>(GetPath("children")) : null,
                       Context,
                       this,
                       GetPath("children"));

                }

                

                return this._childrenCollection;

            }

        }

        public FolderFetcher()

        {

        }

        public async System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFolder> copyAsync(System.String destFolderId, System.String destFolderPath, System.String newName)

        {

            if (this.Context == null)

                throw new InvalidOperationException("Not Initialized");

            Uri myUri = this.GetUrl();

            if (myUri == (Uri) null)

             throw new Exception("cannot find entity");

            Uri requestUri = new Uri(myUri.ToString().TrimEnd('/') + "/copy");

            return (Microsoft.Office365.SharePoint.FileServices.IFolder) Enumerable.Single<Microsoft.Office365.SharePoint.FileServices.Folder>(await this.Context.ExecuteAsync<Microsoft.Office365.SharePoint.FileServices.Folder>(requestUri, "POST", true, new OperationParameter[]

            {

                new BodyOperationParameter("destFolderId", (object) destFolderId),

                new BodyOperationParameter("destFolderPath", (object) destFolderPath),

                new BodyOperationParameter("newName", (object) newName),

            }

            ));

        }

        public async global::System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFolder> ExecuteAsync()

        {

            return await EnsureQuery().ExecuteSingleAsync();

        }

        public Microsoft.Office365.SharePoint.FileServices.IFolderFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.SharePoint.FileServices.IFolder, TTarget>> navigationPropertyAccessor)

        {

            return (Microsoft.Office365.SharePoint.FileServices.IFolderFetcher) new Microsoft.Office365.SharePoint.FileServices.FolderFetcher()

            {

                _query = this.EnsureQuery().Expand<TTarget>(navigationPropertyAccessor)

            }

            ;

        }

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IFolder> EnsureQuery()

        {

            if (this._query == null)

            {

                this._query = CreateQuery<Microsoft.Office365.SharePoint.FileServices.Folder, Microsoft.Office365.SharePoint.FileServices.IFolder>();

            }

            return this._query;

        }

        

        private Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSet<Microsoft.Office365.SharePoint.FileServices.IFolder> _query;

    }

    internal partial class FolderCollection:Microsoft.OData.ProxyExtensions.QueryableSet<Microsoft.Office365.SharePoint.FileServices.IFolder>, Microsoft.Office365.SharePoint.FileServices.IFolderCollection

    {

        internal FolderCollection(global::Microsoft.OData.Client.DataServiceQuery inner,Microsoft.OData.ProxyExtensions.DataServiceContextWrapper context,object entity,string path): base(inner, context, entity as Microsoft.OData.ProxyExtensions.EntityBase, path)

        {

        }

        public Microsoft.Office365.SharePoint.FileServices.IFolderFetcher GetById(System.String id)

        {

            return this[id];

        }

        public global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.SharePoint.FileServices.IFolder>> ExecuteAsync()

        {

            return ExecuteAsyncInternal();

        }

        public global::System.Threading.Tasks.Task AddFolderAsync(Microsoft.Office365.SharePoint.FileServices.IFolder item, System.Boolean dontSave = false)

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

        public Microsoft.Office365.SharePoint.FileServices.IFolderFetcher this[System.String id]

        {

            get

            {

                var path = GetPath<Microsoft.Office365.SharePoint.FileServices.Folder>((i) => i.Id == id);

                var fetcher = new Microsoft.Office365.SharePoint.FileServices.FolderFetcher();

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

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IDrive:Microsoft.OData.ProxyExtensions.IEntityBase

    {

        System.String Id
        {get;set;}

        Microsoft.Office365.SharePoint.FileServices.Identity Owner
        {get;set;}

        Microsoft.Office365.SharePoint.FileServices.DriveQuota Quota
        {get;set;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IDriveFetcher

    {

        global::System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IDrive> ExecuteAsync();

        Microsoft.Office365.SharePoint.FileServices.IDriveFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.SharePoint.FileServices.IDrive, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IDriveCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.SharePoint.FileServices.IDrive>

    {

        Microsoft.Office365.SharePoint.FileServices.IDriveFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.SharePoint.FileServices.IDrive>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddDriveAsync(Microsoft.Office365.SharePoint.FileServices.IDrive item, System.Boolean dontSave = false);

         Microsoft.Office365.SharePoint.FileServices.IDriveFetcher this[System.String id]

        {get;}

    }

    public partial interface IDriveCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    public partial interface IDriveFetcher

    {

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItem:Microsoft.OData.ProxyExtensions.IEntityBase

    {

        Microsoft.Office365.SharePoint.FileServices.IdentitySet CreatedBy
        {get;set;}

        System.String ETag
        {get;set;}

        System.String Id
        {get;set;}

        Microsoft.Office365.SharePoint.FileServices.IdentitySet LastModifiedBy
        {get;set;}

        System.String Name
        {get;set;}

        Microsoft.Office365.SharePoint.FileServices.ItemReference ParentReference
        {get;set;}

        System.Int64 Size
        {get;set;}

        System.DateTimeOffset DateTimeCreated
        {get;set;}

        System.DateTimeOffset DateTimeLastModified
        {get;set;}

        System.String Type
        {get;set;}

        System.String WebUrl
        {get;set;}

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemFetcher

    {

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IItemCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.SharePoint.FileServices.IItem>

    {

        System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IItem> getByPathAsync(System.String path);

        Microsoft.Office365.SharePoint.FileServices.IItemFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.SharePoint.FileServices.IItem>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddItemAsync(Microsoft.Office365.SharePoint.FileServices.IItem item, System.Boolean dontSave = false);

         Microsoft.Office365.SharePoint.FileServices.IItemFetcher this[System.String id]

        {get;}

    }

    public partial interface IItemCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    public partial interface IItemFetcher

    {

        Microsoft.Office365.SharePoint.FileServices.IFileFetcher ToFile();

        Microsoft.Office365.SharePoint.FileServices.IFolderFetcher ToFolder();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFile:Microsoft.Office365.SharePoint.FileServices.IItem

    {

        System.String ContentUrl
        {get;set;}

        System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFile> copyAsync(System.String destFolderId, System.String destFolderPath, System.String newName);

        System.Threading.Tasks.Task uploadContentAsync(Microsoft.OData.Client.DataServiceStreamLink contentStream);

        System.Threading.Tasks.Task<Microsoft.OData.Client.DataServiceStreamLink> contentAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFileFetcher:Microsoft.Office365.SharePoint.FileServices.IItemFetcher

    {

        System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFile> copyAsync(System.String destFolderId, System.String destFolderPath, System.String newName);

        System.Threading.Tasks.Task uploadContentAsync(Microsoft.OData.Client.DataServiceStreamLink contentStream);

        System.Threading.Tasks.Task<Microsoft.OData.Client.DataServiceStreamLink> contentAsync();

        global::System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFile> ExecuteAsync();

        Microsoft.Office365.SharePoint.FileServices.IFileFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.SharePoint.FileServices.IFile, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFileCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.SharePoint.FileServices.IFile>

    {

        Microsoft.Office365.SharePoint.FileServices.IFileFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.SharePoint.FileServices.IFile>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddFileAsync(Microsoft.Office365.SharePoint.FileServices.IFile item, System.Boolean dontSave = false);

         Microsoft.Office365.SharePoint.FileServices.IFileFetcher this[System.String id]

        {get;}

    }

    public partial interface IFileCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFolder:Microsoft.Office365.SharePoint.FileServices.IItem

    {

        System.Int32 ChildCount
        {get;set;}

        Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.SharePoint.FileServices.IItem> Children
        {get;}

        System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFolder> copyAsync(System.String destFolderId, System.String destFolderPath, System.String newName);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFolderFetcher:Microsoft.Office365.SharePoint.FileServices.IItemFetcher

    {

        Microsoft.Office365.SharePoint.FileServices.IItemCollection Children
        {get;}

        System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFolder> copyAsync(System.String destFolderId, System.String destFolderPath, System.String newName);

        global::System.Threading.Tasks.Task<Microsoft.Office365.SharePoint.FileServices.IFolder> ExecuteAsync();

        Microsoft.Office365.SharePoint.FileServices.IFolderFetcher Expand<TTarget>(System.Linq.Expressions.Expression<System.Func<Microsoft.Office365.SharePoint.FileServices.IFolder, TTarget>> navigationPropertyAccessor);

    }

    [Microsoft.OData.ProxyExtensions.LowerCaseProperty]

    public partial interface IFolderCollection:Microsoft.OData.ProxyExtensions.IReadOnlyQueryableSetBase<Microsoft.Office365.SharePoint.FileServices.IFolder>

    {

        Microsoft.Office365.SharePoint.FileServices.IFolderFetcher GetById(System.String id);

        global::System.Threading.Tasks.Task<Microsoft.OData.ProxyExtensions.IPagedCollection<Microsoft.Office365.SharePoint.FileServices.IFolder>> ExecuteAsync();

        global::System.Threading.Tasks.Task AddFolderAsync(Microsoft.Office365.SharePoint.FileServices.IFolder item, System.Boolean dontSave = false);

         Microsoft.Office365.SharePoint.FileServices.IFolderFetcher this[System.String id]

        {get;}

    }

    public partial interface IFolderCollection

    {

        global::System.Threading.Tasks.Task<System.Int64> CountAsync();

    }

}

