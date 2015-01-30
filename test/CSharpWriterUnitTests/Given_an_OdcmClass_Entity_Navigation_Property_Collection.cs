// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.OData.ProxyExtensions;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Navigation_Property_Collection : NavigationPropertyTestBase
    {
        
        public Given_an_OdcmClass_Entity_Navigation_Property_Collection()
        {
            base.Init(m =>
            {
                var @namespace = m.Namespaces[0];
                _navTargetClass = Any.EntityOdcmClass(@namespace);
                @namespace.Types.Add(_navTargetClass);

                var @class = @namespace.Classes.First();
                _navigationProperty = Any.OdcmProperty(p =>
                {
                    p.Class = @class;
                    p.Type = _navTargetClass;
                    p.Field = new OdcmField(Any.CSharpIdentifier())
                    {
                        Class = @class,
                        Type = _navTargetClass,
                        IsCollection = true
                    };
                });

                m.Namespaces[0].Classes.First().Properties.Add(_navigationProperty);
            });
        }

        [Fact]
        public void The_Concrete_class_exposes_a_List_Of_Concrete_Type_property()
        {
            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                typeof(IList<>).MakeGenericType(_navTargetConcreteType),
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Fetcher_interface_exposes_a_readonly_CollectionInterface_property()
        {
            FetcherInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _navTargetCollectionInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Fetcher_class_exposes_a_readonly_CollectionInterface_property()
        {
            FetcherType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _navTargetCollectionInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_class_explicitly_implements_readonly_FetcherInterface_CollectionInterface_property()
        {
            ConcreteType.Should().HaveExplicitProperty(
                FetcherInterface,
                CSharpAccessModifiers.Public,
                null,
                _navTargetCollectionInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_interface_exposes_a_readonly_IPagedCollectionOfConcreteInterface_property()
        {
            ConcreteInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                typeof(IPagedCollection<>).MakeGenericType(_navTargetConcreteInterface),
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_class_explicitly_implements_readonly_ConcreteInterface_IPagedCollectionOfConcreteInterface_property()
        {
            ConcreteType.Should().HaveExplicitProperty(
                ConcreteInterface,
                CSharpAccessModifiers.Public,
                null,
                typeof(IPagedCollection<>).MakeGenericType(_navTargetConcreteInterface),
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Collection_interface_does_not_expose_it()
        {
            CollectionInterface.Should().NotHaveProperty(_navigationProperty.Name);
        }
    }
}
