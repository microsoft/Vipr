// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Validation;
using Vipr.Core;
using Vipr.Core.CodeModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace ODataReader.v3
{
    public class OdcmReader : IOdcmReader
    {
        private static readonly string[][] PrimitiveTypes =
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

        public OdcmModel GenerateOdcmModel(TextFileCollection serviceMetadata)
        {
            var daemon = new ReaderDaemon();
            return daemon.GenerateOdcmModel(serviceMetadata);
        }

        private class ReaderDaemon
        {
            private const string MetadataKey = "$metadata";

            private IEdmModel _edmModel = null;
            private OdcmModel _odcmModel;

            public OdcmModel GenerateOdcmModel(TextFileCollection serviceMetadata)
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

            private void WriteNamespaces()
            {
                var declaredNamespaces = (from se in _edmModel.SchemaElements
                                          select se.Namespace).Distinct();
                foreach (var declaredNamespace in declaredNamespaces)
                {
                    WriteNamespaceShallow(_edmModel, declaredNamespace);
                }
                foreach (var declaredNamespace in declaredNamespaces)
                {
                    WriteNamespaceDeep(_edmModel, declaredNamespace);
                }
            }

            private void WriteNamespaceShallow(IEdmModel edmModel, string @namespace)
            {
                _odcmModel.AddNamespace(@namespace);

                var namespaceElements = from se in edmModel.SchemaElements
                                        where string.Equals(se.Namespace, @namespace)
                                        select se;

                var types = from se in namespaceElements
                            where se.SchemaElementKind == EdmSchemaElementKind.TypeDefinition
                            select se as IEdmType;

                var complexTypes = from se in types
                                   where se.TypeKind == EdmTypeKind.Complex
                                   select se as IEdmComplexType;

                var entityTypes = from se in types
                                  where se.TypeKind == EdmTypeKind.Entity
                                  select se as IEdmEntityType;

                var enumTypes = from se in types
                                where se.TypeKind == EdmTypeKind.Enum
                                select se as IEdmEnumType;

                var entityContainers = from se in namespaceElements
                                       where se.SchemaElementKind == EdmSchemaElementKind.EntityContainer
                                       select se as IEdmEntityContainer;

                foreach (var enumType in enumTypes)
                {
                    _odcmModel.AddType(new OdcmEnum(enumType.Name, ResolveNamespace(enumType.Namespace)));
                }

                foreach (var complexType in complexTypes)
                {
                    _odcmModel.AddType(new OdcmComplexClass(complexType.Name, ResolveNamespace(complexType.Namespace)));
                }

                foreach (var entityType in entityTypes)
                {
                    _odcmModel.AddType(new OdcmEntityClass(entityType.Name, ResolveNamespace(entityType.Namespace)));
                }

                foreach (var entityContainer in entityContainers)
                {
                    _odcmModel.AddType(new OdcmServiceClass(entityContainer.Name, ResolveNamespace(entityContainer.Namespace)));
                }
            }

            private void WriteNamespaceDeep(IEdmModel edmModel, string @namespace)
            {
                var namespaceElements = from se in edmModel.SchemaElements
                                        where string.Equals(se.Namespace, @namespace)
                                        select se;

                var types = from se in namespaceElements
                            where se.SchemaElementKind == EdmSchemaElementKind.TypeDefinition
                            select se as IEdmType;

                var complexTypes = from se in types
                                   where se.TypeKind == EdmTypeKind.Complex
                                   select se as IEdmComplexType;

                var entityTypes = from se in types
                                  where se.TypeKind == EdmTypeKind.Entity
                                  select se as IEdmEntityType;

                var enumTypes = from se in types
                                where se.TypeKind == EdmTypeKind.Enum
                                select se as IEdmEnumType;

                var entityContainers = from se in namespaceElements
                                       where se.SchemaElementKind == EdmSchemaElementKind.EntityContainer
                                       select se as IEdmEntityContainer;

                var boundFunctions = from se in
                                         (from ec in entityContainers
                                          select ec.Elements).SelectMany(v => v)
                                     where
                                         se.ContainerElementKind == EdmContainerElementKind.FunctionImport &&
                                         ((IEdmFunctionImport)se).IsBindable
                                     select se as IEdmFunctionImport;

                foreach (var enumType in enumTypes)
                {
                    OdcmEnum odcmEnum;
                    if (!_odcmModel.TryResolveType(enumType.Name, enumType.Namespace, out odcmEnum))
                    {
                        throw new InvalidOperationException();
                    }

                    odcmEnum.UnderlyingType = (OdcmPrimitiveType)ResolveType(enumType.UnderlyingType.Name, enumType.UnderlyingType.Namespace);
                    odcmEnum.IsFlags = enumType.IsFlags;

                    foreach (var enumMember in enumType.Members)
                    {
                        odcmEnum.Members.Add(new OdcmEnumMember(enumMember.Name)
                        {
                            Value = ((EdmIntegerConstant)enumMember.Value).Value
                        });
                    }
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

                    var entityTypeFunctions = from se in boundFunctions
                                              where IsFunctionBound(se, entityType)
                                              select se;
                    foreach (var function in entityTypeFunctions)
                    {
                        WriteMethod(odcmClass, function);
                    }
                }

                foreach (var entityContainer in entityContainers)
                {
                    OdcmServiceClass odcmClass;
                    if (!_odcmModel.TryResolveType(entityContainer.Name, entityContainer.Namespace, out odcmClass))
                    {
                        throw new InvalidOperationException();
                    }

                    var entitySets = from se in entityContainer.Elements
                                     where se.ContainerElementKind == EdmContainerElementKind.EntitySet
                                     select se as IEdmEntitySet;
                    foreach (var entitySet in entitySets)
                    {
                        WriteProperty(odcmClass, entitySet);
                    }

                    var functionImports = from se in entityContainer.Elements
                                          where
                                              se.ContainerElementKind == EdmContainerElementKind.FunctionImport &&
                                              !((IEdmFunctionImport)se).IsBindable
                                          select se as IEdmFunctionImport;
                    foreach (var functionImport in functionImports)
                    {
                        WriteMethod(odcmClass, functionImport);
                    }
                }
            }

            private bool IsFunctionBound(IEdmFunctionImport function, IEdmEntityType entityType)
            {
                var bindingParameterType = function.Parameters.First().Type;

                return (bindingParameterType.Definition == entityType) ||
                       (bindingParameterType.IsCollection() &&
                        bindingParameterType.AsCollection().ElementType().Definition == entityType);
            }

            private void WriteProperty(OdcmClass odcmClass, IEdmEntitySet entitySet)
            {
                var odcmProperty = new OdcmProperty(entitySet.Name)
                {
                    Type = ResolveType(entitySet.ElementType.Name, entitySet.ElementType.Namespace),
                    IsCollection = true,
                    IsLink = true,
                    Class = odcmClass
                };

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

            private void WriteMethod(OdcmClass odcmClass, IEdmFunctionImport operation)
            {
                IEnumerable<IEdmFunctionParameter> parameters = operation.IsBindable
                    ? (from parameter in operation.Parameters
                       where parameter != operation.Parameters.First()
                       select parameter)
                    : (operation.Parameters);

                bool isBoundToCollection = operation.IsBindable && operation.Parameters.First().Type.IsCollection();

                var odcmMethod = new OdcmMethod(operation.Name, odcmClass.Namespace)
                {
                    Verbs = operation.IsSideEffecting ? OdcmAllowedVerbs.Post : OdcmAllowedVerbs.Any,
                    IsBoundToCollection = isBoundToCollection,
                    IsComposable = operation.IsComposable,
                    Class = odcmClass
                };

                odcmClass.Methods.Add(odcmMethod);

                if (operation.ReturnType != null)
                {
                    odcmMethod.ReturnType = ResolveType(operation.ReturnType);
                    odcmMethod.IsCollection = operation.ReturnType.IsCollection();
                }

                var callingConvention =
                    operation.IsSideEffecting
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
                    Type = ResolveType(property.Type),
                    IsCollection = property.Type.IsCollection(),
                    IsNullable = property.Type.IsNullable,
                    ContainsTarget =
                        property is IEdmNavigationProperty && ((IEdmNavigationProperty)property).ContainsTarget,
                    IsLink = property is IEdmNavigationProperty,
                    DefaultValue = 
                        property is IEdmStructuralProperty ? 
                            ((IEdmStructuralProperty)property).DefaultValueString : 
                            null
                };

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
