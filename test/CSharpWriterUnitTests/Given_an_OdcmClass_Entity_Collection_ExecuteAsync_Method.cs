// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.OData.ProxyExtensions;
using Moq;
using Moq.Protected;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_ExecuteAsync_Method : EntityTestBase
    {
        private MethodInfo _executeAsyncMethod;
        private object _executeAsyncResult;

        
        public Given_an_OdcmClass_Entity_Collection_ExecuteAsync_Method()
        {
            Init(null, true);

            _executeAsyncMethod = CollectionInterface.GetMethod("ExecuteAsync",
                PermissiveBindingFlags,
                null,
                new Type[0],
                null);
        }

        [Fact]
        public void It_returns_result_of_ExecuteAsyncInternal()
        {
            CallExecuteAsyncMethod(CollectionInstance).Should().Be(_executeAsyncResult);
        }

        protected override void ConfigureCollectionMock<TCollection, TInstance, TIInstance>(Mock<TCollection> mock)
        {
            var result = Task.FromResult(new Mock<IPagedCollection<TIInstance>>().Object);

            _executeAsyncResult = result;

            mock.Protected()
                .Setup<Task<IPagedCollection<TIInstance>>>("ExecuteAsyncInternal")
                .Returns(result);
        }

        protected object CallExecuteAsyncMethod(object collectionInstance)
        {
            return _executeAsyncMethod.Invoke(CollectionInstance, new object[0]);
        }
    }
}
