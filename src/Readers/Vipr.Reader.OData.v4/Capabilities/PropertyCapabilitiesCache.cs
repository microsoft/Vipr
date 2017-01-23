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
        private IDictionary<OdcmObject, ICollection<OdcmCapability>> _propertyCache =
            new Dictionary<OdcmObject, ICollection<OdcmCapability>>();

        public IEnumerable<OdcmObject> AnnotatedObjects
        {
            get
            {
                return _propertyCache.Keys.Where(k => _propertyCache[k].Any());
            }
        }

        public void Add(OdcmObject property, ICollection<OdcmCapability> capabilities)
        {
            if (property == null) throw new ArgumentNullException("property");
            if (capabilities == null) throw new ArgumentNullException("capabilities");

            _propertyCache[property] = capabilities;
        }

        public ICollection<OdcmCapability> GetCapabilities(OdcmObject property)
        {
            if (property == null) throw new ArgumentNullException("property");

            ICollection<OdcmCapability> capabilities;
            if (!_propertyCache.TryGetValue(property, out capabilities))
            {
                throw new InvalidOperationException(
                    string.Format("Property {0} not found in the cache", property.Name));
            }

            return capabilities;
        }

        private IEnumerable<OdcmCapability> GetCapabilitiesOrEmpty(OdcmObject property)
        {
            ICollection<OdcmCapability> capabilities;
            if (!_propertyCache.TryGetValue(property, out capabilities))
            {
                return Enumerable.Empty<OdcmCapability>();
            }

            return capabilities;
        }

        public void EnsureProjectionsForProperties()
        {
            foreach (var propertyPair in _propertyCache)
            {
                var odcmProperty = propertyPair.Key;
                if (odcmProperty.Projection == null)
                {
                    continue;
                }

                var capabilities = propertyPair.Value;
                var propertyType = odcmProperty.Projection.Type;

                // EntityType might have some annotations defined
                if (propertyType is OdcmEntityClass)
                {
                    TryToMergeWithTypeCapabilities(propertyType, capabilities);
                }

                odcmProperty.Projection.Capabilities = capabilities;
            }
        }

        private void TryToMergeWithTypeCapabilities(OdcmType propertyType, ICollection<OdcmCapability> capabilities)
        {
            foreach (var typeCapability in GetCapabilitiesOrEmpty(propertyType))
            {
                if (capabilities.SingleOrDefault(c => c.TermName == typeCapability.TermName) == null)
                {
                    capabilities.Add(typeCapability);
                }
            }
        }

        public void CreateDistinctProjectionsForWellKnownBooleanTypes()
        {
            foreach (var propertyPair in _propertyCache.Where(x => x.Key is OdcmProperty))
            {
                var odcmProperty = propertyPair.Key;
                var propertyType = odcmProperty.Projection.Type;

                propertyType.AddProjection(propertyPair.Value, odcmProperty);
            }
        }
    }
}
