// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Vipr.Core.CodeModel;
using Vipr.Writer.CSharp.Lite;
using Type = System.Type;

namespace CSharpLiteWriterUnitTests
{
    /// <summary>
    /// Summary description for Given_an_OdcmModel
    /// </summary>    
    public class NavigationPropertyTestBase : EntityTestBase
    {
        protected OdcmProperty NavigationProperty;

        protected OdcmEntityClass NavTargetClass;
        protected EntityArtifacts NavTargetEntity;
        protected Type NavTargetConcreteType;
        protected Type NavTargetConcreteInterface;
        protected Type NavTargetFetcherType;
        protected Type NavTargetFetcherInterface;
        protected Type NavTargetCollectionType;
        protected Type NavTargetCollectionInterface;

        public void Init(Action<OdcmModel> config)
        {
            base.Init(config);

            NavTargetConcreteType = Proxy.GetClass(NavTargetClass.Namespace, NavTargetClass.Name);

            NavTargetConcreteInterface = Proxy.GetInterface(NavTargetClass.Namespace, "I" + NavTargetClass.Name);

            NavTargetFetcherType = Proxy.GetClass(NavTargetClass.Namespace, NavTargetClass.Name + "Fetcher");

            var identifier = NamesService.GetFetcherInterfaceName(NavTargetClass);
            NavTargetFetcherInterface = Proxy.GetInterface(NavTargetClass.Namespace, identifier.Name);

            NavTargetCollectionType = Proxy.GetClass(NavTargetClass.Namespace, NavTargetClass.Name + "Collection");

            identifier = NamesService.GetCollectionInterfaceName(NavTargetClass);
            NavTargetCollectionInterface = Proxy.GetInterface(NavTargetClass.Namespace, identifier.Name);

            NavTargetEntity = new EntityArtifacts()
            {
                Class = NavTargetClass,
                ConcreteType = NavTargetConcreteType,
                ConcreteInterface = NavTargetConcreteInterface,
                FetcherType = NavTargetFetcherType,
                FetcherInterface = NavTargetFetcherInterface,
                CollectionType = NavTargetCollectionType,
                CollectionInterface = NavTargetCollectionInterface
            };
        }
    }
}
