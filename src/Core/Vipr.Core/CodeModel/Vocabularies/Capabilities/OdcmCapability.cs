// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public abstract class OdcmCapability : IEquatable<OdcmCapability>
    {
        public abstract string TermName { get; }

        public abstract bool Equals(OdcmCapability otherCapability);
    }
}
