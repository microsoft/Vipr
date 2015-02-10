// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Types;
using System.Linq;

namespace CSharpWriterUnitTests
{
    public static class TypeAssertionExtensions
    {
        /// <summary>
        /// Asserts that the current type has explicitly implemented a property named <paramref name="name"/> of type
        /// <paramref name="type"/> defined on interface <paramref name="definingInterface"/>.
        /// 
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="definingInterface">The interface being explicitly implemented.</param>
        /// <param name="getAccessModifier">The expected getter access modifier.</param>
        /// <param name="setAccessModifier">The expected getter access modifier.</param>
        /// <param name="type">The property type.</param>
        /// <param name="name">The name of the property</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<TypeAssertions> HaveExplicitProperty(this TypeAssertions typeAssertions,
            Type definingInterface, CSharpAccessModifiers? getAccessModifier, CSharpAccessModifiers? setAccessModifier,
            Type type, string name, string because = "", params object[] reasonArgs)
        {
            var isGettable = getAccessModifier != null;
            var isSettable = setAccessModifier != null;

            var explicitImplementations =
                typeAssertions.Subject.GetInterfaceMap(definingInterface)
                    .TargetMethods.Where(m => m.IsPrivate && m.IsFinal).ToList();

            var getterName = "get_" + name;
            var setterName = "set_" + name;

            var getters =
                explicitImplementations.Where(
                    m =>
                        !m.GetParameters().Any() &&
                        m.Name.EndsWith(getterName)).ToList();

            var setters =
                explicitImplementations.Where(
                    m =>
                        m.GetParameters().Count() == 1 &&
                        m.Name.EndsWith(setterName)).ToList();

            var typedGetters = getters.Where(m => m.ReturnType == type).ToList();
            var typedSetters = setters.Where(m => m.GetParameters()[0].ParameterType == type).ToList();

            var hasGetter = typedGetters.Any();
            var hasSetter = typedSetters.Any();

            Execute.Assertion.ForCondition(getters.Any() || setters.Any())
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected property {0}.{1} to exist, but it does not.", definingInterface.FullName, name);
            Execute.Assertion.ForCondition(typedGetters.Any() || typedSetters.Any())
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected property {0}.{1} of type {2} to exist, but it does not.", definingInterface.FullName,
                    name, type);
            Execute.Assertion.ForCondition(setAccessModifier == null || typedGetters.Any())
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected property {0}.{1} to{2} be gettable, but it is{3}.", definingInterface.FullName, name,
                    isGettable ? "" : " not", hasGetter ? "" : " not");
            Execute.Assertion.ForCondition(hasGetter == isGettable)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected property {0}.{1} to{2} be gettable, but it is{3}.", definingInterface.FullName, name,
                    isGettable ? "" : " not", hasGetter ? "" : " not");
            Execute.Assertion.ForCondition(hasSetter == isSettable)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected property {0}.{1} to{2} be settable, but it is{3}.", definingInterface.FullName, name,
                    isSettable ? "" : " not", hasSetter ? "" : " not");
            return new AndConstraint<TypeAssertions>(typeAssertions);
        }

        /// <summary>
        /// Asserts that the current type has explicitly implemented a method named <paramref name="name"/> defined on interface <paramref name="definingInterface"/>, accepting 
        /// <paramref name="parameterTypes">parameterTypes of specified types</paramref> and returning
        /// <paramref name="returnType"/>.
        /// 
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="definingInterface">The interface being explicitly implemented.</param>
        /// <param name="returnType">The type of the property.</param>
        /// <param name="name">The name of the property</param>
        /// <param name="parameterTypes">The types of the parameterTypes.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<TypeAssertions> HaveExplicitMethod(this TypeAssertions typeAssertions,
            Type definingInterface, String name, Type returnType, Type[] parameterTypes,
            string because = "", params object[] reasonArgs)
        {
            var explicitImplementations =
                typeAssertions.Subject.GetInterfaceMap(definingInterface)
                    .TargetMethods.Where(m => m.IsPrivate && m.IsFinal);

            var methods =
                explicitImplementations.Where(
                    m =>
                        m.ReturnType == returnType &&
                        m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes) &&
                        m.Name.EndsWith(name));

            Execute.Assertion.ForCondition(methods.Any())
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected method {0}.{1} to exist.", definingInterface.FullName, name);

            return new AndConstraint<TypeAssertions>(typeAssertions);
        }

        /// <summary>
        /// Asserts that the current type does not explicitly implemented a method named <paramref name="name"/> defined on interface <paramref name="definingInterface"/>.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="definingInterface">The interface being explicitly implemented.</param>
        /// <param name="name">The name of the property</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<TypeAssertions> NotHaveExplicitMethod(this TypeAssertions typeAssertions,
            Type definingInterface, String name,
            string because = "", params object[] reasonArgs)
        {
            var explicitImplementations = typeAssertions.Subject.GetInterfaceMap(definingInterface);

            var explicitMethod = explicitImplementations.TargetMethods.FirstOrDefault(
                m => m.IsPrivate && m.IsFinal && m.Name.EndsWith("." + name));

            Execute.Assertion.ForCondition(explicitMethod == null)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected method {0} not to exist.", explicitMethod == null ? string.Empty : explicitMethod.Name);

            return new AndConstraint<TypeAssertions>(typeAssertions);
        }

        /// <summary>
        /// Asserts that the current type derives from the <paramref name="baseType"/>.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="baseType">The expected base type.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<TypeAssertions> BeDerivedFrom(this TypeAssertions typeAssertions,
            Type baseType, string because = "", params object[] reasonArgs)
        {
            var baseTypeMatched = typeAssertions.Subject.IsSubclassOf(baseType);

            Execute.Assertion.ForCondition(baseTypeMatched)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {1} to be derived from {2}.", typeAssertions.Subject.FullName, baseType.FullName);

            return new AndConstraint<TypeAssertions>(typeAssertions);
        }

        /// <summary>
        /// Asserts that the current type has a default constructor with an expected access modifier.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="accessModifier">The C# access modifier for the constructor.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<TypeAssertions> HaveDefaultConstructor(this TypeAssertions typeAssertions,
            CSharpAccessModifiers accessModifier, string because = "",
            params object[] reasonArgs)
        {
            return HaveConstructor(typeAssertions, accessModifier, new Type[0], because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current type has a constructor with parameters types of <paramref name="parameterTypes"/> and an expected access modifier.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="accessModifier">The C# access modifier for the constructor.</param>
        /// <param name="parameterTypes">The types of the constructor's parameters.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<TypeAssertions> HaveConstructor(this TypeAssertions typeAssertions,
            CSharpAccessModifiers accessModifier, IEnumerable<Type> parameterTypes, string because = "", params object[] reasonArgs)
        {
            var parameterTypesArray = parameterTypes as Type[] ?? parameterTypes.ToArray();
            var constructorInfo =
                typeAssertions.Subject.GetConstructor(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null,
                    parameterTypesArray.ToArray(), null);

            var constructorAccessModifier = CSharpAccessModifiers.None;

            if (constructorInfo != null)
                constructorAccessModifier = constructorInfo.GetCSharpAccessModifier();

            Execute.Assertion.ForCondition(constructorInfo != null && constructorAccessModifier == accessModifier)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {1} to have a {2} constructor with type parameterTypes {3}.", typeAssertions.Subject.FullName, string.Join(", ", parameterTypesArray.Select(p => p.FullName)), accessModifier);

            return new AndConstraint<TypeAssertions>(typeAssertions);
        }

        /// <summary>
        /// Asserts that the current type has a method with parameters types of <paramref name="parameterTypes"/> and an expected access modifier.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="accessModifier">The C# access modifier for the constructor.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The types of the constructor's parameters.</param>
        /// <param name="returnType">The expected method's return type.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<TypeAssertions> HaveMethod(this TypeAssertions typeAssertions,
            CSharpAccessModifiers accessModifier, Type returnType, string name, IEnumerable<Type> parameterTypes,
            string because = "", params object[] reasonArgs)
        {
            return HaveMethod(typeAssertions, accessModifier, false, returnType, name, parameterTypes, because,
                reasonArgs);
        }

        /// <summary>
        /// Asserts that the current type has a method with parameters types of <paramref name="parameterTypes"/> and an expected access modifier.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="accessModifier">The C# access modifier for the constructor.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The types of the constructor's parameters.</param>
        /// <param name="returnType">The expected method's return type.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="isAsync">Is the method expected to be async?</param>
        public static AndConstraint<TypeAssertions> HaveMethod(this TypeAssertions typeAssertions,
            CSharpAccessModifiers accessModifier, bool isAsync, Type returnType, string name, IEnumerable<Type> parameterTypes, string because = "", params object[] reasonArgs)
        {
            var parameterTypesArray = parameterTypes as Type[] ?? parameterTypes.ToArray();
            var methodInfo =
                typeAssertions.Subject.GetMethod(name,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null,
                    parameterTypesArray, null);

            var methodAccessModifier = CSharpAccessModifiers.None;
            Type methodReturnType = null;

            if (methodInfo != null)
            {
                methodAccessModifier = methodInfo.GetCSharpAccessModifier();
                methodReturnType = methodInfo.ReturnType;
            }

            Execute.Assertion.ForCondition(methodInfo != null && methodAccessModifier == accessModifier &&
                                           methodReturnType == returnType &&
                                           (methodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null) == isAsync)

                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {1} to contain method {2} {3} {4}({5}).", typeAssertions.Subject.FullName,
                    accessModifier, returnType.Name, name,
                    string.Join(", ", parameterTypesArray.Select(p => p.FullName)), accessModifier);

            return new AndConstraint<TypeAssertions>(typeAssertions);
        }

        /// <summary>
        /// Asserts that the current type does not have a method named <paramref name="name"/>.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="name">The name of the method.</param>
        public static AndConstraint<TypeAssertions> NotHaveMethod(this TypeAssertions typeAssertions,
            string name, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion.ForCondition(typeAssertions.Subject != null &&
                                           typeAssertions.Subject.GetMethod(name) == null)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {1} to not contain method {2}.", typeAssertions.Subject.Name, name);

            return new AndConstraint<TypeAssertions>(typeAssertions);
        }

        /// <summary>
        /// Asserts that the current type exposes an indexer with parameters types of <paramref name="parameterTypes"/> and an expected access modifier.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="parameterTypes">The types of the indexer's parameters.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="indexerType">The type of the indexer</param>
        /// <param name="setAccessModifier">Setter access modifier. Null if not settable.</param>
        /// /// <param name="getAccessModifier">Getter access modifier. Null if not gettable.</param>
        public static AndConstraint<TypeAssertions> HaveIndexer(this TypeAssertions typeAssertions,
            CSharpAccessModifiers? getAccessModifier, CSharpAccessModifiers? setAccessModifier, Type indexerType, IEnumerable<Type> parameterTypes, string because = "", params object[] reasonArgs)
        {
            var parameterTypesArray = parameterTypes as Type[] ?? parameterTypes.ToArray();
            var propertyInfo =
                typeAssertions.Subject.GetProperty(
                    "Item",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null,
                    indexerType, parameterTypesArray.ToArray(), null);

            var validGetter = false;

            var validSetter = false;

            if (propertyInfo != null)
            {
                if (getAccessModifier.HasValue)
                    validGetter = propertyInfo.GetMethod != null &&
                                  getAccessModifier == propertyInfo.GetMethod.GetCSharpAccessModifier();
                else
                    validGetter = !propertyInfo.CanRead;
                if (setAccessModifier.HasValue)
                    validSetter = propertyInfo.SetMethod != null &&
                                  setAccessModifier == propertyInfo.SetMethod.GetCSharpAccessModifier();
                else
                    validSetter = !propertyInfo.CanWrite;
            }

            Execute.Assertion.ForCondition(propertyInfo != null && validGetter && validSetter)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {0} to have a {1} indexer[{2}] with {3} getter and {4} setter.",
                    typeAssertions.Subject.FullName, indexerType.FullName, string.Join(", ", parameterTypesArray.Select(p => p.FullName)),
                    indexerType, getAccessModifier.HasValue ? getAccessModifier.ToString() : "no", setAccessModifier.HasValue ? setAccessModifier.ToString() : "no");

            return new AndConstraint<TypeAssertions>(typeAssertions);
        }

        /// <summary>
        /// Asserts that the current type has a property with the expected access modifier.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="setAccessModifier">Setter access modifier. Null if not settable.</param>
        /// <param name="getAccessModifier">Getter access modifier. Null if not gettable.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="propertyType">The type of the indexer</param>
        public static AndConstraint<TypeAssertions> HaveProperty(this TypeAssertions typeAssertions,
            CSharpAccessModifiers? getAccessModifier, CSharpAccessModifiers? setAccessModifier, Type propertyType, string name, string because = "", params object[] reasonArgs)
        {
            var propertyInfo = typeAssertions.Subject.GetProperty(
                    name,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null,
                    propertyType, new Type[0], null);

            var validGetter = false;

            var validSetter = false;

            if (propertyInfo != null)
            {
                if (getAccessModifier.HasValue)
                    validGetter = propertyInfo.GetMethod != null &&
                                  getAccessModifier == propertyInfo.GetMethod.GetCSharpAccessModifier();
                else
                    validGetter = !propertyInfo.CanRead;
                if (setAccessModifier.HasValue)
                    validSetter = propertyInfo.SetMethod != null &&
                                  setAccessModifier == propertyInfo.SetMethod.GetCSharpAccessModifier();
                else
                    validSetter = !propertyInfo.CanWrite;
            }

            var getAccessModifierString = "no";
            var setAccessModifierString = "no";

            if (getAccessModifier.HasValue)
                getAccessModifierString = getAccessModifier.Value.ToString();
            if (setAccessModifier.HasValue)
                setAccessModifierString = setAccessModifier.Value.ToString();

            Execute.Assertion.ForCondition(propertyInfo != null && validGetter && validSetter)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {0} to have a {1} property {2} with {3} getter and {4} setter.",
                    typeAssertions.Subject.FullName, propertyType.FullName, name,
                    propertyType, getAccessModifierString, setAccessModifierString);

            return new AndConstraint<TypeAssertions>(typeAssertions);
        }

        /// <summary>
        /// Asserts that the current type does not expose a property named <paramref name="name"/>.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="name">The name of the method.</param>
        public static AndConstraint<TypeAssertions> NotHaveProperty(this TypeAssertions typeAssertions,
            string name, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion.ForCondition(typeAssertions.Subject != null &&
                                           typeAssertions.Subject.GetProperty(name) == null)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {0} to not contain property {1}.", typeAssertions.Subject.Name, name);

            return new AndConstraint<TypeAssertions>(typeAssertions);
        }

        /// <summary>
        /// Asserts that the current type does not implement Interface <paramref name="interfaceType"/>.
        /// </summary>
        /// <param name="typeAssertions">The TypeAssertion we are extending.</param>
        /// <param name="interfaceType">The interface that should not be implemented.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public static AndConstraint<TypeAssertions> Implement(this TypeAssertions typeAssertions,
            Type interfaceType, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion.ForCondition(typeAssertions.Subject != null &&
                                           typeAssertions.Subject.GetInterface(interfaceType.Name) == interfaceType)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {1} to implement interface {2}.", typeAssertions.Subject.Name, interfaceType.Name);

            return new AndConstraint<TypeAssertions>(typeAssertions);
        }
    }
}
