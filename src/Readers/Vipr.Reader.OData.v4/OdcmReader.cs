// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.OData.Client;
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
                if (!EdmxReader.TryParse(edmx.CreateReader(ReaderOptions.None), out _edmModel, out errors))
                {
                    Debug.Assert(errors != null, "errors != null");

                    if (errors.FirstOrDefault() == null)
                    {
                        throw new InvalidOperationException();
                    }

                    throw new InvalidOperationException(errors.FirstOrDefault().ErrorMessage);
                }

                _odcmModel = new OdcmModel(serviceMetadata);

                AddPrimitives();

                WriteNamespaces();

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
                odcmObject.Description = _edmModel.GetDescriptionAnnotation(annotatableEdmEntity);
                odcmObject.LongDescription = _edmModel.GetLongDescriptionAnnotation(annotatableEdmEntity);
            }

            private void WriteNamespaces()
            {
                foreach (var declaredNamespace in _edmModel.DeclaredNamespaces)
                {
                    WriteNamespaceShallow(_edmModel, declaredNamespace);
                }
                foreach (var declaredNamespace in _edmModel.DeclaredNamespaces)
                {
                    WriteNamespaceDeep(_edmModel, declaredNamespace);
                }
            }

            private void WriteNamespaceShallow(IEdmModel edmModel, string @namespace)
            {
                _odcmModel.AddNamespace(@namespace);

                var namespaceElements = from elements in edmModel.SchemaElements
                                        where string.Equals(elements.Namespace, @namespace)
                                        select elements;

                var types = from element in namespaceElements
                            where element.SchemaElementKind == EdmSchemaElementKind.TypeDefinition
                            select element as IEdmType;
                var typeDefinitions = from element in types
                               where element.TypeKind == EdmTypeKind.TypeDefinition
                               select element as IEdmTypeDefinition;
                var complexTypes = from element in types
                                   where element.TypeKind == EdmTypeKind.Complex
                                   select element as IEdmComplexType;
                var entityTypes = from element in types
                                  where element.TypeKind == EdmTypeKind.Entity
                                  select element as IEdmEntityType;
                var enumTypes = from elements in types
                                where elements.TypeKind == EdmTypeKind.Enum
                                select elements as IEdmEnumType;

                var entityContainers = from element in namespaceElements
                                       where element.SchemaElementKind == EdmSchemaElementKind.EntityContainer
                                       select element as IEdmEntityContainer;

                foreach (var enumType in enumTypes)
                {
                    _odcmModel.AddType(new OdcmEnum(enumType.Name, ResolveNamespace(enumType.Namespace)));
                }

                foreach (var typeDefinition in typeDefinitions)
                {
                    _odcmModel.AddType(new OdcmTypeDefinition(typeDefinition.Name, ResolveNamespace(typeDefinition.Namespace)));
                }

                foreach (var complexType in complexTypes)
                {
                    _odcmModel.AddType(new OdcmComplexClass(complexType.Name, ResolveNamespace(complexType.Namespace)));
                }

                foreach (var entityType in entityTypes)
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

                foreach (var entityContainer in entityContainers)
                {
                    _odcmModel.AddType(new OdcmServiceClass(entityContainer.Name, ResolveNamespace(entityContainer.Namespace)));
                }
            }

            private void WriteNamespaceDeep(IEdmModel edmModel, string @namespace)
            {
                var namespaceElements = from elements in edmModel.SchemaElements
                                        where string.Equals(elements.Namespace, @namespace)
                                        select elements;

                var types = from element in namespaceElements
                            where element.SchemaElementKind == EdmSchemaElementKind.TypeDefinition
                            select element as IEdmType;
                var typeDefinitions = from element in types
                               where element.TypeKind == EdmTypeKind.TypeDefinition
                               select element as IEdmTypeDefinition;
                var complexTypes = from element in types
                                   where element.TypeKind == EdmTypeKind.Complex
                                   select element as IEdmComplexType;
                var entityTypes = from element in types
                                  where element.TypeKind == EdmTypeKind.Entity
                                  select element as IEdmEntityType;
                var enumTypes = from elements in types
                                where elements.TypeKind == EdmTypeKind.Enum
                                select elements as IEdmEnumType;

                var entityContainers = from element in namespaceElements
                                       where element.SchemaElementKind == EdmSchemaElementKind.EntityContainer
                                       select element as IEdmEntityContainer;

                var actions = from element in namespaceElements
                              where element.SchemaElementKind == EdmSchemaElementKind.Action && ((IEdmAction)element).IsBound
                              select element as IEdmAction;

                var functions = from element in namespaceElements
                                where element.SchemaElementKind == EdmSchemaElementKind.Function && ((IEdmFunction)element).IsBound
                                select element as IEdmFunction;

                foreach (var enumType in enumTypes)
                {
                    OdcmEnum odcmEnum;
                    if (!_odcmModel.TryResolveType(enumType.Name, enumType.Namespace, out odcmEnum))
                    {
                        throw new InvalidOperationException();
                    }

                    odcmEnum.UnderlyingType =
                        (OdcmPrimitiveType)ResolveType(enumType.UnderlyingType.Name, enumType.UnderlyingType.Namespace);
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

                foreach (var typeDefinition in typeDefinitions)
                {
                    OdcmTypeDefinition odcmTypeDefinition;
                    if (!_odcmModel.TryResolveType(typeDefinition.Name, typeDefinition.Namespace, out odcmTypeDefinition))
                    {
                        throw new InvalidOperationException();
                    }

                    // Type definitions should only support primitives as their base types [http://docs.oasis-open.org/odata/odata/v4.0/odata-v4.0-part3-csdl.html]
                    OdcmPrimitiveType baseType = ResolveType(typeDefinition.UnderlyingType.Name, typeDefinition.UnderlyingType.Namespace) as OdcmPrimitiveType;
                    if (baseType == null)
                    {
                        throw new InvalidOperationException("Type definitions should only accept primitive type as their base type.");
                    }

                    odcmTypeDefinition.BaseType = baseType;

                }

                foreach (var complexType in complexTypes)
                {
                    OdcmClass odcmClass;
                    if (!_odcmModel.TryResolveType(complexType.Name, complexType.Namespace, out odcmClass))
                    {
                        throw new InvalidOperationException();
                    }

                    odcmClass.IsAbstract = complexType.IsAbstract;
                    odcmClass.IsOpen = complexType.IsOpen;
                    AddVocabularyAnnotations(odcmClass, complexType);

                    if (complexType.BaseType != null)
                    {
                        var baseType = (IEdmSchemaElement)complexType.BaseType;

                        OdcmClass baseClass;
                        if (!_odcmModel.TryResolveType(baseType.Name, baseType.Namespace, out baseClass))
                        {
                            throw new InvalidOperationException();
                        }

                        odcmClass.Base = baseClass;

                        if (!baseClass.Derived.Contains(odcmClass))
                        {
                            baseClass.Derived.Add(odcmClass);
                        }
                    }

                    foreach (var property in complexType.DeclaredProperties)
                    {
                        WriteProperty(odcmClass, property);
                    }
                }

                foreach (var entityType in entityTypes)
                {
                    OdcmEntityClass odcmClass;
                    if (!_odcmModel.TryResolveType(entityType.Name, entityType.Namespace, out odcmClass))
                    {
                        throw new InvalidOperationException();
                    }

                    odcmClass.IsAbstract = entityType.IsAbstract;
                    odcmClass.IsOpen = entityType.IsOpen;
                    AddVocabularyAnnotations(odcmClass, entityType);

                    if (entityType.BaseType != null)
                    {
                        var baseType = (IEdmSchemaElement)entityType.BaseType;

                        OdcmClass baseClass;
                        if (!_odcmModel.TryResolveType(baseType.Name, baseType.Namespace, out baseClass))
                        {
                            throw new InvalidOperationException();
                        }

                        odcmClass.Base = baseClass;

                        if (!baseClass.Derived.Contains(odcmClass))
                        {
                            baseClass.Derived.Add(odcmClass);
                        }
                    }

                    foreach (var property in entityType.DeclaredProperties)
                    {
                        WriteProperty(odcmClass, property);
                    }

                    foreach (IEdmStructuralProperty keyProperty in entityType.Key())
                    {
                        OdcmProperty property;
                        if (!TryFindProperty(odcmClass, keyProperty, out property))
                        {
                            throw new InvalidOperationException();
                        }

                        if (property.IsNullable)
                        {
                            //TODO: need to create a warning...
                        }
                        
                        odcmClass.Key.Add(property);
                    }

                    var entityTypeActions = from element in actions
                                            where IsOperationBoundTo(element, entityType)
                                            select element;
                    foreach (var action in entityTypeActions)
                    {
                        WriteMethod(odcmClass, action);
                    }

                    var entityTypeFunctions = from element in functions
                                              where IsOperationBoundTo(element, entityType)
                                              select element;
                    foreach (var function in entityTypeFunctions)
                    {
                        WriteMethod(odcmClass, function);
                    }
                }

                foreach (var entityContainer in entityContainers)
                {
                    OdcmClass odcmClass;
                    if (!_odcmModel.TryResolveType(entityContainer.Name, entityContainer.Namespace, out odcmClass))
                    {
                        throw new InvalidOperationException();
                    }

                    AddVocabularyAnnotations(odcmClass, entityContainer);

                    var entitySets = from element in entityContainer.Elements
                                     where element.ContainerElementKind == EdmContainerElementKind.EntitySet
                                     select element as IEdmEntitySet;
                    foreach (var entitySet in entitySets)
                    {
                        WriteProperty(odcmClass, entitySet);
                    }

                    var singletons = from element in entityContainer.Elements
                                     where element.ContainerElementKind == EdmContainerElementKind.Singleton
                                     select element as IEdmSingleton;
                    foreach (var singleton in singletons)
                    {
                        WriteProperty(odcmClass, singleton);
                    }

                    var actionImports = from element in entityContainer.Elements
                                        where element.ContainerElementKind == EdmContainerElementKind.ActionImport
                                        select element as IEdmActionImport;
                    foreach (var actionImport in actionImports)
                    {
                        WriteMethod(odcmClass, actionImport.Action, actionImport);
                    }

                    var functionImports = from element in entityContainer.Elements
                                          where element.ContainerElementKind == EdmContainerElementKind.FunctionImport
                                          select element as IEdmFunctionImport;
                    foreach (var functionImport in functionImports)
                    {
                        WriteMethod(odcmClass, functionImport.Function, functionImport);
                    }
                }
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
                var odcmProperty = new OdcmProperty(entitySet.Name)
                {
                    Class = odcmClass,
                    Type = ResolveType(entitySet.EntityType().Name, entitySet.EntityType().Namespace),
                    IsCollection = true,
                    IsLink = true
                };

                AddVocabularyAnnotations(odcmProperty, entitySet);

                odcmClass.Properties.Add(odcmProperty);
            }

            private void WriteProperty(OdcmClass odcmClass, IEdmSingleton singleton)
            {
                var odcmProperty = new OdcmProperty(singleton.Name)
                {
                    Class = odcmClass,
                    Type = ResolveType(singleton.EntityType().Name, singleton.EntityType().Namespace),
                    IsLink = true
                };

                AddVocabularyAnnotations(odcmProperty, singleton);

                odcmClass.Properties.Add(odcmProperty);
            }

            private bool TryFindProperty(OdcmClass odcmClass, IEdmStructuralProperty keyProperty, out OdcmProperty odcmProperty)
            {
                if (odcmClass == null)
                {
                    odcmProperty = null;
                    return false;
                }

                foreach (OdcmProperty property in odcmClass.Properties)
                {
                    if (property.Name.Equals(keyProperty.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        odcmProperty = property;
                        return true;
                    }
                }

                return TryFindProperty(odcmClass.Base, keyProperty, out odcmProperty);
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
                var odcmProperty = new OdcmProperty(property.Name)
                {
                    Class = odcmClass,
                    IsNullable = property.Type.IsNullable,
                    Type = ResolveType(property.Type),
                    IsCollection = property.Type.IsCollection(),
                    ContainsTarget =
                        property is IEdmNavigationProperty && ((IEdmNavigationProperty)property).ContainsTarget,
                    IsLink = property is IEdmNavigationProperty,
                    DefaultValue =
                        property is IEdmStructuralProperty ?
                            ((IEdmStructuralProperty)property).DefaultValueString :
                            null
                };

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
        }
    }
}
