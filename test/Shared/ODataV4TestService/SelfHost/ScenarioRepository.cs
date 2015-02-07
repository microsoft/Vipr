// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Services.Description;

namespace ODataV4TestService.SelfHost
{
    public static class ScenarioRepository
    {
        private static readonly IDictionary<int, IStartedScenario> s_Scenarios =
            new Dictionary<int, IStartedScenario>();

        internal static HttpConfiguration GetHttpConfiguration(int portNumber)
        {
            return s_Scenarios.ContainsKey(portNumber) ? s_Scenarios[portNumber].GetHttpConfiguration() : null;
        }

        internal static IStartedScenario GetScenario(int portNumber)
        {
            return s_Scenarios.ContainsKey(portNumber) ? s_Scenarios[portNumber] : null;
        }

        internal static void Register(int portNumber, IStartedScenario startedScenario)
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