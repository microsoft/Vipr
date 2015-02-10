// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Microsoft.MockService.SelfHost
{
    public static class MockServiceRepository
    {
        private static readonly IDictionary<int, MockService> s_Scenarios =
            new Dictionary<int, MockService>();
        
        internal static MockService GetScenario(int portNumber)
        {
            return s_Scenarios.ContainsKey(portNumber) ? s_Scenarios[portNumber] : null;
        }

        internal static void Register(int portNumber, MockService startedScenario)
        {
            lock (s_Scenarios)
            {
                if (s_Scenarios.ContainsKey(portNumber))
                    throw new InvalidOperationException("port in use");

                s_Scenarios[portNumber] = startedScenario;
            }
        }

        internal static void Unregister(int portNumber)
        {
            lock (s_Scenarios)
            {
                if (!s_Scenarios.ContainsKey(portNumber))
                    throw new InvalidOperationException("port not registered");

                s_Scenarios.Remove(portNumber);
            }
        }
    }
}