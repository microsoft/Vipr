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
using System.Dynamic;
using System.Diagnostics;

namespace Vipr.Reader.OData.v4.Capabilities
{
    public class CapabilityAnnotationParser
    {
        private readonly PropertyCapabilitiesCache _propertyCapabilitiesCache;

        public CapabilityAnnotationParser(PropertyCapabilitiesCache propertyCapabilitiesCache)
        {
            _propertyCapabilitiesCache = propertyCapabilitiesCache;
        }

        public void  ParseCapabilityAnnotation(OdcmObject odcmObject, IEdmValueAnnotation annotation)
        {
            if (annotation.Term.Name != "Description" && annotation.Term.Name != "LongDescription")
            {
                TryParseCapability(odcmObject, annotation.Value, annotation.Term.FullName());
            }
        }

        private bool HasSpecializedParser(OdcmObject odcmObject, IEdmExpression expression, string annotationTerm)
        {
            if (annotationTerm == "Org.OData.Capabilities.V1.CallbackSupported")
            {
                Debug.Assert(expression is IEdmRecordExpression);

                var collectionProperty = (expression as IEdmRecordExpression).Properties.First();

                var collection = ParseRecordCollection(collectionProperty.Value as IEdmCollectionExpression);

                dynamic value = new ExpandoObject();
                value.CallbackProtocols = collection;

                SetRecordCapability(odcmObject, value, annotationTerm);
                return true;
            }

            return false;
        }

        private void TryParseCapability(OdcmObject odcmObject, IEdmExpression expression, string annotationTerm)
        {
            if (HasSpecializedParser(odcmObject, expression, annotationTerm))
            {
                // Do nothing
            }
            else if (expression is IEdmBooleanConstantExpression)
            {
                bool value = (expression as IEdmBooleanConstantExpression).Value;
                SetBooleanCapability(odcmObject, value, annotationTerm);
            }
            else if (expression is IEdmEnumMemberExpression)
            {
                var values = (expression as IEdmEnumMemberExpression).EnumMembers.Select(v => v.Name);
                SetEnumCapability(odcmObject, values, annotationTerm);
            }
            else if (expression is IEdmStringConstantExpression)
            {
                var value = (expression as IEdmStringConstantExpression).Value;
                SetStringCapability(odcmObject, value, annotationTerm);
            }
            else if (expression is IEdmPathExpression)
            {
                SetPathCapability(odcmObject, expression as IEdmPathExpression, annotationTerm);
            }
            else if (expression is IEdmRecordExpression)
            {
                foreach (var propertyConstructor in (expression as IEdmRecordExpression).Properties)
                {
                    TryParseCapability(odcmObject, propertyConstructor.Value, annotationTerm + "/" + propertyConstructor.Name);
                }
            }
            else if (expression is IEdmCollectionExpression)
            {
                ParseCollection(odcmObject, expression as IEdmCollectionExpression, annotationTerm);
            }
            else
            {
                Logger.Log($"Unsupported annotation expression of kind {expression.ExpressionKind.ToString()} for Term \"{annotationTerm}\"");
            }
        }

        private void ParseCollection(OdcmObject odcmObject, IEdmCollectionExpression collectionExpression, string annotationTerm)
        {
            if (!collectionExpression.Elements.Any())
            {
                return;
            }

            var elementExpression = collectionExpression.Elements.First();
            var recordExpression = elementExpression as IEdmRecordExpression;

            if (recordExpression != null)
            {
                if (recordExpression.Properties.Count(p => p.Value is IEdmPathExpression) == 1)
                {
                    // We assume that if a collection element is a record with a single path expression, the rest 
                    // of record properties should be associated with this path expression
                    // (e.g. NavigationRestrictions).
                    ParsePropertyCollection(odcmObject, collectionExpression, annotationTerm);
                }
                else
                {
                    var records = ParseRecordCollection(collectionExpression);
                    SetListCapability(odcmObject, records, annotationTerm);
                }
            }
            else if (elementExpression is IEdmStringConstantExpression)
            {
                var strings = collectionExpression.Elements.Select(e => (e as IEdmStringConstantExpression).Value);
                SetListCapability(odcmObject, strings, annotationTerm);
            }
            else if (elementExpression is IEdmPathExpression)
            {
                foreach (IEdmPathExpression expression in collectionExpression.Elements)
                {
                    SetPathCapability(odcmObject, expression, annotationTerm);
                }
            }
            else
            {
                Logger.Log($"Unsupported collection of kind {elementExpression.ExpressionKind.ToString()} for Term \"{annotationTerm}\"");
            }
        }

        private IEnumerable<object> ParseRecordCollection(IEdmCollectionExpression collectionExpression)
        {
            var records = new List<ExpandoObject>();

            foreach (IEdmRecordExpression recordExpression in collectionExpression.Elements)
            {
                records.Add(ParseRecord(recordExpression));
            }

            return records;
        }

        private dynamic ParseRecord(IEdmRecordExpression recordExpression)
        {
            var recordObject = new ExpandoObject();
            var record = recordObject as IDictionary<String, object>;

            foreach (var property in recordExpression.Properties)
            {
                var expression = property.Value;

                if (expression is IEdmStringConstantExpression)
                {
                    record[property.Name] = (expression as IEdmStringConstantExpression).Value;
                }
                else
                {
                    Logger.Log($"Unsupported annotation expression of kind {expression.ExpressionKind.ToString()} in a record for property {property.Name}");
                }
            }

            return record;
        }

        private void ParsePropertyCollection(OdcmObject odcmObject, IEdmCollectionExpression collectionExpression, string annotationTerm)
        {
            foreach (IEdmRecordExpression recordExpression in collectionExpression.Elements)
            {
                var pathExpression = (IEdmPathExpression) recordExpression.Properties
                                                .First(p => p.Value is IEdmPathExpression)
                                                .Value;

                var odcmProperty = SetPathCapability(odcmObject, pathExpression, annotationTerm);

                foreach (var propertyConstructor in recordExpression.Properties.Where(p => !(p.Value is IEdmPathExpression)))
                {
                    TryParseCapability(odcmProperty, propertyConstructor.Value, annotationTerm + "/" + propertyConstructor.Name);
                }
            }
        }

        private void SetBooleanCapability(OdcmObject odcmObject, bool value, string annotationTerm)
        {
            AddCapability(odcmObject, new OdcmBooleanCapability(value, annotationTerm));
        }

        private OdcmProperty SetPathCapability(OdcmObject odcmObject, IEdmPathExpression expression, string annotationTerm)
        {
            var odcmProperty = PropertyFromPathExpression(expression, odcmObject);
            // The term name serves as a tag for this capability
            SetBooleanCapability(odcmProperty, false, annotationTerm);
            return odcmProperty;
        }

        private void SetEnumCapability(OdcmObject odcmObject, IEnumerable<string> value, string annotationTerm)
        {
            AddCapability(odcmObject, new OdcmEnumCapability(value, annotationTerm));
        }

        private void SetStringCapability(OdcmObject odcmObject, string value, string annotationTerm)
        {
            AddCapability(odcmObject, new OdcmStringCapability(value, annotationTerm));
        }

        private void SetRecordCapability(OdcmObject odcmObject, object value, string annotationTerm)
        {
            AddCapability(odcmObject, new OdcmRecordCapability(value, annotationTerm));
        }

        private void SetListCapability(OdcmObject odcmObject, IEnumerable<object> value, string annotationTerm)
        {
            AddCapability(odcmObject, new OdcmCollectionCapability(value, annotationTerm));
        }

        private void AddCapability(OdcmObject odcmObject, OdcmCapability capability)
        {
            var capabilities = _propertyCapabilitiesCache.GetCapabilities(odcmObject);
            capabilities.Add(capability);
        }

        private OdcmProperty PropertyFromPathExpression(IEdmPathExpression pathExpression, OdcmObject odcmObject)
        {
            OdcmClass odcmClass = null;

            if (odcmObject is OdcmProperty)
            {
                var odcmProperty = odcmObject as OdcmProperty;
                odcmClass = odcmProperty.Type as OdcmClass;
            }
            else
            {
                odcmClass = odcmObject as OdcmClass;
            }

            var pathBuilder = new StringBuilder();
            foreach (var path in pathExpression.Path)
            {
                pathBuilder.AppendFormat("{0}.", path);
            }

            pathBuilder.Remove(pathBuilder.Length - 1, 1);

            OdcmProperty navProperty;
            if (!odcmClass.TryFindProperty(pathBuilder.ToString(), out navProperty))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Unable to find property {0} in class {1}. This can be caused by malformed Capability Annotation on an EntitySet",
                        pathBuilder.ToString(), odcmClass.FullName));
            }

            return navProperty;
        }
    }
}