// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.Its.Recipes;
using Microsoft.OData.ProxyExtensions;
using ODataV4TestService.SelfHost;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_GetById_Method : EntityTestBase
    {
        private IStartedScenario _serviceMock;


        public Given_an_OdcmClass_Entity_Collection_GetById_Method()
        {
            Init();
        }

        [Fact]
        public void When_GetById_is_called_it_GETs_the_collection_by_name_and_passes_the_id_in_the_path()
        {
            var entitySetname = Any.CSharpIdentifier(1);
            var keyValues = Class.GetSampleKeyArguments().ToArray();
            var keyPredicate = ODataKeyPredicate.AsString(keyValues);
            var entityPath = string.Format("/{0}({1})", entitySetname, keyPredicate);

            using (_serviceMock = new MockScenario()
                    .Setup(c => c.Request.Method == "GET" && c.Request.Path.Value == entityPath,
                           c => c.Response.StatusCode = 200)
                    .Start())
            {
                var context = _serviceMock.GetContext()
                    .UseJson(Model.ToEdmx(), true);

                var collection = context.CreateCollection(CollectionType, ConcreteType, entitySetname);

                var fetcher = collection.InvokeMethod<RestShallowObjectFetcher>("GetById",
                    args: keyValues.Select(k => k.Item2).ToArray());

                var task = fetcher.ExecuteAsync();
                task.Wait();
            }
        }
    }
}
