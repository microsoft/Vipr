// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Annotations;
using Vipr.Core.CodeModel;
using Microsoft.OData.Edm.Expressions;
using Vipr.Core;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace ODataReader.v4.Capabilities
{
    public class BooleanCapabilityAnnotationParser : CapabilityAnnotationParser
    {
        private List<IEdmValueTerm> _supportedRestrictionTerms = new List<IEdmValueTerm>()
        {
            CapabilitiesModel.FindDeclaredValueTerm("Org.OData.Capabilities.V1.InsertRestrictions"),
            CapabilitiesModel.FindDeclaredValueTerm("Org.OData.Capabilities.V1.UpdateRestrictions"),
            CapabilitiesModel.FindDeclaredValueTerm("Org.OData.Capabilities.V1.DeleteRestrictions"),
            CapabilitiesModel.FindDeclaredValueTerm("Org.OData.Capabilities.V1.ExpandRestrictions"),
        };
        
        public override bool TryParseCapabilityAnnotations(OdcmProperty odcmEntitySet,
            IEdmValueAnnotation annotation, ODataCapabilitiesReader.PropertyCapabilitiesCache propertyCache)
        {
            if (!_supportedRestrictionTerms.Any(term => term.FullName() == annotation.Term.FullName()))
            {
                return false;
            }

            var recordExpression = (IEdmRecordExpression)annotation.Value;

            // First get the boolean value for this annotation on an EntitySet.
            // Then create a corresponding OdcmCapability and add it to the OdcmProperty of the EntitySet
            var boolVal = GetBooleanValue(recordExpression);
            var odcmCapability = GetBooleanCapabiltity(boolVal, annotation.Term.FullName());
            propertyCache.AddCapabilityToProperty(odcmEntitySet, odcmCapability);

            // Get the list of annotated navigation properties
            // Then resolve these navigation properties to OdcmProperties
            // Lastly create corresponding OdcmCapabilities and add them to OdcmProperties (of the resolved navigation Properties)
            List<OdcmProperty> navigationProperties = GetNavigationProperties(recordExpression,
                odcmEntitySet.Projection.Type as OdcmClass);

            foreach (var navigationProperty in navigationProperties)
            {
                odcmCapability = GetBooleanCapabiltity(false, annotation.Term.FullName());
                propertyCache.AddCapabilityToProperty(navigationProperty, odcmCapability);
            }

            return true;
        }

        private bool GetBooleanValue(IEdmRecordExpression recordExpression)
        {
            var booleanExpression = (IEdmBooleanConstantExpression)recordExpression.Properties.Single(p => p.Value is IEdmBooleanConstantExpression).Value;
            return booleanExpression.Value;
        }

        private List<OdcmProperty> GetNavigationProperties(IEdmRecordExpression recordExpression, OdcmClass @class)
        {
            var properties = new List<OdcmProperty>();
            var collectionExpression = (IEdmCollectionExpression)recordExpression.Properties.Single(p => p.Value is IEdmCollectionExpression).Value;

            if (collectionExpression == null)
            {
                return properties;
            }

            foreach (IEdmPathExpression pathExpression in collectionExpression.Elements)
            {
                var pathBuilder = new StringBuilder();
                foreach (var path in pathExpression.Path)
                {
                    pathBuilder.AppendFormat("{0}.", path);
                }

                pathBuilder.Remove(pathBuilder.Length - 1, 1);

                OdcmProperty navProperty;
                if (!@class.TryFindProperty(pathBuilder.ToString(), out navProperty))
                {
                    throw new InvalidOperationException(string.Format("Unable to find property {0} in class {1}", pathBuilder.ToString(), @class.FullName));
                }
                properties.Add(navProperty);
            }

            return properties;
        }

        private OdcmCapability GetBooleanCapabiltity(bool boolVal, string annotationTermName)
        {
            OdcmCapability capability = ODataCapabilitiesReader.DefaultOdcmCapabilities.SingleOrDefault(c => c.TermName == annotationTermName);

            if (capability == null)
            {
                throw new InvalidOperationException(string.Format("Unable to find an OdcmCapability object to represent the {0} annotation", annotationTermName));
            }

            Type capabilityType = capability.GetType();
            capability = (OdcmCapability)Activator.CreateInstance(capabilityType);
            var booleanProperty = capabilityType.GetProperties().Single(p => p.PropertyType == typeof(bool));
            booleanProperty.SetValue(capability, boolVal);

            return capability;
        }
    }
}
