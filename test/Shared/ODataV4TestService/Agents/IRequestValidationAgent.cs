// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http;

namespace ODataV4TestService.Agents
{
    public interface IRequestValidationAgent
    {
        void Validate(HttpRequestMessage request);
    }
}
