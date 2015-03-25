// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System.Linq;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Microsoft.OData.ProxyExtensions;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_EntityNavigation_Property_Instance : NavigationPropertyTestBase
    {
        private MockService _mockedService;

        public Given_an_OdcmClass_EntityNavigation_Property_Instance()
        {
            NavigationProperty = Any.OdcmProperty(p => p.Type = Class);

            base.Init(m =>
            {
                var @namespace = m.Namespaces[0];
                NavTargetClass = Any.EntityOdcmClass(@namespace);
                @namespace.Types.Add(NavTargetClass);

                var @class = @namespace.Classes.First();
                NavigationProperty = Any.OdcmProperty(p =>
                {
                    p.Class = @class;
                    p.Type = NavTargetClass;
                    p.IsCollection = false;
                });

                m.Namespaces[0].Classes.First().Properties.Add(NavigationProperty);
            });
        }

        [Fact]
        public void The_Concrete_class_exposes_a_ConcreteType_property()
        {
            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                NavTargetConcreteType,
                NavigationProperty.Name,
                "Because Entity types should be accessible through their related Entity types.");
        }

        [Fact]
        public void The_Concrete_class_explicitly_implements_ConcreteInterface_Interface_property()
        {
            ConcreteType.Should().HaveExplicitProperty(
                ConcreteInterface,
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                NavTargetConcreteInterface,
                NavigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_class_explicitly_implements_readonly_FetcherInterface_FetcherInterface_property()
        {
            ConcreteType.Should().HaveExplicitProperty(
                FetcherInterface,
                CSharpAccessModifiers.Public,
                null,
                NavTargetFetcherInterface,
                NavigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_interface_exposes_a_ConcreteInterface_property()
        {
            ConcreteInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                NavTargetConcreteInterface,
                NavigationProperty.Name);
        }

        [Fact]
        public void The_Fetcher_interface_exposes_a_readonly_FetcherInterface_property()
        {
            FetcherInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                NavTargetFetcherInterface,
                NavigationProperty.Name);
        }

        [Fact]
        public void The_Fetcher_class_exposes_a_readonly_Fetcher_Interface_property()
        {
            FetcherType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                NavTargetFetcherInterface,
                NavigationProperty.Name);
        }

        [Fact]
        public void The_Collection_interface_doesNot_expose_it()
        {
            CollectionInterface.Should().NotHaveProperty(NavigationProperty.Name);
        }

        [Fact]
        public void The_Collection_class_doesNot_expose_it()
        {
            CollectionType.Should().NotHaveProperty(NavigationProperty.Name);
        }
        
        [Fact]
        public void When_retrieved_through_Fetcher_then_request_is_sent_to_server_with_originalName()
        {
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupGetEntityProperty(TargetEntity, keyValues, NavigationProperty))
            {
                var fetcher = _mockedService
                    .GetDefaultContext(Model)
                    .CreateFetcher(FetcherType, TargetEntity.Class.GetDefaultEntityPath(keyValues));

                var propertyFetcher = fetcher.GetPropertyValue<RestShallowObjectFetcher>(NavigationProperty.Name);

                propertyFetcher.ExecuteAsync().Wait();
            }
        }

        [Fact(Skip = "Issue #24 https://github.com/Microsoft/vipr/issues/24")]
        public void When_retrieved_through_Concrete_ConcreteInterface_Property_then_request_is_sent_with_originalName()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var expectedPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + NavigationProperty.Name;
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                .SetupPostEntity(TargetEntity, entityKeyValues)
                .SetupGetEntity(TargetEntity))
            {
                var instance = _mockedService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                instance.SetPropertyValues(Class.GetSampleKeyArguments());

                var propertyValue = instance.GetPropertyValue<RestShallowObjectFetcher>(ConcreteInterface,
                    NavigationProperty.Name);

                propertyValue.ExecuteAsync().Wait();
            }
        }

        [Fact]
        public void When_retrieved_through_Concrete_then_request_is_sent_to_server_with_originalName()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
         
            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupGetEntityProperty(TargetEntity, entityKeyValues, NavigationProperty))
            {
                var instance = _mockedService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                instance.SetPropertyValues(Class.GetSampleKeyArguments());

                var propertyFetcher = instance.GetPropertyValue<RestShallowObjectFetcher>(FetcherInterface,
                    NavigationProperty.Name);

                propertyFetcher.ExecuteAsync().Wait();
            }
        }

        [Fact]
        public void When_updated_through_Concrete_accessor_then_request_is_sent_to_server_with_originalName()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var expectedPath = Class.GetDefaultEntityPath(entityKeyValues);

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .OnPatchEntityRequest(expectedPath)
                        .RespondWithODataOk())
            {
                var context = _mockedService
                    .GetDefaultContext(Model);

                var instance = context
                    .CreateConcrete(ConcreteType);

                var relatedInstance = Activator.CreateInstance(NavTargetConcreteType);

                instance.SetPropertyValue(NavigationProperty.Name, relatedInstance);

                instance.UpdateAsync().Wait();
            }
        }
    }
}
