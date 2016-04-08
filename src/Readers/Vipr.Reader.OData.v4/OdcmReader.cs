// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Library.Values;
using Microsoft.OData.Edm.Validation;
using Vipr.Core;
using Vipr.Core.CodeModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Vipr.Reader.OData.v4.Capabilities;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;
using Microsoft.OData.Edm.Annotations;
using System.Xml;
using System.IO;

namespace Vipr.Reader.OData.v4
{
    public class OdcmReader : IOdcmReader
    {
        private static readonly string[][] PrimitiveTypes = new[]
        {
            new[] {"Edm", "Binary"},
            new[] {"Edm", "Boolean"},
            new[] {"Edm", "Byte"},
            new[] {"Edm", "Date"},
            new[] {"Edm", "DateTimeOffset"},
            new[] {"Edm", "Decimal"},
            new[] {"Edm", "Double"},
            new[] {"Edm", "Duration"},
            new[] {"Edm", "Guid"},
            new[] {"Edm", "Int16"},
            new[] {"Edm", "Int32"},
            new[] {"Edm", "Int64"},
            new[] {"Edm", "SByte"},
            new[] {"Edm", "Single"},
            new[] {"Edm", "Stream"},
            new[] {"Edm", "String"},
            new[] {"Edm", "TimeOfDay"},
            new[] {"Edm", "Geography"},
            new[] {"Edm", "GeographyPoint"},
            new[] {"Edm", "GeographyLineString"},
            new[] {"Edm", "GeographyPolygon"},
            new[] {"Edm", "GeographyMultiPoint"},
            new[] {"Edm", "GeographyMultiLineString"},
            new[] {"Edm", "GeographyMultiPolygon"},
            new[] {"Edm", "GeographyCollection"},
            new[] {"Edm", "Geometry"},
            new[] {"Edm", "GeometryPoint"},
            new[] {"Edm", "GeometryLineString"},
            new[] {"Edm", "GeometryPolygon"},
            new[] {"Edm", "GeometryMultiPoint"},
            new[] {"Edm", "GeometryMultiLineString"},
            new[] {"Edm", "GeometryMultiPolygon"},
            new[] {"Edm", "GeometryCollection"}
        };

        public OdcmModel GenerateOdcmModel(IEnumerable<TextFile> serviceMetadata)
        {
            var daemon = new ReaderDaemon();
            return daemon.GenerateOdcmModel(serviceMetadata);
        }

        private class ReaderDaemon
        {
            private const string MetadataKey = "$metadata";

            private IEdmModel _edmModel = null;
            private OdcmModel _odcmModel;
            private PropertyCapabilitiesCache _propertyCapabilitiesCache;

            public OdcmModel GenerateOdcmModel(IEnumerable<TextFile> serviceMetadata)
            {
                if (serviceMetadata == null)
                    throw new ArgumentNullException("serviceMetadata");

                var edmxFile = serviceMetadata.FirstOrDefault(f => f.RelativePath == MetadataKey);

                if (edmxFile == null)
                    throw new ArgumentException(
                        String.Format("Argument must contain file with RelateivePath \"{0}", MetadataKey),
                        "serviceMetadata");

                var edmx = XElement.Parse(edmxFile.Contents);

                IEnumerable<EdmError> errors;

                if (!EdmxReader.TryParse(edmx.CreateReader(ReaderOptions.None), /*_capabilitiesModel,*/ out _edmModel, out errors))
                {
                    Debug.Assert(errors != null, "errors != null");

                    if (errors.FirstOrDefault() == null)
                    {
                        throw new InvalidOperationException();
                    }

                    throw new InvalidOperationException(errors.FirstOrDefault().ErrorMessage);
                }

                _odcmModel = new OdcmModel(serviceMetadata);
                _propertyCapabilitiesCache = new PropertyCapabilitiesCache();

                AddPrimitives();

                WriteNamespaces();

                _propertyCapabilitiesCache.EnsureProjectionsForProperties();

                _propertyCapabilitiesCache.CreateDistinctProjectionsForWellKnownBooleanTypes();

                return _odcmModel;
            }

            private void AddPrimitives()
            {
                foreach (var entry in PrimitiveTypes)
                {
                    _odcmModel.AddType(new OdcmPrimitiveType(entry[1], OdcmNamespace.GetWellKnownNamespace(entry[0])));
                }
            }

            private void AddVocabularyAnnotations(OdcmObject odcmObject, IEdmVocabularyAnnotatable annotatableEdmEntity)
            {
#if false
                odcmObject.Annotations = ODataVocabularyReader.GetOdcmAnnotations(_edmModel, annotatableEdmEntity).ToList();
#endif
                odcmObject.Description = _edmModel.GetDescriptionAnnotation(annotatableEdmEntity);
                odcmObject.LongDescription = _edmModel.GetLongDescriptionAnnotation(annotatableEdmEntity);

                var annotations = _edmModel.FindVocabularyAnnotations(annotatableEdmEntity);

                if (annotations.Any())
                {
                    SetCapabilitiesForEntity(odcmObject, annotations);
                }
            }

            private void WriteNamespaces()
            {
                OdcmProjection.NameMapper = new ODataAnnotationTermMapper();

                foreach (var declaredNamespace in _edmModel.DeclaredNamespaces)
                {
                    WriteNamespaceShallow(_edmModel, declaredNamespace);
                }

                foreach (var declaredNamespace in _edmModel.DeclaredNamespaces)
                {
                    WriteNamespaceDeep(_edmModel, declaredNamespace);
                }

                // Make sure we write functions defined in namespaces different from its entity type
                var allEntityTypes = AllEntityTypes(AllTypes(_edmModel.SchemaElements)).ToList();

                foreach (var declaredNamespace in _edmModel.DeclaredNamespaces)
                {
                    WriteNamespaceMethods(_edmModel, declaredNamespace, allEntityTypes);
                }
            }

            private void WriteNamespaceShallow(IEdmModel edmModel, string @namespace)
            {
                _odcmModel.AddNamespace(@namespace);

                var allElements = AllElementsByNamespace(edmModel.SchemaElements, @namespace).ToList();

                var types = AllTypes(allElements).ToList();

                foreach (var enumType in AllEnumTypes(types))
                {
                    _odcmModel.AddType(new OdcmEnum(enumType.Name, ResolveNamespace(enumType.Namespace)));
                }

                foreach (var typeDefinition in AllTypeDefinitions(types))
                {
                    _odcmModel.AddType(new OdcmTypeDefinition(typeDefinition.Name, ResolveNamespace(typeDefinition.Namespace)));
                }

                foreach (var complexType in AllComplexTypes(types))
                {
                    _odcmModel.AddType(new OdcmComplexClass(complexType.Name, ResolveNamespace(complexType.Namespace)));
                }

                foreach (var entityType in AllEntityTypes(types))
                {
                    if (entityType.HasStream)
                    {
                        _odcmModel.AddType(new OdcmMediaClass(entityType.Name, ResolveNamespace(entityType.Namespace)));
                    }
                    else
                    {
                        _odcmModel.AddType(new OdcmEntityClass(entityType.Name, ResolveNamespace(entityType.Namespace)));
                    }
                }

                foreach (var entityContainer in AllEntityContainers(allElements))
                {
                    _odcmModel.AddType(new OdcmServiceClass(entityContainer.Name, ResolveNamespace(entityContainer.Namespace)));
                }
            }

            private void WriteNamespaceDeep(IEdmModel edmModel, string @namespace)
            {
                var allElements = AllElementsByNamespace(edmModel.SchemaElements, @namespace).ToList();

                var types = AllTypes(allElements).ToList();

                foreach (var enumType in AllEnumTypes(types))
                {
                    var odcmEnum = TryResolveType<OdcmEnum>(enumType.Name, enumType.Namespace);

                    odcmEnum.UnderlyingType = (OdcmPrimitiveType)ResolveType(enumType.UnderlyingType.Name, enumType.UnderlyingType.Namespace);
                    odcmEnum.IsFlags = enumType.IsFlags;
                    AddVocabularyAnnotations(odcmEnum, enumType);

                    foreach (var enumMember in enumType.Members)
                    {
                        odcmEnum.Members.Add(new OdcmEnumMember(enumMember.Name)
                        {
                            Value = ((EdmIntegerConstant)enumMember.Value).Value
                        });
                    }
                }

                foreach (var typeDefinition in AllTypeDefinitions(types))
                {
                    var odcmTypeDefinition = TryResolveType<OdcmTypeDefinition>(typeDefinition.Name, typeDefinition.Namespace);

                    // Type definitions should only support primitives as their base types [http://docs.oasis-open.org/odata/odata/v4.0/odata-v4.0-part3-csdl.html]
                    var baseType = ResolveType(typeDefinition.UnderlyingType.Name, typeDefinition.UnderlyingType.Namespace) as OdcmPrimitiveType;
                    if (baseType == null)
                    {
                        throw new InvalidOperationException("Type definitions should only accept primitive type as their base type.");
                    }

                    odcmTypeDefinition.BaseType = baseType;
                }

                foreach (var complexType in AllComplexTypes(types))
                {
                    var odcmClass = TryResolveType<OdcmClass>(complexType.Name, complexType.Namespace);

                    odcmClass.IsAbstract = complexType.IsAbstract;
                    odcmClass.IsOpen = complexType.IsOpen;
                    AddVocabularyAnnotations(odcmClass, complexType);

                    ResolveBaseClass(odcmClass, complexType);

                    foreach (var property in complexType.DeclaredProperties)
                    {
                        WriteProperty(odcmClass, property);
                    }
                }

                var entityTypes = AllEntityTypes(types).ToList();

                // First make a pass through entity types to establish their hierarchy;
                // this is useful for cases when base entity type is defined after derived one
                foreach (var entityType in entityTypes)
                {
                    var odcmClass = TryResolveType<OdcmEntityClass>(entityType.Name, entityType.Namespace);

                    ResolveBaseClass(odcmClass, entityType);
                }

                foreach (var entityType in entityTypes)
                {
                    var odcmClass = TryResolveType<OdcmEntityClass>(entityType.Name, entityType.Namespace);

                    odcmClass.IsAbstract = entityType.IsAbstract;
                    odcmClass.IsOpen = entityType.IsOpen;

                    foreach (var property in entityType.DeclaredProperties)
                    {
                        WriteProperty(odcmClass, property);
                    }

                    foreach (IEdmStructuralProperty keyProperty in entityType.Key())
                    {
                        OdcmProperty property;
                        if (!odcmClass.TryFindProperty(keyProperty.Name, out property))
                        {
                            throw new InvalidOperationException();
                        }

                        if (property.IsNullable)
                        {
                            //TODO: need to create a warning...
                        }
                        
                        odcmClass.Key.Add(property);
                    }

                    AddVocabularyAnnotations(odcmClass, entityType);
                }

                foreach (var entityContainer in AllEntityContainers(allElements))
                {
                    var odcmClass = TryResolveType<OdcmClass>(entityContainer.Name, entityContainer.Namespace);

                    odcmClass.Projection = new OdcmProjection
                    {
                        Type = odcmClass
                    };

                    _propertyCapabilitiesCache.Add(odcmClass, new List<OdcmCapability>());
                    AddVocabularyAnnotations(odcmClass, entityContainer);

                    var entitySets = ContainerElementsByKind<IEdmEntitySet>(entityContainer, EdmContainerElementKind.EntitySet);
                    foreach (var entitySet in entitySets)
                    {
                        WriteProperty(odcmClass, entitySet);
                    }

                    var singletons = ContainerElementsByKind<IEdmSingleton>(entityContainer, EdmContainerElementKind.Singleton);
                    foreach (var singleton in singletons)
                    {
                        WriteProperty(odcmClass, singleton);
                    }

                    var actionImports = ContainerElementsByKind<IEdmActionImport>(entityContainer, EdmContainerElementKind.ActionImport);
                    foreach (var actionImport in actionImports)
                    {
                        WriteMethod(odcmClass, actionImport.Action, actionImport);
                    }

                    var functionImports = ContainerElementsByKind<IEdmFunctionImport>(entityContainer, EdmContainerElementKind.FunctionImport);
                    foreach (var functionImport in functionImports)
                    {
                        WriteMethod(odcmClass, functionImport.Function, functionImport);
                    }
                }
            }

            private void WriteNamespaceMethods(IEdmModel edmModel, string @namespace, IEnumerable<IEdmEntityType> allEntityTypes)
            {
                var allElements = AllElementsByNamespace(edmModel.SchemaElements, @namespace).ToList();
                var actions = AllActions(allElements).ToList();
                var functions = AllFunctions(allElements).ToList();

                if (!actions.Any() && !functions.Any())
                {
                    return;
                }

                foreach (var entityType in allEntityTypes)
                {
                    var odcmClass = TryResolveType<OdcmEntityClass>(entityType.Name, entityType.Namespace);

                    foreach (var action in actions.Where(element => IsOperationBoundTo(element, entityType)))
                    {
                        WriteMethod(odcmClass, action);
                    }

                    foreach (var function in functions.Where(element => IsOperationBoundTo(element, entityType)))
                    {
                        WriteMethod(odcmClass, function);
                    }
                }
            }

            private void ResolveBaseClass(OdcmClass odcmClass, IEdmStructuredType structuredType)
            {
                if (structuredType.BaseType != null)
                {
                    var baseType = (IEdmSchemaElement)structuredType.BaseType;

                    OdcmClass baseClass = TryResolveType<OdcmClass>(baseType.Name, baseType.Namespace);

                    odcmClass.Base = baseClass;

                    if (!baseClass.Derived.Contains(odcmClass))
                    {
                        baseClass.Derived.Add(odcmClass);
                    }
                }
            }

            private T TryResolveType<T>(string name, string @namespace) where T : OdcmType
            {
                T type;
                if (!_odcmModel.TryResolveType(name, @namespace, out type))
                {
                    throw new InvalidOperationException();
                }
                return type;
            }

            private IEnumerable<T> ContainerElementsByKind<T>(IEdmEntityContainer entityContainer, EdmContainerElementKind kind) where T : class
            {
                return entityContainer
                            .Elements
                            .Where(element => element is T)
                            .Select(element => element as T);
            }

            private IEnumerable<IEdmSchemaElement> AllElementsByNamespace(IEnumerable<IEdmSchemaElement> schemaElements, string @namespace)
            {
                return schemaElements
                            .Where(element => element.Namespace == @namespace);
            }

            private IEnumerable<IEdmType> AllTypes(IEnumerable<IEdmSchemaElement> schemaElements)
            {
                return SchemaElementsByKind<IEdmType>(schemaElements, EdmSchemaElementKind.TypeDefinition);
            }

            private IEnumerable<IEdmEntityContainer> AllEntityContainers(IEnumerable<IEdmSchemaElement> schemaElements)
            {
                return SchemaElementsByKind<IEdmEntityContainer>(schemaElements, EdmSchemaElementKind.EntityContainer);
            }

            private IEnumerable<IEdmAction> AllActions(IEnumerable<IEdmSchemaElement> schemaElements)
            {
                return SchemaElementsByKind<IEdmAction>(schemaElements, EdmSchemaElementKind.Action)
                                            .Where(action => action.IsBound);
            }

            private IEnumerable<IEdmFunction> AllFunctions(IEnumerable<IEdmSchemaElement> schemaElements)
            {
                return SchemaElementsByKind<IEdmFunction>(schemaElements, EdmSchemaElementKind.Function)
                                            .Where(function => function.IsBound);
            }

            private IEnumerable<T> SchemaElementsByKind<T>(IEnumerable<IEdmSchemaElement> schemaElements, EdmSchemaElementKind kind) where T : class
            {
                return schemaElements
                            .Where(element => element.SchemaElementKind == kind)
                            .Select(element => element as T);
            }

            private IEnumerable<IEdmTypeDefinition> AllTypeDefinitions(IEnumerable<IEdmType> types)
            {
                return TypesByKind<IEdmTypeDefinition>(types, EdmTypeKind.TypeDefinition);
            }

            private IEnumerable<IEdmComplexType> AllComplexTypes(IEnumerable<IEdmType> types)
            {
                return TypesByKind<IEdmComplexType>(types, EdmTypeKind.Complex);
            }

            private IEnumerable<IEdmEntityType> AllEntityTypes(IEnumerable<IEdmType> types)
            {
                return TypesByKind<IEdmEntityType>(types, EdmTypeKind.Entity);
            }

            private IEnumerable<IEdmEnumType> AllEnumTypes(IEnumerable<IEdmType> types)
            {
                return TypesByKind<IEdmEnumType>(types, EdmTypeKind.Enum);
            }

            private IEnumerable<T> TypesByKind<T>(IEnumerable<IEdmType> types, EdmTypeKind kind) where T : class
            {
                return types
                            .Where(element => element.TypeKind == kind)
                            .Select(element => element as T);
            }

            private bool IsOperationBoundTo(IEdmOperation operation, IEdmEntityType entityType)
            {
                if (!operation.IsBound)
                {
                    return false;
                }

                var bindingParameterType = operation.Parameters.First().Type;

                return entityType.Equals(
                    bindingParameterType.IsCollection()
                        ? bindingParameterType.AsCollection().ElementType().Definition
                        : bindingParameterType.Definition);
            }

            private void WriteProperty(OdcmClass odcmClass, IEdmEntitySet entitySet)
            {
                var odcmType = ResolveType(entitySet.EntityType().Name, entitySet.EntityType().Namespace);
                var odcmProperty = new OdcmEntitySet(entitySet.Name)
                {
                    Class = odcmClass,
                    IsCollection = true,
                    IsLink = true
                };

                odcmProperty.Projection = new OdcmProjection
                {
                    Type = odcmType,
                    BackLink = odcmProperty
                };
                
                _propertyCapabilitiesCache.Add(odcmProperty, OdcmCapability.DefaultEntitySetCapabilities);

                AddVocabularyAnnotations(odcmProperty, entitySet);

                odcmClass.Properties.Add(odcmProperty);
            }

            private void WriteProperty(OdcmClass odcmClass, IEdmSingleton singleton)
            {
                var odcmType = ResolveType(singleton.EntityType().Name, singleton.EntityType().Namespace);
                var odcmProperty = new OdcmSingleton(singleton.Name)
                {
                    Class = odcmClass,
                    IsLink = true
                };

                odcmProperty.Projection = new OdcmProjection
                {
                    Type = odcmType,
                    BackLink = odcmProperty
                };

                _propertyCapabilitiesCache.Add(odcmProperty, OdcmCapability.DefaultSingletonCapabilities);

                AddVocabularyAnnotations(odcmProperty, singleton);

                odcmClass.Properties.Add(odcmProperty);
            }

            private void WriteMethod(OdcmClass odcmClass, IEdmOperation operation, IEdmOperationImport operationImport = null)
            {
                var parameters = operation.IsBound
                    ? (from parameter in operation.Parameters
                       where parameter != operation.Parameters.First()
                       select parameter)
                    : (operation.Parameters);

                var isBoundToCollection = operation.IsBound && operation.Parameters.First().Type.IsCollection();

                var odcmMethod = new OdcmMethod(operation.Name, odcmClass.Namespace)
                {
                    IsComposable = operation.IsFunction() && ((IEdmFunction)operation).IsComposable,
                    IsFunction = operation.IsFunction(),
                    IsBoundToCollection = isBoundToCollection,
                    Verbs = operation.IsAction() ? OdcmAllowedVerbs.Post : OdcmAllowedVerbs.Any,
                    Class = odcmClass
                };

                AddVocabularyAnnotations(odcmMethod, operation);

                if (operationImport != null)
                {
                    AddVocabularyAnnotations(odcmMethod, operationImport);
                }

                odcmClass.Methods.Add(odcmMethod);

                if (operation.ReturnType != null)
                {
                    odcmMethod.ReturnType = ResolveType(operation.ReturnType);
                    odcmMethod.IsCollection = operation.ReturnType.IsCollection();
                }

                var callingConvention =
                    operation.IsAction()
                        ? OdcmCallingConvention.InHttpMessageBody
                        : OdcmCallingConvention.InHttpRequestUri;

                foreach (var parameter in parameters)
                {
                    var odcmParameter = new OdcmParameter(parameter.Name)
                    {
                        CallingConvention = callingConvention,
                        Type = ResolveType(parameter.Type),
                        IsCollection = parameter.Type.IsCollection(),
                        IsNullable = parameter.Type.IsNullable
                    };

                    AddVocabularyAnnotations(odcmParameter, parameter);

                    odcmMethod.Parameters.Add(odcmParameter);
                }
            }

            private void WriteProperty(OdcmClass odcmClass, IEdmProperty property)
            {
                var odcmType = ResolveType(property.Type);
                var odcmProperty = new OdcmProperty(property.Name)
                {
                    Class = odcmClass,
                    IsNullable = property.Type.IsNullable,
                    IsCollection = property.Type.IsCollection(),
                    ContainsTarget =
                        property is IEdmNavigationProperty && ((IEdmNavigationProperty)property).ContainsTarget,
                    IsLink = property is IEdmNavigationProperty,
                    DefaultValue =
                        property is IEdmStructuralProperty ?
                            ((IEdmStructuralProperty)property).DefaultValueString :
                            null
                };

                odcmProperty.Projection = new OdcmProjection
                {
                    Type = odcmType,
                    BackLink = odcmProperty
                };


                _propertyCapabilitiesCache.Add(odcmProperty, OdcmCapability.DefaultPropertyCapabilities);

                AddVocabularyAnnotations(odcmProperty, property);

                odcmClass.Properties.Add(odcmProperty);
            }

            private OdcmType ResolveType(IEdmTypeReference realizedType)
            {
                if (realizedType.IsCollection())
                {
                    return ResolveType(realizedType.AsCollection().ElementType());
                }

                var realizedSchemaElement = (IEdmSchemaElement)realizedType.Definition;

                return ResolveType(realizedSchemaElement.Name, realizedSchemaElement.Namespace);
            }

            private OdcmType ResolveType(string name, string @namespace)
            {
                OdcmType type;
                if (!_odcmModel.TryResolveType(name, @namespace, out type))
                {
                    throw new InvalidOperationException();
                }

                OdcmTypeDefinition typeDefinition = type as OdcmTypeDefinition;
                if (typeDefinition != null)
                {
                    type = typeDefinition.BaseType;
                }

                return type;
            }

            private OdcmNamespace ResolveNamespace(string @namespace)
            {
                OdcmNamespace odcmNamespace;
                if (!_odcmModel.TryResolveNamespace(@namespace, out odcmNamespace))
                {
                    throw new InvalidOperationException();
                }

                return odcmNamespace;
            }

            /// <summary>
            /// Sets the OdcmCapabilities for the given annotated entity and also for the annotated navigation properties.
            /// </summary>
            public void SetCapabilitiesForEntity(OdcmObject odcmObject, IEnumerable<IEdmVocabularyAnnotation> annotations)
            {
                if (!(odcmObject is OdcmProperty))
                {
                     _propertyCapabilitiesCache.Add(odcmObject, OdcmCapability.DefaultEntitySetCapabilities);
                }

                ParseAllCapabilities(odcmObject, annotations);
            }

            private void ParseAllCapabilities(OdcmObject odcmObject, IEnumerable<IEdmVocabularyAnnotation> annotations)
            {
                var parser = new CapabilityAnnotationParser(_propertyCapabilitiesCache);

                foreach (IEdmValueAnnotation annotation in annotations)
                {
                    parser.ParseCapabilityAnnotation(odcmObject, annotation);
                }
            }
        }
    }
}
