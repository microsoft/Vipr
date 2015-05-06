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
    }
}
