// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Vipr.Reader.OData.v4;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace ODataReader.v4UnitTests
{
    public class Given_typedefintion_types_in_a_valid_edmx_when_passed_to_the_ODataReader
    {
        private Vipr.Reader.OData.v4.OdcmReader _odcmReader;

        public Given_typedefintion_types_in_a_valid_edmx_when_passed_to_the_ODataReader()
        {
            _odcmReader = new OdcmReader();
        }

        [Fact]
        public void It_returns_one_OdcmType_for_each_TypeDefinitionType()
        {
            var underlyingType = Any.Csdl.RandomPrimitiveType();

            var testCase = new EdmxTestCase()
                .AddTypeDefinitionType(EdmxTestCase.Keys.TypeDefinitionType, (_, typeDefinitionType) => typeDefinitionType.AddAttribute("UnderlyingType", underlyingType));

            var typeDefinitionTypeTestNode = testCase[EdmxTestCase.Keys.TypeDefinitionType];
            
            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmTypeDefinition;
            odcmModel.TryResolveType(typeDefinitionTypeTestNode.Name, typeDefinitionTypeTestNode.Namespace, out odcmTypeDefinition)
                .Should()
                .BeTrue("because a typedefinition type in the schema should result in an OdcmType");

            odcmTypeDefinition.Should().BeOfType<OdcmTypeDefinition>("because a typedefinition type in the schema should result in an OdcmTypeDefinition");
            
            odcmTypeDefinition.As<OdcmTypeDefinition>().BaseType.FullName
                .Should()
                .Be(underlyingType,
                    "because a typedefintion type with a underlying type in the schema should have the specified underlying type in an OdcmTypeDefinition");
        }

        [Fact]
        public void When_Property_is_of_TypeDefinitionType_then_it_should_resolve_as_UnderlyingPrimitiveType()
        {
            var underlyingType = Any.Csdl.RandomPrimitiveType();

            var testCase = new EdmxTestCase()
                .AddTypeDefinitionType(EdmxTestCase.Keys.TypeDefinitionType, (_, typeDefinitionType) => typeDefinitionType.AddAttribute("UnderlyingType", underlyingType));

            var typeDefinitionTypeTestNode = testCase[EdmxTestCase.Keys.TypeDefinitionType];

            testCase.AddComplexType(EdmxTestCase.Keys.ComplexType, (_, complexType) => complexType.Add(Any.Csdl.Property(property => property.AddAttribute("Type", typeDefinitionTypeTestNode.FullName()))));

            var complexTypeTestNode = testCase[EdmxTestCase.Keys.ComplexType];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());
            
            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeTestNode.Name, complexTypeTestNode.Namespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");

            odcmComplexType
                .Should()
                .BeOfType<OdcmComplexClass>("because complex types should result in an OdcmClass");

            odcmComplexType.As<OdcmComplexClass>().Properties
                .Count(property => property.Type is OdcmTypeDefinition)
                .Should()
                .Be(0, "because OdcmTypeDefinition type should be resolved as underlying OdcmPrimitiveType");
        }

        [Fact]
        public void When_TypeDefinitionType_is_declared_with_UnderlyingComplexType_then_it_should_raise_exception()
        {
            var testCase = new EdmxTestCase()
                    .AddComplexType(EdmxTestCase.Keys.ComplexType);

            var complexTypeTestNode = testCase[EdmxTestCase.Keys.ComplexType];
            var underlyingType = complexTypeTestNode.FullName();
            
            testCase.AddTypeDefinitionType(EdmxTestCase.Keys.TypeDefinitionType, (_, typeDefinitionType) => typeDefinitionType.AddAttribute("UnderlyingType", underlyingType));
            
            Exception ex=Assert.Throws<InvalidOperationException>(() => _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata()));
        }
    }
}
