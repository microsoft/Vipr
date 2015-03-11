// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using ODataReader.v4;
using System.Collections.Generic;
using System.Xml.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;
using Xunit.Sdk;

namespace ODataReader.v4UnitTests
{
    public class Given_a_valid_edmx_with_Description_Annotations
    {
        private ODataReader.v4.OdcmReader _odcmReader;

        public Given_a_valid_edmx_with_Description_Annotations()
        {
            _odcmReader = new OdcmReader();
        }

        [Fact]
        public void When_EnumType_is_annotated_then_its_OdcmObject_has_Description_set()
        {
            var schemaNamespace = string.Empty;
            var enumName = string.Empty;            
            var description = Any.Paragraph(Any.Int(10, 20));
            var longDescription = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EnumType(enumType =>
                {                    
                    enumName = enumType.Attribute("Name").Value;
                    enumType.Add(Any.Csdl.DescriptionAnnotation(description));
                    enumType.Add(Any.Csdl.LongDescriptionAnnotation(longDescription));
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });            

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmEnum odcmEnum;
            odcmModel.TryResolveType(enumName, schemaNamespace, out odcmEnum)
                .Should()
                .BeTrue("because an enum type in the schema should result in an OdcmType");
            odcmEnum.Description.
                Should().
                BeEquivalentTo(description, "because EnumType description annotation should be captured in OdcmObject");
            odcmEnum.LongDescription.
                Should().
                BeEquivalentTo(longDescription, "because EnumType long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_ComplexType_is_annotated_then_its_OdcmObject_has_Description_set()
        {
            var complexTypeName = string.Empty;
            var schemaNamespace = string.Empty;            
            var description = Any.Paragraph(Any.Int(10, 20));
            var longDescription = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.ComplexType(complexType =>
                {                    
                    complexTypeName = complexType.Attribute("Name").Value;
                    complexType.Add(Any.Csdl.DescriptionAnnotation(description));
                    complexType.Add(Any.Csdl.LongDescriptionAnnotation(longDescription));
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeName, schemaNamespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType.Description.
                Should().
                BeEquivalentTo(description, "because ComplexType description annotation should be captured in OdcmObject");
            odcmComplexType.LongDescription.
                Should().
                BeEquivalentTo(longDescription, "because ComplexType long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_EntityType_is_annotated_then_its_OdcmObject_has_Description_set()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;            
            var description = Any.Paragraph(Any.Int(10, 20));
            var longDescription = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityTypeName = entityType.Attribute("Name").Value;                                      
                    entityType.Add(Any.Csdl.DescriptionAnnotation(description));
                    entityType.Add(Any.Csdl.LongDescriptionAnnotation(longDescription));
                }));
                schema.Add(Any.Csdl.EntityContainer());
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType.Description.
                Should().
                BeEquivalentTo(description, "because EntityType description annotation should be captured in OdcmObject");
            odcmEntityType.LongDescription.
                Should().
                BeEquivalentTo(longDescription, "because EntityType long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_EntityContainer_is_annotated_then_its_OdcmObject_has_Description_set()
        {
            var entityContainerName = string.Empty;
            var schemaNamespace = string.Empty;
            var description = Any.Paragraph(Any.Int(10, 20));
            var longDescription = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;                
                schema.Add(Any.Csdl.EntityContainer(entityContainer =>
                {
                    entityContainerName = entityContainer.Attribute("Name").Value;
                    entityContainer.Add(Any.Csdl.DescriptionAnnotation(description));
                    entityContainer.Add(Any.Csdl.LongDescriptionAnnotation(longDescription));
                }));
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmEntityContainer;
            odcmModel.TryResolveType(entityContainerName, schemaNamespace, out odcmEntityContainer)
                .Should()
                .BeTrue("because an entity container in the schema should result in an OdcmType");
            odcmEntityContainer.Description.
                Should().
                BeEquivalentTo(description, "because EntityContainer description annotation should be captured in OdcmObject");
            odcmEntityContainer.LongDescription.
                Should().
                BeEquivalentTo(longDescription, "because EntityContainer long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_Structural_Property_is_annotated_then_its_OdcmObject_has_Description_set()
        {
            var complexTypeName = string.Empty;            
            var schemaNamespace = string.Empty;
            var description = Any.Paragraph(Any.Int(10, 20));
            var longDescription = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.ComplexType(complexType =>
                {
                    var structuralProperty = Any.Csdl.Property(property => 
                        {
                            property.AddAttribute("Type", Any.Csdl.RandomPrimitiveType());
                            property.Add(Any.Csdl.DescriptionAnnotation(description));
                            property.Add(Any.Csdl.LongDescriptionAnnotation(longDescription));
                        });                    
                    complexType.Add(structuralProperty);
                    complexTypeName = complexType.Attribute("Name").Value;
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeName, schemaNamespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .As<OdcmClass>()
                .Properties[0]
                .Description
                .Should()
                .BeEquivalentTo(description, "because Structural Property description annotation should be captured in OdcmObject");
            odcmComplexType
                .As<OdcmClass>()
                .Properties[0]
                .LongDescription
                .Should()
                .BeEquivalentTo(longDescription, "because Structural Property long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_Navigation_Property_is_annotated_then_its_OdcmObject_has_Description_set()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var description = Any.Paragraph(Any.Int(10, 20));
            var longDescription = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    schemaNamespace = schema.Attribute("Namespace").Value;
                    entityTypeName = entityType.Attribute("Name").Value;
                    var navigationProperty = Any.Csdl.NavigationProperty(property =>
                    {
                        property.AddAttribute("Type", string.Format("Collection({0}.{1})", schemaNamespace, entityTypeName));
                        property.Add(Any.Csdl.DescriptionAnnotation(description));
                        property.Add(Any.Csdl.LongDescriptionAnnotation(longDescription));
                    });
                    entityType.Add(navigationProperty);                    
                }));
                schema.Add(Any.Csdl.EntityContainer());                
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmentityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmentityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmentityType
                .As<OdcmClass>()
                .Properties[0]
                .Description
                .Should()
                .BeEquivalentTo(description, "because Navigation Property description annotation should be captured in OdcmObject");
            odcmentityType
                .As<OdcmClass>()
                .Properties[0]
                .LongDescription
                .Should()
                .BeEquivalentTo(longDescription, "because Navigation Property long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_Action_and_Parameters_are_annotated_then_their_OdcmObject_has_Description_set()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var description1 = Any.Paragraph(Any.Int(10, 20));
            var description2 = Any.Paragraph(Any.Int(10, 20));
            var longDescription1 = Any.Paragraph(Any.Int(15, 25));
            var longDescription2 = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityTypeName = entityType.Attribute("Name").Value;
                }));

                schema.Add(Any.Csdl.Action(action =>
                {
                    action.AddAttribute("IsBound", "true");
                    action.Add(Any.Csdl.DescriptionAnnotation(description1));
                    action.Add(Any.Csdl.LongDescriptionAnnotation(longDescription1));
                    action.Add(Any.Csdl.Parameter(param =>
                    {
                        param.SetAttributeValue("Name", "bindingParameter");
                        param.AddAttribute("Type", string.Format("{0}.{1}", schemaNamespace, entityTypeName));
                    }));
                    action.Add(Any.Csdl.Parameter(param =>
                    {
                        param.AddAttribute("Type", Any.Csdl.RandomPrimitiveType());
                        param.Add(Any.Csdl.DescriptionAnnotation(description2));
                        param.Add(Any.Csdl.LongDescriptionAnnotation(longDescription2));
                    }));
                }));
                schema.Add(Any.Csdl.EntityContainer());
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmentityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmentityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            var odcmMethod = odcmentityType.As<OdcmClass>().Methods[0];
            odcmMethod.Description
                .Should()
                .BeEquivalentTo(description1, "because Action description annotation should be captured in OdcmObject");
            odcmMethod.LongDescription
                .Should()
                .BeEquivalentTo(longDescription1, "because Action long description annotation should be captured in OdcmObject");
            odcmMethod.Parameters[0]
                .Description
                .Should()
                .BeEquivalentTo(description2, "because Parameter description annotation should be captured in OdcmObject");
            odcmMethod.Parameters[0]
                .LongDescription
                .Should()
                .BeEquivalentTo(longDescription2, "because Parameter long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_Function_and_Parameters_are_annotated_then_their_OdcmObject_has_Description_set()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var description1 = Any.Paragraph(Any.Int(10, 20));
            var description2 = Any.Paragraph(Any.Int(10, 20));
            var longDescription1 = Any.Paragraph(Any.Int(15, 25));
            var longDescription2 = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.EntityType(entityType =>
                { 
                    entityTypeName = entityType.Attribute("Name").Value;
                }));

                schema.Add(Any.Csdl.Function(function =>
                {
                    function.AddAttribute("IsBound", "true");
                    function.Add(Any.Csdl.DescriptionAnnotation(description1));
                    function.Add(Any.Csdl.LongDescriptionAnnotation(longDescription1));
                    function.Add(Any.Csdl.Parameter(param =>
                    {
                        param.SetAttributeValue("Name", "bindingParameter");
                        param.AddAttribute("Type", string.Format("{0}.{1}", schemaNamespace, entityTypeName));
                    }));
                    function.Add(Any.Csdl.Parameter(param =>
                    {
                        param.AddAttribute("Type", Any.Csdl.RandomPrimitiveType());
                        param.Add(Any.Csdl.DescriptionAnnotation(description2));
                        param.Add(Any.Csdl.LongDescriptionAnnotation(longDescription2));
                    }));
                }));
                schema.Add(Any.Csdl.EntityContainer());
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmentityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmentityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            var odcmMethod = odcmentityType.As<OdcmClass>().Methods[0];
            odcmMethod.Description
                .Should()
                .BeEquivalentTo(description1, "because Function description annotation should be captured in OdcmObject");
            odcmMethod.LongDescription
                .Should()
                .BeEquivalentTo(longDescription1, "because Function long description annotation should be captured in OdcmObject");
            odcmMethod.Parameters[0]
                .Description
                .Should()
                .BeEquivalentTo(description2, "because Parameter description annotation should be captured in OdcmObject");
            odcmMethod.Parameters[0]
                .LongDescription
                .Should()
                .BeEquivalentTo(longDescription2, "because Parameter long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_EntitySet_is_annotated_then_its_OdcmObject_has_Description_set()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var entityContainerName = string.Empty;
            var description = Any.Paragraph(Any.Int(10, 20));
            var longDescription = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityTypeName = entityType.Attribute("Name").Value;
                }));

                schema.Add(Any.Csdl.EntityContainer(entityContainer =>
                {
                    entityContainer.Add(Any.Csdl.EntitySet(entitySet => 
                    {
                        entitySet.AddAttribute("EntityType", string.Format("{0}.{1}", schemaNamespace, entityTypeName));
                        entitySet.Add(Any.Csdl.DescriptionAnnotation(description));
                        entitySet.Add(Any.Csdl.LongDescriptionAnnotation(longDescription));
                    }));
                    entityContainerName = entityContainer.Attribute("Name").Value;
                }));
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmType;
            odcmModel.TryResolveType(entityContainerName, schemaNamespace, out odcmType)
                .Should()
                .BeTrue("because an entity container in the schema should result in an OdcmType");
            var odcmProperty = odcmType.As<OdcmClass>().Properties[0];
            odcmProperty.Description
                .Should()
                .BeEquivalentTo(description, "because EntitySet description annotation should be captured in OdcmObject");
            odcmProperty.LongDescription
                .Should()
                .BeEquivalentTo(longDescription, "because EntitySet long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_Singleton_is_annotated_then_its_OdcmObject_has_Description_set()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var entityContainerName = string.Empty;
            var description = Any.Paragraph(Any.Int(10, 20));
            var longDescription = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityTypeName = entityType.Attribute("Name").Value;
                }));

                schema.Add(Any.Csdl.EntityContainer(entityContainer =>
                {
                    entityContainer.Add(Any.Csdl.Singleton(singleton =>
                    {
                        singleton.AddAttribute("Type", string.Format("{0}.{1}", schemaNamespace, entityTypeName));
                        singleton.Add(Any.Csdl.DescriptionAnnotation(description));
                        singleton.Add(Any.Csdl.LongDescriptionAnnotation(longDescription));
                    }));
                    entityContainerName = entityContainer.Attribute("Name").Value;
                }));
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmType;
            odcmModel.TryResolveType(entityContainerName, schemaNamespace, out odcmType)
                .Should()
                .BeTrue("because an entity container in the schema should result in an OdcmType");
            var odcmProperty = odcmType.As<OdcmClass>().Properties[0];
            odcmProperty.Description
                .Should()
                .BeEquivalentTo(description, "because Singleton description annotation should be captured in OdcmObject");
            odcmProperty.LongDescription
                .Should()
                .BeEquivalentTo(longDescription, "because Singleton long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_ActionImport_is_annotated_then_its_OdcmObject_has_Description_set()
        {
            var entityContainerName = string.Empty;
            var schemaNamespace = string.Empty;
            var actionName = string.Empty;
            var description = Any.Paragraph(Any.Int(10, 20));
            var longDescription = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.Action(action =>
                {
                    action.AddAttribute("IsBound", "false");
                    actionName = action.Attribute("Name").Value;
                }));

                schema.Add(Any.Csdl.EntityContainer(entityContainer =>
                {
                    entityContainer.Add(Any.Csdl.ActionImport(action =>
                    {
                        action.AddAttribute("Action", string.Format("{0}.{1}", schemaNamespace, actionName));
                        action.Add(Any.Csdl.DescriptionAnnotation(description));
                        action.Add(Any.Csdl.LongDescriptionAnnotation(longDescription));
                    }));
                    entityContainerName = entityContainer.Attribute("Name").Value;
                }));
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmType;
            odcmModel.TryResolveType(entityContainerName, schemaNamespace, out odcmType)
                .Should()
                .BeTrue("because an entity container in the schema should result in an OdcmType");
            var odcmMethod = odcmType.As<OdcmClass>().Methods[0];
            odcmMethod.Description
                .Should()
                .BeEquivalentTo(description, "because Action Import description annotation should be captured in OdcmObject");
            odcmMethod.LongDescription
                .Should()
                .BeEquivalentTo(longDescription, "because Action Import long description annotation should be captured in OdcmObject");
        }

        [Fact]
        public void When_FunctionImport_is_annotated_then_its_OdcmObject_has_Description_set()
        {            
            var entityContainerName = string.Empty;
            var schemaNamespace = string.Empty;
            var functionName = string.Empty;
            var description = Any.Paragraph(Any.Int(10, 20));
            var longDescription = Any.Paragraph(Any.Int(15, 25));

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.Function(function =>
                {
                    function.AddAttribute("IsBound", "false");                    
                    functionName = function.Attribute("Name").Value;
                }));

                schema.Add(Any.Csdl.EntityContainer(entityContainer => 
                {
                    entityContainer.Add(Any.Csdl.FunctionImport(function =>
                    {
                        function.AddAttribute("Function", string.Format("{0}.{1}", schemaNamespace, functionName));
                        function.Add(Any.Csdl.DescriptionAnnotation(description));
                        function.Add(Any.Csdl.LongDescriptionAnnotation(longDescription));
                    }));
                    entityContainerName = entityContainer.Attribute("Name").Value;
                }));
            });

            var odcmModel = GetOdcmModel(edmxElement);

            OdcmType odcmType;
            odcmModel.TryResolveType(entityContainerName, schemaNamespace, out odcmType)
                .Should()
                .BeTrue("because an entity container in the schema should result in an OdcmType");
            var odcmMethod = odcmType.As<OdcmClass>().Methods[0];
            odcmMethod.Description
                .Should()
                .BeEquivalentTo(description, "because Function Import description annotation should be captured in OdcmObject");
            odcmMethod.LongDescription
                .Should()
                .BeEquivalentTo(longDescription, "because Function Import long description annotation should be captured in OdcmObject");
        }

        private OdcmModel GetOdcmModel(XElement edmxElement)
        {
            var serviceMetadata = new TextFileCollection
            {
                new TextFile("$metadata", edmxElement.ToString())
            };

            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);
            return odcmModel;
        }
    }
}
