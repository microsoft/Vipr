// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CSharpWriter;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Service_Navigation_Property_Instance : NavigationPropertyTestBase
    {
        
        public Given_an_OdcmClass_Service_Navigation_Property_Instance()
        {
            base.Init(m =>
            {
                _navigationProperty = Any.OdcmProperty(p => p.Type = Class);

                var @namespace = m.Namespaces[0];

                _navTargetClass = Any.EntityOdcmClass(@namespace);

                @namespace.Types.Add(_navTargetClass);

                var @class = @namespace.Classes.First();

                _navigationProperty = Any.OdcmProperty(p =>
                {
                    p.Class = OdcmContainer;
                    p.Type = _navTargetClass;
                    p.IsCollection = false;
                });

                OdcmContainer.Properties.Add(_navigationProperty);
            });
        }

        [Fact]
        public void The_EntityContainer_class_exposes_a_readonly_ConcreteInterface_property()
        {
            EntityContainerType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Private,
                _navTargetFetcherInterface,
                _navigationProperty.Name,
                "Because Entity types should be accessible through their related Entity types.");
        }

        [Fact]
        public void The_EntityContainer_interface_exposes_a_ConcreteInterface_property()
        {
            EntityContainerInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _navTargetFetcherInterface,
                _navigationProperty.Name);
        }
    }
}
