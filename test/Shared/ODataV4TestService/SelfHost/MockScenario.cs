// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using ODataV4TestService.Handlers;

namespace ODataV4TestService.SelfHost
{
    public class MockScenario : ScenarioBase<MockScenario>
    {
        private readonly IMockHandlerAgent _mockHandlerAgent;
        public MockScenario(IMockHandlerAgent mockHandlerAgent)
        {
            _mockHandlerAgent = mockHandlerAgent;
        }

        public new IStartedScenario Start()
        {
            base.Start();

            GetHttpConfiguration().MessageHandlers.Add(new MockHandler(_mockHandlerAgent));

            return this;
        }
    }
}