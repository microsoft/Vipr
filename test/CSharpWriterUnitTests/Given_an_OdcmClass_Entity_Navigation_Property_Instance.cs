// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System.Linq;
using Microsoft.OData.ProxyExtensions;
using ODataV4TestService.SelfHost;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Navigation_Property_Instance : NavigationPropertyTestBase
    {
        private IStartedScenario _mockedService;

        public Given_an_OdcmClass_Entity_Navigation_Property_Instance()
        {
            _navigationProperty = Any.OdcmProperty(p => p.Type = Class);

            base.Init(m =>
            {
                var @namespace = m.Namespaces[0];
                _navTargetClass = Any.EntityOdcmClass(@namespace);
                @namespace.Types.Add(_navTargetClass);

                var @class = @namespace.Classes.First();
                _navigationProperty = Any.OdcmProperty(p =>
                {
                    p.Class = @class;
                    p.Type = _navTargetClass;
                    p.IsCollection = false;
                });

                m.Namespaces[0].Classes.First().Properties.Add(_navigationProperty);
            });
        }

        [Fact]
        public void The_Concrete_class_exposes_a_ConcreteType_property()
        {
            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                _navTargetConcreteType,
                _navigationProperty.Name,
                "Because Entity types should be accessible through their related Entity types.");
        }

        [Fact]
        public void The_Concrete_class_explicitly_implements_ConcreteInterface_Interface_property()
        {
            ConcreteType.Should().HaveExplicitProperty(
                ConcreteInterface,
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                _navTargetConcreteInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_class_explicitly_implements_readonly_FetcherInterface_FetcherInterface_property()
        {
            ConcreteType.Should().HaveExplicitProperty(
                FetcherInterface,
                CSharpAccessModifiers.Public,
                null,
                _navTargetFetcherInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_interface_exposes_a_ConcreteInterface_property()
        {
            ConcreteInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                _navTargetConcreteInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Fetcher_interface_exposes_a_readonly_FetcherInterface_property()
        {
            FetcherInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _navTargetFetcherInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Fetcher_class_exposes_a_readonly_Fetcher_Interface_property()
        {
            FetcherType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _navTargetFetcherInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Collection_interface_does_not_expose_it()
        {
            CollectionInterface.Should().NotHaveProperty(_navigationProperty.Name);
        }

        [Fact]
        public void The_Collection_class_does_not_expose_it()
        {
            CollectionType.Should().NotHaveProperty(_navigationProperty.Name);
        }
        
        [Fact]
        public void When_retrieved_through_Fetcher_then_request_is_sent_to_server_with_original_name()
        {
            var entityPath = Any.UriPath(1);
            var expectedPath = "/" + entityPath + "/" + _navigationProperty.Name;
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockScenario()
                    .Setup(c => c.Request.Method == "GET" &&
                                c.Request.Path.Value == expectedPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 200;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, keyValues));
                           })
                    .Start())
            {
                var fetcher = _mockedService
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true)
                    .CreateFetcher(FetcherType, entityPath);

                var propertyFetcher = fetcher.GetPropertyValue<RestShallowObjectFetcher>(_navigationProperty.Name);

                propertyFetcher.ExecuteAsync().Wait();
            }
        }

        [Fact(Skip = "Issue #24 https://github.com/Microsoft/vipr/issues/24")]
        public void When_retrieved_through_Concrete_ConcreteInterface_Property_then_request_is_sent_with_original_name()
        {
            var entitySetName = Class.Name + "s";
            var entitySetPath = "/" + entitySetName;
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var entityPath = string.Format("{0}({1})", entitySetPath, ODataKeyPredicate.AsString(entityKeyValues));
            var expectedPath = entityPath + "/" + _navigationProperty.Name;
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockScenario()
                .Setup(c => c.Request.Method == "POST" &&
                            c.Request.Path.Value == entitySetPath,
                    (b, c) =>
                    {
                        c.Response.StatusCode = 201;
                        c.Response.WithDefaultODataHeaders();
                        c.Response.Write(ConcreteType.AsJson(b, entityKeyValues));
                    })
                .Setup(c => c.Request.Method == "GET" &&
                            c.Request.Path.Value == expectedPath,
                    (b, c) =>
                    {
                        c.Response.StatusCode = 200;
                        c.Response.WithDefaultODataHeaders();
                        c.Response.Write(ConcreteType.AsJson(b, keyValues));
                    })
                .Start())
            {
                var instance = _mockedService
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true)
                    .CreateConcrete(ConcreteType);

                instance.SetPropertyValues(Class.GetSampleKeyArguments());

                var propertyValue = instance.GetPropertyValue<RestShallowObjectFetcher>(ConcreteInterface,
                    _navigationProperty.Name);

                propertyValue.ExecuteAsync().Wait();
            }
        }

        [Fact]
        public void When_retrieved_through_Concrete_then_request_is_sent_to_server_with_original_name()
        {
            var entitySetName = Class.Name + "s";
            var entitySetPath = "/" + entitySetName;
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var entityPath = string.Format("{0}({1})", entitySetPath, ODataKeyPredicate.AsString(entityKeyValues));
            var expectedPath = entityPath + "/" + _navigationProperty.Name;
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockScenario()
                    .Setup(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == entitySetPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 201;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, entityKeyValues));
                           })
                    .Setup(c => c.Request.Method == "GET" &&
                                c.Request.Path.Value == expectedPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 200;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, keyValues));
                           })
                    .Start())
            {
                var instance = _mockedService
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true)
                    .CreateConcrete(ConcreteType);

                instance.SetPropertyValues(Class.GetSampleKeyArguments());

                var propertyFetcher = instance.GetPropertyValue<RestShallowObjectFetcher>(FetcherInterface,
                    _navigationProperty.Name);

                propertyFetcher.ExecuteAsync().Wait();
            }
        }

        [Fact]
        public void When_updated_through_Concrete_accessor_then_request_is_sent_to_server_with_original_name()
        {
            var entitySetName = Class.Name + "s";
            var entitySetPath = "/" + entitySetName;
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var entityPath = string.Format("{0}({1})", entitySetPath, ODataKeyPredicate.AsString(entityKeyValues));
            var expectedPath = entityPath;

            using (_mockedService = new MockScenario()
                    .Setup(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == entitySetPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 201;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, entityKeyValues));
                           })
                    .Setup(c => c.Request.Method == "PATCH" &&
                                c.Request.Path.Value == expectedPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 200;
                               c.Response.WithDefaultODataHeaders();
                           })
                    .Start())
            {
                var context = _mockedService
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true);
                var instance = context
                    .CreateConcrete(ConcreteType);

                var relatedInstance = Activator.CreateInstance(_navTargetConcreteType);

                instance.SetPropertyValue(_navigationProperty.Name, relatedInstance);

                instance.UpdateAsync().Wait();
            }
        }
    }
}
