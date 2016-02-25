// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.OData.ProxyExtensions.Lite
{
    public interface IBatchElementResult
    {
        IPagedCollection SuccessResult { get; }
        Exception FailureResult { get; }
    }
}
