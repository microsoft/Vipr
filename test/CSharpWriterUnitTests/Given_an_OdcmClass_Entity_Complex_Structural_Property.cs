// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using System;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Complex_Structural_Property : EntityTestBase
    {
        private Type _structuralPropertyType;
        private OdcmProperty _structuralProperty;

        
        public Given_an_OdcmClass_Entity_Complex_Structural_Property()
        {
            base.Init(m =>
            {
                _structuralProperty = Any.ComplexOdcmProperty(m.Namespaces[0]);
                _structuralProperty.Class = Class;
                Class.Properties.Add(_structuralProperty);
                m.AddType(_structuralProperty.Type);
            });

            _structuralPropertyType = Proxy.GetClass(_structuralProperty.Type.Namespace, _structuralProperty.Type.Name);
        }

        [Fact]
        public void The_Concrete_class_exposes_an_instance_property()
        {
            ConcreteType.Should()
                .HaveProperty(CSharpAccessModifiers.Public, CSharpAccessModifiers.Public, _structuralPropertyType,
                    _structuralProperty.Name,
                    "Because complex types should be accessible through their containing Entity types.");
        }

        [Fact]
        public void The_Concrete_interface_exposes_an_instance_property()
        {
            ConcreteType.Should()
                .HaveProperty(CSharpAccessModifiers.Public, CSharpAccessModifiers.Public, _structuralPropertyType,
                    _structuralProperty.Name);
        }

        [Fact]
        public void The_Fetcher_interface_does_not_expose_it()
        {
            var propertyInfo = FetcherInterface.GetProperty(_structuralProperty.Name, PermissiveBindingFlags);

            propertyInfo
                .Should().BeNull("Because you cannot independently fetch a complex type.");
        }

        [Fact]
        public void The_Fetcher_class_does_not_expose_it()
        {
            var propertyInfo = FetcherType.GetProperty(_structuralProperty.Name, PermissiveBindingFlags);

            propertyInfo
                .Should().BeNull();
        }

        [Fact]
        public void The_Collection_interface_does_not_expose_it()
        {
            var propertyInfo = CollectionInterface.GetProperty(_structuralProperty.Name, PermissiveBindingFlags);

            propertyInfo
                .Should().BeNull("Because you cannot independently fetch a complex type.");
        }

        [Fact]
        public void The_Collection_class_does_not_expose_it()
        {
            var propertyInfo = CollectionInterface.GetProperty(_structuralProperty.Name, PermissiveBindingFlags);

            propertyInfo
                .Should().BeNull();
        }
    }
}
