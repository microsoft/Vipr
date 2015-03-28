// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public abstract class OdcmBooleanCapability : OdcmCapability
    {
        protected bool Value;

        public override bool Equals(OdcmCapability otherCapability)
        {
            var other = otherCapability as OdcmBooleanCapability;
            if (other == null)
            {
                return false;
            }

            return other.GetType() == this.GetType() && other.Value == this.Value;
        }

        public override void ParseCapabilityAnnotationForEntitySet(OdcmProperty odcmEntitySet,
            OdcmVocabularyAnnotation annotation, PropertyCapabilitiesCache propertyCache)
        {
            if (odcmEntitySet == null) throw new ArgumentNullException("odcmEntitySet");
            if (annotation == null) throw new ArgumentNullException("annotation");
            if (propertyCache == null) throw new ArgumentNullException("propertyCache");

            if (annotation.FullName != TermName)
            {
                return;
            }

            var restrictionTerm = annotation.Value;

            // First get the boolean value for this annotation on an EntitySet.
            // Then create a corresponding OdcmCapability and add it to the OdcmProperty of the EntitySet
            var booleanValue = GetRestrictionTermProperty<bool>(restrictionTerm);
            var odcmCapability = CreateOdcmBooleanCapability(booleanValue);
            propertyCache.AddCapabilityToProperty(odcmEntitySet, odcmCapability);

            // Get the list of annotated navigation properties
            // Then resolve these navigation properties to OdcmProperties
            // Lastly create corresponding OdcmCapabilities and add them to OdcmProperties (of the resolved navigation Properties)
            var propertyPaths = GetRestrictionTermProperty<List<string>>(restrictionTerm);
            List<OdcmProperty> navigationProperties = GetNavigationProperties(propertyPaths, odcmEntitySet.Type as OdcmClass);

            foreach (var navigationProperty in navigationProperties)
            {
                odcmCapability = CreateOdcmBooleanCapability(false);
                propertyCache.AddCapabilityToProperty(navigationProperty, odcmCapability);
            }

        }

        private OdcmBooleanCapability CreateOdcmBooleanCapability(bool booleanValue)
        {
            var odcmCapability = (OdcmBooleanCapability)Activator.CreateInstance(this.GetType());
            odcmCapability.Value = booleanValue;
            return odcmCapability;
        }

        private T GetRestrictionTermProperty<T>(object restrictionTerm)
        {
            return (T)restrictionTerm.GetType().GetProperties().Single(p => p.PropertyType == typeof(T)).GetValue(restrictionTerm);
        }

        private List<OdcmProperty> GetNavigationProperties(List<string> propertyPaths, OdcmClass odcmClass)
        {
            var properties = new List<OdcmProperty>();
            foreach (var propertyPath in propertyPaths)
            {
                OdcmProperty navProperty;
                if (!odcmClass.TryFindProperty(propertyPath, out navProperty))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Unable to find property {0} in class {1}. This can be caused by malformed Capability Annotation on an EntitySet",
                            propertyPath, odcmClass.FullName));
                }
                properties.Add(navProperty);
            }
            return properties;
        } 
    }
}
