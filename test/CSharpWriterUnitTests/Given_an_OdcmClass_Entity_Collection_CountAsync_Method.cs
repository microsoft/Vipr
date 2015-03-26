// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_CountAsync_Method : EntityTestBase
    {
        private MockService _serviceMock;

        public Given_an_OdcmClass_Entity_Collection_CountAsync_Method()
        {
            Init();
        }

        [Fact]
        public void When_CountAsync_is_called_it_GETs_the_collection_by_name()
        {
            var expectedCount = Any.Long();

            using (_serviceMock = new MockService()
                        .OnGetEntityCountRequest(TargetEntity.Class.GetDefaultEntitySetPath())
                        .RespondWithODataText(expectedCount.ToString()))
            {
                var context = _serviceMock
                    .GetDefaultContext(Model);

                var collection = context.CreateCollection(CollectionType, ConcreteType, Class.GetDefaultEntitySetPath());

                var task = collection.InvokeMethod<Task<long>>("CountAsync");

                task.Result
                    .Should().Be(expectedCount);
            }
        }
    }
}
