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

namespace ODataReader.v4
{
    public class OdcmReader : IOdcmReader
    {
        public OdcmModel GenerateOdcmModel(IDictionary<string, string> serviceMetadata)
        {
            var daemon = new ReaderDaemon();
            return daemon.GenerateOdcmModel(serviceMetadata);
        }

        private class ReaderDaemon
        {
            private const string MetadataKey = "$metadata";

            private IEdmModel _edmModel = null;
            private OdcmModel _odcmModel;

            public OdcmModel GenerateOdcmModel(IDictionary<string, string> serviceMetadata)
            {
                if (serviceMetadata == null)
                    throw new ArgumentNullException("serviceMetadata");

                if (!serviceMetadata.ContainsKey(MetadataKey))
                    throw new ArgumentException("Argument must contain value for key \"$metadata\"", "serviceMetadata");

                var edmx = XElement.Parse(serviceMetadata[MetadataKey]);

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

                WriteNamespaces();

                return _odcmModel;
            }

            private void WriteNamespaces()
            {
                foreach (var declaredNamespace in _edmModel.DeclaredNamespaces)
                {
                    WriteNamespace(_edmModel, declaredNamespace);
                }
            }

            private void WriteNamespace(IEdmModel edmModel, string @namespace)
            {
                var namespaceElements = from elements in edmModel.SchemaElements
                    where string.Equals(elements.Namespace, @namespace)
                    select elements;

                var types = from element in namespaceElements
                    where element.SchemaElementKind == EdmSchemaElementKind.TypeDefinition
                    select element as IEdmType;
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
                    where element.SchemaElementKind == EdmSchemaElementKind.Action && ((IEdmAction) element).IsBound
                    select element as IEdmAction;

                var functions = from element in namespaceElements
                    where element.SchemaElementKind == EdmSchemaElementKind.Function && ((IEdmFunction) element).IsBound
                    select element as IEdmFunction;

                foreach (var enumType in enumTypes)
                {
                    OdcmEnum odcmEnum;
                    if (!_odcmModel.TryResolveType(enumType.Name, enumType.Namespace, out odcmEnum))
                    {
                        odcmEnum = new OdcmEnum(enumType.Name, enumType.Namespace);
                        _odcmModel.AddType(odcmEnum);
                    }

                    odcmEnum.UnderlyingType =
                        (OdcmPrimitiveType)
                            ResolveType(enumType.UnderlyingType.Name, enumType.UnderlyingType.Namespace,
                                TypeKind.Primitive);
                    odcmEnum.IsFlags = enumType.IsFlags;

                    foreach (var enumMember in enumType.Members)
                    {
                        odcmEnum.Members.Add(new OdcmEnumMember(enumMember.Name)
                        {
                            Value = ((EdmIntegerConstant) enumMember.Value).Value
                        });
                    }
                }

                foreach (var complexType in complexTypes)
                {
                    OdcmClass odcmClass;
                    if (!_odcmModel.TryResolveType(complexType.Name, complexType.Namespace, out odcmClass))
                    {
                        odcmClass = new OdcmClass(complexType.Name, complexType.Namespace, OdcmClassKind.Complex);
                        _odcmModel.AddType(odcmClass);
                    }

                    odcmClass.IsAbstract = complexType.IsAbstract;
                    odcmClass.IsOpen = complexType.IsOpen;

                    if (complexType.BaseType != null)
                    {
                        OdcmClass baseClass;
                        if (
                            !_odcmModel.TryResolveType(((IEdmSchemaElement) complexType.BaseType).Name,
                                ((IEdmSchemaElement) complexType.BaseType).Namespace, out baseClass))
                        {
                            baseClass = new OdcmClass(((IEdmSchemaElement) complexType.BaseType).Name,
                                ((IEdmSchemaElement) complexType.BaseType).Namespace, OdcmClassKind.Complex);
                            _odcmModel.AddType(baseClass);
                        }
                        odcmClass.Base = baseClass;
                        if (!baseClass.Derived.Contains(odcmClass))
                            baseClass.Derived.Add(odcmClass);
                    }

                    foreach (var property in complexType.DeclaredProperties)
                    {
                        WriteProperty(odcmClass, property);
                    }
                }

                foreach (var entityType in entityTypes)
                {
                    OdcmClass odcmClass;
                    if (!_odcmModel.TryResolveType(entityType.Name, entityType.Namespace, out odcmClass))
                    {
                        odcmClass = new OdcmClass(entityType.Name, entityType.Namespace, OdcmClassKind.Entity);
                        _odcmModel.AddType(odcmClass);
                    }

                    odcmClass.IsAbstract = entityType.IsAbstract;
                    odcmClass.IsOpen = entityType.IsOpen;

                    if (entityType.HasStream)
                    {
                        odcmClass.Kind = OdcmClassKind.MediaEntity;
                    }

                    if (entityType.BaseType != null)
                    {
                        var baseEntityType = (IEdmEntityType) entityType.BaseType;

                        OdcmClass baseClass;
                        if (!_odcmModel.TryResolveType(baseEntityType.Name, baseEntityType.Namespace, out baseClass))
                        {
                            baseClass = new OdcmClass(baseEntityType.Name, baseEntityType.Namespace,
                                OdcmClassKind.Entity);
                            _odcmModel.AddType(baseClass);
                        }

                        odcmClass.Base = baseClass;

                        if (!baseClass.Derived.Contains(odcmClass))
                            baseClass.Derived.Add(odcmClass);
                    }

                    foreach (var property in entityType.DeclaredProperties)
                    {
                        WriteProperty(odcmClass, property);
                    }

                    foreach (IEdmStructuralProperty keyProperty in entityType.Key())
                    {
                        var property = FindProperty(odcmClass, keyProperty);
                        if (property != null)
                        {
                            // The properties that compose the key MUST be non-nullable.
                            property.IsNullable = false;
                        }

                        odcmClass.Key.Add(property);
                    }

                    var entityTypeActions = from element in actions
                        where IsOperationBound(element, entityType)
                        select element;
                    foreach (var action in entityTypeActions)
                    {
                        WriteMethod(odcmClass, action);
                    }

                    var entityTypeFunctions = from element in functions
                        where IsOperationBound(element, entityType)
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
                        odcmClass = new OdcmClass(entityContainer.Name, entityContainer.Namespace, OdcmClassKind.Service);
                        _odcmModel.AddType(odcmClass);
                    }

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
                        WriteMethod(odcmClass, actionImport.Action);
                    }

                    var functionImports = from element in entityContainer.Elements
                        where element.ContainerElementKind == EdmContainerElementKind.FunctionImport
                        select element as IEdmFunctionImport;
                    foreach (var functionImport in functionImports)
                    {
                        WriteMethod(odcmClass, functionImport.Function);
                    }
                }
            }

            private bool IsOperationBound(IEdmOperation operation, IEdmEntityType entityType)
            {
                var bindingParameterType = operation.Parameters.First().Type;
                return string.Equals(bindingParameterType.FullName(), entityType.FullTypeName()) ||
                       (bindingParameterType.IsCollection() &&
                        string.Equals(bindingParameterType.AsCollection().ElementType().FullName(),
                            entityType.FullTypeName()));
            }

            private void WriteProperty(OdcmClass odcmClass, IEdmEntitySet entitySet)
            {
                var odcmProperty = new OdcmProperty(entitySet.Name)
                {
                    Class = odcmClass,
                    Type = ResolveType(entitySet.EntityType().Name, entitySet.EntityType().Namespace,
                        TypeKind.Entity),
                    IsCollection = true,
                    IsLink = true
                };

                odcmClass.Properties.Add(odcmProperty);
            }

            private void WriteProperty(OdcmClass odcmClass, IEdmSingleton singleton)
            {
                var odcmProperty = new OdcmProperty(singleton.Name)
                {
                    Class = odcmClass,
                    Type = ResolveType(singleton.EntityType().Name, singleton.EntityType().Namespace,
                        TypeKind.Entity),
                    IsLink = true
                };

                odcmClass.Properties.Add(odcmProperty);
            }

            private OdcmProperty FindProperty(OdcmClass odcmClass, IEdmStructuralProperty keyProperty)
            {
                if (odcmClass == null)
                    return null;

                foreach (OdcmProperty property in odcmClass.Properties)
                {
                    if (property.Name.Equals(keyProperty.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        return property;
                    }
                }

                return FindProperty(odcmClass.Base, keyProperty);
            }

            private void WriteMethod(OdcmClass odcmClass, IEdmOperation operation)
            {
                IEnumerable<IEdmOperationParameter> parameters = operation.IsBound
                    ? (from parameter in operation.Parameters
                        where parameter != operation.Parameters.First()
                        select parameter)
                    : (operation.Parameters);

                bool isBoundToCollection = operation.IsBound && operation.Parameters.First().Type.IsCollection();

                var odcmMethod = new OdcmMethod(operation.Name)
                {
                    IsComposable = operation.IsFunction() && ((IEdmFunction) operation).IsComposable,
                    IsBoundToCollection = isBoundToCollection,
                    Verbs = operation.IsAction() ? OdcmAllowedVerbs.Post : OdcmAllowedVerbs.Any,
                    Class = odcmClass
                };

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
                    odcmMethod.Parameters.Add(new OdcmParameter(parameter.Name)
                    {
                        CallingConvention = callingConvention,
                        Type = ResolveType(parameter.Type),
                        IsCollection = parameter.Type.IsCollection(),
                        IsNullable = parameter.Type.IsNullable
                    });
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
                        property is IEdmNavigationProperty && ((IEdmNavigationProperty) property).ContainsTarget,
                    IsLink = property is IEdmNavigationProperty
                };

                odcmClass.Properties.Add(odcmProperty);
            }

            private OdcmType ResolveType(IEdmTypeReference realizedType)
            {
                if (realizedType.IsCollection())
                {
                    realizedType = realizedType.AsCollection().ElementType();
                }

                if (realizedType.IsComplex())
                    return ResolveType(realizedType.Definition as IEdmSchemaElement, TypeKind.Complex);

                if (realizedType.IsEntity())
                    return ResolveType(realizedType.Definition as IEdmSchemaElement, TypeKind.Entity);

                if (realizedType.IsEnum())
                    return ResolveType(realizedType.Definition as IEdmSchemaElement, TypeKind.Enum);

                return ResolveType(realizedType.Definition as IEdmSchemaElement, TypeKind.Primitive);
            }

            private OdcmType ResolveType(IEdmSchemaElement realizedSchemaElement, TypeKind kind)
            {
                return ResolveType(realizedSchemaElement.Name, realizedSchemaElement.Namespace, kind);
            }

            private OdcmType ResolveType(string name, string @namespace, TypeKind kind)
            {
                OdcmType type;

                if (!_odcmModel.TryResolveType(name, @namespace, out type))
                {
                    switch (kind)
                    {
                        case TypeKind.Complex:
                            type = new OdcmClass(name, @namespace, OdcmClassKind.Complex);
                            break;
                        case TypeKind.Entity:
                            type = new OdcmClass(name, @namespace, OdcmClassKind.Entity);
                            break;
                        case TypeKind.Enum:
                            type = new OdcmEnum(name, @namespace);
                            break;
                        case TypeKind.Primitive:
                            type = new OdcmPrimitiveType(name, @namespace);
                            break;
                    }

                    _odcmModel.AddType(type);
                }

                return type;
            }
        }
    }
}
