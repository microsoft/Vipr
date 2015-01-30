// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Vipr.Core.CodeModel;

namespace CSharpWriterUnitTests
{
    /// <summary>
    /// Summary description for Given_an_OdcmModel
    /// </summary>    
    public class NavigationPropertyTestBase : EntityTestBase
    {
        protected OdcmProperty _navigationProperty;

        protected OdcmClass _navTargetClass;
        protected Type _navTargetConcreteType;
        protected Type _navTargetConcreteInterface;
        protected Type _navTargetFetcherType;
        protected Type _navTargetFetcherInterface;
        protected Type _navTargetCollectionType;
        protected Type _navTargetCollectionInterface;

        public void Init(Action<OdcmModel> config)
        {
            base.Init(config);

            _navTargetConcreteType = Proxy.GetClass(_navTargetClass.Namespace, _navTargetClass.Name);

            _navTargetConcreteInterface = Proxy.GetInterface(_navTargetClass.Namespace, "I" + _navTargetClass.Name);

            _navTargetFetcherType = Proxy.GetClass(_navTargetClass.Namespace, _navTargetClass.Name + "Fetcher");

            _navTargetFetcherInterface = Proxy.GetInterface(_navTargetClass.Namespace,
                "I" + _navTargetClass.Name + "Fetcher");

            _navTargetCollectionType = Proxy.GetClass(_navTargetClass.Namespace, _navTargetClass.Name + "Collection");

            _navTargetCollectionInterface = Proxy.GetInterface(_navTargetClass.Namespace,
                "I" + _navTargetClass.Name + "Collection");
        }
    }
}
