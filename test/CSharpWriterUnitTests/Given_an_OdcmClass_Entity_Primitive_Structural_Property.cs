﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Primitive_Structural_Property : EntityTestBase
    {
        private OdcmProperty _structuralInstanceProperty;
        private OdcmProperty _structuralCollectionProperty;

        
        public Given_an_OdcmClass_Entity_Primitive_Structural_Property()
        {
            base.Init(m =>
            {
                _structuralInstanceProperty = Any.PrimitiveOdcmProperty(p => p.Class = Class);

                _structuralCollectionProperty = Any.PrimitiveOdcmProperty(p =>
                {
                    p.Class = Class;
                    p.IsCollection = true;
                });

                Class.Properties.Add(_structuralInstanceProperty);

                Class.Properties.Add(_structuralCollectionProperty);
            });
        }

        [Fact]
        public void When_it_is_an_instance_property_then_the_Concrete_class_exposes_an_instance_property()
        {
            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                EdmToClrTypeMap[_structuralInstanceProperty.Projection.Type.Name],
                _structuralInstanceProperty.Name);
        }

        [Fact]
        public void When_it_is_a_collection_property_then_the_Concrete_class_exposes_an_IList_of_instance_property()
        {
            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                typeof(IList<>).MakeGenericType(EdmToClrTypeMap[_structuralInstanceProperty.Projection.Type.Name]),
                _structuralCollectionProperty.Name);
        }

        [Fact]
        public void When_it_is_an_instance_property_then_the_Concrete_interface_exposes_an_instance_property()
        {
            ConcreteInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                EdmToClrTypeMap[_structuralInstanceProperty.Projection.Type.Name],
                _structuralInstanceProperty.Name);
        }

        [Fact]
        public void When_it_is_a_collection_property_then_the_Concrete_interface_exposes_an_IList_of_instance_property()
        {
            ConcreteInterface.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                typeof(IList<>).MakeGenericType(EdmToClrTypeMap[_structuralInstanceProperty.Projection.Type.Name]),
                _structuralCollectionProperty.Name);
        }

        [Fact]
        public void When_it_is_an_instance_property_then_the_Fetcher_interface_does_not_expose_it()
        {
            FetcherInterface.Should().NotHaveProperty(_structuralInstanceProperty.Name);
        }

        [Fact]
        public void When_it_is_an_instance_property_then_the_Fetcher_class_does_not_expose_it()
        {
            FetcherType.Should().NotHaveProperty(_structuralInstanceProperty.Name);
        }

        [Fact]
        public void When_it_is_an_instance_property_then_the_Collection_interface_does_not_expose_it()
        {
            CollectionInterface.Should().NotHaveProperty(_structuralInstanceProperty.Name);
        }

        [Fact]
        public void When_it_is_an_instance_property_then_the_Collection_class_does_not_expose_it()
        {
            CollectionType.Should().NotHaveProperty(_structuralInstanceProperty.Name);
        }
    }
}
