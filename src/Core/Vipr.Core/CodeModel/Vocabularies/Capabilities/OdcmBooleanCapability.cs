// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public abstract class OdcmBooleanCapability : OdcmCapability
    {
        protected bool BooleanValue;

        public override void ParseCapabilityAnnotationForEntitySet(OdcmProperty odcmEntitySet,
            OdcmVocabularyAnnotation annotation, PropertyCapabilitiesCache propertyCache)
        {
            if (odcmEntitySet == null) throw new ArgumentNullException("odcmEntitySet");
            if (annotation.FullName != TermName)
            {
                return;
            }

            var restrictionTerm = annotation.Value;

            // First get the boolean value for this annotation on an EntitySet.
            // Then create a corresponding OdcmCapability and add it to the OdcmProperty of the EntitySet
            var booleanValue = (bool)restrictionTerm.GetType().GetProperties().Single(p => p.PropertyType == typeof(bool)).GetValue(restrictionTerm);
            var odcmCapability = (OdcmBooleanCapability)Activator.CreateInstance(this.GetType());
            odcmCapability.BooleanValue = booleanValue;
            propertyCache.AddCapabilityToProperty(odcmEntitySet, odcmCapability);

            // Get the list of annotated navigation properties
            // Then resolve these navigation properties to OdcmProperties
            // Lastly create corresponding OdcmCapabilities and add them to OdcmProperties (of the resolved navigation Properties)
            var propertyPaths = (List<string>)restrictionTerm.GetType().GetProperties().Single(p => p.PropertyType == typeof(List<string>)).GetValue(restrictionTerm);
            List<OdcmProperty> navigationProperties = GetNavigationProperties(propertyPaths, odcmEntitySet.Projection.Type as OdcmClass);

            foreach (var navigationProperty in navigationProperties)
            {
                odcmCapability = (OdcmBooleanCapability)Activator.CreateInstance(this.GetType());
                odcmCapability.BooleanValue = false;
                propertyCache.AddCapabilityToProperty(navigationProperty, odcmCapability);
            }

        }

        private List<OdcmProperty> GetNavigationProperties(List<string> propertyPaths, OdcmClass odcmClass)
        {
            var properties = new List<OdcmProperty>();
            foreach (var propertyPath in propertyPaths)
            {
                OdcmProperty navProperty;
                if (!odcmClass.TryFindProperty(propertyPath, out navProperty))
                {
                    throw new InvalidOperationException(string.Format("Unable to find property {0} in class {1}", propertyPath, odcmClass.FullName));
                }
                properties.Add(navProperty);
            }
            return properties;
        } 
    }
}
