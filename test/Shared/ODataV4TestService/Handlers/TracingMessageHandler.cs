// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace ODataV4TestService.Handlers
{
    internal class TracingMessageHandler : DelegatingHandler
    {
        public static readonly TraceSource TraceSource = new TraceSource("TracingMessageHandler", SourceLevels.All);

        static TracingMessageHandler()
        {
            TraceSource.Listeners.Clear();
            TraceSource.Listeners.AddRange(Trace.Listeners);
        }

        protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            TraceSource.TraceEvent(TraceEventType.Start, 0,
                @"
>>>REQUEST>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
{0} {1} HTTP/{2}
{3}
{4}
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>",
                request.Method, request.RequestUri, request.Version,
                String.Join("",
                            request.Headers.Select(
                                h =>
                                String.Format("{0}: {1}\r\n", h.Key,
                                              String.Join(", ", h.Value.Select(v => v.ToString()))))),
                request.Content == null ? "" : await request.Content.ReadAsStringAsync());

            var response = await base.SendAsync(request, cancellationToken);

            TraceSource.TraceEvent(TraceEventType.Stop, 0,
                @"
<<<RESPONSE<<<<<<<<<<<<<<<<<<<<<<<<<<<<
HTTP/{2} {0} {1}
{3}
{4}
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
                (int)response.StatusCode, response.ReasonPhrase, response.Version,
                String.Join("",
                            response.Headers.Select(
                                h =>
                                String.Format("{0}: {1}\r\n", h.Key,
                                              String.Join(", ", h.Value.Select(v => v.ToString()))))),
                response.Content == null ? "" : await response.Content.ReadAsStringAsync());

            return response;
        }
    }
}