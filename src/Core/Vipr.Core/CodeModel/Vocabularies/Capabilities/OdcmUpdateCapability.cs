// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public class OdcmUpdateCapability : OdcmBooleanCapability
    {
        public override string TermName
        {
            get { return "Org.OData.Capabilities.V1.UpdateRestrictions"; }
        }

        public override string ShortName
        {
            get { return "Upd"; }
        }

        /// <summary>
        /// Entities can be updated
        /// </summary>
        public bool Updatable
        {
            get { return Value; }
        }

        public OdcmUpdateCapability()
        {
            Value = true;
        }
    }
}
