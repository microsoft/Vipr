// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public class OdcmDeleteLinkCapability : OdcmBooleanCapability
    {
        public override string TermName
        {
            get { return "Org.OData.Capabilities.V1.DeleteRestrictions/NonDeletableNavigationProperties"; }
        }

        public override string ShortName
        {
            get { return "Dlk"; }
        }

        /// <summary>
        /// Reference/link to an entity can be deleted
        /// </summary>
        public bool Deletable
        {
            get { return Value; }
        }

        public OdcmDeleteLinkCapability()
        {
            Value = true;
        }
    }
}
