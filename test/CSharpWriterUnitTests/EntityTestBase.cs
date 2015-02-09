// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using Microsoft.Its.Recipes;
using System;
using System.Reflection;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriterUnitTests
{
    public class EntityTestBase : CodeGenTestBase
    {
        protected OdcmModel Model;
        protected OdcmNamespace Namespace;
        protected OdcmClass Class;
        protected Assembly Proxy;
        protected EntityArtifacts TargetEntity;
        protected Type ConcreteType;
        protected Type ConcreteInterface;
        protected Type FetcherType;
        protected Type FetcherInterface;
        protected Type CollectionType;
        protected Type CollectionInterface;
        protected OdcmClass OdcmContainer;
        protected Type EntityContainerType;
        protected Type EntityContainerInterface;
        protected IConfigurationProvider ConfigurationProvider;

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

            Proxy = GetProxy(Model, ConfigurationProvider, generateMocks ? new[] { "DynamicProxyGenAssembly2" } : null);

            ConcreteType = Proxy.GetClass(Class.Namespace, Class.Name);

            ConcreteInterface = Proxy.GetInterface(Class.Namespace, "I" + Class.Name);

            FetcherType = Proxy.GetClass(Class.Namespace, Class.Name + "Fetcher");

            FetcherInterface = Proxy.GetInterface(Class.Namespace, "I" + Class.Name + "Fetcher");

            CollectionType = Proxy.GetClass(Class.Namespace, Class.Name + "Collection");

            CollectionInterface = Proxy.GetInterface(Class.Namespace, "I" + Class.Name + "Collection");

            EntityContainerType = Proxy.GetClass(Model.EntityContainer.Namespace, Model.EntityContainer.Name);

            EntityContainerInterface = Proxy.GetInterface(Model.EntityContainer.Namespace, "I" + Model.EntityContainer.Name);

            TargetEntity = new EntityArtifacts()
            {
                Class = Class,
                ConcreteType = ConcreteType,
                ConcreteInterface = ConcreteInterface,
                FetcherType = FetcherType,
                FetcherInterface = FetcherInterface,
                CollectionType = CollectionType,
                CollectionInterface = CollectionInterface
            };
        }
    }
}
