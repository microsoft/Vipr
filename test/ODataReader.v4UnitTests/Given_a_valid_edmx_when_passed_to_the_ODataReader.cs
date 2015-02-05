// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using ODataReader.v4;
using System;
using System.Collections.Generic;
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
            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
                schema.Add(Any.Csdl.EntityContainer()));

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            odcmModel
                .Should()
                .NotBeNull("because a valid edmx should yield a valid model");
        }

        [Fact]
        public void It_results_an_OdcmNamespace_for_the_Schema()
        {
            var schemaNamespace = string.Empty;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            odcmModel.Namespaces
                .FindAll(
                    @namespace => @namespace.Name.Equals(schemaNamespace, StringComparison.InvariantCultureIgnoreCase))
                .Count
                .Should()
                .Be(1, "because only one namespace shoud be created per schema element");
        }

        [Fact]
        public void It_results_in_an_OdcmClass_for_the_EntityContainer()
        {
            var schemaNamespace = string.Empty;
            var entityContainerName = string.Empty;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EntityContainer(entityContainer =>
                {
                    entityContainerName = entityContainer.Attribute("Name").Value;
                }));
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmClass;
            odcmModel.TryResolveType(entityContainerName, schemaNamespace, out odcmClass)
                .Should()
                .BeTrue("because an EntityContainer should result in a matching OdcmType");
            odcmClass
                .Should()
                .BeOfType<OdcmServiceClass>("because an EntityContainer should result in a matching OdcmServiceClass");
        }
    }
}
