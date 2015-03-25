// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public class OdcmInsertCapability : OdcmBooleanCapability
    {
        public override string TermName
        {
            get { return "Org.OData.Capabilities.V1.InsertRestrictions"; }
        }

        /// <summary>
        /// Entities can be inserted
        /// </summary>
        public bool Insertable
        {
            get { return BooleanValue; }
        }

        public OdcmInsertCapability()
        {
            BooleanValue = true;
        }

        public override bool Equals(OdcmCapability otherCapability)
        {
            var other = otherCapability as OdcmInsertCapability;
            if (other == null)
            {
                return false;
            }

            return Insertable == other.Insertable;
         }
    }
}
