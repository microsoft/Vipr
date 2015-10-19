// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using Vipr.Reader.OData.v4;
using System;
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
    }
}
