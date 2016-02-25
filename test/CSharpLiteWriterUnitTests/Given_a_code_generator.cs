// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using Xunit;

namespace CSharpLiteWriterUnitTests
{
    public class Given_a_code_generator : CodeGenTestBase
    {
        [Fact]
        public void When_given_a_GetString_class_it_gets_the_hard_coded_string()
        {
            var @namespace = "_" + Any.AlphanumericString(1, 10);
            var typeName = "_" + Any.AlphanumericString(1, 10);
            var expectedSource = "_" + Any.AlphanumericString(1, 10);

            var proxySource = CodeSamples.GetStringSource(@namespace, typeName, expectedSource);
            var asm = CompileText(null, proxySource);

            GetValue<string>(asm, "GetString", typeName, @namespace)
                .Should().Be(expectedSource);

            asm.GetNamespaces()
                .ShouldAllBeEquivalentTo(new[] { @namespace });
        }

        [Fact]
        public void When_given_two_GetString_classes_they_both_return_the_hard_coded_result()
        {
            var @namespace1 = "_" + Any.AlphanumericString(1, 10);
            var typeName1 = "_" + Any.AlphanumericString(1, 10);
            var expectedSource1 = "_" + Any.AlphanumericString(1, 10);

            var @namespace2 = "_" + Any.AlphanumericString(1, 10);
            var typeName2 = "_" + Any.AlphanumericString(1, 10);
            var expectedSource2 = "_" + Any.AlphanumericString(1, 10);

            var proxySource1 = CodeSamples.GetStringSource(@namespace1, typeName1, expectedSource1);
            var proxySource2 = CodeSamples.GetStringSource(@namespace2, typeName2, expectedSource2);
            var asm = CompileText(null, proxySource1 + proxySource2);

            GetValue<string>(asm, "GetString", typeName1, @namespace1)
                .Should().Be(expectedSource1);
            GetValue<string>(asm, "GetString", typeName2, @namespace2)
                .Should().Be(expectedSource2);

            asm.GetNamespaces()
                .ShouldAllBeEquivalentTo(new[] { @namespace1, @namespace2 });
        }
    }
}
