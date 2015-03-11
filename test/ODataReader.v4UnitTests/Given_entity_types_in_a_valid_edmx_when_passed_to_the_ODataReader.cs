// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using ODataReader.v4;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace ODataReader.v4UnitTests
{
    public class Given_entity_types_in_a_valid_edmx_when_passed_to_the_ODataReader
    {
        private ODataReader.v4.OdcmReader _odcmReader;

        public Given_entity_types_in_a_valid_edmx_when_passed_to_the_ODataReader()
        {
            _odcmReader = new OdcmReader();
        }

        [Fact]
        public void It_returns_one_OdcmEntityClass_for_each_EntityType()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType);

            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];
            var keyCount = (from x in entityTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("PropertyRef") select x).Count();
            var propertyCount = (from x in entityTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("Property") select x).Count();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());
            
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
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

            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());
            
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().IsAbstract
                .Should()
                .BeTrue("because an entity type with the Abstract facet set should be abstract in the OdcmModel");
        }

        [Fact]
        public void When_OpenType_is_set_it_returns_an_OdcmEntityClass_with_IsOpen_set()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("OpenType", true));

            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());
            
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().IsOpen
                .Should()
                .BeTrue("because an entity type with the OpenType facet set should be open in the OdcmModel");
        }

        [Fact]
        public void When_HasStream_is_set_it_returns_an_OdcmMediaClass()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("HasStream", true));

            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());
            
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
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

            var baseEntityTypeTestNode = testCase[EdmxTestCase.Keys.EntityTypeBase];
            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());
            
            OdcmType odcmBaseType;
            odcmModel.TryResolveType(baseEntityTypeTestNode.Name, baseEntityTypeTestNode.Namespace, out odcmBaseType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmBaseType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Base
                .Should()
                .Be(odcmBaseType,
                    "because an entity type with a base type set should have a corresponding OdcmClass and base OdcmClass");
            odcmBaseType.As<OdcmEntityClass>().Derived
                .Should()
                .Contain(odcmEntityType.As<OdcmEntityClass>(),
                    "because an entity type with a base type set should have a correspond OdcmClass that derives from a base OdcmClass");
        }

        [Fact]
        public void When_BaseType_is_set_and_only_derived_EntityType_has_Key_it_returns_an_OdcmEntityClass_using_Key_of_derived_EntityType()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityTypeBase, true, (_, entityType) => entityType.AddAttribute("Abstract", true))
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("BaseType", _[EdmxTestCase.Keys.EntityTypeBase].FullName()));

            var baseEntityTypeTestNode = testCase[EdmxTestCase.Keys.EntityTypeBase];
            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];
            var keyCount = (from x in entityTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("PropertyRef") select x).Count();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());
            
            OdcmType odcmBaseType;
            odcmModel.TryResolveType(baseEntityTypeTestNode.Name, baseEntityTypeTestNode.Namespace, out odcmBaseType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmBaseType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Base
                .Should()
                .Be(odcmBaseType,
                    "because an entity type with a base type set should have a corresponding OdcmClass and base OdcmClass");
            odcmBaseType.As<OdcmEntityClass>().Derived
                .Should()
                .Contain(odcmEntityType.As<OdcmEntityClass>(),
                    "because an entity type with a base type set should have a correspond OdcmClass that derives from a base OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Key
                .Count().Should().Be(keyCount, "because the Key of an OdcmEntityClass should have the same number of OdcmProperties as there are properties in the entity type key");
            (from key in odcmEntityType.As<OdcmEntityClass>().Key
             where
                 (from x in entityTypeTestNode.Element.Descendants()
                  where x.Name.LocalName.Equals("PropertyRef") && x.GetAttribute("Name").Equals(key.CanonicalName())
                  select x).Any()
             select key).Count().Should().Be(keyCount, "because the Key of an OdcmEntityClass should have the corresponding OdcmProperties as in the entity type key");
        }

        [Fact]
        public void When_BaseType_is_set_and_only_base_EntityType_has_Key_it_returns_an_OdcmEntityClass_using_Key_of_base_EntityType()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityTypeBase, (_, entityType) => entityType.AddAttribute("Abstract", true))
                .AddEntityType(EdmxTestCase.Keys.EntityType, true, (_, entityType) => entityType.AddAttribute("BaseType", _[EdmxTestCase.Keys.EntityTypeBase].FullName()));

            var baseEntityTypeTestNode = testCase[EdmxTestCase.Keys.EntityTypeBase];
            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];
            var keyCount = (from x in baseEntityTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("PropertyRef") select x).Count();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());
            
            OdcmType odcmBaseType;
            odcmModel.TryResolveType(baseEntityTypeTestNode.Name, baseEntityTypeTestNode.Namespace, out odcmBaseType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmBaseType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Base
                .Should()
                .Be(odcmBaseType,
                    "because an entity type with a base type set should have a corresponding OdcmClass and base OdcmClass");
            odcmBaseType.As<OdcmEntityClass>().Derived
                .Should()
                .Contain(odcmEntityType.As<OdcmEntityClass>(),
                    "because an entity type with a base type set should have a correspond OdcmClass that derives from a base OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Key
                .Count().Should().Be(keyCount, "because the Key of an OdcmEntityClass should have the same number of OdcmProperties as there are properties in the entity type key");
            (from key in odcmEntityType.As<OdcmEntityClass>().Key
             where
                 (from x in baseEntityTypeTestNode.Element.Descendants()
                  where x.Name.LocalName.Equals("PropertyRef") && x.GetAttribute("Name").Equals(key.CanonicalName())
                  select x).Any()
             select key).Count().Should().Be(keyCount, "because the Key of an OdcmEntityClass should have the corresponding OdcmProperties as in the entity type key");
        }

        [Fact]
        public void When_BaseType_is_set_and_both_have_Keys_it_returns_an_OdcmEntityClass_using_Key_of_derived_EntityType()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityTypeBase, (_, entityType) => entityType.AddAttribute("Abstract", true))
                .AddEntityType(EdmxTestCase.Keys.EntityType, (_, entityType) => entityType.AddAttribute("BaseType", _[EdmxTestCase.Keys.EntityTypeBase].FullName()));

            var baseEntityTypeTestNode = testCase[EdmxTestCase.Keys.EntityTypeBase];
            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];
            var keyCount = (from x in entityTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("PropertyRef") select x).Count();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());
            
            OdcmType odcmBaseType;
            odcmModel.TryResolveType(baseEntityTypeTestNode.Name, baseEntityTypeTestNode.Namespace, out odcmBaseType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmBaseType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Base
                .Should()
                .Be(odcmBaseType,
                    "because an entity type with a base type set should have a corresponding OdcmClass and base OdcmClass");
            odcmBaseType.As<OdcmEntityClass>().Derived
                .Should()
                .Contain(odcmEntityType.As<OdcmEntityClass>(),
                    "because an entity type with a base type set should have a correspond OdcmClass that derives from a base OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Key
                .Count().Should().Be(keyCount, "because the Key of an OdcmEntityClass should have the same number of OdcmProperties as there are properties in the entity type key");
            (from key in odcmEntityType.As<OdcmEntityClass>().Key
             where
                 (from x in entityTypeTestNode.Element.Descendants()
                  where x.Name.LocalName.Equals("PropertyRef") && x.GetAttribute("Name").Equals(key.CanonicalName())
                  select x).Any()
             select key).Count().Should().Be(keyCount, "because the Key of an OdcmEntityClass should have the corresponding OdcmProperties as in the entity type key");
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
            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];
            var keyCount = (from x in entityTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("PropertyRef") select x).Count();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeTestNode.Name, complexTypeTestNode.Namespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmComplexClass>("because complex types should result in an OdcmComplexClass");
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmEntityClass");
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

            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];
            var actionTestNode = testCase[EdmxTestCase.Keys.Action];
            var parameterCount = (from x in actionTestNode.Element.Descendants() where x.Name.LocalName.Equals("Parameter") select x).Count() - 1;

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());
            
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Methods
                .Should()
                .Contain(odcmMethod => odcmMethod.Name == actionTestNode.Name, "because an action bound to an entity type should result in a method in the OdcmClass");
            odcmEntityType.As<OdcmEntityClass>()
                .Methods.Find(odcmMethod => odcmMethod.Name == actionTestNode.Name)
                .Parameters.Count
                .Should()
                .Be(parameterCount,
                    "because for each parameter in the action there should be a OdcmParameter in the OdcmMethod");
        }

        [Fact]
        public void When_a_function_is_bound_to_an_entity_type_there_is_a_method_on_the_OdcmClass()
        {
            var testCase = new EdmxTestCase()
                .AddEntityType(EdmxTestCase.Keys.EntityType)
                .AddBoundFunction(EdmxTestCase.Keys.Function, EdmxTestCase.Keys.EntityType);

            var entityTypeTestNode = testCase[EdmxTestCase.Keys.EntityType];
            var functionTestNode = testCase[EdmxTestCase.Keys.Function];
            var parameterCount = (from x in functionTestNode.Element.Descendants() where x.Name.LocalName.Equals("Parameter") select x).Count() - 1;
            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());


            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeTestNode.Name, entityTypeTestNode.Namespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Methods
                .Should()
                .Contain(odcmMethod => odcmMethod.Name == functionTestNode.Name, "because a function bound to an entity type should result in a method in the OdcmClass");
            odcmEntityType.As<OdcmEntityClass>()
                .Methods.Find(odcmMethod => odcmMethod.Name == functionTestNode.Name)
                .Parameters.Count
                .Should()
                .Be(parameterCount,
                    "because for each parameter in the action there should be a OdcmParameter in the OdcmMethod");
        }
    }
}
