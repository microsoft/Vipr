// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.OData.Client;
using Microsoft.OData.ProxyExtensions;
using Moq;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_AddAsync_Method : EntityTestBase
    {
        private MethodInfo _addAsyncMethod;
        private object _entity;
        private object _itemToAdd;
        private string _entityName;
        private string _path;
        private Mock<DataServiceContextWrapper> _dscwMock;
        private Task<DataServiceResponse> _saveChangesAsyncReturnValue;

        
        public Given_an_OdcmClass_Entity_Collection_AddAsync_Method()
        {
            Init(null, true);

            _addAsyncMethod = CollectionInterface.GetMethod("Add" + Class.Name + "Async",
                PermissiveBindingFlags,
                null,
                new[] { ConcreteInterface, typeof(bool) },
                null);

            _entity = new object();

            _itemToAdd = ConstructConcreteInstance();

            _entityName = Any.AlphanumericString(1);

            _path = Any.String() + "/" + _entityName;

            _dscwMock = new Mock<DataServiceContextWrapper>(MockBehavior.Strict);

            _saveChangesAsyncReturnValue = null;

            ConfigureCollectionInstance();
        }

        [Fact]
        public void When_the_entity_is_null_it_calls_Context_AddObject()
        {
            _entity = null;

            ConfigureCollectionInstance();

            CallAddAsync(CollectionInstance, _itemToAdd, false);

            _dscwMock.Verify(d => d.AddObject(It.IsAny<string>(), It.IsAny<object>()), Times.Once);

            _dscwMock.Verify(d => d.AddRelatedObject(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void When_the_entity_is_not_null_and_path_is_simple_it_calls_Context_AddRelatedObject()
        {
            _path = _entityName;

            CallAddAsync(CollectionInstance, _itemToAdd);

            _dscwMock.Verify(d => d.AddObject(It.IsAny<string>(), It.IsAny<object>()), Times.Never);

            _dscwMock.Verify(d => d.AddRelatedObject(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public void When_the_entity_is_not_null_and_path_is_complex_it_calls_Context_AddRelatedObject()
        {
            CallAddAsync(CollectionInstance, _itemToAdd);

            _dscwMock.Verify(d => d.AddObject(It.IsAny<string>(), It.IsAny<object>()), Times.Never);

            _dscwMock.Verify(d => d.AddRelatedObject(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public void When_dontSave_is_false_it_returns_result_of_Context_SaveChangesAsync()
        {
            CallAddAsync(CollectionInstance, _itemToAdd, false)
                .Should()
                .Be(_saveChangesAsyncReturnValue);

            _dscwMock.Verify(d => d.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void When_dontSave_is_true_it_returns_a_Completed_Task()
        {
            CallAddAsync(CollectionInstance, _itemToAdd, true)
                .IsCompleted.Should().BeTrue();

            _dscwMock.Verify(d => d.SaveChangesAsync(), Times.Never);
        }

        private void ConfigureCollectionInstance()
        {
            _dscwMock.Setup(d => d.AddObject(_path, _itemToAdd));

            _dscwMock.Setup(d => d.AddRelatedObject(_entity, _entityName, _itemToAdd));

            _dscwMock.Setup(d => d.SaveChangesAsync())
                .Returns(_saveChangesAsyncReturnValue = Task.FromResult<DataServiceResponse>(null));

            CollectionType.GetField("_entity", PermissiveBindingFlags).SetValue(CollectionInstance, _entity);

            CollectionType.GetField("_path", PermissiveBindingFlags).SetValue(CollectionInstance, _path);

            CollectionType.GetField("_context", PermissiveBindingFlags).SetValue(CollectionInstance, _dscwMock.Object);
        }

        private Task CallAddAsync(object collectionInstance, object item, bool? dontSave = null)
        {
            return (Task)_addAsyncMethod.Invoke(collectionInstance, new[] { item, dontSave });
        }

        private object ConstructConcreteInstance()
        {
            return ConcreteType.GetConstructor(
                PermissiveBindingFlags,
                null,
                new Type[] { },
                null)
                .Invoke(new object[] { });
        }
    }
}
