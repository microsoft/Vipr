// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static List<OdcmBooleanCapability> _supportedOdcmCapabilities;

        public BooleanCapabilityAnnotationParser()
        {
            _supportedOdcmCapabilities = new List<OdcmBooleanCapability>();
            var viprCoreName = Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.Name == "Vipr.Core");
            var capabilityTypes = Assembly.Load(viprCoreName).GetTypes().Where(t => t.IsSubclassOf(typeof(OdcmBooleanCapability)) && !t.IsAbstract);

            foreach (var capabilityType in capabilityTypes)
            {
                var capability = (OdcmBooleanCapability)Activator.CreateInstance(capabilityType);
                _supportedOdcmCapabilities.Add(capability);
            }
        }

        public override void  ParseCapabilityAnnotationForEntitySet(OdcmProperty odcmEntitySet,
            IEdmValueAnnotation annotation, ODataCapabilitiesReader.PropertyCapabilitiesCache propertyCache)
        {
            if (!_supportedOdcmCapabilities.Any(c => c.TermName == annotation.Term.FullName()))
            {
                return;
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
                odcmEntitySet.Type as OdcmClass);

            foreach (var navigationProperty in navigationProperties)
            {
                odcmCapability = GetBooleanCapabiltity(false, annotation.Term.FullName());
                propertyCache.AddCapabilityToProperty(navigationProperty, odcmCapability);
            }
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
                    throw new InvalidOperationException(
                        string.Format(
                            "Unable to find property {0} in class {1}. This can be caused by malformed Capability Annotation on an EntitySet",
                            pathBuilder.ToString(), @class.FullName));
                }
                properties.Add(navProperty);
            }

            return properties;
        }

        private OdcmCapability GetBooleanCapabiltity(bool boolVal, string annotationTermName)
        {
            Type capabilityType = _supportedOdcmCapabilities.Single(c => c.TermName == annotationTermName).GetType();
            var capability = (OdcmBooleanCapability)Activator.CreateInstance(capabilityType);
            capability.Value = boolVal;
            return capability;
        }
    }
}