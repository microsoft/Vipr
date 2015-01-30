// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Bound_Function : EntityTestBase
    {
        private OdcmMethod _method;
        private Type _expectedReturnType;
        private string _expectedMethodName;

        
        public Given_an_OdcmClass_Entity_Bound_Function()
        {
            Init(model => model.Namespaces[0].Classes.First()
                .Methods.Add(_method = Any.OdcmMethod(m => m.ReturnType = model.Namespaces[0].Classes.First())));

            _expectedReturnType = typeof(Task<>).MakeGenericType(ConcreteInterface);

            _expectedMethodName = _method.Name + "Async";
        }

        [Fact]
        public void The_Concrete_interface_exposes_the_method()
        {
            ConcreteInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                _expectedReturnType,
                _expectedMethodName,
                GetMethodParameterTypes());
        }

        [Fact]
        public void The_Concrete_class_exposes_the_async_method()
        {
            ConcreteType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                true,
                _expectedReturnType,
                _expectedMethodName,
                GetMethodParameterTypes());
        }

        [Fact]
        public void The_Fetcher_interface_exposes_the_method()
        {
            FetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                _expectedReturnType,
                _expectedMethodName,
                GetMethodParameterTypes());
        }

        [Fact]
        public void The_Fetcher_class_exposes_the_async_method()
        {
            FetcherType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                true,
                _expectedReturnType,
                _expectedMethodName,
                GetMethodParameterTypes());
        }

        [Fact]
        public void The_Collection_interface_does_not_expose_the_method()
        {
            CollectionInterface.Should().NotHaveMethod(_expectedMethodName);
        }

        [Fact]
        public void The_Collection_class_does_not_expose_the_method()
        {
            CollectionType.Should().NotHaveMethod(_expectedMethodName);
        }

        [Fact]
        public void When_the_return_type_is_primitive_it_is_mapped_to_a_DotNet_Primitive()
        {
            Init(model => model.Namespaces[0].Classes.First()
                .Methods.Add(_method = Any.OdcmMethod(m => m.ReturnType = new OdcmPrimitiveType("Stream", "Edm"))));

            _expectedReturnType = typeof(Task<>).MakeGenericType(typeof(Microsoft.OData.Client.DataServiceStreamLink));

            _expectedMethodName = _method.Name + "Async";

            var methodInfos = new[]
            {
                ConcreteInterface.GetMethod(_expectedMethodName),
                ConcreteType.GetMethod(_expectedMethodName),
                FetcherInterface.GetMethod(_expectedMethodName),
                FetcherType.GetMethod(_expectedMethodName)
            };

            foreach (var methodInfo in methodInfos)
            {
                methodInfo.ReturnType
                    .Should().Be(_expectedReturnType);
            }
        }

        private IEnumerable<Type> GetMethodParameterTypes()
        {
            return _method.Parameters.Select(p => Proxy.GetClass(p.Type.Namespace, p.Type.Name));
        }
    }
}
