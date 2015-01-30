// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Its.Recipes;
using Microsoft.OData.ProxyExtensions;
using Moq;
using Moq.Protected;
using Xunit;

namespace CSharpWriterUnitTests
{
    public abstract class Given_an_OdcmClass_Entity_Collection_GetById_Base : EntityTestBase
    {
        protected Mock<DataServiceContextWrapper> DscwMock;
        private IEnumerable<object> _instances;
        private object _instance;
        protected List<Tuple<string, object>> Params;
        protected string InstancePath = Any.String(1);

        
        public void Init()
        {
            Init(null, true);

            _instances = Any.Sequence(x => ConstructConcreteInstance()).ToList();

            _instance = _instances.RandomElement();

            DscwMock = new Mock<DataServiceContextWrapper>(MockBehavior.Strict);

            Params = new List<Tuple<string, object>>();

            foreach (var keyProperty in ConcreteType.GetKeyProperties())
            {
                foreach (var instance in _instances)
                {
                    instance.GetType().GetProperty(keyProperty.Name).SetValue(instance, Any.String(4));
                }

                Params.Add(new Tuple<string, object>(keyProperty.Name, keyProperty.GetValue(_instance)));
            }

            ConfigureCollectionInstance();
        }

        protected void ConfigureCollectionInstance()
        {
            CollectionType.GetField("_context", PermissiveBindingFlags).SetValue(CollectionInstance, DscwMock.Object);
        }

        protected override void ConfigureCollectionMock<TCollection, TInstance, TIInstance>(Mock<TCollection> mock)
        {
            mock.Protected().Setup<string>("GetPath", ItExpr.Is<Expression<Func<TIInstance, bool>>>(e => FiltersCorrectly(e))).Returns(InstancePath);
        }

        private bool FiltersCorrectly<TInstance>(Expression<Func<TInstance, bool>> filterExpression)
        {
            var queryableInstances = _instances.Select(i => (TInstance)i).AsQueryable();

            var filteredInstances = queryableInstances.Where(filterExpression);

            return (filteredInstances.Count() == 1 && filteredInstances.First().Equals((TInstance)_instance));
        }

        private object ConstructConcreteInstance()
        {
            return Activator.CreateInstance(ConcreteType);
        }
    }
}
