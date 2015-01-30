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

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;

namespace ODataV4TestService.Handlers
{
    public class ProxyHandler : DelegatingHandler
    {
        private readonly Uri _baseUri;

        public ProxyHandler(Uri baseUri)
        {
            ServicePointManager.ServerCertificateValidationCallback = ErrorIgnoringCertHandler;

            var baseUriString = baseUri.OriginalString;

            _baseUri = new Uri(baseUriString.TrimEnd('/'));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var originalRequestUri = request.RequestUri;
            var originalHost = request.RequestUri.GetLeftPart(UriPartial.Authority);

            ConvertToOutboundRequest(request);

            var response = await new HttpClient().SendAsync(request, cancellationToken);

            var responseContent = await response.Content.ReadAsStringAsync();

            responseContent = responseContent.Replace(_baseUri.OriginalString, originalHost);

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(responseContent)) { Position = 0 };
            var newContent = new StreamContent(ms);

            //newContent.Headers.ContentType = response.Content.Headers.ContentType;

            //var newContent = new StringContent(responseContent, Encoding.UTF8, "application/json");

            //newContent.Headers.Clear();

            response.Content.Headers.ForEach(h => newContent.Headers.TryAddWithoutValidation(h.Key, h.Value));

            var newResponse = request.CreateResponse(response.StatusCode);//newContent);

            newResponse.Content = new StringContent(responseContent);

            //newResponse.Headers.Clear();

            //response.Headers.ForEach(h => newResponse.Headers.Add(h.Key, h.Value));

            newResponse.Headers.Location = originalRequestUri;

            //response.Content = newContent;

            return newResponse;
        }

        private void ConvertToOutboundRequest(HttpRequestMessage request)
        {
            //var overlap = _baseUri.OriginalString.Length > request.RequestUri.GetLeftPart(UriPartial.Authority).Length
            //    ? "/" + _baseUri.OriginalString.Substring(request.RequestUri.GetLeftPart(UriPartial.Authority).Length + 2)
            //    : "";
            //Console.WriteLine(overlap);
            var overlap = "";
            request.RequestUri = request.RequestUri.PathAndQuery.StartsWith(overlap) ?
                new Uri(_baseUri.OriginalString + request.RequestUri.PathAndQuery.Substring(overlap.Length)) :
                new Uri(_baseUri.OriginalString + request.RequestUri.PathAndQuery);

            request.Headers.Host = _baseUri.Host;

            Console.WriteLine("Updated Uri: " + request.RequestUri);

            if (request.Method == HttpMethod.Get)
                request.Content = null;
        }

        private static bool ErrorIgnoringCertHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
    }
}
