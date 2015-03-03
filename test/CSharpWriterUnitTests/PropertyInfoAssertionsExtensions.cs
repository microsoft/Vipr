// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Types;
using System.Linq;

namespace CSharpWriterUnitTests
{
    public static class PropertyInfoAssertionsExtensions
    {
        /// <summary>
        /// Asserts that the current <see cref="T:System.Reflection.PropertyInfo"/> is decorated with an attribute of type <typeparamref name="TAttribute"/>
        ///             that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// 
        /// </summary>
        /// <param name="propertyInfoAssertions">The PropertyInfoAssertions to act upon.</param>
        /// <param name="isMatchingAttributePredicate">The predicate that the attribute must match.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<PropertyInfoAssertions> BeDecoratedWith<TAttribute>(this PropertyInfoAssertions propertyInfoAssertions, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] reasonArgs) where TAttribute : Attribute
        {
            propertyInfoAssertions.BeDecoratedWith<TAttribute>(because, reasonArgs);
            
            Execute.Assertion.ForCondition(propertyInfoAssertions.Subject.GetCustomAttributes(typeof (TAttribute))
                .Count(a => isMatchingAttributePredicate.Compile().Invoke((TAttribute) a)) > 0)
                .BecauseOf(because, reasonArgs)
                .FailWith(
                    "Expected type {0} to be decorated with {1} that matches {2}{reason}, but no matching attribute was found.",
                    (object) propertyInfoAssertions.Subject, (object) typeof (TAttribute),
                    (object) isMatchingAttributePredicate.Body);

            return new AndConstraint<PropertyInfoAssertions>(propertyInfoAssertions);
        }
    }
}
