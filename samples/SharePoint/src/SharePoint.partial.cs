using Microsoft.OData.Client;

#if false

// Using client runtime (GetReadStreamAsync) for streams would not enable us to use actions with stream return values on fetchers

namespace SP.Extensions
{
    partial class DataServiceContextWrapper
    {
        partial void OnCreated()
        {
            Configurations.ResponsePipeline.OnEntryStarted(new Action<ReadingEntryArgs>(e2 =>
            {
                if (e2.Entry.TypeName == "MS.FileServices.File")
                {
                    e2.Entry.MediaResource = new ODataStreamReferenceValue() { ReadLink = new Uri(e2.Entry.Id + "/Download()") };
                }
            }));

        }

    }
}

namespace MS.FileServices
{
    partial class File
    {
        public async global::System.Threading.Tasks.Task<System.IO.Stream> DownloadAsync()
        {
            var myUri = GetUrl();
            if (myUri == null)
            {
                throw new global::System.Exception("cannot find entity");
            }

            var result = await Context.GetReadStreamAsync(this, string.Empty, "application/octet-stream");

            return result.Stream;
        }
    }

    partial class FileFetcher
    {
/*        public async global::System.Threading.Tasks.Task<System.IO.Stream> DownloadAsync()
        {
            var myUri = GetUrl();
            if (myUri == null)
            {
                throw new global::System.Exception("cannot find entity");
            }

            global::System.Uri requestUriTmp = new global::System.Uri(myUri.ToString() + "/" + "Download" + "(" + ")");


            var result = await Context.GetReadStreamAsync(this, string.Empty, "application/octet-stream");

            return result.Stream;
        }*/


    }
}


#else
namespace Microsoft.Office365.SharePoint.FileServices
{
    partial interface IFileFetcher
    {
        global::System.Threading.Tasks.Task<System.IO.Stream> DownloadAsync();

        global::System.Threading.Tasks.Task UploadAsync(global::System.IO.Stream stream);
    }

    partial class File
    {
        public global::System.Threading.Tasks.Task<System.IO.Stream> DownloadAsync()
        {
            var myUri = GetUrl();
            if (myUri == null)
            {
                throw new global::System.Exception("cannot find entity");
            }

            global::System.Uri requestUriTmp = new global::System.Uri(myUri.ToString() + "/" + "Content" + "(" + ")");


            return Context.GetStreamAsync(requestUriTmp);
        }

        public async global::System.Threading.Tasks.Task UploadAsync(global::System.IO.Stream stream)
        {
            try
            {
                var myUri = GetUrl();
                if (myUri == null)
                {
                    throw new global::System.Exception("cannot find entity");
                }
                global::System.Uri requestUriTmp = new global::System.Uri(myUri.ToString() + "/" + "UploadContent" + "(" + ")");

                await Context.ExecuteAsync(requestUriTmp, "POST", stream,
                    new global::Microsoft.OData.Client.OperationParameter[] { });
            }
            catch (System.InvalidOperationException)
            {
                // Upload for some reason returns a very strange payload which causes the InvalidOperationException we can safely ignore
                // <?xml version="1.0" encoding="utf-8"?><d:Upload xmlns:d="http://schemas.microsoft.com/ado/2007/08/dataservices" xmlns:m="http://schemas.microsoft.com/ado/2007/08/dataservices/metadata" xmlns:georss="http://www.georss.org/georss" xmlns:gml="http://www.opengis.net/gml" m:null="true" />
            }
        }
    }

    partial class FileFetcher
    {
        public global::System.Threading.Tasks.Task<System.IO.Stream> DownloadAsync()
        {
            var myUri = GetUrl();
            if (myUri == null)
            {
                throw new global::System.Exception("cannot find entity");
            }

            global::System.Uri requestUriTmp = new global::System.Uri(myUri.ToString() + "/" + "Content" + "(" + ")");


            return Context.GetStreamAsync(requestUriTmp);
        }

        public async global::System.Threading.Tasks.Task UploadAsync(global::System.IO.Stream stream)
        {
            try
            {
                var myUri = GetUrl();
                if (myUri == null)
                {
                    throw new global::System.Exception("cannot find entity");
                }
                global::System.Uri requestUriTmp = new global::System.Uri(myUri.ToString() + "/" + "UploadContent" + "(" + ")");

                await Context.ExecuteAsync(requestUriTmp, "POST", stream,
                    new global::Microsoft.OData.Client.OperationParameter[] { });
            }
            catch (System.InvalidOperationException)
            {
                // Upload for some reason returns a very strange payload which causes the InvalidOperationException we can safely ignore
                // <?xml version="1.0" encoding="utf-8"?><d:Upload xmlns:d="http://schemas.microsoft.com/ado/2007/08/dataservices" xmlns:m="http://schemas.microsoft.com/ado/2007/08/dataservices/metadata" xmlns:georss="http://www.georss.org/georss" xmlns:gml="http://www.opengis.net/gml" m:null="true" />
            }
        }
    }

    partial class ItemCollection
    {
        // Bug - currently SharePoint does not return a location header
        public async global::System.Threading.Tasks.Task AddFolderAsync(IFolder item)
        {
            if (_entity == null)
            {
                Context.AddObject(_path, item);
            }
            else
            {
                Context.AddLink(_entity, _path, item);
            }

            try
            {
                await Context.SaveChangesAsync(options: SaveChangesOptions.ContinueOnError);
            }
            catch
            {
                Context.Detach(item);
            }
        }
    }

    partial interface IFileSystemItemCollection
    {
        global::System.Threading.Tasks.Task AddFolderAsync(IFolder item);
    }
}

#endif
