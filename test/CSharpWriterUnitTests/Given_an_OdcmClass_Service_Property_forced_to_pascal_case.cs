// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
    public class Given_an_OdcmClass_Service_Property_forced_to_pascal_case : EntityTestBase
    {
        private OdcmProperty _property;

        public Given_an_OdcmClass_Service_Property_forced_to_pascal_case()
        {
            var configurationProviderMock = new Mock<IConfigurationProvider>();
            configurationProviderMock.Setup(c => c.ForcePropertyPascalCasing).Returns(true);
            ConfigurationProvider = configurationProviderMock.Object;

            base.Init(m =>
            {
                _property = Model.EntityContainer.Properties.RandomElement();

                _property = _property.Rename(Any.Char('a', 'z') + _property.Name);
            });
        }

        [Fact]
        public void The_EntityContainer_class_has_the_renamed_property()
        {
            bool isCollection = _property.IsCollection;

            EntityContainerType.Should().HaveProperty(
                CSharpAccessModifiers.Public, 
                isCollection ? (CSharpAccessModifiers?)null : CSharpAccessModifiers.Private, 
                Proxy.GetInterface(_property.Type.Namespace, "I" + _property.Type.Name + (isCollection ? "Collection" : "Fetcher")),
                GetPascalCaseName(_property));
        }

        [Fact]
        public void The_EntityContainer_class_does_not_have_the_original_property_deprecated()
        {
            EntityContainerType.Should().NotHaveProperty(_property.Name);
        }

        private static string GetPascalCaseName(OdcmProperty property)
        {
            return property.Name.Substring(0, 1).ToUpper() + property.Name.Substring(1);
        }
    }
}
