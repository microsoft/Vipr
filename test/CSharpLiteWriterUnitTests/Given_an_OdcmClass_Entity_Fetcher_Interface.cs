// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.OData.ProxyExtensions.Lite;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Fetcher_Interface : EntityTestBase
    {
        
        public Given_an_OdcmClass_Entity_Fetcher_Interface()
        {
            base.Init(m => m.Namespaces[0].Classes.First().Properties.Add(Any.PrimitiveOdcmProperty(p => p.Class = Class)));
        }

        [Fact]
        public void It_is_Public()
        {
            FetcherInterface.IsPublic
                .Should().BeTrue("Because it allows users to interact with the Fetcher Class and with" +
                                 "Fetcher behaviors on the Concrete class.");
        }

        [Fact]
        public void It_is_decorated_with_LowerCasePropertyAttribute()
        {
            ConcreteInterface.Should()
                .BeDecoratedWith<LowerCasePropertyAttribute>();
        }

        [Fact]
        public void It_exposes_an_ExecuteAsync_Method()
        {
            FetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                typeof(Task<>).MakeGenericType(ConcreteInterface),
                "ExecuteAsync",
                new Type[0],
                "Because it allows executing the query.");
        }

        [Fact]
        public void It_exposes_an_UpdateAsync_Method()
        {
            FetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                typeof(Task),
                "UpdateAsync",
                new Type[]{ConcreteInterface, typeof(bool)},
                "Because it allows updating an entity.");
        }

        [Fact]
        public void It_exposes_a_DeleteAsync_Method()
        {
            FetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                typeof(Task),
                "DeleteAsync",
                new Type[] { ConcreteInterface, typeof(bool) },
                "Because it allows deleting an entity.");
        }

        [Fact]
        public void It_exposes_a_SaveChangesAsync_Method()
        {
            FetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                typeof(Task),
                "SaveChangesAsync",
                new Type[] { typeof(bool), typeof(Microsoft.OData.Client.SaveChangesOptions) },
                "Because it allows saving changes made to an entity.");
        }

        [Fact]
        public void It_exposes_an_Expand_Method()
        {
            var expandMethod = FetcherInterface.GetMethod("Expand");

            expandMethod.Should().NotBeNull("Because it allows executing the query.");

            expandMethod.IsPublic.Should().BeTrue("Because it allows executing the query.");

            expandMethod.GetGenericArguments().Count()
                .Should().Be(1);

            var genericArgType = expandMethod.GetGenericArguments()[0];

            var expectedParamType =
                typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(ConcreteInterface, genericArgType));

            expandMethod.GetParameters().Count()
                .Should().Be(1);

            expandMethod.GetParameters()[0].ParameterType
                .Should().Be(expectedParamType);

            expandMethod.ReturnType
                .Should().Be(FetcherInterface, "Because it returns a Task<IConcreteInterface> which" +
                                                 "allows callers to await a call to the server.");
        }
    }
}
