// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Microsoft.OData.ProxyExtensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Navigation_Property_Collection : NavigationPropertyTestBase
    {
        private MockService _mockedService;
        public Given_an_OdcmClass_Entity_Navigation_Property_Collection()
        {
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
                    p.IsCollection = true;
                });

                m.Namespaces[0].Classes.First().Properties.Add(_navigationProperty);
            });
        }

        [Fact]
        public void The_Concrete_class_exposes_a_List_Of_Concrete_Type_property()
        {
            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                typeof(IList<>).MakeGenericType(_navTargetConcreteType),
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Fetcher_interface_exposes_a_readonly_CollectionInterface_property()
        {
            FetcherInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _navTargetCollectionInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Fetcher_class_exposes_a_readonly_CollectionInterface_property()
        {
            FetcherType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _navTargetCollectionInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_class_explicitly_implements_readonly_FetcherInterface_CollectionInterface_property()
        {
            ConcreteType.Should().HaveExplicitProperty(
                FetcherInterface,
                CSharpAccessModifiers.Public,
                null,
                _navTargetCollectionInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_interface_exposes_a_readonly_IPagedCollectionOfConcreteInterface_property()
        {
            ConcreteInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                typeof(IPagedCollection<>).MakeGenericType(_navTargetConcreteInterface),
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_class_explicitly_implements_readonly_ConcreteInterface_IPagedCollectionOfConcreteInterface_property()
        {
            ConcreteType.Should().HaveExplicitProperty(
                ConcreteInterface,
                CSharpAccessModifiers.Public,
                null,
                typeof(IPagedCollection<>).MakeGenericType(_navTargetConcreteInterface),
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Collection_class_does_not_expose_it()
        {
            CollectionType.Should().NotHaveProperty(_navigationProperty.Name);
        }

        [Fact]
        public void The_Collection_interface_does_not_expose_it()
        {
            CollectionInterface.Should().NotHaveProperty(_navigationProperty.Name);
        }

        [Fact(Skip = "Issue #24 https://github.com/Microsoft/vipr/issues/24")]
        public void When_retrieved_through_Concrete_ConcreteInterface_Property_then_request_is_sent_with_original_name()
        {
            using (_mockedService = new MockService()
                .SetupPostEntity(TargetEntity)
                .SetupGetEntity(TargetEntity)
                .Start())
            {
                var instance = _mockedService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                instance.SetPropertyValues(Class.GetSampleKeyArguments());

                var propertyValue = instance.GetPropertyValue<IPagedCollection>(ConcreteInterface,
                    _navigationProperty.Name);

                propertyValue.GetNextPageAsync().Wait();
            }
        }

        [Fact]
        public void When_retrieved_through_Concrete_FetcherInterface_Property_then_request_is_sent_with_original_name()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                .SetupPostEntity(TargetEntity, entityKeyValues)
                .SetupGetEntityProperty(TargetEntity, entityKeyValues, _navigationProperty)
                .Start())
            {
                var instance = _mockedService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                instance.SetPropertyValues(Class.GetSampleKeyArguments());

                var propertyFetcher = instance.GetPropertyValue<ReadOnlyQueryableSetBase>(FetcherInterface,
                    _navigationProperty.Name);

                propertyFetcher.ExecuteAsync().Wait();
            }
        }

        [Fact]
        public void When_retrieved_through_Fetcher_then_request_is_sent_to_server_with_original_name()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + _navigationProperty.Name;

            using (_mockedService = new MockService()
                    .SetupGetEntity(propertyPath, Class.GetDefaultEntitySetName(), ConcreteType.Initialize(Class.GetSampleKeyArguments()))
                    .Start())
            {
                var fetcher = _mockedService
                    .GetDefaultContext(Model)
                    .CreateFetcher(FetcherType, Class.GetDefaultEntityPath(entityKeyValues));

                var propertyFetcher = fetcher.GetPropertyValue<ReadOnlyQueryableSetBase>(_navigationProperty.Name);

                propertyFetcher.ExecuteAsync().Wait();
            }
        }

        [Fact]
        public void When_updated_through_Concrete_accessor_then_request_is_sent_to_server_with_original_name()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                .SetupPostEntity(TargetEntity, entityKeyValues)
                .SetupPostEntityPropertyChanges(TargetEntity, entityKeyValues, _navigationProperty)
                .Start())
            {
                var context = _mockedService
                    .GetDefaultContext(Model);

                var instance = context
                    .CreateConcrete(ConcreteType);

                var relatedInstance = Activator.CreateInstance(_navTargetConcreteType);

                var collection = Activator.CreateInstance(typeof(List<>).MakeGenericType(_navTargetConcreteType));

                collection.InvokeMethod("Add", new[] { relatedInstance });

                instance.SetPropertyValue(_navigationProperty.Name, collection);

                context.SaveChangesAsync().Wait();
            }
        }
    }
}
