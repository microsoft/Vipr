// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CSharpWriter;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_Indenter
    {
        private Indenter _indenter = new Indenter();

        [Fact]
        public void When_created_it_has_an_empty_indent()
        {
            _indenter.Indentation.Should().BeEmpty();
        }

        [Fact]
        public void When_in_an_indent_block_it_increases_the_indent_by_one_step()
        {
            _indenter.Indentation.Should().BeEmpty();

            ValidateIndentSteps("    ");
        }

        [Fact]
        public void When_created_without_arguments_it_defaults_to_a_four_space_indent()
        {
            using (_indenter.Indent)
            {
                _indenter.Indentation.Should().Be("    ");
            }
        }

        [Fact]
        public void When_created_with_a_custom_indentStep_it_uses_that_indentStep()
        {
            var indentStep = Any.String();

            _indenter = new Indenter(indentStep);

            ValidateIndentSteps(indentStep);
        }

        private void ValidateIndentSteps(string indentStep)
        {
            _indenter.Indentation.Should().BeEmpty();

            using (_indenter.Indent)
            {
                _indenter.Indentation.Should().Be(indentStep);

                using (_indenter.Indent)
                {
                    _indenter.Indentation.Should().Be(indentStep + indentStep);
                }

                _indenter.Indentation.Should().Be(indentStep);
            }

            _indenter.Indentation.Should().BeEmpty();
        }
    }
}
