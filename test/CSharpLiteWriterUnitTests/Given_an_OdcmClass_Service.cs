// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.OData.ProxyExtensions;
using System;
using System.Threading.Tasks;
using Type = System.Type;
using Xunit;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmClass_Service : NavigationPropertyTestBase
    {
        
        public Given_an_OdcmClass_Service()
        {
            base.Init();
        }

        [Fact]
        public void It_has_the_right_namespace_and_name()
        {
            EntityContainerType
                .Should().NotBeNull();
        }

        [Fact]
        public void It_is_Public()
        {
            EntityContainerType.IsPublic
                .Should().BeTrue("Because it is Public.");
        }

        [Fact]
        public void It_implements_the_container_interface()
        {
            EntityContainerType.Should().Implement(EntityContainerInterface);
        }

        [Fact]
        public void It_has_a_Context_Property()
        {
            EntityContainerType.Should().HaveProperty(
                CSharpAccessModifiers.Public,
                CSharpAccessModifiers.Private,
                typeof(DataServiceContextWrapper),
                "Context");
        }

        [Fact]
        public void It_has_an_EntityContainer_Constructor()
        {
            EntityContainerType.Should().HaveConstructor(
                CSharpAccessModifiers.Public,
                GetEntityContainerConstructorParameterTypes());
        }

        [Fact]
        public void It_can_be_constructed()
        {
            Activator.CreateInstance(EntityContainerType, new object[]{new Uri("http://localhost"), (Func<Task<string>>)(() => Task.FromResult(""))});
        }

        private IEnumerable<Type> GetEntityContainerConstructorParameterTypes()
        {
            return new[]
            {
                typeof (Uri),
                typeof (Func<Task<string>>)
            };
        }
    }
}
