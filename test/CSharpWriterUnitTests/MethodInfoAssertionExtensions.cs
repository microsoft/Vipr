// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Types;

namespace CSharpWriterUnitTests
{
    public static class MethodInfoAssertionExtensions
    {
        /// <summary>
        /// Asserts that the current method is async
        /// 
        /// </summary>
        /// <param name="methodInfoAssertion">The targetted MethodInfoAssertion</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<MethodInfoAssertions> BeAsync(this MethodInfoAssertions methodInfoAssertion, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(methodInfoAssertion.Subject.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected method {0} to be async, but it is not.", methodInfoAssertion.Subject.Name);

            return new AndConstraint<MethodInfoAssertions>(methodInfoAssertion);
        }
    }
}