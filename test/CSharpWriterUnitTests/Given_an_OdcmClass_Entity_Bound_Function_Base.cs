using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public abstract class Given_an_OdcmClass_Entity_Bound_Function_Base : EntityTestBase
    {
        protected OdcmMethod Method;
        protected Func<Type, Type> ReturnTypeGenerator;
        protected bool IsCollection;
        private Type _expectedReturnType;
        private string _expectedMethodName;

        protected void Init(Action<OdcmMethod> config = null)
        {
            Init(model => model.Namespaces[0].Classes.First()
                .Methods.Add(Method = Any.OdcmMethod(m =>
                {
                    m.ReturnType = model.Namespaces[0].Classes.First();
                    m.IsCollection = IsCollection;

                    if (config != null) config(m);
                })));

            _expectedReturnType = ReturnTypeGenerator(ConcreteInterface);

            _expectedMethodName = Method.Name + "Async";
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
        public void When_the_return_type_is_primitive_it_is_mapped_to_an_IEnumerable_of_DotNet_Primitives()
        {
            Init(model => model.Namespaces[0].Classes.First()
                .Methods.Add(Method = Any.OdcmMethod(m =>
                {
                    m.ReturnType = new OdcmPrimitiveType("Stream", OdcmNamespace.Edm);
                    m.IsCollection = IsCollection;
                })));

            _expectedReturnType = ReturnTypeGenerator(typeof(Microsoft.OData.Client.DataServiceStreamLink));

            _expectedMethodName = Method.Name + "Async";

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
            return Method.Parameters.Select(p => Proxy.GetClass(p.Type.Namespace, p.Type.Name));
        }

        [Fact]
        public void When_the_verb_is_POST_the_Concrete_passes_parameters_on_the_URI_and_in_the_body()
        {
            base.Init(m =>
            {
                Method = Any.OdcmMethodPost();
                Method.Class = Class;
                Method.ReturnType = Class;
                Method.IsCollection = IsCollection;
                Class.Methods.Add(Method);
            });

            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var instancePath = Class.GetDefaultEntityPath(entityKeyValues);

            using (var mockService = new MockService(true)
                .SetupPostEntity(TargetEntity, entityKeyValues))
            {
                var concrete = mockService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                mockService.ValidateParameterPassing("POST", concrete, instancePath, Method,
                    TargetEntity);
            }
        }

        [Fact]
        public void When_the_verb_is_POST_the_Fetcher_passes_parameters_on_the_URI_and_in_the_body()
        {
            base.Init(m =>
            {
                Method = Any.OdcmMethodPost();
                Method.Class = Class;
                Method.ReturnType = Class;
                Method.IsCollection = IsCollection;
                Class.Methods.Add(Method);
            });

            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var fetcherPath = Class.GetDefaultEntityPath(entityKeyValues);

            using (var mockService = new MockService())
            {
                var fetcher = mockService
                    .GetDefaultContext(Model)
                    .CreateFetcher(FetcherType, fetcherPath);

                mockService.ValidateParameterPassing("POST", fetcher, fetcherPath, Method,
                    TargetEntity);
            }
        }

        [Fact]
        public void When_the_verb_is_GET_the_Concrete_passes_parameters_on_the_URI()
        {
            base.Init(m =>
            {
                Method = Any.OdcmMethodGet();
                Method.Class = Class;
                Method.ReturnType = Class;
                Method.IsCollection = IsCollection;
                Class.Methods.Add(Method);
            });

            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var instancePath = Class.GetDefaultEntityPath(entityKeyValues);

            using (var mockService = new MockService()
                .SetupPostEntity(TargetEntity, entityKeyValues))
            {
                var concrete = mockService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                mockService.ValidateParameterPassing("GET", concrete, instancePath, Method,
                    TargetEntity);
            }
        }

        [Fact]
        public void When_the_verb_is_GET_the_Fetcher_passes_parameters_on_the_URI()
        {
            base.Init(m =>
            {
                Method = Any.OdcmMethodGet();
                Method.Class = Class;
                Method.ReturnType = Class;
                Method.IsCollection = IsCollection;
                Class.Methods.Add(Method);
            });

            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var fetcherPath = Class.GetDefaultEntityPath(entityKeyValues);

            using (var mockService = new MockService())
            {
                var fetcher = mockService
                    .GetDefaultContext(Model)
                    .CreateFetcher(FetcherType, fetcherPath);

                mockService.ValidateParameterPassing("GET", fetcher, fetcherPath, Method,
                    TargetEntity);
            }
        }
    }
}