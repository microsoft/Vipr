// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Web.Http;
using Owin;

namespace ODataV4TestService.SelfHost
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            var portNumber =
                Int32.Parse(
                    (string)((IList<IDictionary<string, object>>)appBuilder.Properties["host.Addresses"])[0]["port"]);

            var configuration = ScenarioRepository.GetHttpConfiguration(portNumber);

            if (configuration == null)
            {
                configuration = new HttpConfiguration();

                WebApiConfig.Register(configuration);
            }

            appBuilder.UseWebApi(configuration);
        }
    }
}