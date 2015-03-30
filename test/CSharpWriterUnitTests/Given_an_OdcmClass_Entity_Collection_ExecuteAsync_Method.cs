// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.MockService;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_ExecuteAsync_Method : EntityTestBase
    {
        private MockService _serviceMock;

        public Given_an_OdcmClass_Entity_Collection_ExecuteAsync_Method()
        {
            Init();
        }

        [Fact]
        public void When_ExecuteAsync_is_called_it_GETs_the_collection_by_name()
        {
            using (_serviceMock = new MockService()
                    .SetupGetEntitySet(TargetEntity))
            {
                var context = _serviceMock
                    .GetDefaultContext(Model);

                var collection = context.CreateCollection(CollectionType, ConcreteType, Class.GetDefaultEntitySetPath());

                var task = collection.ExecuteAsync();
                task.Wait();
            }
        }
    }
}
