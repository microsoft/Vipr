// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using ODataReader.v4;
using System;
using System.Collections.Generic;
using Vipr.Core;
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
            var schemaNamespace = string.Empty;
            var enumName = string.Empty;
            var enumMemberCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EnumType(enumType =>
                {
                    foreach (var member in Any.Sequence((i) => Any.Csdl.Member(), Any.Int(1, 5)))
                    {
                        enumType.Add(member);
                        enumMemberCount++;
                    }
                    enumName = enumType.Attribute("Name").Value;
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new TextFileCollection
            {
                new TextFile("$metadata", edmxElement.ToString())
            };

            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEnum;
            odcmModel.TryResolveType(enumName, schemaNamespace, out odcmEnum)
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
            var schemaNamespace = string.Empty;
            var enumName = string.Empty;
            var enumMemberCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EnumType(enumType =>
                {
                    foreach (var member in Any.Sequence((i) => Any.Csdl.Member(), Any.Int(1, 5)))
                    {
                        member.AddAttribute("Value", (int)Math.Pow(enumMemberCount, 2));
                        enumType.Add(member);
                        enumMemberCount++;
                    }
                    enumName = enumType.Attribute("Name").Value;
                    enumType.AddAttribute("IsFlags", true);
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new TextFileCollection
            {
                new TextFile("$metadata", edmxElement.ToString())
            };

            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEnum;
            odcmModel.TryResolveType(enumName, schemaNamespace, out odcmEnum)
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
            var schemaNamespace = string.Empty;
            var enumName = string.Empty;
            var enumMemberCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EnumType(enumType =>
                {
                    foreach (var member in Any.Sequence((i) => Any.Csdl.Member(), Any.Int(1, 5)))
                    {
                        enumType.Add(member);
                        enumMemberCount++;
                    }
                    enumName = enumType.Attribute("Name").Value;
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new TextFileCollection
            {
                new TextFile("$metadata", edmxElement.ToString())
            };

            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEnum;
            odcmModel.TryResolveType(enumName, schemaNamespace, out odcmEnum)
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
            var schemaNamespace = string.Empty;
            var enumName = string.Empty;
            var enumMemberCount = 0;
            var underlyingType = string.Empty;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EnumType(enumType =>
                {
                    foreach (var member in Any.Sequence((i) => Any.Csdl.Member(), Any.Int(1, 5)))
                    {
                        enumType.Add(member);
                        enumMemberCount++;
                    }
                    enumName = enumType.Attribute("Name").Value;
                    underlyingType = Any.Csdl.RandomEnumUnderlyingType();
                    enumType.AddAttribute("UnderlyingType", underlyingType);
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new TextFileCollection
            {
                new TextFile("$metadata", edmxElement.ToString())
            };

            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEnum;
            odcmModel.TryResolveType(enumName, schemaNamespace, out odcmEnum)
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
