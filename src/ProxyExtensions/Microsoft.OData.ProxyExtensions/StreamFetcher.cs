// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Threading.Tasks;

namespace Microsoft.OData.ProxyExtensions
{
    public class StreamFetcher : IStreamFetcher
    {
        private readonly Client.DataServiceStreamLink _link;
        private readonly EntityBase _entity;
        private readonly string _propertyName;
        private readonly DataServiceContextWrapper _context;

        public string ContentType
        {
            get
            {
                return _link.ContentType;
            }
        }

        public StreamFetcher(DataServiceContextWrapper context, EntityBase entity, string propertyName, Client.DataServiceStreamLink link)
        {
            _context = context;
            _entity = entity;
            _link = link;
            _propertyName = propertyName;
        }

        /// <param name="stream">The stream content to upload.</param>
        /// <param name="contentType">The content type of the stream content.</param>
        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        /// <param name="closeStream">true to close stream after the upload is complete; false to leave stream open.</param>
        public Task UploadAsync(Stream stream, string contentType, bool deferSaveChanges = false, bool closeStream = false)
        {
            var args = new Client.DataServiceRequestArgs
            {
                ContentType = contentType
            };

            if (_link.ETag != null)
            {
                args.Headers.Add("If-Match", _link.ETag);
            }

            _context.SetSaveStream(_entity, _propertyName, stream, closeStream, args);

            _entity.OnPropertyChanged(_propertyName);

            return _entity.SaveChangesAsync(deferSaveChanges);
        }

        public Task<Client.DataServiceStreamResponse> DownloadAsync()
        {
            return _context.GetReadStreamAsync(_entity, _propertyName, ContentType);
        }
    }
}
