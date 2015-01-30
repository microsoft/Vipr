// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Threading;

namespace ODataV4TestService.Handlers
{
    public interface IMockHandlerAgent
    {
        HttpResponseMessage SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}