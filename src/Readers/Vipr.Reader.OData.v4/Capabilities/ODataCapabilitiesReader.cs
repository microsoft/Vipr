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
        /// <summary>
        /// Default list of Capability Annotation parsers
        /// </summary>
        private static List<CapabilityAnnotationParser> GetCapabilityAnnotationParsers(PropertyCapabilitiesCache propertyCapabilitiesCache)
        {
            var defaultCapabilityAnnotationParsers = new List<CapabilityAnnotationParser>();
            IEnumerable<Type> parserTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(CapabilityAnnotationParser)) && !t.IsAbstract);

            foreach (var parserType in parserTypes)
            {
                var parser =
                    (CapabilityAnnotationParser) Activator.CreateInstance(parserType, propertyCapabilitiesCache);
                defaultCapabilityAnnotationParsers.Add(parser);
            }

            return defaultCapabilityAnnotationParsers;
        }

        public static void SetCapabilitiesForEntityContainer(OdcmServiceClass odcmServiceClass, IEdmEntityContainer edmEntityContainer, IEdmModel serviceModel)
        {
            //TODO: Add Capability Annotation support for EntityContainers
        }

        /// <summary>
        /// Sets the OdcmCapabilities for the given annotated EntitySet and also for the annotated navigation properties.
        /// </summary>
        public static void SetCapabilitiesForEntitySet(OdcmProperty odcmEntitySet,
            IEdmEntitySet edmEntitySet, IEdmModel serviceModel,
            PropertyCapabilitiesCache propertyCapabilitiesCache)
        {
            if (odcmEntitySet == null) throw new ArgumentNullException("odcmEntitySet");
            if (edmEntitySet == null) throw new ArgumentNullException("edmEntitySet");
            if (serviceModel == null) throw new ArgumentNullException("serviceModel");
            if (propertyCapabilitiesCache == null) throw new ArgumentNullException("propertyCapabilitiesCache");

            var annotations = serviceModel.FindVocabularyAnnotations(edmEntitySet);

            foreach (var annotation in annotations)
            {
                foreach (var annotationParser in GetCapabilityAnnotationParsers(propertyCapabilitiesCache))
                {
                    annotationParser.ParseCapabilityAnnotationForEntitySet(odcmEntitySet,
                        (IEdmValueAnnotation) annotation);
                }
            }
        }
    }
}