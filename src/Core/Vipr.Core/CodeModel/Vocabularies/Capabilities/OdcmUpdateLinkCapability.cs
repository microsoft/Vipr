// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public class OdcmUpdateLinkCapability : OdcmBooleanCapability
    {
        public override string TermName
        {
            get { return "Org.OData.Capabilities.V1.UpdateRestrictions/NonUpdatableNavigationProperties"; }
        }

        /// <summary>
        /// Reference/link to an entity can be updated
        /// </summary>
        public bool Updatable
        {
            get { return Value; }
        }

        public OdcmUpdateLinkCapability()
        {
            Value = true;
        }
    }
}
