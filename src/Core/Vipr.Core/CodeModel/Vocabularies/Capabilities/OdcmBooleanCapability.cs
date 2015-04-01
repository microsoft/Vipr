// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public abstract class OdcmBooleanCapability : OdcmCapability
    {
        public bool Value;

        public override bool Equals(OdcmCapability otherCapability)
        {
            var other = otherCapability as OdcmBooleanCapability;
            if (other == null)
            {
                return false;
            }

            return other.GetType() == this.GetType() && other.Value == this.Value;
        }
    }
}
