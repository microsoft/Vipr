// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.AccessControl;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.OData.ProxyExtensions.Lite;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Fetcher : EntityTestBase
    {
        
        public Given_an_OdcmClass_Entity_Fetcher()
        {
            base.Init(m => m.Namespaces[0].Classes.First().Properties.Add(Any.PrimitiveOdcmProperty(p => p.Class = Class)));
        }

        [Fact]
        public void The_Collection_class_is_Internal()
        {
            FetcherType.IsInternal()
                .Should().BeTrue("Because entity types are accessed by the Concrete, Fetcher, " +
                                 "and Collection interfaces they implement.");
        }

        [Fact]
        public void The_fetcher_proxy_class_inherits_from_RestShallowObjectFetcher()
        {
            FetcherType.Should().BeDerivedFrom(
                typeof(RestShallowObjectFetcher),
                "Because it manages access to the Uri to this logical entity relative to the query.");
        }

        [Fact]
        public void The_fetcher_proxy_class_implements_the_Fetcher_Interface()
        {
            FetcherType.Should().Implement(
                FetcherInterface,
                "Because the implementaiton is internal and only accessible via the interface.");
        }

        [Fact]
        public void The_Fetcher_class_implements_a_new_async_ExecuteAsync_method()
        {
            FetcherType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                true,
                typeof(Task<>).MakeGenericType(ConcreteInterface),
                "ExecuteAsync",
                new System.Type[0],
                "Because it allows executing queries.");
        }

        [Fact]
        public void The_Fetcher_class_implements_an_UpdateAsync_method()
        {
            FetcherType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                false,
                typeof(Task),
                "UpdateAsync",
                new System.Type[] { ConcreteInterface, typeof(bool) },
                "Because it allows updating entities.");
        }

        [Fact]
        public void The_Fetcher_class_implements_a_DeleteAsync_method()
        {
            FetcherType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                false,
                typeof(Task),
                "DeleteAsync",
                new System.Type[] { ConcreteInterface, typeof(bool) },
                "Because it allows deleting entities.");
        }

        [Fact]
        public void The_Fetcher_class_implements_a_SaveChangesAsync_method()
        {
            FetcherType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                false,
                typeof(Task),
                "SaveChangesAsync",
                new System.Type[] { typeof(bool), typeof(Microsoft.OData.Client.SaveChangesOptions) },
                "Because it allows saving changes made to entities.");
        }

        [Fact]
        public void The_Fetcher_class_implements_an_UpdateLinkAsync_method()
        {
            FetcherType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                false,
                typeof(Task),
                "UpdateLinkAsync",
                new System.Type[] { typeof(object), ConcreteInterface, typeof(bool) },
                "Because it allows updating relationship between entities.");
        }

        [Fact]
        public void The_Fetcher_class_implements_a_DeleteLinkAsync_method()
        {
            FetcherType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                false,
                typeof(Task),
                "DeleteLinkAsync",
                new System.Type[] { ConcreteInterface, typeof(bool) },
                "Because it allows deleting relationship between entities.");
        }

        [Fact]
        public void The_Fetcher_class_implements_an_SetAsync_method()
        {
            FetcherType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                false,
                typeof(Task),
                "SetAsync",
                new System.Type[] {typeof(object), ConcreteInterface, typeof(bool) },
                "Because it allows creating new entities.");
        }
    }
}
