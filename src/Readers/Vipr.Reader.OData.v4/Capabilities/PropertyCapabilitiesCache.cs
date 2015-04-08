// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Vipr.Reader.OData.v4.Capabilities
{
    public partial class ODataCapabilitiesReader
    {
        /// <summary>
        /// Maintains a cache of OdcmProperties along with the OdcmCapabilities. 
        /// Once fully populated it can be used to create 'Projections' for the cached OdcmProperties
        /// </summary>
        public class PropertyCapabilitiesCache
        {
            private Dictionary<OdcmProperty, List<OdcmCapability>> _propertyCache =
                new Dictionary<OdcmProperty, List<OdcmCapability>>();

            public void AddCapabilityToProperty(OdcmProperty property, OdcmCapability capability)
            {
                List<OdcmCapability> capabilities;
                if (!_propertyCache.TryGetValue(property, out capabilities))
                {
                    capabilities = new List<OdcmCapability>();
                    _propertyCache.Add(property, capabilities);
                }

                capabilities.Add(capability);
            }

            public void EnsureProjectionsForProperties()
            {
                foreach (var propertyPair in _propertyCache)
                {
                    var property = propertyPair.Key;
                    var capabilities = propertyPair.Value;
                    var propertyType = property.Type;
                    property.Projection = propertyType.GetProjection(capabilities);
                }
            }
        }
    }
}
