// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.OData.ProxyExtensions;
using System;
using System.Linq;
using System.Reflection;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Complex : CodeGenTestBase
    {
        private OdcmModel _model;
        private OdcmNamespace _namespace;
        private OdcmClass _class;
        private Assembly _proxy;
        private Type _classType;

        
        public Given_an_OdcmClass_Complex()
        {
            _model = new OdcmModel(Any.ServiceMetadata());

            _namespace = Any.EmptyOdcmNamespace();

            _model.Namespaces.Add(_namespace);

            _class = Any.OdcmComplexClass(e => e.Namespace = _namespace);

            _model.AddType(_class);

            _proxy = GetProxy(_model);

            _classType = _proxy.GetClass(_class.Namespace, _class.Name);
        }

        [Fact]
        public void The_proxy_class_derives_from_ComplexTypeBase()
        {
            _classType.Should().BeDerivedFrom(typeof(ComplexTypeBase),
                "Because it enables notifying the containing entity of changes.");
        }

        [Fact]
        public void The_Collection_class_is_Public()
        {
            _classType.IsPublic
                .Should().BeTrue("Because it can be accessed directly by user code..");
        }

        [Fact]
        public void The_proxy_class_does_not_implement_any_interface()
        {
            _classType.GetInterfaces().Count()
                .Should().Be(0, "Because it is accessed without indirection.");
        }
    }
}
