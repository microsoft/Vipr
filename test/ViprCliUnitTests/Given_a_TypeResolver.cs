using System;
using System.IO;
using DoubleReader;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Vipr;
using Vipr.Core;
using Xunit;

namespace ViprCliUnitTests
{
    public class Given_a_TypeResolver
    {
        public Given_a_TypeResolver()
        {
            var testReaderWriter = new TestReaderWriter.TestReaderWriter();
            var @double = new DoubleReader1();
        }

        [Fact]
        public void When_the_requested_type_is_not_an_interface_it_throws_an_IllegalOperationException()
        {
            Action getInstance = () => TypeResolver.GetInstance<string>(Any.String());

            getInstance.ShouldThrow<InvalidOperationException>("Because string is not an interface.");
        }

        [Fact]
        public void When_the_assembly_cannot_be_found_it_throws_a_FileNotFoundException()
        {
            Action getInstance = () => TypeResolver.GetInstance<ITestInterface>(Any.String(1));

            getInstance.ShouldThrow<FileNotFoundException>("Because the assembly is not found.");
        }

        [Fact]
        public void When_the_assembly_does_not_have_a_type_that_implements_the_interface_it_throws_an_IllegalOperationException()
        {
            Action getInstance = () => TypeResolver.GetInstance<ITestInterface>("TestReaderWriter");

            getInstance.ShouldThrow<InvalidOperationException>("Because the assembly does not have a type that implements the interface.");
        }

        [Fact]
        public void When_the_assembly_has_multiple_types_that_implement_the_interface_it_throws_an_InvalidOperationException()
        {
            Action getInstance = () => TypeResolver.GetInstance<IOdcmReader>("DoubleReader");

            getInstance.ShouldThrow<InvalidOperationException>(
                "Because the assembly does not have a type that implements the interface.");
        }

        [Fact]
        public void When_the_assembly_has_a_single_type_that_implements_the_interface_it_returns_an_instance_of_that_type()
        {
            TypeResolver.GetInstance<IOdcmReader>("TestReaderWriter").GetType()
                .Should().Be(typeof(TestReaderWriter.TestReaderWriter));
        }

        private interface ITestInterface
        {
        }
    }
}
