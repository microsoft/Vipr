// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Vipr.Core.CodeModel
{
    public class OdcmProjection : IEquatable<OdcmProjection>
    {
        public OdcmType Type { get; set; }

        public IEnumerable<OdcmCapability> Capabilities { get; set; }

        public bool SupportsUpdate()
        {
            var capability = GetCapability<OdcmUpdateCapability>();
            return capability != null && capability.Updatable;
        }

        public bool SupportsUpdateLink()
        {
            var capability = GetCapability<OdcmUpdateLinkCapability>();
            return capability != null && capability.Updatable;
        }

        public bool SupportsDelete()
        {
            var capability = GetCapability<OdcmDeleteCapability>();
            return capability != null && capability.Deletable;
        }

        public bool SupportsDeleteLink()
        {
            var capability = GetCapability<OdcmDeleteLinkCapability>();
            return capability != null && capability.Deletable;
        }

        public bool SupportsInsert()
        {
            var capability = GetCapability<OdcmInsertCapability>();
            return capability != null && capability.Insertable;
        }

        public bool SupportsExpand()
        {
            var capability = GetCapability<OdcmExpandCapability>();
            return capability != null && capability.Expandable;
        }

        public bool ContainsAllCapabilities(IEnumerable<OdcmCapability> capabilities)
        {
            return capabilities.Count() == this.Capabilities.Count() &&
                   this.Capabilities.All(capabilities.Contains);
        }

        public bool Equals(OdcmProjection otherProjection)
        {
            return this.Type == otherProjection.Type && this.ContainsAllCapabilities(otherProjection.Capabilities);
        }

        public override int GetHashCode()
        {
            int hashcode = 17;
            if (Type != null)
            {
                hashcode = hashcode * 31 + Type.GetHashCode();
            }

            foreach (var capability in Capabilities)
            {
                hashcode = hashcode * 31 + capability.GetHashCode();
            }

            return hashcode;
        }

        public T GetCapability<T>() where T : OdcmCapability
        {
            return Capabilities.SingleOrDefault(c => c.GetType() == typeof(T)) as T;
        }
    }
}
