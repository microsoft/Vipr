// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_ServiceNavigation_Property_Instance : NavigationPropertyTestBase
    {
        
        public Given_an_OdcmClass_ServiceNavigation_Property_Instance()
        {
            base.Init(m =>
            {
                NavigationProperty = Any.OdcmProperty(p => p.Type = Class);

                var @namespace = m.Namespaces[0];

                NavTargetClass = Any.EntityOdcmClass(@namespace);

                @namespace.Types.Add(NavTargetClass);

                NavigationProperty = Any.OdcmProperty(p =>
                {
                    p.Class = OdcmContainer;
                    p.Type = NavTargetClass;
                    p.IsCollection = false;
                });

                OdcmContainer.Properties.Add(NavigationProperty);
            });
        }

        [Fact]
        public void The_EntityContainer_class_exposes_a_readonly_ConcreteInterface_property()
        {
            EntityContainerType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Private,
                NavTargetFetcherInterface,
                NavigationProperty.Name,
                "Because Entity types should be accessible through their related Entity types.");
        }

        [Fact]
        public void The_EntityContainer_interface_exposes_a_ConcreteInterface_property()
        {
            EntityContainerInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                NavTargetFetcherInterface,
                NavigationProperty.Name);
        }
    }
}
