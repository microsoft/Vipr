// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vipr.Core.CodeModel;

namespace Vipr.Reader.OData.v4.Capabilities
{
    public partial class ODataCapabilitiesReader
    {
        private static List<CapabilityAnnotationParser> _defaultCapabilityAnnotationParsers;

        /// <summary>
        /// Default list of Capability Annotation parsers
        /// </summary>
        public static List<CapabilityAnnotationParser> DefaultCapabilityAnnotationParsers
        {
            get
            {
                if (_defaultCapabilityAnnotationParsers == null)
                {
                    _defaultCapabilityAnnotationParsers = new List<CapabilityAnnotationParser>();
                    IEnumerable<Type> parserTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(CapabilityAnnotationParser)) && !t.IsAbstract);

                    foreach (var parserType in parserTypes)
                    {
                        var parser = (CapabilityAnnotationParser)Activator.CreateInstance(parserType);
                        _defaultCapabilityAnnotationParsers.Add(parser);
                    }
                }

                return _defaultCapabilityAnnotationParsers;
            }
        }

        public static void SetCapabilitiesForEntityContainer(OdcmServiceClass odcmServiceClass, IEdmEntityContainer edmEntityContainer, IEdmModel serviceModel)
        {
            //TODO: Add Capability Annotation support for EntityContainers
        }

        /// <summary>
        /// Sets the OdcmCapabilities for the given annotated EntitySet and also for the annotated navigation properties.
        /// </summary>
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
                    annotationParser.ParseCapabilityAnnotationForEntitySet(odcmEntitySet, (IEdmValueAnnotation)annotation, propertyCache);
                }
            }

            propertyCache.EnsureProjectionsForProperties();
        }
    }
}