// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel
{
    public abstract class OdcmObject
    {
        public string Name { get; private set; }

        public OdcmObject(string name)
        {
            Name = name;
        }
    }
}
