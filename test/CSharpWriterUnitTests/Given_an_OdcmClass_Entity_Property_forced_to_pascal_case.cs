// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CSharpWriter.Settings;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Moq;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Property_forced_to_pascal_case : EntityTestBase
    {
        private OdcmProperty _property;
        private readonly Type _propertyType;

        public Given_an_OdcmClass_Entity_Property_forced_to_pascal_case()
        {
            SetConfiguration(new CSharpWriterSettings
            {
                ForcePropertyPascalCasing = true
            });

            Init(m =>
            {
                var originalProperty = Class.Properties.RandomElement();

                var camelCasedName = Any.Char('a', 'z') + originalProperty.Name;

                _property = originalProperty.Rename(camelCasedName);
            });

            _propertyType = Proxy.GetClass(_property.Type.Namespace, _property.Type.Name);
            if (_property.IsCollection)
                _propertyType = typeof (IList<>).MakeGenericType(_propertyType);
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
    }
}
