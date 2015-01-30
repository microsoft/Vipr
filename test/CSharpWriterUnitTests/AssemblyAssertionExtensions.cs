// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Reflection;

namespace CSharpWriterUnitTests
{
    public static class AssemblyAssertionExtensions
    {
        /// <summary>
        /// Asserts that the Assembly contains a type of kind <paramref name="typeKind"/> with the expected <paramref name="accessModifier"/>, <paramref name="name"/> and <paramref name="namespace"/>.
        /// </summary>
        /// <param name="assemblyAssertions">The AssemblyAssertions we are extending.</param>
        /// <param name="accessModifier">The C# access modifier of the class.</param>
        /// <param name="typeKind">The expected kind of the type.</param>
        /// <param name="namespace">The namespace of the class.</param>
        /// <param name="name">The name of the class.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<AssemblyAssertions> DefineType(this AssemblyAssertions assemblyAssertions,
            CSharpAccessModifiers accessModifier, TypeKinds typeKind, string @namespace, string name, string because = "", params object[] reasonArgs)
        {
            var isNull = assemblyAssertions.Subject == null;

            var definesType = assemblyAssertions.Subject != null &&
                              assemblyAssertions.Subject.DefinedTypes.Any(
                                  t =>
                                      t.GetCSharpAccessModifier() == accessModifier &&
                                      t.GetTypeKind() == typeKind &&
                                      t.Namespace == @namespace &&
                                      t.Name == name);

            Execute.Assertion.ForCondition(definesType)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected assembly {0} to define a {1} {2} {3}.{4}.", assemblyAssertions.Subject.FullName,
                    accessModifier, typeKind, @namespace, name);

            return new AndConstraint<AssemblyAssertions>(assemblyAssertions);
        }
    }
}
