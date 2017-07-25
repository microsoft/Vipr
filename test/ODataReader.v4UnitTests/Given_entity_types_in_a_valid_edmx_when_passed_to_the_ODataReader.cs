// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using Vipr.Reader.OData.v4;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;
using System.Xml.Linq;

namespace ODataReader.v4UnitTests
{
    public class Given_entity_types_in_a_valid_edmx_when_passed_to_the_ODataReader
    {
        private Vipr.Reader.OData.v4.OdcmReader _odcmReader;

        public Given_entity_types_in_a_valid_edmx_when_passed_to_the_ODataReader()
        {
            _odcmReader = new OdcmReader();
        }

        [Fact]
        public void It_returns_one_OdcmEntityClass_for_each_EntityType()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType);

            var entityTypeDescendants= testCase[EdmxTestCase.Keys.EntityType].Element.Descendants();
            var keyCount = entityTypeDescendants.Count(x => x.Name.LocalName == "PropertyRef");
            var propertyCount = entityTypeDescendants.Count(x => x.Name.LocalName == "Property");

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            odcmEntityType.As<OdcmEntityClass>().Key.Count
                .Should()
                .Be(keyCount, "because each property reference added to the key of an entity type should result in an OdcmClass property");
            odcmEntityType.As<OdcmEntityClass>().Properties.Count
                .Should()
                .Be(propertyCount, "because each property added to an entity type should result in an OdcmClass property");
            odcmEntityType.As<OdcmEntityClass>().IsAbstract
                .Should()
                .BeFalse("because an entity type with no Abstract facet should be false in the OdcmModel");
            odcmEntityType.As<OdcmEntityClass>().IsOpen
                .Should()
                .BeFalse("because an entity type with no OpenType facet should be false in the OdcmModel");
        }

        [Fact]
        public void When_Abstract_is_set_it_returns_an_OdcmEntityClass_with_IsAbstract_set()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("Abstract", true));

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            odcmEntityType.As<OdcmEntityClass>().IsAbstract
                .Should()
                .BeTrue("because an entity type with the Abstract facet set should be abstract in the OdcmModel");
        }

        [Fact]
        public void When_OpenType_is_set_it_returns_an_OdcmEntityClass_with_IsOpen_set()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("OpenType", true));

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            odcmEntityType.As<OdcmEntityClass>().IsOpen
                .Should()
                .BeTrue("because an entity type with the OpenType facet set should be open in the OdcmModel");
        }

        [Fact]
        public void When_HasStream_is_set_it_returns_an_OdcmMediaClass()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("HasStream", true));

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            odcmEntityType
                .Should()
                .BeOfType<OdcmMediaClass>("because an entity type with the HasStream facet set should be an OdcmMediaClass");
        }

        [Fact]
        public void When_BaseType_is_set_it_returns_an_OdcmEntityClass_with_a_BaseType()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityTypeBase, (_, entityType) => entityType.AddAttribute("Abstract", true))
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("BaseType", _[EdmxTestCase.Keys.EntityTypeBase].FullName()));

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            var odcmBaseType =  VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityTypeBase]);
            var odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            VerifyInheritanceLink(odcmBaseType, odcmEntityType);
        }

        [Fact]
        public void When_BaseType_precedes_derived_type_it_returns_an_OdcmEntityClass_with_a_BaseType()
        {
            var testCase = new EdmxTestCase();

            var baseTypeElement = testCase.CreateEntityType(EdmxTestCase.Keys.EntityTypeBase, false, (_, entityType) => entityType.AddAttribute("Abstract", true));

            testCase
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("BaseType", _[EdmxTestCase.Keys.EntityTypeBase].FullName()))
                .AddEntityType(baseTypeElement);

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            var odcmBaseType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityTypeBase]);
            var odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            VerifyInheritanceLink(odcmBaseType, odcmEntityType);
        }

        [Fact]
        public void When_BaseType_is_set_and_only_derived_EntityType_has_Key_it_returns_an_OdcmEntityClass_using_Key_of_derived_EntityType()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityTypeBase, true, (_, entityType) => entityType.AddAttribute("Abstract", true))
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("BaseType", _[EdmxTestCase.Keys.EntityTypeBase].FullName()));

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            var odcmBaseType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityTypeBase]);
            var odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            VerifyInheritanceLink(odcmBaseType, odcmEntityType);
            VerifyKey(testCase[EdmxTestCase.Keys.EntityType], odcmEntityType);
        }

        [Fact]
        public void When_BaseType_is_set_and_only_base_EntityType_has_Key_it_returns_an_OdcmEntityClass_using_Key_of_base_EntityType()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityTypeBase, (_, entityType) => entityType.AddAttribute("Abstract", true))
                .AddEntityType(EdmxTestCase.Keys.EntityType, true, (_, entityType) => entityType.AddAttribute("BaseType", _[EdmxTestCase.Keys.EntityTypeBase].FullName()));

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            var odcmBaseType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityTypeBase]);
            var odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            VerifyInheritanceLink(odcmBaseType, odcmEntityType);
            VerifyKey(testCase[EdmxTestCase.Keys.EntityTypeBase], odcmEntityType);
        }

        [Fact]
        public void When_BaseType_is_set_and_both_have_Keys_it_returns_an_OdcmEntityClass_using_Key_of_derived_EntityType()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityTypeBase, (_, entityType) => entityType.AddAttribute("Abstract", true))
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("BaseType", _[EdmxTestCase.Keys.EntityTypeBase].FullName()));

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            var odcmBaseType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityTypeBase]);
            var odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            VerifyInheritanceLink(odcmBaseType, odcmEntityType);
            VerifyKey(testCase[EdmxTestCase.Keys.EntityType], odcmEntityType);
        }

        [Fact]
        public void When_a_Property_references_a_ComplexType_it_returns_an_OdcmEntityClass_with_a_property_having_the_corresponding_OdcmComplexType()
        {
            string complexTypePropertyName = string.Empty;

            var testCase = new EdmxTestCase()
                .AddComplexType(EdmxTestCase.Keys.ComplexType)
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) =>
                {
                    var property = Any.Csdl.Property(_[EdmxTestCase.Keys.ComplexType].FullName());
                    complexTypePropertyName = property.Attribute("Name").Value;
                    entityType.Add(property);
                });

            var complexTypeTestNode = testCase[EdmxTestCase.Keys.ComplexType];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeTestNode.Name, complexTypeTestNode.Namespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmComplexClass>("because complex types should result in an OdcmComplexClass");

            OdcmType odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            var complexProperty = (from property in odcmEntityType.As<OdcmEntityClass>().Properties
                                   where property.Name.Equals(complexTypePropertyName)
                                   select property).FirstOrDefault();
            complexProperty
                .Should()
                .NotBeNull("because a property referencing a complex type should result in a property in the OdcmEntityClass");
            complexProperty.Type
                .Should()
                .Be(odcmComplexType, "because a property referencing a complex type should result in a property with a type matching the OdcmComplexClass of the property");
        }

        [Fact]
        public void When_an_action_is_bound_to_an_entity_type_there_is_a_method_on_the_OdcmClass()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType)
                .AddBoundAction(EdmxTestCase.Keys.Action, EdmxTestCase.Keys.EntityType);

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            VerifyMethodTestCase(odcmModel, testCase, EdmxTestCase.Keys.Action);
        }

        [Fact]
        public void When_a_function_is_bound_to_an_entity_type_there_is_a_method_on_the_OdcmClass()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType)
                .AddBoundFunction(EdmxTestCase.Keys.Function, EdmxTestCase.Keys.EntityType);

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            VerifyMethodTestCase(odcmModel, testCase, EdmxTestCase.Keys.Function);
        }

        [Fact]
        public void When_a_function_is_bound_to_an_entity_type_in_a_different_namespace_there_is_a_method_on_the_OdcmClass()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType)
                .AddBoundFunction(EdmxTestCase.Keys.Function, EdmxTestCase.Keys.EntityType, Any.Csdl.Schema());
            
            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            VerifyMethodTestCase(odcmModel, testCase, EdmxTestCase.Keys.Function);
        }

        private void VerifyInheritanceLink(OdcmType odcmBaseType, OdcmType odcmEntityType)
        {
            odcmEntityType.As<OdcmEntityClass>().Base
                .Should()
                .Be(odcmBaseType,
                    "because an entity type with a base type set should have a corresponding OdcmClass and base OdcmClass");
            odcmBaseType.As<OdcmEntityClass>().Derived
                .Should()
                .Contain(odcmEntityType.As<OdcmEntityClass>(),
                    "because an entity type with a base type set should have a correspond OdcmClass that derives from a base OdcmClass");
        }

        private void VerifyKey(EdmxTestCase.TestNode entityTypeNode, OdcmType odcmEntityType)
        {
            var entityTypeDescendants = entityTypeNode.Element.Descendants();
            var properties = entityTypeDescendants.Where(x => x.Name.LocalName == "PropertyRef");
            var keyCount = properties.Count();

            odcmEntityType.As<OdcmEntityClass>().Key
                .Count().Should().Be(keyCount, "because the Key of an OdcmEntityClass should have the same number of OdcmProperties as there are properties in the entity type key");

            odcmEntityType.As<OdcmEntityClass>().Key
                .Count(key => properties.Any(x => x.GetName() == key.CanonicalName()))
                .Should().Be(keyCount, "because the Key of an OdcmEntityClass should have the corresponding OdcmProperties as in the entity type key");
        }

        private void VerifyMethodTestCase(OdcmModel odcmModel, EdmxTestCase testCase, string methodKey)
        {
            var functionTestNode = testCase[methodKey];
            var parameterCount = functionTestNode.Element.Descendants().Count(x => x.Name.LocalName == "Parameter") - 1;

            OdcmType odcmEntityType = VerifyEntityType(odcmModel, testCase[EdmxTestCase.Keys.EntityType]);

            odcmEntityType.As<OdcmEntityClass>().Methods
                .Should()
                .Contain(odcmMethod => odcmMethod.Name == functionTestNode.Name, "because a function bound to an entity type should result in a method in the OdcmClass");
            odcmEntityType.As<OdcmEntityClass>()
                .Methods.Single(odcmMethod => odcmMethod.Name == functionTestNode.Name)
                .Parameters.Count
                .Should()
                .Be(parameterCount,
                    "because for each parameter in the action there should be a OdcmParameter in the OdcmMethod");
        }

        private OdcmType VerifyEntityType(OdcmModel odcmModel, EdmxTestCase.TestNode entityTypeNode)
        {
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeNode.Name, entityTypeNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeAssignableTo<OdcmEntityClass>("because entity types should result in an OdcmClass");

            return odcmEntityType;
        }

        [Fact]
        public void When_property_is_invalid_it_skips_addition()
        {
            var entityNamespace = Any.Csdl.EntityContainer().Attribute("Name").Value.ToString();

            var customPropertyName = string.Empty;

            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) =>
                {
                    var property = Any.Csdl.Property(entityNamespace + ".invalid");
                    customPropertyName = property.Attribute("Name").Value;
                    entityType.Add(property);
                });

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            var createdType = odcmModel.EntityContainer.Namespace.Types.Where(x => x.FullName == customPropertyName);

            createdType.Should().BeEmpty("because the invalid property should have been skipped");
        }

        [Fact]
        public void When_primitive_does_not_exist_it_skips_addition()
        {
            var customPropertyName = string.Empty;

            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) =>
                {
                    var property = Any.Csdl.Property("Edm.Boolean!");
                    customPropertyName = property.Attribute("Name").Value;
                    entityType.Add(property);
                });

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            var createdType = odcmModel.EntityContainer.Namespace.Types.Where(x => x.FullName == customPropertyName);

            var createdPrimitive = odcmModel.EntityContainer.Namespace.Classes.Where(x => x.Properties.Where(y => y.Type.FullName == "Edm.Boolean!").Count() > 0);

            createdType.Should().BeEmpty("because the invalid property should have been skipped");
            createdPrimitive.Should().BeEmpty("because the invalid primitive should have been skipped");
        }

        [Fact]
        public void When_entityset_property_is_invalid_it_skips_addition()
        {

        }

        [Fact]
        public void When_singleton_property_is_invalid_it_skips_addition()
        {

        }

        [Fact]
        public void When_property_cannot_be_written_to_cache_it_skips_addition()
        {

        }
    }
}
