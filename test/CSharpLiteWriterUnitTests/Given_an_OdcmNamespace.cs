// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmNamespace : CodeGenTestBase
    {
        private OdcmModel _model;

        
        public Given_an_OdcmNamespace()
        {
            _model = new OdcmModel(Any.ServiceMetadata());
        }

        [Fact]
        public void When_model_has_several_non_empty_namespaces_each_is_exposed_in_the_proxy()
        {
            var @namespace1 = Any.EmptyOdcmNamespace();

            _model.Namespaces.Add(@namespace1);

            _model.AddType(new OdcmEnum(Any.CSharpIdentifier(1, 10), @namespace1));

            var @namespace2 = Any.EmptyOdcmNamespace();

            _model.Namespaces.Add(@namespace2);

            _model.AddType(new OdcmEnum(Any.CSharpIdentifier(1, 10), @namespace2));

            var proxy = GetProxy(_model);

            proxy.GetNamespaces()
                .Should().Contain(new[] { @namespace1.Name, @namespace2.Name });
        }

        [Fact]
        public void When_the_model_has_a_namespace_called_Edm_it_is_ignored()
        {
            var @namespace = new OdcmNamespace("EDM");

            _model.Namespaces.Add(@namespace);

            _model.AddType(Any.OdcmEnum(e => e.Namespace = OdcmNamespace.Edm));

            var proxy = GetProxy(_model);

            proxy.GetNamespaces()
                .Should().BeEquivalentTo(new object[] { });
        }

        [Fact]
        public void When_the_model_has_a_namespace_and_class_with_the_same_name_then_compilation_succeeds()
        {
            var name = Any.CSharpIdentifier();

            var @namespace = new OdcmNamespace(name);

            _model.Namespaces.Add(@namespace);

            var @class = Any.OdcmEntityClass(@namespace, name);

            @class.Properties.Add(new OdcmProperty(Any.CSharpIdentifier())
            {
                Class = @class,
                Projection = new OdcmProjection()
                {
                    Type = @class
                }
            });

            _model.AddType(@class);

            var proxy = GetProxy(_model);

            proxy.GetClass(name, name)
                .Should().NotBeNull();
        }
    }
}
