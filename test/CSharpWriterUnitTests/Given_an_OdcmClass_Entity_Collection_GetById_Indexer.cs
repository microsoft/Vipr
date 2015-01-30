// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.OData.ProxyExtensions;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_GetById_Indexer : Given_an_OdcmClass_Entity_Collection_GetById_Base
    {
        private PropertyInfo _getByIdIndexer;

        
        public Given_an_OdcmClass_Entity_Collection_GetById_Indexer()
        {
            base.Init();

            _getByIdIndexer = CollectionInterface.GetProperty("Item",
                PermissiveBindingFlags,
                null,
                FetcherInterface,
                ConcreteType.GetKeyProperties()
                    .Select(p => p.PropertyType)
                    .ToArray(), null);
        }

        [Fact]
        public void It_returns_a_Fetcher_with_the_right_Context_and_Path()
        {
            var fetcher = CallGetByIdIndexer(CollectionInstance, Params.Select(p => p.Item2));

            fetcher.Context.Should().Be(DscwMock.Object);

            FetcherType.BaseType.GetField("_path", PermissiveBindingFlags).GetValue(fetcher)
                .Should().Be(InstancePath);
        }

        protected RestShallowObjectFetcher CallGetByIdIndexer(object collectionInstance, IEnumerable<object> parameters)
        {
            return _getByIdIndexer.GetValue(collectionInstance, parameters.ToArray()) as RestShallowObjectFetcher;
        }
    }
}
