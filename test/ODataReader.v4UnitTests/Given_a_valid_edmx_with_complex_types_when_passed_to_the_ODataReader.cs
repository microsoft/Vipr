// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using ODataReader.v4;
using System.Collections.Generic;
using Vipr.Core.CodeModel;
using Xunit;

namespace ODataReader.v4UnitTests
{
    public class Given_a_valid_edmx_with_complex_types_when_passed_to_the_ODataReader
    {
        private ODataReader.v4.Reader reader;

        public Given_a_valid_edmx_with_complex_types_when_passed_to_the_ODataReader()
        {
            reader = new Reader();
        }

        [Fact]
        public void It_returns_one_OdcmClass_for_each_ComplexType()
        {
            var complexTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.ComplexType(complexType =>
                {
                    foreach (
                        var property in
                            Any.Sequence(
                                i =>
                                    Any.Csdl.Property(
                                        property => property.AddAttribute("Type", Any.Csdl.RandomPrimitiveType())),
                                Any.Int(1, 5)))
                    {
                        complexType.Add(property);
                        propertyCount++;
                    }

                    complexTypeName = complexType.Attribute("Name").Value;
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = reader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeName, schemaNamespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmClass>("because complex types should result in an OdcmClass");
            odcmComplexType.As<OdcmClass>().Properties.Count
                .Should()
                .Be(propertyCount, "because each blah...");
        }

        [Fact]
        public void When_IsAbstract_is_set_it_returns_an_OdcmClass_with_IsAbstract_set()
        {
            var complexTypeName = string.Empty;
            var schemaNamespace = string.Empty;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.ComplexType(complexType =>
                {
                    complexTypeName = complexType.Attribute("Name").Value;
                    complexType.AddAttribute("Abstract", true);
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = reader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeName, schemaNamespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmClass>("because complex types should result in an OdcmClass");
            odcmComplexType.As<OdcmClass>().IsAbstract
                .Should()
                .BeTrue("because a complex type with the IsAbstract facet set should be abstact");
        }
    }
}
