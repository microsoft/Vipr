// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmParameter : CodeGenTestBase
    {
        private OdcmModel _model;
        private OdcmNamespace _namespace;
        private OdcmClass _class;
        private OdcmMethod _method;
        private OdcmParameter _param;
        private string _expectedMethodName;

        
        public Given_an_OdcmParameter()
        {
            _model = new OdcmModel(Any.ServiceMetadata());

            _namespace = Any.EmptyOdcmNamespace();
            _model.Namespaces.Add(_namespace);

            _class = Any.EntityOdcmClass(_namespace);
            _model.AddType(_class);

            _method = Any.OdcmMethod(m => m.Parameters.Clear());
            _class.Methods.Add(_method);

            _param = Any.OdcmParameter();
            _method.Parameters.Add(_param);

            _expectedMethodName = _method.Name + "Async";
        }

        [Fact]
        public void When_primitive_parameter_is_nullable_then_its_proxy_is_exposed_as_nullable()
        {
            _param.IsNullable = true;
            _param.Type = new OdcmPrimitiveType("Boolean", OdcmNamespace.Edm);

            var proxy = GetProxy(_model);
            var methodInfo = proxy.GetClass(_class.Namespace, _class.Name).GetMethod(_expectedMethodName);

            methodInfo.GetParameters()[0].ParameterType
                .Should().Be(typeof(Nullable<Boolean>));
        }

        [Fact]
        public void When_primitive_parameter_is_non_nullable_then_its_proxy_is_exposed_as_non_nullable()
        {
            _param.IsNullable = false;
            _param.Type = new OdcmPrimitiveType("Int64", OdcmNamespace.Edm);

            var proxy = GetProxy(_model);
            var methodInfo = proxy.GetClass(_class.Namespace, _class.Name).GetMethod(_expectedMethodName);

            methodInfo.GetParameters()[0].ParameterType
                .Should().Be(typeof(Int64));
        }

        [Fact]
        public void When_collection_of_primitive_parameter_is_nullable_then_its_proxy_is_exposed_as_collection_of_primitive()
        {
            _param.IsNullable = true;
            _param.IsCollection = true;
            _param.Type = new OdcmPrimitiveType("Int32", OdcmNamespace.Edm);

            var proxy = GetProxy(_model);
            var methodInfo = proxy.GetClass(_class.Namespace, _class.Name).GetMethod(_expectedMethodName);

            methodInfo.GetParameters()[0].ParameterType
                .Should().Be(typeof(ICollection<Int32>));
        }
    }
}
