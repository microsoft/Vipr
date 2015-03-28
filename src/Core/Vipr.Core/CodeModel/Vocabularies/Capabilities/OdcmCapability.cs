// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public abstract partial class OdcmCapability : IEquatable<OdcmCapability>
    {
        public abstract string TermName { get; }

        public abstract bool Equals(OdcmCapability otherCapability);

        public abstract void ParseCapabilityAnnotationForEntitySet(OdcmProperty odcmEntitySet,
            OdcmVocabularyAnnotation annotation, PropertyCapabilitiesCache propertyCache);

        private static List<OdcmCapability> _defaultOdcmCapabilities;

        /// <summary>
        /// Default list of OdcmCapabilities supported in the OdcmModel.
        /// </summary>
        public static List<OdcmCapability> DefaultOdcmCapabilities
        {
            get
            {
                if (_defaultOdcmCapabilities == null)
                {
                    _defaultOdcmCapabilities = new List<OdcmCapability>();
                    var capabilityTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(OdcmCapability)) && !t.IsAbstract);

                    foreach (var capabilityType in capabilityTypes)
                    {
                        var capability = (OdcmCapability)Activator.CreateInstance(capabilityType);
                        _defaultOdcmCapabilities.Add(capability);
                    }
                }

                return _defaultOdcmCapabilities;
            }
        }

        public static void SetCapabilitiesForOdcmServiceClass(OdcmServiceClass odcmServiceClass)
        {
            //TODO: Add Capability Annotation support for EntityContainers
        }

        public static void SetCapabilitiesForEntitySet(OdcmProperty odcmEntitySet)
        {
            if (odcmEntitySet == null) throw new ArgumentNullException("odcmEntitySet");

            var annotations = odcmEntitySet.Annotations;
            PropertyCapabilitiesCache propertyCache = new PropertyCapabilitiesCache();

            foreach (var annotation in annotations)
            {
                var capability = DefaultOdcmCapabilities.SingleOrDefault(c => c.TermName == annotation.FullName);
                if (capability != null)
                {
                    capability.ParseCapabilityAnnotationForEntitySet(odcmEntitySet, annotation, propertyCache);
                }
            }

            propertyCache.EnsureProjectionsForProperties();
        }
    }
}
