// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions.Lite
{
    public class MediaEntityBase : EntityBase, IMediaEntityBase
    {
        /// <param name="stream">The stream content to upload.</param>
        /// <param name="contentType">The content type of the stream content.</param>
        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        /// <param name="closeStream">true to close stream after the upload is complete; false to leave stream open.</param>
        public Task UploadMediaAsync(Stream stream, string contentType, bool deferSaveChanges = false, bool closeStream = false)
        {
            var args = new Client.DataServiceRequestArgs
            {
                ContentType = contentType
            };

            Context.SetSaveStream(this, stream, closeStream, args);

            return SaveChangesAsync(deferSaveChanges);
        }

        public Task<Client.DataServiceStreamResponse> DownloadMediaAsync()
        {
            return Context.GetReadStreamAsync(this, null);
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        /// <param name="saveChangesOption">Save changes option to control how change requests are sent to the service.</param>
        public Task SaveChangesAsync(bool deferSaveChanges = false, SaveChangesOptions saveChangesOption = SaveChangesOptions.None)
        {
            if (deferSaveChanges)
            {
                var retVal = new TaskCompletionSource<object>();
                retVal.SetResult(null);
                return retVal.Task;
            }

            return Context.SaveChangesAsync(saveChangesOption);
        }
    }
}
