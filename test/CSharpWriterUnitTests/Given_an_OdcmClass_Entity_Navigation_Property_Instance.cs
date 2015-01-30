// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Navigation_Property_Instance : NavigationPropertyTestBase
    {
        
        public Given_an_OdcmClass_Entity_Navigation_Property_Instance()
        {
            _navigationProperty = Any.OdcmProperty(p => p.Type = Class);

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
                        IsCollection = false
                    };
                });

                m.Namespaces[0].Classes.First().Properties.Add(_navigationProperty);
            });
        }

        [Fact]
        public void The_Concrete_class_exposes_a_ConcreteType_property()
        {
            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                _navTargetConcreteType,
                _navigationProperty.Name,
                "Because Entity types should be accessible through their related Entity types.");
        }

        [Fact]
        public void The_Concrete_class_explicitly_implements_ConcreteInterface_Interface_property()
        {
            ConcreteType.Should().HaveExplicitProperty(
                ConcreteInterface,
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                _navTargetConcreteInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_class_explicitly_implements_readonly_FetcherInterface_FetcherInterface_property()
        {
            ConcreteType.Should().HaveExplicitProperty(
                FetcherInterface,
                CSharpAccessModifiers.Public,
                null,
                _navTargetFetcherInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Concrete_interface_exposes_a_ConcreteInterface_property()
        {
            ConcreteInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                _navTargetConcreteInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Fetcher_interface_exposes_a_readonly_FetcherInterface_property()
        {
            FetcherInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _navTargetFetcherInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Fetcher_class_exposes_a_readonly_Fetcher_Interface_property()
        {
            FetcherType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _navTargetFetcherInterface,
                _navigationProperty.Name);
        }

        [Fact]
        public void The_Collection_interface_does_not_expose_it()
        {
            CollectionInterface.Should().NotHaveProperty(_navigationProperty.Name);
        }

        [Fact]
        public void The_Collection_class_does_not_expose_it()
        {
            CollectionType.Should().NotHaveProperty(_navigationProperty.Name);
        }
    }
}
