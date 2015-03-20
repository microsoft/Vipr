// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace ODataReader.v4.Capabilities
{
    public class ODataCapabilitiesReader
    {
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

            public void CreateProjectionsForProperties()
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

        private static List<OdcmCapability> _defaultOdcmCapabilities;

        public static List<OdcmCapability> DefaultOdcmCapabilities
        {
            get
            {
                if (_defaultOdcmCapabilities == null)
                {
                    _defaultOdcmCapabilities = new List<OdcmCapability>();
                    var viprCoreName = Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.Name == "Vipr.Core");
                    var capabilityTypes = Assembly.Load(viprCoreName).GetTypes().Where(t => t.BaseType == typeof(OdcmCapability));
                    
                    foreach (var capabilityType in capabilityTypes)
                    {
                        var capability = (OdcmCapability)Activator.CreateInstance(capabilityType);
                        _defaultOdcmCapabilities.Add(capability);
                    }
                }

                return _defaultOdcmCapabilities;
            }
        }

        private static List<CapabilityAnnotationParser> _defaultCapabilityAnnotationParsers;

        public static List<CapabilityAnnotationParser> DefaultCapabilityAnnotationParsers
        {
            get
            {
                if (_defaultCapabilityAnnotationParsers == null)
                {
                    _defaultCapabilityAnnotationParsers = new List<CapabilityAnnotationParser>();
                    IEnumerable<Type> parserTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(CapabilityAnnotationParser));

                    foreach (var parserType in parserTypes)
                    {
                        var parser = (CapabilityAnnotationParser)Activator.CreateInstance(parserType);
                        _defaultCapabilityAnnotationParsers.Add(parser);
                    }
                }

                return _defaultCapabilityAnnotationParsers;
            }
        }

        public static void SetCapabilitiesForEntityContainer(OdcmServiceClass odcmServiceClass, IEdmEntityContainer edmEntityContainer)
        {
            //TODO: Add Capability Annotation support for EntityContainers
        }

        public static void SetCapabilitiesForEntitySet(OdcmProperty odcmEntitySet, IEdmEntitySet edmEntitySet, IEdmModel serviceModel)
        {
            if (odcmEntitySet == null) throw new ArgumentNullException("odcmEntitySet");
            if (edmEntitySet == null) throw new ArgumentNullException("edmEntitySet");
            if (serviceModel == null) throw new ArgumentNullException("serviceModel");

            var annotations = serviceModel.FindVocabularyAnnotations(edmEntitySet);
            PropertyCapabilitiesCache propertyCache = new PropertyCapabilitiesCache();

            foreach (var annotation in annotations)
            {
                foreach (var annotationParser in DefaultCapabilityAnnotationParsers)
                {
                    annotationParser.TryParseCapabilityAnnotations(odcmEntitySet, (IEdmValueAnnotation)annotation, propertyCache);
                }
            }

            propertyCache.CreateProjectionsForProperties();
        }
    }
}