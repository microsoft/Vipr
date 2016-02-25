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

        public override string ShortName
        {
            get { return "Ins"; }
        }

        /// <summary>
        /// Entities can be inserted
        /// </summary>
        public bool Insertable
        {
            get { return Value; }
        }

        public OdcmInsertCapability()
        {
            Value = true;
        }
    }
}
