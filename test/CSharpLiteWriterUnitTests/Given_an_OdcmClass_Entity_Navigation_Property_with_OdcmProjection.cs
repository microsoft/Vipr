// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System.Linq;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Microsoft.OData.ProxyExtensions.Lite;
using Xunit;
using System.Threading.Tasks;
using Vipr.Writer.CSharp.Lite;
using Vipr.Core.CodeModel;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Navigation_Property_with_OdcmProjection : NavigationPropertyTestBase
    {
        private System.Type m_NavTargetFetcherInterface;
        private System.Type m_NavTargetCollectionInterface;
        private OdcmProjection m_NavTargetProjection;
        

        private void Init(bool isCollection)
        {
            base.Init(m =>
            {
                var @namespace = m.Namespaces[0];
                NavTargetClass = Any.OdcmEntityClass(@namespace);
                @namespace.Types.Add(NavTargetClass);

                m_NavTargetProjection = NavTargetClass.AnyOdcmProjection();
#if false
                if (!NavTargetClass.Projections.Contains(m_NavTargetProjection))
                {
                    NavTargetClass.Projections.Add(m_NavTargetProjection);
                }
#else
                NavTargetClass.AddProjection(m_NavTargetProjection.Capabilities);
#endif
                var @class = @namespace.Classes.First();                
                NavigationProperty = Any.OdcmProperty(p =>
                {
                    p.Class = @class;
                    p.Projection = m_NavTargetProjection;
                    p.IsCollection = isCollection;
                });

                m.Namespaces[0].Classes.First().Properties.Add(NavigationProperty);
            });

             var identifier = NamesService.GetFetcherInterfaceName(NavTargetClass, m_NavTargetProjection);
             m_NavTargetFetcherInterface = Proxy.GetInterface(NavTargetClass.Namespace, identifier.Name);

             identifier = NamesService.GetCollectionInterfaceName(NavTargetClass, m_NavTargetProjection);
             m_NavTargetCollectionInterface = Proxy.GetInterface(NavTargetClass.Namespace, identifier.Name);             
        }

        [Fact]
        public void When_single_valued_nav_prop_Then_Fetcher_interface_exposes_a_readonly_property_with_projected_FetcherInterface()
        {
            Init(false);

            FetcherInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                m_NavTargetFetcherInterface,
                NavigationProperty.Name);
        }

        [Fact]
        public void When_single_valued_nav_prop_Then_Fetcher_class_exposes_a_readonly_property_with_projected_FetcherInterface()
        {
            Init(false);

            FetcherType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                m_NavTargetFetcherInterface,
                NavigationProperty.Name);
        }

        [Fact]
        public void When_collection_valued_nav_prop_Then_Fetcher_interface_exposes_a_readonly_property_with_projected_CollectionInterface()
        {
            Init(true);

            FetcherInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                m_NavTargetCollectionInterface,
                NavigationProperty.Name);
        }

        [Fact]
        public void When_collection_valued_nav_prop_Then_Fetcher_class_exposes_a_readonly_property_with_projected_CollectionInterface()
        {
            Init(true);

            FetcherType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                m_NavTargetCollectionInterface,
                NavigationProperty.Name);
        }
    }
}
