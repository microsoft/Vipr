// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Vipr.Core;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_AddAsync_Method : EntityTestBase
    {
        private MockService _serviceMock;

        public Given_an_OdcmClass_Entity_Collection_AddAsync_Method()
        {
            Init();
        }

        [Fact]
        public void When_the_entity_is_null_then_AddAsync_POSTs_to_the_EntitySet_and_updates_the_added_instance()
        {
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_serviceMock = new MockService()
                    .SetupPostEntity(TargetEntity, keyValues))
            {
                var collection = _serviceMock
                    .GetDefaultContext(Model)
                    .CreateCollection(CollectionType, ConcreteType, TargetEntity.Class.GetDefaultEntitySetPath());

                var instance = Activator.CreateInstance(ConcreteType);

                var task = collection.InvokeMethod<Task>("Add" + Class.Name + "Async", args: new[] { instance, false });

                task.Wait();

                instance.ValidatePropertyValues(keyValues);
            }
        }

        [Fact]
        public void When_the_entity_is_not_null_then_AddAsync_POSTs_to_the_Entitys_canoncial_Uri_property_and_updates_the_added_instance()
        {
            var parentKeyValues = Class.GetSampleKeyArguments().ToArray();
            var navPropertyName = Class.NavigationProperties().First(p => p.Type == Class && p.IsCollection).Name;
            var navPropertyPath = string.Format("{0}/{1}", TargetEntity.Class.GetDefaultEntityPath(parentKeyValues), navPropertyName);
            var childKeyValues = Class.GetSampleKeyArguments().ToArray();

            using (_serviceMock = new MockService()
                    .SetupPostEntity(TargetEntity, parentKeyValues)
                    .OnPostEntityRequest(navPropertyPath)
                    .RespondWithCreateEntity(Class.GetDefaultEntitySetName(), ConcreteType.Initialize(childKeyValues)))
            {
                var parentEntity = Activator.CreateInstance(ConcreteType);
                var childEntity = Activator.CreateInstance(ConcreteType);

                var entitySetCollection = _serviceMock
                    .GetDefaultContext(Model)
                    .CreateCollection(CollectionType, ConcreteType, TargetEntity.Class.GetDefaultEntitySetPath());

                var navigationPropertyCollection = entitySetCollection
                    .Context
                    .CreateCollection(CollectionType, ConcreteType, navPropertyName, parentEntity);

                var parentTask = entitySetCollection.InvokeMethod<Task>("Add" + Class.Name + "Async", args: new[] { parentEntity, false });

                parentTask.Wait();

                var childTask = navigationPropertyCollection.InvokeMethod<Task>("Add" + Class.Name + "Async", args: new object[] { childEntity, false });

                childTask.Wait();

                childEntity.ValidatePropertyValues(childKeyValues);
            }
        }

        [Fact]
        public void When_the_entity_is_not_null_and_the_property_path_is_not_a_single_segment_then_AddAsync_POSTs_to_the_Entitys_canoncial_Uri_property_and_updates_the_added_instance()
        {
            var parentKeyValues = Class.GetSampleKeyArguments().ToArray();
            var navPropertyName = Class.NavigationProperties().First(p => p.Type == Class && p.IsCollection).Name;
            var navPropertyPath = string.Format("{0}/{1}", TargetEntity.Class.GetDefaultEntityPath(parentKeyValues), navPropertyName);
            var childKeyValues = Class.GetSampleKeyArguments().ToArray();

            using (_serviceMock = new MockService()
                    .SetupPostEntity(TargetEntity, parentKeyValues)
                    .OnPostEntityRequest(navPropertyPath)
                    .RespondWithCreateEntity(TargetEntity.Class.GetDefaultEntitySetName(), ConcreteType.Initialize(childKeyValues)))
            {
                var parentEntity = Activator.CreateInstance(ConcreteType);
                var childEntity = Activator.CreateInstance(ConcreteType);

                var entitySetCollection = _serviceMock
                    .GetDefaultContext(Model)
                    .CreateCollection(CollectionType, ConcreteType, TargetEntity.Class.GetDefaultEntitySetPath());

                var navigationPropertyCollection = entitySetCollection
                    .Context
                    .CreateCollection(CollectionType, ConcreteType, Any.String() + "/" + navPropertyName, parentEntity);

                var parentTask = entitySetCollection.InvokeMethod<Task>("Add" + Class.Name + "Async", args: new[] { parentEntity, false });

                parentTask.Wait();

                var childTask = navigationPropertyCollection.InvokeMethod<Task>("Add" + Class.Name + "Async", args: new object[] { childEntity, false });

                childTask.Wait();

                childEntity.ValidatePropertyValues(childKeyValues);
            }
        }

        [Fact]
        public void When_dont_save_is_true_then_the_server_is_not_called_until_Context_SaveChangesAsync_is_invoked()
        {
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_serviceMock = new MockService()
                    .SetupPostEntity(TargetEntity, keyValues))
            {
                var collection = _serviceMock
                    .GetDefaultContext(Model)
                    .CreateCollection(CollectionType, ConcreteType, TargetEntity.Class.GetDefaultEntitySetPath());

                var instance = Activator.CreateInstance(ConcreteType);

                var task = collection.InvokeMethod<Task>("Add" + Class.Name + "Async", args: new[] { instance, true });

                task.Wait();

                foreach (var keyValue in keyValues)
                {
                    instance.GetPropertyValue(keyValue.Item1)
                        .Should().Be(null);
                }
                
                task = collection.Context.SaveChangesAsync();

                task.Wait();

                instance.ValidatePropertyValues(keyValues);
            }
        }
    }
}
