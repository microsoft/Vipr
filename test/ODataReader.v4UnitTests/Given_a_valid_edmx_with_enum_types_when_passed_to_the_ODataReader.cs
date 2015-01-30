// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using ODataReader.v4;
using System.Collections.Generic;
using System.Xml.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace ODataReader.v4UnitTests
{
    public class Given_a_valid_edmx_with_enum_types_when_passed_to_the_ODataReader
    {
        private ODataReader.v4.Reader reader;

        public Given_a_valid_edmx_with_enum_types_when_passed_to_the_ODataReader()
        {
            reader = new Reader();
        }

        [Fact]
        public void It_returns_one_OdcmType_for_each_EnumType()
        {
            var schemaNamespace = string.Empty;
            var enumName = string.Empty;
            var enumMemberCount = 0;

            var edmxElement =
                Any.Edmx(edmx => edmx.Add(
                    Any.DataServices(dataServices => dataServices.Add(
                        Any.Schema(schema =>
                        {
                            schema.Add(Any.EnumType(enumType =>
                            {
                                foreach (var member in Any.Sequence((i) => Any.Member(), Any.Int(1, 5)))
                                {
                                    enumType.Add(member);
                                    enumMemberCount++;
                                }
                                enumName = enumType.Attribute("Name").Value;
                            }));
                            schema.Add(Any.EntityContainer());
                            schemaNamespace = schema.Attribute("Namespace").Value;
                        })))));

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = reader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEnum;
            odcmModel.TryResolveType(enumName, schemaNamespace, out odcmEnum)
                .Should()
                .BeTrue("because an enum type in the schema should result in an OdcmType");
            odcmEnum.Should().BeOfType<OdcmEnum>("because an enum type in the schema should result in an OdcmEnum");
            odcmEnum.As<OdcmEnum>()
                .Members.Count.Should()
                .Be(enumMemberCount, "because each member added to the schema should result in an OdcmMember");
        }
    }
}
