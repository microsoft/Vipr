// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.MockService;
using Microsoft.OData.ProxyExtensions;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_GetById_Method : EntityTestBase
    {
        private MockService _serviceMock;


        public Given_an_OdcmClass_Entity_Collection_GetById_Method()
        {
            Init();
        }

        [Fact]
        public void When_GetById_is_called_it_GETs_the_collection_by_name_and_passes_the_id_in_the_path()
        {
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_serviceMock = new MockService()
                    .SetupGetEntity(TargetEntity, keyValues)
                    .Start())
            {
                var context = _serviceMock
                    .GetDefaultContext(Model);

                var collection = context.CreateCollection(CollectionType, ConcreteType, Class.GetDefaultEntitySetPath());

                var fetcher = collection.InvokeMethod<RestShallowObjectFetcher>("GetById",
                    keyValues.Select(k => k.Item2).ToArray());

                var task = fetcher.ExecuteAsync();
                task.Wait();
            }
        }
    }
}
