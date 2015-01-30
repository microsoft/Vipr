// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ODataV4TestService
{
    // This new class is needed to avoid pipeline recreation.
    // For the DefaultODataBatchHandler an in-memory HttpServer instance is created by reusing the HttpConfiguration.
    // When this in-memory HttpServer is first initialized it tries to re-create the pipeline.
    // Therefore we are overriding the 'Initialize' method to see if the pipeline is already created.    
    // Issue - http://aspnetwebstack.codeplex.com/workitem/260
    // Workaround - http://trocolate.wordpress.com/2012/07/19/mitigate-issue-260-in-batching-scenario/
    public class BatchServer : HttpServer
    {
        private readonly HttpConfiguration _config;

        public BatchServer(HttpConfiguration configuration)
            : base(configuration)
        {
            _config = configuration;
        }

        protected override void Initialize()
        {
            var firstInPipeline = _config.MessageHandlers.FirstOrDefault();
            if (firstInPipeline != null && firstInPipeline.InnerHandler != null)
            {
                // this is required for custom handlers like 'RequestValidationHandler' and 'TracingMessageHandler' to work
                // on individual requests/responses within a single batch request/response.
                InnerHandler = firstInPipeline;
                // If you do not want custom handlers to work on individual requests/responses of a batch request/response
                // then comment the above line and uncomment the below line.
                // Note: The custom handlers will always work on the batch request itself but not on the 
                // individual requests/responses.
                //InnerHandler = new System.Web.Http.Dispatcher.HttpRoutingDispatcher(_config);
            }
            else
            {
                base.Initialize();
            }
        }
    }
}