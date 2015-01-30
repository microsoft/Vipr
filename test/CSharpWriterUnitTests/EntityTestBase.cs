// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Its.Recipes;
using System;
using System.Reflection;
using Vipr.Core.CodeModel;
using Microsoft.OData.Client;
using Microsoft.OData.ProxyExtensions;
using Moq;

namespace CSharpWriterUnitTests
{
    public class EntityTestBase : CodeGenTestBase
    {
        protected OdcmModel Model;
        protected OdcmNamespace Namespace;
        protected OdcmClass Class;
        protected Assembly Proxy;
        protected Type ConcreteType;
        protected Type ConcreteInterface;
        protected Type FetcherType;
        protected Type FetcherInterface;
        protected Type CollectionType;
        protected Type CollectionInterface;
        protected object CollectionInstance;
        protected OdcmClass OdcmContainer;
        protected Type EntityContainerType;
        protected Type EntityContainerInterface;

        public void Init(Action<OdcmModel> config = null, bool generateMocks = false)
        {
            Model = new OdcmModel(Any.ServiceMetadata());

            Namespace = Any.EmptyOdcmNamespace();

            Model.Namespaces.Add(Namespace);

            Class = Any.EntityOdcmClass(Namespace);

            Model.AddType(Class);

            OdcmContainer = Any.ServiceOdcmClass(Namespace);

            Model.AddType(OdcmContainer);

            if (config != null) config(Model);

            Proxy = GetProxy(Model, null, generateMocks ? new[] { "DynamicProxyGenAssembly2" } : null);

            ConcreteType = Proxy.GetClass(Class.Namespace, Class.Name);

            ConcreteInterface = Proxy.GetInterface(Class.Namespace, "I" + Class.Name);

            FetcherType = Proxy.GetClass(Class.Namespace, Class.Name + "Fetcher");

            FetcherInterface = Proxy.GetInterface(Class.Namespace, "I" + Class.Name + "Fetcher");

            CollectionType = Proxy.GetClass(Class.Namespace, Class.Name + "Collection");

            CollectionInterface = Proxy.GetInterface(Class.Namespace, "I" + Class.Name + "Collection");

            EntityContainerType = Proxy.GetClass(Model.EntityContainer.Namespace, Model.EntityContainer.Name);

            EntityContainerInterface = Proxy.GetInterface(Model.EntityContainer.Namespace, "I" + Model.EntityContainer.Name);

            if (generateMocks)
            {
                CollectionInstance = ConstructCollectionInstance();
            }
        }

        private object ConstructCollectionInstance(DataServiceQuery inner = null, DataServiceContextWrapper context = null,
            object entity = null, string path = null)
        {
            var mock = typeof(Mock<>)
                .MakeGenericType(CollectionType)
                .GetConstructor(PermissiveBindingFlags, null, new[] { typeof(MockBehavior), typeof(object[]) }, null)
                .Invoke(new object[] { MockBehavior.Default, new object[] { inner, context, entity, path } });

            mock.GetType()
                .GetProperty("CallBase")
                .SetValue(mock, true);

            this.GetType()
                .GetMethods(PermissiveBindingFlags)
                .Where(m => m.Name.Equals("ConfigureCollectionMock"))
                .First(m => m.IsGenericMethod)
                .MakeGenericMethod(CollectionType, ConcreteType, ConcreteInterface)
                .Invoke(this, PermissiveBindingFlags, null, new[] { mock }, null);

            return mock.GetType()
                .GetProperties()
                .Where(p => p.Name == "Object")
                .First(p => p.PropertyType == CollectionType)
                .GetValue(mock);
        }

        protected virtual void ConfigureCollectionMock<TCollection, TInstance, TIInstance>(Mock<TCollection> mock)
            where TCollection : QueryableSet<TIInstance>
            where TInstance : EntityBase, TIInstance
            where TIInstance : class
        { }
    }
}
