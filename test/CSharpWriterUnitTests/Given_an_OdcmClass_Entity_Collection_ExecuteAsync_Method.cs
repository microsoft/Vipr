// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Its.Recipes;
using ODataV4TestService.SelfHost;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_ExecuteAsync_Method : EntityTestBase
    {
        private IStartedScenario _serviceMock;

        public Given_an_OdcmClass_Entity_Collection_ExecuteAsync_Method()
        {
            Init();
        }

        [Fact]
        public void When_ExecuteAsync_is_called_it_GETs_the_collection_by_name()
        {
            var entitySetName = Any.CSharpIdentifier();
            var entitySetPath = "/" + entitySetName;

            using (_serviceMock = new MockScenario()
                    .Setup(c => c.Request.Method == "GET" && c.Request.Path.Value == entitySetPath,
                           c => c.Response.StatusCode = 200)
                    .Start())
            {
                var context = _serviceMock.GetContext()
                    .UseJson(Model.ToEdmx(), true);

                var collection = context.CreateCollection(CollectionType, ConcreteType, entitySetName);

                var task = collection.ExecuteAsync();
                task.Wait();
            }
        }
    }
}
