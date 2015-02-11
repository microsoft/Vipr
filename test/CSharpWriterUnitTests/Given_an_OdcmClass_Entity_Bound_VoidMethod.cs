// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.MockService;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Bound_VoidMethod : EntityTestBase
    {
        private OdcmMethod _method;
        private readonly Type _expectedReturnType = typeof(Task);
        private readonly string _expectedMethodName;

        public Given_an_OdcmClass_Entity_Bound_VoidMethod()
        {
            _method = Any.OdcmMethod();

            _expectedMethodName = _method.Name + "Async";

            Init(m => m.Namespaces[0].Classes.First().Methods.Add(_method));
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
            CollectionInterface.Should().NotHaveMethod(_method.Name);
        }

        [Fact]
        public void The_Collection_class_does_not_expose_the_method()
        {
            CollectionType.Should().NotHaveMethod(_method.Name);
        }

        private IEnumerable<Type> GetMethodParameterTypes()
        {
            return _method.Parameters.Select(p => Proxy.GetClass(p.Type.Namespace, p.Type.Name));
        }

        [Fact]
        public void When_the_verb_is_POST_the_Concrete_passes_parameters_on_the_URI_and_in_the_body()
        {
            Init(m =>
            {
                _method = Any.OdcmMethodPost();
                _method.Class = Class;
                _method.ReturnType = null;
                _method.IsCollection = false;
                Class.Methods.Add(_method);
            });

            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var instancePath = Class.GetDefaultEntityPath(entityKeyValues);

            using (var mockService = new MockService(true)
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .Start())
            {
                var concrete = mockService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                mockService.ValidateParameterPassing("POST", concrete, instancePath, _method, null);
            }
        }

        [Fact]
        public void When_the_verb_is_POST_the_Fetcher_passes_parameters_on_the_URI_and_in_the_body()
        {
            Init(m =>
            {
                _method = Any.OdcmMethodPost();
                _method.Class = Class;
                _method.ReturnType = null;
                _method.IsCollection = false;
                Class.Methods.Add(_method);
            });

            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var fetcherPath = Class.GetDefaultEntityPath(entityKeyValues);

            using (var mockService = new MockService()
                .Start())
            {
                var fetcher = mockService
                    .GetDefaultContext(Model)
                    .CreateFetcher(FetcherType, fetcherPath);

                mockService.ValidateParameterPassing("POST", fetcher, fetcherPath, _method,
                    null);
            }
        }

        [Fact]
        public void When_the_verb_is_GET_the_Concrete_passes_parameters_on_the_URI()
        {
            Init(m =>
            {
                _method = Any.OdcmMethodGet();
                _method.Class = Class;
                _method.ReturnType = null;
                _method.IsCollection = false;
                Class.Methods.Add(_method);
            });

            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var instancePath = Class.GetDefaultEntityPath(entityKeyValues);

            using (var mockService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .Start())
            {
                var concrete = mockService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                mockService.ValidateParameterPassing("GET", concrete, instancePath, _method, null);
            }
        }

        [Fact]
        public void When_the_verb_is_GET_the_Fetcher_passes_parameters_on_the_URI()
        {
            Init(m =>
            {
                _method = Any.OdcmMethodGet();
                _method.Class = Class;
                _method.ReturnType = null;
                _method.IsCollection = false;
                Class.Methods.Add(_method);
            });

            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var fetcherPath = Class.GetDefaultEntityPath(entityKeyValues);

            using (var mockService = new MockService()
                .Start())
            {
                var fetcher = mockService
                    .GetDefaultContext(Model)
                    .CreateFetcher(FetcherType, fetcherPath);

                mockService.ValidateParameterPassing("GET", fetcher, fetcherPath, _method,
                    null);
            }
        }
    }
}
