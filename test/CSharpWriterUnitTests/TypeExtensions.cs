// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.OData.Client;

namespace CSharpWriterUnitTests
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetKeyProperties(this Type type)
        {
            var keyAttribute = type.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault(a => a is KeyAttribute) as KeyAttribute;

            return keyAttribute == null
                ? Enumerable.Empty<PropertyInfo>()
                : keyAttribute.KeyNames.Select(type.GetProperty);
        }

        public static object Initialize(this Type type, IEnumerable<Tuple<string, object>> propertyValues = null)
        {
            if (propertyValues == null)
                propertyValues = new List<Tuple<string, object>>();

            return type.Initialize<object>(propertyValues);
        }

        public static T Initialize<T>(this Type type, IEnumerable<Tuple<string, object>> propertyValues = null) where T : class
        {
            if (propertyValues == null)
                propertyValues = new List<Tuple<string, object>>();

            var instance = Activator.CreateInstance(type);

            instance.SetPropertyValues(propertyValues);

            return instance as T;
        }
    }
}
