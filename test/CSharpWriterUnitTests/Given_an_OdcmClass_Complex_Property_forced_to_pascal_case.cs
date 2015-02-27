// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using CSharpWriter.Settings;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Moq;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    /// <summary>
    /// Summary description for Given_an_OdcmModel
    /// </summary>
    public class Given_an_OdcmClass_Complex_Property_forced_to_pascal_case : EntityTestBase
    {
        private OdcmProperty _property;

        private OdcmClass _targetClass;

        private readonly Type _targetType;

        public Given_an_OdcmClass_Complex_Property_forced_to_pascal_case()
        {
            SetConfiguration(new CSharpWriterSettings
            {
                ForcePropertyPascalCasing = true
            });

            base.Init(m =>
            {
                _targetClass = Any.ComplexOdcmClass(Namespace);

                m.AddType(_targetClass);

                _property = _targetClass.Properties.RandomElement();

                _property = _property.Rename(Any.Char('a', 'z') + _property.Name);
            });

            _targetType = Proxy.GetClass(_targetClass.Namespace, _targetClass.Name);
        }

        [Fact]
        public void The_Concrete_class_has_the_renamed_property()
        {
            _targetType.Should().HaveProperty(
                CSharpAccessModifiers.Public, 
                CSharpAccessModifiers.Public, 
                Proxy.GetClass(_property.Type.Namespace, _property.Type.Name),
                GetPascalCaseName(_property));
        }

        [Fact]
        public void The_Concrete_class_has_the_original_property_deprecated()
        {
            var propertyType = Proxy.GetClass(_property.Type.Namespace, _property.Type.Name);
            var expectedObsoleteMessage = String.Format("Use {0} instead.", GetPascalCaseName(_property));

            _targetType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Public,
                propertyType,
                _property.Name);

            _targetType.GetProperty(_property.Name, propertyType).Should()
                .BeDecoratedWith<ObsoleteAttribute>(a => a.Message == expectedObsoleteMessage, "Because the renamed property is preferred.")
                .And.BeDecoratedWith<EditorBrowsableAttribute>(a => a.State == EditorBrowsableState.Never,
                    "Because it should not show up in intellisense.");
        }

        private static string GetPascalCaseName(OdcmProperty property)
        {
            return property.Name.Substring(0, 1).ToUpper() + property.Name.Substring(1);
        }
    }
}
