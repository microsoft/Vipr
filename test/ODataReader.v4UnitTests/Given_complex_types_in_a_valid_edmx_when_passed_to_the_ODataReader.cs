// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using ODataReader.v4;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace ODataReader.v4UnitTests
{
    public class Given_complex_types_in_a_valid_edmx_when_passed_to_the_ODataReader
    {
        private ODataReader.v4.OdcmReader _odcmReader;

        public Given_complex_types_in_a_valid_edmx_when_passed_to_the_ODataReader()
        {
            _odcmReader = new OdcmReader();
        }

        [Fact]
        public void It_returns_one_OdcmComplexClass_for_each_ComplexType()
        {
            var testCase = new EdmxTestCase()
                .AddComplexType(EdmxTestCase.Keys.ComplexType);

            var complexTypeTestNode = testCase[EdmxTestCase.Keys.ComplexType];
            var propertyCount = (from x in complexTypeTestNode.Element.Descendants() where x.Name.LocalName.Equals("Property") select x).Count();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeTestNode.Name, complexTypeTestNode.Namespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmComplexClass>("because complex types should result in an OdcmClass");
            odcmComplexType.As<OdcmComplexClass>().Properties.Count
                .Should()
                .Be(propertyCount, "because each property added to a complex type should result in an OdcmClass property");
        }

        [Fact]
        public void When_Abstract_is_set_it_returns_an_OdcmClass_with_IsAbstract_set()
        {
            var testCase = new EdmxTestCase()
                .AddComplexType(EdmxTestCase.Keys.ComplexType, (_, complexType) => complexType.AddAttribute("Abstract", true));

            var complexTypeTestNode = testCase[EdmxTestCase.Keys.ComplexType];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeTestNode.Name, complexTypeTestNode.Namespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmComplexClass>("because complex types should result in an OdcmClass");
            odcmComplexType.As<OdcmComplexClass>().IsAbstract
                .Should()
                .BeTrue("because a complex type with the Abstract facet set should be abstract in the OdcmModel");
        }

        [Fact]
        public void When_OpenType_is_set_it_returns_an_OdcmClass_with_IsOpen_set()
        {
            var testCase = new EdmxTestCase()
                .AddComplexType(EdmxTestCase.Keys.ComplexType, (_, complexType) => complexType.AddAttribute("OpenType", true));

            var complexTypeTestNode = testCase[EdmxTestCase.Keys.ComplexType];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeTestNode.Name, complexTypeTestNode.Namespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmComplexClass>("because complex types should result in an OdcmClass");
            odcmComplexType.As<OdcmComplexClass>().IsOpen
                .Should()
                .BeTrue("because a complex type with the OpenType facet set should be open in the OdcmModel");
        }

        [Fact]
        public void When_BaseType_is_set_it_returns_an_OdcmClass_with_a_BaseType_set()
        {
            var testCase = new EdmxTestCase()
                .AddComplexType(EdmxTestCase.Keys.ComplexTypeBase, (_, complexType) => complexType.AddAttribute("Abstract", true))
                .AddComplexType(EdmxTestCase.Keys.ComplexType, (_, complexType) => complexType.AddAttribute("BaseType", _[EdmxTestCase.Keys.ComplexTypeBase].FullName()));

            var baseComplexTypeTestNode = testCase[EdmxTestCase.Keys.ComplexTypeBase];
            var complexTypeTestNode = testCase[EdmxTestCase.Keys.ComplexType];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());


            OdcmType odcmBaseType;
            odcmModel.TryResolveType(baseComplexTypeTestNode.Name, baseComplexTypeTestNode.Namespace, out odcmBaseType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmBaseType
                .Should()
                .BeOfType<OdcmComplexClass>("because complex types should result in an OdcmClass");
            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeTestNode.Name, complexTypeTestNode.Namespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmComplexClass>("because complex types should result in an OdcmClass");
            odcmComplexType.As<OdcmComplexClass>().Base
                .Should()
                .Be(odcmBaseType,
                    "because a complex type with a base type set should have a corresponding OdcmClass and base OdcmClass");
            odcmBaseType.As<OdcmComplexClass>().Derived
                .Should()
                .Contain(odcmComplexType.As<OdcmComplexClass>(),
                    "because a complex type with a base type set should have a correspond OdcmClass that derives from a base OdcmClass");
        }
    }
}
