// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using ODataReader.v4;
using System;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace ODataReader.v4UnitTests
{
    public class Given_enum_types_in_a_valid_edmx_when_passed_to_the_ODataReader
    {
        private ODataReader.v4.OdcmReader _odcmReader;

        public Given_enum_types_in_a_valid_edmx_when_passed_to_the_ODataReader()
        {
            _odcmReader = new OdcmReader();
        }

        [Fact]
        public void It_returns_one_OdcmType_for_each_EnumType()
        {
            var testCase = new EdmxTestCase()
                .AddEnumType(EdmxTestCase.Keys.EnumType);

            var enumTypeTestNode = testCase[EdmxTestCase.Keys.EnumType];
            var enumMemberCount = (from x in enumTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("Member") select x).Count();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmEnum;
            odcmModel.TryResolveType(enumTypeTestNode.Name, enumTypeTestNode.Namespace, out odcmEnum)
                .Should()
                .BeTrue("because an enum type in the schema should result in an OdcmType");
            odcmEnum.Should().BeOfType<OdcmEnum>("because an enum type in the schema should result in an OdcmEnum");
            odcmEnum.As<OdcmEnum>()
                .Members.Count.Should()
                .Be(enumMemberCount, "because each member added to the schema should result in an OdcmMember");
        }

        [Fact]
        public void When_IsFlags_is_set_it_returns_an_OdcmEnum_with_IsFlags_set()
        {
            var testCase = new EdmxTestCase()
                .AddEnumType(EdmxTestCase.Keys.EnumType, (_, enumType) =>
                {
                    var count = 0;
                    foreach (var descendent in enumType.Descendants())
                    {
                        if (descendent.Name.LocalName.Equals("Member"))
                        {
                            descendent.AddAttribute("Value", (int)Math.Pow(count++, 2));
                        }
                    }
                    enumType.AddAttribute("IsFlags", true);
                });

            var enumTypeTestNode = testCase[EdmxTestCase.Keys.EnumType];
            var enumMemberCount = (from x in enumTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("Member") select x).Count();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmEnum;
            odcmModel.TryResolveType(enumTypeTestNode.Name, enumTypeTestNode.Namespace, out odcmEnum)
                .Should()
                .BeTrue("because an enum type in the schema should result in an OdcmType");
            odcmEnum.Should().BeOfType<OdcmEnum>("because an enum type in the schema should result in an OdcmEnum");
            odcmEnum.As<OdcmEnum>()
                .Members.Count.Should()
                .Be(enumMemberCount, "because each member added to the schema should result in an OdcmMember");
            odcmEnum.As<OdcmEnum>()
                .IsFlags.Should()
                .BeTrue(
                    "because an enum type in the schema with the IsFlags facet set to true should result in an OdcmEnum with the IsFlags property set to true");
        }

        [Fact]
        public void When_No_Underlying_Type_is_set_it_returns_an_OdcmEnum_with_a_Default_UnderlyingType()
        {
            var testCase = new EdmxTestCase()
                .AddEnumType(EdmxTestCase.Keys.EnumType);

            var enumTypeTestNode = testCase[EdmxTestCase.Keys.EnumType];
            var enumMemberCount = (from x in enumTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("Member") select x).Count();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmEnum;
            odcmModel.TryResolveType(enumTypeTestNode.Name, enumTypeTestNode.Namespace, out odcmEnum)
                .Should()
                .BeTrue("because an enum type in the schema should result in an OdcmType");
            odcmEnum.Should().BeOfType<OdcmEnum>("because an enum type in the schema should result in an OdcmEnum");
            odcmEnum.As<OdcmEnum>().Members.Count
                .Should()
                .Be(enumMemberCount, "because each member added to the schema should result in an OdcmMember");
            odcmEnum.As<OdcmEnum>().UnderlyingType.FullName
                .Should()
                .Be(Any.Csdl.DefaultEnumUnderlyingType(),
                    "because an enum type without an explicit underlying type in the schema should have the default underlying type in an OdcmEnum");
        }

        [Fact]
        public void When_An_Underlying_Type_is_set_it_returns_an_OdcmEnum_with_the_specified_UnderlyingType()
        {
            var underlyingType = Any.Csdl.RandomEnumUnderlyingType();

            var testCase = new EdmxTestCase()
                .AddEnumType(EdmxTestCase.Keys.EnumType, (_, enumType) => enumType.AddAttribute("UnderlyingType", underlyingType));

            var enumTypeTestNode = testCase[EdmxTestCase.Keys.EnumType];
            var enumMemberCount = (from x in enumTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("Member") select x).Count();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmEnum;
            odcmModel.TryResolveType(enumTypeTestNode.Name, enumTypeTestNode.Namespace, out odcmEnum)
                .Should()
                .BeTrue("because an enum type in the schema should result in an OdcmType");
            odcmEnum.Should().BeOfType<OdcmEnum>("because an enum type in the schema should result in an OdcmEnum");
            odcmEnum.As<OdcmEnum>().Members.Count
                .Should()
                .Be(enumMemberCount, "because each member added to the schema should result in an OdcmMember");
            odcmEnum.As<OdcmEnum>().UnderlyingType.FullName
                .Should()
                .Be(underlyingType,
                    "because an enum type with an explicit underlying type in the schema should have the specified underlying type in an OdcmEnum");
        }
    }
}
