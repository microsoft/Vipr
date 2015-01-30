// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using ODataV4TestService.Extensions;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace ODataV4TestService.Agents
{
    public class RequestValidationAgent : IRequestValidationAgent
    {
        public void Validate(HttpRequestMessage request)
        {
            //Add any code to validate the request here.
        }

        static public IRequestValidationAgent GetRegisteredOrNew(HttpConfiguration httpConfig)
        {
            IRequestValidationAgent retVal;

            if (httpConfig == null || httpConfig.DependencyResolver == null ||
                (retVal = httpConfig.DependencyResolver.GetServiceOrDefault<IRequestValidationAgent>()) == null)
                retVal = new RequestValidationAgent();

            return retVal;
        }
    }
}