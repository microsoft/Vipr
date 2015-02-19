// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Threading.Tasks;

namespace Microsoft.OData.ProxyExtensions
{
    public interface IMediaEntityBase : IEntityBase
    {
        /// <param name="stream">The stream content to upload.</param>
        /// <param name="contentType">The content type of the stream content.</param>
        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        /// <param name="closeStream">true to close stream after the upload is complete; false to leave stream open.</param>
        Task UploadMediaAsync(Stream stream, string contentType, bool deferSaveChanges = false, bool closeStream = false);

        Task<Client.DataServiceStreamResponse> DownloadMediaAsync();
    }
}
