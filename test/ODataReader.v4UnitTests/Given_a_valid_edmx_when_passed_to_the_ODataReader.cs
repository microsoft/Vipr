// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using ODataReader.v4;
using System;
using Vipr.Core.CodeModel;
using Xunit;

namespace ODataReader.v4UnitTests
{
    public class Given_a_valid_edmx_when_passed_to_the_ODataReader
    {
        private ODataReader.v4.OdcmReader _odcmReader;

        public Given_a_valid_edmx_when_passed_to_the_ODataReader()
        {
            _odcmReader = new OdcmReader();
        }

        [Fact]
        public void It_returns_an_odcm_model()
        {
            var testCase = new EdmxTestCase();

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            odcmModel
                .Should()
                .NotBeNull("because a valid edmx should yield a valid model");
        }

        [Fact]
        public void It_results_an_OdcmNamespace_for_the_Schema()
        {
            var testCase = new EdmxTestCase();

            var schemaTestNode = testCase[EdmxTestCase.Keys.Schema];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            odcmModel.Namespaces
                .FindAll(
                    @namespace => @namespace.Name.Equals(schemaTestNode.Namespace, StringComparison.InvariantCultureIgnoreCase))
                .Count
                .Should()
                .Be(1, "because only one namespace shoud be created per schema element");
        }

        [Fact]
        public void It_results_in_an_OdcmClass_for_the_EntityContainer()
        {
            var testCase = new EdmxTestCase();

            var entityContainerTestNode = testCase[EdmxTestCase.Keys.EntityContainer];

            var odcmModel = _odcmReader.GenerateOdcmModel(testCase.ServiceMetadata());

            OdcmType odcmClass;
            odcmModel.TryResolveType(entityContainerTestNode.Name, entityContainerTestNode.Namespace, out odcmClass)
                .Should()
                .BeTrue("because an EntityContainer should result in a matching OdcmType");
            odcmClass
                .Should()
                .BeOfType<OdcmServiceClass>("because an EntityContainer should result in a matching OdcmServiceClass");
        }
    }
}
