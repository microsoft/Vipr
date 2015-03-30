// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.MockService;
using Microsoft.OData.ProxyExtensions;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_GetById_Indexer : EntityTestBase
    {
        private MockService _serviceMock;

        public Given_an_OdcmClass_Entity_Collection_GetById_Indexer()
        {
            Init();
        }

        [Fact]
        public void When_the_indexer_is_called_it_GETs_the_collection_by_name_and_passes_the_id_in_the_path()
        {
            var keyValues = Class.GetSampleKeyArguments().ToList();

            using (_serviceMock = new MockService()
                    .SetupGetEntity(TargetEntity, keyValues))
            {
                var collection = _serviceMock
                    .GetDefaultContext(Model)
                    .CreateCollection(CollectionType, ConcreteType, Class.GetDefaultEntitySetPath());

                var fetcher = collection.GetIndexerValue<RestShallowObjectFetcher>(keyValues.Select(k => k.Item2).ToArray());

                var task = fetcher.ExecuteAsync();
                task.Wait();
            }
        }
    }
}
