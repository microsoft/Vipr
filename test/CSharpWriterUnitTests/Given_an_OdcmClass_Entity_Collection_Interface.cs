// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CSharpWriter;
using FluentAssertions;
using Microsoft.OData.ProxyExtensions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Type = System.Type;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_Interface : EntityTestBase
    {
        
        public Given_an_OdcmClass_Entity_Collection_Interface()
        {
            base.Init();
        }

        [Fact]
        public void It_is_Public()
        {
            CollectionInterface.IsPublic
                .Should().BeTrue("Because it allows users to interact with the Collection Class and with" +
                                 "Collection behaviors on the Concrete class.");
        }

        [Fact]
        public void It_implements_IReadOnlyQueryableSetBaseofT()
        {
            CollectionInterface
                .Should().Implement(
                    typeof(IReadOnlyQueryableSetBase<>).MakeGenericType(ConcreteInterface),
                    "Because it implements IReadOnlyQueryableSetBase<T> which empowers it" +
                    "with Linq expressions like Where, OrderBy, Expand, Select," +
                    "Skip, and Take.");
        }

        [Fact]
        public void It_is_decorated_with_LowerCasePropertyAttribute()
        {
            ConcreteInterface.Should()
                .BeDecoratedWith<LowerCasePropertyAttribute>();
        }

        [Fact]
        public void It_exposes_a_GetById_method()
        {
            CollectionInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                FetcherInterface,
                "GetById",
                GetKeyPropertyTypes(),
                "Because it allows retrieving an instance by key");
        }

        [Fact]
        public void It_exposes_a_GetById_Indexer()
        {
            CollectionInterface.Should().HaveIndexer(
                CSharpAccessModifiers.Public,
                null,
                FetcherInterface,
                GetKeyPropertyTypes(),
                "Because it allows retrieving an instance by key");
        }

        [Fact]
        public void It_exposes_an_ExecuteAsync_method()
        {
            CollectionInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                typeof(Task<>).MakeGenericType(typeof(IPagedCollection<>).MakeGenericType(ConcreteInterface)),
                "ExecuteAsync", new Type[] { },
                "Because it allows executing the query.");
        }

        [Fact]
        public void It_exposes_an_AddAsync_method()
        {
            CollectionInterface.Should()
                .HaveMethod(
                    CSharpAccessModifiers.Public,
                    typeof(Task), 
                    "Add" + Class.Name + "Async",
                    new[] { ConcreteInterface, typeof(bool) },
                    "Because it allows adding elements to the Entity Set.");
        }

        [Fact]
        public void It_has_a_CountAsync_method()
        {
            CollectionInterface.Should()
                .HaveMethod(
                    CSharpAccessModifiers.Public,
                    typeof(Task<long>), 
                    "CountAsync",
                    new Type[0],
                    "Because it allows adding elements to the Entity Set.");
        }

        private Type[] GetKeyPropertyTypes()
        {
            return Class.Key
                .Select(p => p.Type)
                .Select(t => Proxy.GetClass(t.Namespace, t.Name))
                .ToArray();
        }
    }
}
