// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.Edm.Vocabularies;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;
using Vipr.Reader.OData.v4.Capabilities;

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
        public bool AddCastPropertiesForNavigationProperties { get; set; } = false;

        public OdcmModel GenerateOdcmModel(IEnumerable<TextFile> serviceMetadata)
        {
            var daemon = new ReaderDaemon
            {
                AddCastPropertiesForNavigationProperties = AddCastPropertiesForNavigationProperties
            };
            return daemon.GenerateOdcmModel(serviceMetadata);
        }

        private class ReaderDaemon
        {
            internal Logger Logger => LogManager.GetLogger("OdcmReader");

            private const string MetadataKey = "$metadata";

            private IEdmModel _edmModel = null;
            private OdcmModel _odcmModel;
            private PropertyCapabilitiesCache _propertyCapabilitiesCache;

            public OdcmModel GenerateOdcmModel(IEnumerable<TextFile> serviceMetadata)
            {
                Logger.Info("Generating OdcmModel");

                if (serviceMetadata == null)
                    throw new ArgumentNullException("serviceMetadata");

                var edmxFile = serviceMetadata.FirstOrDefault(f => f.RelativePath == MetadataKey);

                if (edmxFile == null)
                    throw new ArgumentException(
                        String.Format("Argument must contain file with RelativePath \"{0}", MetadataKey),
                        "serviceMetadata");

                var edmx = XElement.Parse(edmxFile.Contents);

                IEnumerable<EdmError> errors;

                if (!CsdlReader.TryParse(edmx.CreateReader(ReaderOptions.None), /*_capabilitiesModel,*/ out _edmModel, out errors))
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

                LogModelStats();

                return _odcmModel;
            }

            private void LogModelStats()
            {
                var namespaces = _odcmModel.Namespaces.Where(x => x.Name != "Edm");
                Logger.Info($"Parsed {namespaces.Count()} namespace(s)");

                var types = namespaces.SelectMany(x => x.Types);

                Logger.Info($"Parsed {types.Count()} types overall");

                Logger.Info($"Parsed {types.Count(t => t is OdcmEntityClass)} EntityTypes");
                Logger.Info($"Parsed {types.Count(t => t is OdcmMediaClass)} EntityTypes with Stream");
                Logger.Info($"Parsed {types.Count(t => t is OdcmComplexClass)} ComplexTypes");
                Logger.Info($"Parsed {types.Count(t => t is OdcmEnum)} EnumTypes");
                Logger.Info($"Parsed {types.Count(t => t is OdcmTypeDefinition)} TypeDefinitions");

                var classes = namespaces.SelectMany(x => x.Classes);

                Logger.Info($"Parsed {classes.Count(t => t.IsAbstract)} abstract EntityTypes");

                var methods = classes.SelectMany(x => x.Methods);

                Logger.Info($"Parsed {methods.Count(m => m.IsFunction)} Functions");
                Logger.Info($"Parsed {methods.Count(m => !m.IsFunction)} Actions");

                Logger.Info($"Parsed {_propertyCapabilitiesCache.AnnotatedObjects.Count()} annotated objects");
            }

            private void AddPrimitives()
            {
                foreach (var entry in PrimitiveTypes)
                {
                    try
                    {
                        _odcmModel.AddType(new OdcmPrimitiveType(entry[1], OdcmNamespace.GetWellKnownNamespace(entry[0])));
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, e.Message);
                    }
                }
            }

            private void AddVocabularyAnnotations(OdcmObject odcmObject, IEdmVocabularyAnnotatable annotatableEdmEntity)
            {
#if false
                odcmObject.Annotations = ODataVocabularyReader.GetOdcmAnnotations(_edmModel, annotatableEdmEntity).ToList();
#endif
                odcmObject.Description = _edmModel.GetDescriptionAnnotation(annotatableEdmEntity);
                odcmObject.LongDescription = _edmModel.GetLongDescriptionAnnotation(annotatableEdmEntity);

                // https://github.com/OData/odata.net/blob/75df8f44f2b81f984589790be4885b6ee8946ad0/src/Microsoft.OData.Edm/ExtensionMethods/ExtensionMethods.cs#L196
                var annotations = _edmModel.FindVocabularyAnnotations(annotatableEdmEntity)
                                      .Where(x => x.Term.Name != "Description" && x.Term.Name != "LongDescription");

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
                // Get all of the top-level elements defined in the metadata <Schema> element. This includes the EntityContainer.
                var allElements = AllElementsByNamespace(edmModel.SchemaElements, @namespace).ToList();

                // Get all of the types found in the metadata <Schema> element. No EntityContainer elements.
                var types = AllTypes(allElements).ToList();

                foreach (var enumType in AllEnumTypes(types))
                {
                    var odcmEnum = TryResolveType<OdcmEnum>(enumType.Name, enumType.Namespace);

                    odcmEnum.UnderlyingType = (OdcmPrimitiveType)ResolveType(enumType.UnderlyingType.Name, enumType.UnderlyingType.Namespace);
                    odcmEnum.IsFlags = enumType.IsFlags;
                    AddVocabularyAnnotations(odcmEnum, enumType);

                    foreach (var enumMember in enumType.Members)
                    {
                        var odcmEnumMember = new OdcmEnumMember(enumMember.Name)
                        {
                            Value = (enumMember.Value).Value
                        };

                        AddVocabularyAnnotations(odcmEnumMember, enumMember);
                        odcmEnum.Members.Add(odcmEnumMember);
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

                    // Add all of the structural and navigation properties to the EntityType, which is an OdcmClass.
                    // Capability annotations are not yet set on the OdcmClass.Properties[x].Projection.
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

                    // Add the capability annotations to OdcmClass.Properties[x].Projection
                    AddVocabularyAnnotations(odcmClass, entityType);
                }

                foreach (var entityContainer in AllEntityContainers(allElements))
                {
                    var odcmClass = TryResolveType<OdcmClass>(entityContainer.Name, entityContainer.Namespace);

                    odcmClass.Projection = new OdcmProjection
                    {
                        Type = odcmClass
                    };

                    try
                    {
                        _propertyCapabilitiesCache.Add(odcmClass, new List<OdcmCapability>());
                    }
                    catch (InvalidOperationException e)
                    {
                        Logger.Warn("Failed to add property to cache", e);
                    }
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
                        WriteMethodImport(odcmClass, actionImport.Action, actionImport);
                    }

                    var functionImports = ContainerElementsByKind<IEdmFunctionImport>(entityContainer, EdmContainerElementKind.FunctionImport);
                    foreach (var functionImport in functionImports)
                    {
                        WriteMethodImport(odcmClass, functionImport.Function, functionImport);
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

                    foreach (var action in actions.Where(element => IsOperationBoundTo(element, entityType)).GroupBy(e => e.Name))
                    {
                        WriteMethodGroup(odcmClass, action);
                    }

                    foreach (var function in functions.Where(element => IsOperationBoundTo(element, entityType)).GroupBy(e => e.Name))
                    {
                        WriteMethodGroup(odcmClass, function);
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
            private T TryResolveType<T>(string fullyQualifiedName) where T : OdcmType
            {
                var lastDotIndex = fullyQualifiedName.LastIndexOf('.');
                return TryResolveType<T>(fullyQualifiedName.Substring(lastDotIndex + 1), fullyQualifiedName.Substring(0, lastDotIndex));
            }
            private T TryResolveType<T>(string name, string @namespace) where T : OdcmType
            {
                T type;
                if (!_odcmModel.TryResolveType(name, @namespace, out type))
                {
                    throw new InvalidOperationException($"Could not resolve type: {name}");
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

                if (!operation.Parameters.Any())
                {
                    throw new InvalidOperationException($"No parameters for bound method {operation.Name}");
                }

                var bindingParameterType = operation.Parameters.First().Type;

                return entityType.Equals(
                    bindingParameterType.IsCollection()
                        ? bindingParameterType.AsCollection().ElementType().Definition
                        : bindingParameterType.Definition);
            }

            private void WriteProperty(OdcmClass odcmClass, IEdmEntitySet entitySet)
            {
                try
                {
                    var navPropBindings = GetNavigationPropertyBindings(entitySet);
                    var odcmType = ResolveType(entitySet.EntityType().Name, entitySet.EntityType().Namespace);
                    var odcmProperty = new OdcmEntitySet(entitySet.Name, navPropBindings)
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

                    try
                    {
                        _propertyCapabilitiesCache.Add(odcmProperty, OdcmCapability.DefaultEntitySetCapabilities);
                    }
                    catch (InvalidOperationException e)
                    {
                        Logger.Warn("Failed to add property to cache", e);
                    }

                    AddVocabularyAnnotations(odcmProperty, entitySet);

                    odcmClass.Properties.Add(odcmProperty);
                }
                // If we hit an invalid type, skip this property
                catch (InvalidOperationException e)
                {
                    Logger.Error("Could not resolve type", e);
                }
            }

            /// <summary>
            /// Gets all of the NavigationPropertyBindings from a IEdmNavigationSource. We need these for building correct $ref paths.
            /// http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/cs01/odata-csdl-xml-v4.01-cs01.html#sec_BindingPath
            /// </summary>
            /// <param name="entitySetOrSingleton">An IEdmSingleton or IEdmEntitySet.</param>
            /// <returns>A dictionary of Binding Paths and respective Binding Targets.</returns>
            private Dictionary<string, string> GetNavigationPropertyBindings<T>(T entitySetOrSingleton) where T : IEdmNavigationSource
            {
                // NavigationPropertyBindings stored in a dictionary. Each Path MUST be unique.
                Dictionary<string, string> navPropBindings = new Dictionary<string, string>();

                foreach (IEdmNavigationPropertyBinding navPropBinding in entitySetOrSingleton.NavigationPropertyBindings)
                {
                    navPropBindings.Add(navPropBinding.Path.Path, navPropBinding.Target.Name);
                }

                return navPropBindings;
            }

            private void WriteProperty(OdcmClass odcmClass, IEdmSingleton singleton)
            {
                try
                {
                    // Get and set the NavigationPropertyBindings on the OdcmModel that we'll use in the generator.
                    var navPropBindings = GetNavigationPropertyBindings(singleton);
                    var odcmProperty = new OdcmSingleton(singleton.Name, navPropBindings)
                    {
                        Class = odcmClass,
                        IsLink = true
                    };

                    var odcmType = ResolveType(singleton.EntityType().Name, singleton.EntityType().Namespace);
                    odcmProperty.Projection = new OdcmProjection
                    {
                        Type = odcmType,
                        BackLink = odcmProperty
                    };

                    try
                    {
                        _propertyCapabilitiesCache.Add(odcmProperty, OdcmCapability.DefaultSingletonCapabilities);
                    }
                    catch (InvalidOperationException e)
                    {
                        Logger.Warn("Failed to add property to cache", e);
                    }

                    AddVocabularyAnnotations(odcmProperty, singleton);

                    odcmClass.Properties.Add(odcmProperty);
                }
                catch (InvalidOperationException e)
                {
                    Logger.Error("Could not resolve type", e);
                }
            }

            private void WriteMethodGroup(OdcmClass odcmClass, IGrouping<string, IEdmOperation> operations)
            {
                var odcmMethod = WriteMethod(odcmClass, operations.First());

                foreach (var operation in operations.Skip(1))
                {
                    odcmMethod.Overloads.Add(WriteMethod(odcmClass, operation));
                }

                odcmClass.Methods.Add(odcmMethod);
            }
            private void WriteMethodImport(OdcmClass odcmClass, IEdmOperation operation, IEdmOperationImport operationImport)
            {
                var odcmMethod = WriteMethod(odcmClass, operation);

                AddVocabularyAnnotations(odcmMethod, operationImport);

                odcmClass.Methods.Add(odcmMethod);
            }

            private OdcmMethod WriteMethod(OdcmClass odcmClass, IEdmOperation operation)
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

                return odcmMethod;
            }

            private void WriteProperty(OdcmClass odcmClass, IEdmProperty property)
            {
                try
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

                    try
                    {
                        _propertyCapabilitiesCache.Add(odcmProperty, OdcmCapability.DefaultPropertyCapabilities);
                    }
                    catch (InvalidOperationException e)
                    {
                        Logger.Warn("Failed to add property to cache", e);
                    }

                    AddVocabularyAnnotations(odcmProperty, property);

                    odcmClass.Properties.Add(odcmProperty);

                    if (AddCastPropertiesForNavigationProperties)
                    {
                        AddCastPropertiesForNavigationProperty(odcmClass, property, odcmProperty);
                    }

                }
                catch (InvalidOperationException e)
                {
                    Logger.Error("Could not resolve type", e);
                }
            }
            public bool AddCastPropertiesForNavigationProperties { get; set; } = false;

            private void AddCastPropertiesForNavigationProperty(OdcmClass odcmClass, IEdmProperty property, OdcmProperty odcmProperty)
            {
                var derivedTypes = _edmModel.GetDerivedTypeConstraints(property)?.Distinct(StringComparer.InvariantCultureIgnoreCase);
                if (derivedTypes != null)
                {
                    foreach (var derivedType in derivedTypes)
                    {
                        var odcmDerivedClass = TryResolveType<OdcmClass>(derivedType);

                        var derivedCastProperty = odcmProperty.Clone($"{odcmProperty.Name}As{odcmDerivedClass.Name.First().ToString().ToUpper()}{odcmDerivedClass.Name.Substring(1)}");
                        derivedCastProperty.ParentPropertyType = odcmProperty;
                        derivedCastProperty.Projection.Type = odcmDerivedClass;
                        odcmProperty.ChildPropertyTypes.Add(derivedCastProperty);
                        odcmClass.Properties.Add(derivedCastProperty);
                        _propertyCapabilitiesCache.Add(derivedCastProperty, OdcmCapability.DefaultPropertyCapabilities);
                    }
                }
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
                    throw new InvalidOperationException("Error: Invalid type \"" + name + "\"");
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
                    try
                    {
                        _propertyCapabilitiesCache.Add(odcmObject, OdcmCapability.DefaultEntitySetCapabilities);
                    }
                    catch (InvalidOperationException e)
                    {
                        Logger.Warn("Failed to add property to cache", e);
                    }
                }

                ParseAllCapabilities(odcmObject, annotations);
            }

            private void ParseAllCapabilities(OdcmObject odcmObject, IEnumerable<IEdmVocabularyAnnotation> annotations)
            {
                var parser = new CapabilityAnnotationParser(_propertyCapabilitiesCache);

                // Sort annotations by distance of their target from this odcmObject;
                // as a result, overridden annotations always precede the inherited ones.
                // This logic supports EntityType inheritance hierarchy.
                var annotationsOrdered = annotations.OrderBy(x => GetTargetDepth(x.Target as IEdmSchemaElement, odcmObject as OdcmClass));

                foreach (var annotation in annotationsOrdered)
                    parser.ParseCapabilityAnnotation(odcmObject, annotation);
            }

            private static int GetTargetDepth(IEdmSchemaElement target, OdcmClass odcmClass)
            {
                int depth = 0;

                if (target != null && odcmClass != null)
                {
                    for (; odcmClass.FullName != target.FullName(); odcmClass = odcmClass.Base)
                    {
                        if (odcmClass.Base == null)
                        {
                            throw new InvalidOperationException($"Could not find target {target.FullName()}");
                        }

                        ++depth;
                    }

                }

                return depth;
            }
        }
    }
}
