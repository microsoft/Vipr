// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Its.Recipes;
using ODataV4TestService.SelfHost;
using Vipr.Core;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_AddAsync_Method : EntityTestBase
    {
        private IStartedScenario _serviceMock;

        public Given_an_OdcmClass_Entity_Collection_AddAsync_Method()
        {
            Init();
        }

        [Fact]
        public void When_the_entity_is_null_then_AddAsync_POSTs_to_the_EntitySet_and_updates_the_added_instance()
        {
            var entitySetName = Any.CSharpIdentifier(1);

            var entitySetPath = string.Format("/{0}", entitySetName);

            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_serviceMock = new MockScenario()
                    .Setup(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == entitySetPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 201;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, keyValues));
                           })
                    .Start())
            {
                var collection = _serviceMock
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true)
                    .WithDefaultResolvers(Class.Namespace)
                    .WithIgnoreMissingProperties()
                    .CreateCollection(CollectionType, ConcreteType, entitySetPath);

                var instance = Activator.CreateInstance(ConcreteType);

                var task = collection.InvokeMethod<Task>("Add" + Class.Name + "Async", args: new[] { instance, false });

                task.Wait();

                instance.ValidatePropertyValues(keyValues);
            }
        }

        [Fact]
        public void When_the_entity_is_not_null_then_AddAsync_POSTs_to_the_Entitys_canoncial_Uri_property_and_updates_the_added_instance()
        {

            var entitySetName = Any.CSharpIdentifier(1);
            var entitySetPath = string.Format("/{0}", entitySetName);
            var parentKeyValues = Class.GetSampleKeyArguments().ToArray();
            var parentCanonicalPath = String.Format("{0}({1})", Class.Name + "s",
                ODataKeyPredicate.AsString(parentKeyValues));
            var navPropertyName = Class.NavigationProperties().First(p => p.Type == Class && p.IsCollection).Name;
            var navPropertyPath = string.Format("/{0}/{1}", parentCanonicalPath, navPropertyName);
            var childKeyValues = Class.GetSampleKeyArguments().ToArray();

            using (_serviceMock = new MockScenario()
                    .Setup(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == entitySetPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 201;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, parentKeyValues));
                           })
                    .Setup(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == navPropertyPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 201;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, childKeyValues));
                           })
                    .Start())
            {
                var parentEntity = Activator.CreateInstance(ConcreteType);
                var childEntity = Activator.CreateInstance(ConcreteType);

                var entitySetCollection = _serviceMock
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true)
                    .WithDefaultResolvers(Class.Namespace)
                    .WithIgnoreMissingProperties()
                    .CreateCollection(CollectionType, ConcreteType, entitySetPath);

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
        public void When_the_entity_is_not_null_and_the_property_path_is_not_a_single_segmentthen_AddAsync_POSTs_to_the_Entitys_canoncial_Uri_property_and_updates_the_added_instance()
        {

            var entitySetName = Any.CSharpIdentifier(1);
            var entitySetPath = string.Format("/{0}", entitySetName);
            var parentKeyValues = Class.GetSampleKeyArguments().ToArray();
            var parentCanonicalPath = String.Format("{0}({1})", Class.Name + "s",
                ODataKeyPredicate.AsString(parentKeyValues));
            var navPropertyName = Class.NavigationProperties().First(p => p.Type == Class && p.IsCollection).Name;
            var navPropertyPath = string.Format("/{0}/{1}", parentCanonicalPath, navPropertyName);
            var childKeyValues = Class.GetSampleKeyArguments().ToArray();

            using (_serviceMock = new MockScenario()
                    .Setup(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == entitySetPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 201;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, parentKeyValues));
                           })
                    .Setup(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == navPropertyPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 201;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, childKeyValues));
                           })
                    .Start())
            {
                var parentEntity = Activator.CreateInstance(ConcreteType);
                var childEntity = Activator.CreateInstance(ConcreteType);

                var entitySetCollection = _serviceMock
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true)
                    .WithDefaultResolvers(Class.Namespace)
                    .WithIgnoreMissingProperties()
                    .CreateCollection(CollectionType, ConcreteType, entitySetPath);

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
            var entitySetName = Any.CSharpIdentifier(1);
            var entitySetPath = string.Format("/{0}", entitySetName);
            var keyValues = Class.GetSampleKeyArguments().ToArray();
            var acceptPost = false;

            using (_serviceMock = new MockScenario()
                    .Setup(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == entitySetPath &&
                                acceptPost,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 201;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, keyValues));
                           })
                    .Start())
            {
                var collection = _serviceMock
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true)
                    .WithDefaultResolvers(Class.Namespace)
                    .WithIgnoreMissingProperties()
                    .CreateCollection(CollectionType, ConcreteType, entitySetPath);

                var instance = Activator.CreateInstance(ConcreteType);

                var task = collection.InvokeMethod<Task>("Add" + Class.Name + "Async", args: new[] { instance, true });

                task.Wait();

                foreach (var keyValue in keyValues)
                {
                    instance.GetPropertyValue(keyValue.Item1)
                        .Should().Be(null);
                }

                acceptPost = true;

                task = collection.Context.SaveChangesAsync();

                task.Wait();

                instance.ValidatePropertyValues(keyValues);
            }
        }
    }
}
