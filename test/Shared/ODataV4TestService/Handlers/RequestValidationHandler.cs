// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http;
using ODataV4TestService.Agents;

namespace ODataV4TestService.Handlers
{
    internal class RequestValidationHandler : DelegatingHandler
    {
        private IRequestValidationAgent _requestValidationAgent;

        public RequestValidationHandler(IRequestValidationAgent requestValidationAgent)
        {
            _requestValidationAgent = requestValidationAgent;
        }

        protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (_requestValidationAgent != null) _requestValidationAgent.Validate(request);

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}