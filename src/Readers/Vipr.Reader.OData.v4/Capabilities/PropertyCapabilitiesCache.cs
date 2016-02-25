// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Vipr.Reader.OData.v4.Capabilities
{
    /// <summary>
    /// Maintains a cache of OdcmProperties along with the OdcmCapabilities.
    /// Once fully populated it can be used to create 'Projections' for the cached OdcmProperties
    /// </summary>
    public class PropertyCapabilitiesCache
    {
        private Dictionary<OdcmProperty, IEnumerable<OdcmCapability>> _propertyCache =
            new Dictionary<OdcmProperty, IEnumerable<OdcmCapability>>();

        public void Add(OdcmProperty property, IEnumerable<OdcmCapability> capabilities)
        {
            if (property == null) throw new ArgumentNullException("property");
            if (capabilities == null) throw new ArgumentNullException("capabilities");

            _propertyCache.Add(property, capabilities);
        }

        public IEnumerable<OdcmCapability> GetCapabilities(OdcmProperty property)
        {
            if (property == null) throw new ArgumentNullException("property");

            IEnumerable<OdcmCapability> capabilities;
            if (!_propertyCache.TryGetValue(property, out capabilities))
            {
                throw new InvalidOperationException(string.Format("Property {0}.{1} not found in the cache",
                    property.Class.Name, property.Name));
            }

            return capabilities;
        }

        public void EnsureProjectionsForProperties()
        {
            foreach (var propertyPair in _propertyCache)
            {
                var property = propertyPair.Key;
                var capabilities = propertyPair.Value;
                var propertyType = property.Projection.Type;
                property.Projection = propertyType.GetProjection(capabilities);
            }
        }
    }
}
