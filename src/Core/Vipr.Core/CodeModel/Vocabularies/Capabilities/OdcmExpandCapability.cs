// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public class OdcmExpandCapability : OdcmBooleanCapability
    {
        public override string TermName
        {
            get { return "Org.OData.Capabilities.V1.ExpandRestrictions"; }
        }

        /// <summary>
        /// $expand is supported
        /// </summary>
        public bool Expandable
        {
            get { return BooleanValue; }
        }

        public OdcmExpandCapability()
        {
            BooleanValue = true;
        }

        public override bool Equals(OdcmCapability otherCapability)
        {
            var other = otherCapability as OdcmExpandCapability;
            if (other == null)
            {
                return false;
            }

            return Expandable == other.Expandable;
         }
    }
}
