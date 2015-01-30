// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using ODataV4TestService.Handlers;

namespace ODataV4TestService.SelfHost
{
    public class ProxyScenario : ScenarioBase<ProxyScenario>
    {
        private readonly Uri _baseUri;

        public ProxyScenario(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public new IStartedScenario Start()
        {
            base.Start();

            GetHttpConfiguration().MessageHandlers.Add(new ProxyHandler(_baseUri));

            return this;
        }
    }
}