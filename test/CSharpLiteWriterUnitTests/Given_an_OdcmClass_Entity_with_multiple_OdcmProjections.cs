// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System.Linq;
using System.Linq.Expressions;
using Vipr.Core.CodeModel;
using Vipr.Writer.CSharp.Lite;
using Xunit;
using Type = System.Type;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_with_multiple_OdcmProjections : EntityTestBase
    {        
        private Dictionary<OdcmProjection, Tuple<Type, Type>> m_projectionFetcherCollectionMap =
            new Dictionary<OdcmProjection, Tuple<Type, Type>>();
        
        public Given_an_OdcmClass_Entity_with_multiple_OdcmProjections()
        {
            IEnumerable<OdcmProjection> projections = null;
            OdcmClass targetClass = null;
            base.Init(
                m =>
                {
                    targetClass = m.Namespaces[0].Classes.First();
                    targetClass.Properties.Add(Any.PrimitiveOdcmProperty(p => p.Class = Class));
                    projections = targetClass.AnyOdcmProjections().Distinct().ToList();
                    foreach (var projection in projections)
                    {
#if false
                        if (!targetClass.Projections.Contains(projection))
                        {
                            targetClass.Projections.Add(projection);
                        }
#else
                        targetClass.AddProjection(projection.Capabilities);
#endif
                    }
                });            

            foreach (var projection in projections)
            {
                var identifier = NamesService.GetFetcherInterfaceName(targetClass, projection);
                var fetcher = Proxy.GetInterface(targetClass.Namespace, identifier.Name);

                identifier = NamesService.GetCollectionInterfaceName(targetClass, projection);
                var collection = Proxy.GetInterface(targetClass.Namespace, identifier.Name);

                var fetcherCollectionTuple = new Tuple<Type, Type>(fetcher, collection);
                m_projectionFetcherCollectionMap.Add(projection, fetcherCollectionTuple);
            }
        }

        [Fact]
        public void The_fetcher_proxy_class_implements_multiple_fetcher_interfaces()
        {
            foreach (var projectionFetcherPair in m_projectionFetcherCollectionMap)
            {
                var fetcherInterface = projectionFetcherPair.Value.Item1;
                FetcherType
                    .Should()
                    .Implement(fetcherInterface,
                        "Because the implementation is internal and only accessible via the interface.");
            }
        }

        [Fact]
        public void The_collection_proxy_class_implements_multiple_collection_interfaces()
        {
            foreach (var projectionCollectionPair in m_projectionFetcherCollectionMap)
            {
                var collectionInterface = projectionCollectionPair.Value.Item2;
                CollectionType
                    .Should()
                    .Implement(collectionInterface,
                        "Because the implementation is internal and only accessible via the interface.");
            }
        }

        [Fact]
        public void The_fetcher_proxy_class_explicitly_implements_multiple_fetcher_interface_Expand_methods()
        {
            foreach (var projectionFetcherPair in m_projectionFetcherCollectionMap)
            {
                if (!projectionFetcherPair.Key.SupportsExpand())
                {
                    continue;
                }

                var fetcherInterface = projectionFetcherPair.Value.Item1;

                var expandMethod =
                    FetcherType.GetInterfaceMap(fetcherInterface).TargetMethods.Single(m => m.Name.EndsWith("Expand"));

                expandMethod.Should().NotBeNull("Because it allows expanding");                

                expandMethod.GetGenericArguments().Count()
                    .Should().Be(1);

                var genericArgType = expandMethod.GetGenericArguments()[0];

                var expectedParamType =
                    typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(ConcreteInterface, genericArgType));

                expandMethod.GetParameters().Count()
                    .Should().Be(1);

                expandMethod.GetParameters()[0].ParameterType
                    .Should().Be(expectedParamType);

                expandMethod.ReturnType
                    .Should().Be(fetcherInterface);
            }
        }

        [Fact]
        public void The_collection_proxy_class_explicitly_implements_multiple_collection_interface_GetById_methods()
        {
            foreach (var projectionCollectionPair in m_projectionFetcherCollectionMap)
            {
                var fetcherInterface = projectionCollectionPair.Value.Item1;
                var collectionInterface = projectionCollectionPair.Value.Item2;
                CollectionType.Should().HaveExplicitMethod(
                    collectionInterface, 
                    "GetById", 
                    fetcherInterface, 
                    GetKeyPropertyTypes());
            }
        }

        [Fact]
        public void The_collection_proxy_class_explicitly_implements_multiple_collection_interface_GetById_indexers()
        {
            foreach (var projectionCollectionPair in m_projectionFetcherCollectionMap)
            {
                var fetcherInterface = projectionCollectionPair.Value.Item1;
                var collectionInterface = projectionCollectionPair.Value.Item2;
                CollectionType.Should().HaveExplicitMethod(
                    collectionInterface,
                    "get_Item",
                    fetcherInterface,
                    GetKeyPropertyTypes());
            }
        }        

        private Type[] GetKeyPropertyTypes()
        {
            return Class.Key
                .Select(p => p.Type)
                .Select(t => Proxy.GetClass(t.Namespace, t.Name))
                .ToArray();
        }
        
    }
}
