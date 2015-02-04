// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Vipr.Core.CodeModel
{
    [Flags]
    public enum OdcmAllowedVerbs
    {
        Delete = 1,
        Get = 2,
        Patch = 4,
        Post = 8,
        Put = 16,
        Any = Delete | Get | Patch | Post | Put
    }
}
