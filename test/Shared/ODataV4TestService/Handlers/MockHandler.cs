// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingHandler.cs" company="Microsoft">
//   copyright 2013
// </copyright>
// <summary>
//   Defines the LoggingHandler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.OData.Edm;

namespace ODataV4TestService.Handlers
{
    public class MockHandler : DelegatingHandler
    {
        private readonly IMockHandlerAgent _mockHandlerAgent;

        public MockHandler(IMockHandlerAgent mockHandlerAgent)
        {
            _mockHandlerAgent = mockHandlerAgent;

            ServicePointManager.ServerCertificateValidationCallback = ErrorIgnoringCertHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = _mockHandlerAgent.SendAsync(request, cancellationToken);

            return response;
        }

        private static bool ErrorIgnoringCertHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
    }
}
