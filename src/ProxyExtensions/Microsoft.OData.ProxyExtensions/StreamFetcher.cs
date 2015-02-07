// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.OData.ProxyExtensions
{
    public class StreamFetcher : IStreamFetcher
    {
        private global::Microsoft.OData.Client.DataServiceStreamLink _link;
        private EntityBase _entity;
        private string _propertyName;
        private DataServiceContextWrapper _context;

        public string ContentType
        {
            get
            {
                return _link.ContentType;
            }
        }

        public StreamFetcher(DataServiceContextWrapper context, EntityBase entity, string propertyName, global::Microsoft.OData.Client.DataServiceStreamLink link)
        {
            _context = context;
            _entity = entity;
            _link = link;
            _propertyName = propertyName;
        }

        /// <param name=""dontSave"">true to delay saving until batch is saved; false to save immediately.</param>
        public global::System.Threading.Tasks.Task UploadAsync(global::System.IO.Stream stream, string contentType, bool dontSave = false, bool closeStream = false)
        {
            var args = new global::Microsoft.OData.Client.DataServiceRequestArgs
            {
                ContentType = contentType
            };

            if (_link.ETag != null)
            {
                args.Headers.Add("If-Match", _link.ETag);
            }

            _context.SetSaveStream(_entity, _propertyName, stream, closeStream, args);

            _entity.OnPropertyChanged(_propertyName);

            return _entity.SaveAsNeeded(dontSave);
        }

        public global::System.Threading.Tasks.Task<global::Microsoft.OData.Client.DataServiceStreamResponse> DownloadAsync()
        {
            return _context.GetReadStreamAsync(_entity, _propertyName, ContentType);
        }
    }
}
