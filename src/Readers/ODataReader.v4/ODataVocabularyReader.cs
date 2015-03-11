// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Annotations;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Evaluation;
using Microsoft.OData.Edm.Expressions;
using Microsoft.OData.Edm.Values;
using Microsoft.OData.Edm.Validation;
using Vipr.Core.CodeModel;

namespace ODataReader.v4
{
    /// <summary>
    ///     Reads an IEdmModel and instances of IEdmType to return annotation objects in the style of Vipr's Odata Code Model
    /// </summary>
    public static class ODataVocabularyReader
    {
        private const string ViprCoreVocabularyRoot = "Vipr.Core.CodeModel.Vocabularies";
        // Provide a hierarchical dictionary of namespace -> {type name -> Type}
        private static Dictionary<string, Dictionary<string, IEdmSchemaElement>> _registeredVocabularyTypes;
        private static EdmExpressionEvaluator _edmEvaluator;
        // Cache constructors for values of Annotations 
        private static Dictionary<IEdmType, Func<object>> _constructorCache = new Dictionary<IEdmType, Func<object>>();
        private static Assembly _viprCore;

        static ODataVocabularyReader()
        {
            // Create a new evaluator to evaluate the delayedValue expressions of annotations from the edm model 
            _edmEvaluator = new EdmExpressionEvaluator(new Dictionary<IEdmOperation, Func<IEdmValue[], IEdmValue>>());
            _registeredVocabularyTypes = new Dictionary<string, Dictionary<string, IEdmSchemaElement>>();
            IEdmModel _capabilitiesModel;
            
            // TODO: As above, Extend / modify this to more clearly support custom annotation registration. 
            // Tracked by https://github.com/Microsoft/vipr/issues/59
            IEnumerable<EdmError> errors;
            if (!CsdlReader.TryParse(new[] { XmlReader.Create(new StringReader(ODataReader.v4.Properties.Resources.CapabilitiesVocabularies)) }, out _capabilitiesModel, out errors))
            {
                throw new InvalidOperationException("Could not load capabilities vocabulary from resources");
            }

            // Store each type in the dictionary for easier access
            foreach (var element in _capabilitiesModel.SchemaElements)
            {
                if (!_registeredVocabularyTypes.ContainsKey(element.Namespace))
                {
                    _registeredVocabularyTypes.Add(element.Namespace, new Dictionary<string, IEdmSchemaElement>());
                }

                _registeredVocabularyTypes[element.Namespace].Add(element.Name, element);
            }
        }

        /// <summary>
        ///     Gets stored IEdmAnnotations from a IEdmModel that correspond to an IEdm object and converts them to an enumerable
        ///     of OdcmAnnotations
        /// </summary>
        /// <param name="model">An IEdmModel which contains annotations and IEdm Objects</param>
        /// <param name="e">An object contained within the IEdmModel, such as an IEdmMethod</param>
        /// <returns>
        ///     An empty enumerable of annotations for objects which lack annotations (or cannot be annotated in EDM), or an
        ///     enumerable of converted annotations
        /// </returns>
        public static IEnumerable<OdcmVocabularyAnnotation> GetOdcmAnnotations(IEdmModel model, object e)
        {
            // Only annotatable types will have annotations; return an empty list
            if (!(e is IEdmVocabularyAnnotatable))
            {
                yield break;
            }

            var annotatable = (IEdmVocabularyAnnotatable) e;

            // We must use the model to obtain annotations
            var annotations = model.FindVocabularyAnnotations(annotatable);

            foreach (var annotation in annotations)
            {
                // Perform the mapping from IEdmAnnotation types to OcdmAnnotations 
                // Mapping name and name space is the simplest piece
                OdcmVocabularyAnnotation odcmAnnotation = new OdcmVocabularyAnnotation
                {
                    Name = annotation.Term.Name,
                    Namespace = annotation.Term.Namespace
                };

                // Vocabulary elements are registered when we have parsed their model and cached it
                // Unmapped elements are either invalid vocabulary terms (mispelled or otherwise) or have not been registered 
                if (VocabularyElementRegistered(odcmAnnotation))
                {
                    var elementType = _registeredVocabularyTypes[odcmAnnotation.Namespace][odcmAnnotation.Name];

                    // We have a delayedValue that will get us to the corresponding type of the annotation
                    if (elementType.SchemaElementKind == EdmSchemaElementKind.ValueTerm && elementType is IEdmValueTerm)
                    {
                        var valueTerm = (IEdmValueTerm)elementType;
                        var valueType = valueTerm.Type;

                        var valueAnnotation = annotation as IEdmValueAnnotation;

                        if (valueAnnotation == null)
                        {
                            throw new InvalidOperationException("Unexpected non-delayedValue annotation");
                        }

                        IEdmExpression valueExpression = valueAnnotation.Value;
                        var result = MapToClr(valueType, valueExpression);

                        odcmAnnotation.Value = result;
                        yield return odcmAnnotation;
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                "Cannot return annotation values for EDM element types that are not ValueTerms. Type was {0} for element name {1}",
                                elementType.SchemaElementKind, elementType.Name));
                    }
                }
            }
        }

        // Todo: These methods can be made internal for testing purposes and then this assembly can be modified to be friendly w/ unit test assemblies 
        internal static object MapToClr(IEdmTypeReference valueType, IEdmExpression valueExpression)
        {
            // Evaluate the expression to get IEdmValues 
            var value = _edmEvaluator.Evaluate(valueExpression);

            if (valueType.Definition.TypeKind == EdmTypeKind.Complex && value is IEdmStructuredValue)
            {
                object instance = FetchNewInstanceOfAnnotationComplexType(valueType.Definition);
                var instanceType = instance.GetType();

                var structuredValue = (IEdmStructuredValue)value;

                foreach (var propertyValue in structuredValue.PropertyValues)
                {
                    var fieldName = propertyValue.Name;
                    var property = instanceType.GetProperty(fieldName);
                    var v = propertyValue.Value;

                    var res = MapToClr(v, property.PropertyType);

                    property.SetValue(instance, res);
                }

                return instance;
            }
            else
            {
                return MapToClr(value);
            }
        }

        /// <summary>
        ///     Maps an IEdmValue to its corresponding CLR type as an object
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="clarifiedType">A type clarification, used in the case that a type is a collection</param>
        /// <returns>An object representing the IEdmValue</returns>
        internal static object MapToClr(IEdmValue value, Type clarifiedType = null)
        {
            switch (value.ValueKind)
            {
                case EdmValueKind.Binary:
                case EdmValueKind.Boolean:
                case EdmValueKind.Date:
                case EdmValueKind.DateTimeOffset:
                case EdmValueKind.Decimal:
                case EdmValueKind.Duration:
                case EdmValueKind.Floating:
                case EdmValueKind.Guid:
                case EdmValueKind.Integer:
                case EdmValueKind.String:
                case EdmValueKind.TimeOfDay:
                {
                    // All of these types of EdmValues expose a .Value parameter which returns an appropriately typed delayedValue 
                    // However, our annotation interface just wants an object. Rather than clutter this code, it's easier to take
                    // a slight reflection cost in the interest of brevity. 
                    return value.GetPropertyByName("Value");
                }
                case EdmValueKind.Collection:
                    return MapToClr(value as IEdmCollectionValue, clarifiedType);
                case EdmValueKind.Structured:
                    // TODO: Refactor structured type generation below and use that code here. 
                case EdmValueKind.Enum: // TODO: Parse enumerations appropriately
                case EdmValueKind.None:
                    // TODO: Find examples of null / none in annotations and either implement or won't fix this gap
                case EdmValueKind.Null:
                default:
                    throw new NotImplementedException(
                        string.Format("MapToClr for annotation values is not yet supported for IEdmValue of kind {0}",
                            value.ValueKind));
            }
        }

        /// <summary>
        ///     Process an IEdmCollectionValue into a IEnumerable of CLR objects
        /// </summary>
        /// <param name="value">The collection delayedValue, recieved from an annotation</param>
        /// <param name="clarifiedType">A type describing the desired target collection type</param>
        /// <returns>A collection, of an instance of the clarifiedType, filled with values within the IEdmCollectionValue</returns>
        private static object MapToClr(IEdmCollectionValue value, Type clarifiedType = null)
        {
            if (clarifiedType == null)
            {
                throw new ArgumentNullException("clarifiedType",
                    "ClarifiedType must be defined to correctly return value of IEdmCollectionValue");
            }

            if (!clarifiedType.IsCollection())
            {
                throw new InvalidOperationException(
                    "Clarified type used for mapping to CLR values is not a valid collection type");
            }

            // Get the default constructor for the collection type and then invoke it. 
            var collection = clarifiedType.CreateDefaultInstance();

            // Get an add method 
            var addMethod = collection.GetType().GetMethod("Add", new[] {clarifiedType.GetGenericArguments().First()});

            foreach (var val in value.Elements)
            {
                addMethod.Invoke(collection, new[] {MapListElements(val)});
            }

            return collection;
        }

        private static bool IsCollection(this Type type)
        {
            if (type == null) return false;

            return
                type.GetInterfaces()
                    .Select(@interface => @interface.GetGenericTypeDefinition())
                    .Any(@interface => @interface == typeof (ICollection<>));
        }

        /// <summary>
        ///     Creates a default instance for a given type
        /// </summary>
        /// <param name="type">Type to create a default instance of, using the default constructor</param>
        /// <returns>An instance of the type generated from the default constructor</returns>
        private static object CreateDefaultInstance(this Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Default constructor for a type was required, but not present or did not work on type {0}",
                        type.Name), e);
            }
        }

        private static T GetPropertyByName<T>(this object value, string propertyName) where T : class
        {
            return GetPropertyByName(value, propertyName) as T;
        }

        private static object GetPropertyByName(this object value, string propertyName)
        {
            if (value == null)
                throw new ArgumentNullException("value",
                    "Cannot attempt to get a property delayedValue from an object that is null");

            PropertyInfo propertyForName;
            try
            {
                propertyForName = value.GetType().GetProperty(propertyName);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format("Property {0} was required, but not present on type {1}", propertyName,
                        value.GetType().Name), e);
            }

            try
            {
                return propertyForName.GetValue(value);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format("Property access for {0} was required but failed on type {1}", propertyName,
                        value.GetType().Name), e);
            }
        }

        /// <summary>
        ///     Maps an IEdmDelayedVAlue to an instance of an object.
        ///     In the case that the delayedValue object represents a navigation path,
        ///     performs additional work to map the navigation path to a delayedValue
        /// </summary>
        /// <param name="delayedValue">DelayedValue that is an element of the list</param>
        /// <returns>The object representation of the delayedValue</returns>
        private static object MapListElements(IEdmDelayedValue delayedValue)
        {
            IEdmValue edmValue;
            try
            {
                edmValue = delayedValue.Value;
                return MapToClr(edmValue);
            }
            catch (ArgumentNullException e)
            {
                // This is a painful hack to deal with the fact that parsing the navigation property returned a delayedValue that evaluates a NullException when you call .Value
                // Attempt to obtain the inner expression because we don't have access to the type of the Delayed Collection 
                var innerExpression = delayedValue.GetPropertyByName<IEdmExpression>("Expression");

                switch (innerExpression.ExpressionKind)
                {
                    case EdmExpressionKind.NavigationPropertyPath:
                    {
                        IEdmPathExpression pe = innerExpression as IEdmPathExpression;
                        if (pe == null)
                        {
                            throw new ArgumentNullException("delayedValue",
                                "A IEdmExpression of delayedValue type NavigationPropertyPath was unable to be cast correctly to a IEdmNavigationPropertyPath");
                        }

                        // Map navigation properties to strings for convenience
                        return string.Join("/", pe.Path);
                    }
                    default:
                        throw new NotImplementedException(
                            string.Format("A custom mapping for ExpressionKind {0} has not yet been implemented.",
                                innerExpression.ExpressionKind));
                }
            }
        }

        /// <summary>
        ///     Fetches a new instance of the complex type that corresponds to the delayedValue of an OData Annotation.
        ///     These types are direct mappings from the complex type values defined in a vocabulary's edm with a mapping to
        ///     their definition in the Vipr.Core code model.
        /// </summary>
        /// <example>
        ///     An attempt to create a new instance for OData.Core.Capabilities.V1.InsertRestrictionsType will map to an
        ///     attempt to create a new instance of the type Vipr.Core.CodeModel.Vocabularies.Capabilities.InsertRestrictionsType
        /// </example>
        /// <param name="type">The IEdmType for which to produce a new CLR instance</param>
        /// <returns>The new instance of the CLR type, or an exception if no such type exists.</returns>
        private static object FetchNewInstanceOfAnnotationComplexType(IEdmType type)
        {
            var complexType = type as IEdmComplexType;
            if (complexType == null)
                throw new ArgumentNullException("type",
                    "Type must be non-null and of type IEdmComplexType to obtain new instance");

            // We should attempt to cache these constructors so this code does not get called repeatedly 
            Func<object> constructor;
            if (_constructorCache == null)
            {
                _constructorCache = new Dictionary<IEdmType, Func<object>>();
            }

            if (!_constructorCache.TryGetValue(type, out constructor))
            {
                var viprCodeModelNamespace = VocabularyNamespaceMappings[complexType.Namespace];

                // We should cache a reference to the assembly rather than obtaining it every time.
                if (_viprCore == null)
                {
                    var viprCoreName =
                        Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.Name == "Vipr.Core");
                    _viprCore = Assembly.Load(viprCoreName);
                }

                // Fetch the appropriate vocabulary instance type from the Vipr.Core CodeModel
                var t = _viprCore.GetType(string.Format("{0}.{1}", viprCodeModelNamespace, complexType.Name),
                    throwOnError: true);

                // Cache an action to call to create this type.
                constructor = () => CreateDefaultInstance(t);
                _constructorCache[type] = constructor;
            }

            // Call the cached constructor and return the instance
            return constructor();
        }

        private static bool VocabularyElementRegistered(OdcmVocabularyAnnotation e)
        {
            if (e == null) return false;
            return VocabularyElementRegistered(e.Namespace, e.Name);
        }

        private static bool VocabularyElementRegistered(string @namespace, string name)
        {
            if (_registeredVocabularyTypes.ContainsKey(@namespace))
            {
                if (_registeredVocabularyTypes[@namespace].ContainsKey(name))
                {
                    return true;
                }
            }
            return false;
        }

        // Provide a mapping from OData namespaces to namespaces which contain complex type declarations 
        // TODO: Extend / modify this to more clearly support custom annotation registration. 
        // Tracked by https://github.com/Microsoft/vipr/issues/59
        private static Dictionary<string, string> VocabularyNamespaceMappings = new Dictionary<string, string>()
        {
            {"Org.OData.Capabilities.V1", ViprCoreVocabularyRoot + ".Capabilities"}
        };
    }
}