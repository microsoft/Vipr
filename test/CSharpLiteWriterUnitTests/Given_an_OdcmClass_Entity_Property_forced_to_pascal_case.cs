// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vipr.Writer.CSharp.Lite.Settings;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Moq;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Property_forced_to_pascal_case : EntityTestBase
    {
        private OdcmProperty _property;
        private OdcmProperty _collectionProperty;
        private readonly Type _propertyType;
        private readonly Type _collectionPropertyType;

        public Given_an_OdcmClass_Entity_Property_forced_to_pascal_case()
        {
            SetConfiguration(new CSharpWriterSettings
            {
                ForcePropertyPascalCasing = true
            });

            Init(m =>
            {
                var originalProperty = Class.Properties.Where(p => !p.IsCollection).RandomElement();

                var camelCasedName = Any.Char('a', 'z') + originalProperty.Name;

                _property = originalProperty.Rename(camelCasedName);

                originalProperty = Class.NavigationProperties().Where(p => p.IsCollection).RandomElement();

                camelCasedName = Any.Char('a', 'z') + originalProperty.Name;

                _collectionProperty = originalProperty.Rename(camelCasedName);
            });

            _propertyType = Proxy.GetClass(_property.Type.Namespace, _property.Type.Name);

            _collectionPropertyType = Proxy.GetClass(_collectionProperty.Type.Namespace, _collectionProperty.Type.Name);
            _collectionPropertyType = typeof(IList<>).MakeGenericType(_collectionPropertyType);
        }

        [Fact]
        public void The_Concrete_class_has_the_renamed_property()
        {
            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public, 
                CSharpAccessModifiers.Public,
                _propertyType,
                _property.Name.ToPascalCase());
        }

        [Fact]
        public void The_Concrete_class_has_the_renamed_collection_navigation_property_without_setter()
        {
            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _collectionPropertyType,
                _collectionProperty.Name.ToPascalCase());
        }

        [Fact]
        public void The_Concrete_class_has_the_original_property_deprecated()
        {
            var expectedObsoleteMessage = String.Format("Use {0} instead.", _property.Name.ToPascalCase());

            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                _propertyType,
                _property.Name);

            ConcreteType.GetProperty(_property.Name, _propertyType).Should()
                .BeDecoratedWith<ObsoleteAttribute>(a => a.Message == expectedObsoleteMessage, "Because the renamed property is preferred.")
                .And.BeDecoratedWith<EditorBrowsableAttribute>(a => a.State == EditorBrowsableState.Never,
                    "Because it should not show up in intellisense.");
        }

        [Fact]
        public void The_Concrete_class_has_the_original_collection_navigation_property_deprecated_without_setter()
        {
            var expectedObsoleteMessage = String.Format("Use {0} instead.", _property.Name.ToPascalCase());

            ConcreteType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                null,
                _collectionPropertyType,
                _collectionProperty.Name);

            ConcreteType.GetProperty(_property.Name, _propertyType).Should()
                .BeDecoratedWith<ObsoleteAttribute>(a => a.Message == expectedObsoleteMessage, "Because the renamed property is preferred.")
                .And.BeDecoratedWith<EditorBrowsableAttribute>(a => a.State == EditorBrowsableState.Never,
                    "Because it should not show up in intellisense.");
        }
    }
}
