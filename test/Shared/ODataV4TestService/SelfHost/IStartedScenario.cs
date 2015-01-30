// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Web.Http;

namespace ODataV4TestService.SelfHost
{
    public interface IStartedScenario : IDisposable
    {
        HttpConfiguration GetHttpConfiguration();
        HttpClient GetHttpClient();
        string GetBaseAddress();
    }
}
