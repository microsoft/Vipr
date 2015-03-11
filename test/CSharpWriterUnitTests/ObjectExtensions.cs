using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace CSharpWriterUnitTests
{
    public static class ObjectExtensions
    {
        public static JObject ToJObject(this object @object)
        {
            return JObject.FromObject(@object);
        }

        public static object GetPropertyValue(this object @object, string propertyName)
        {
            return GetPropertyValue<object>(@object, propertyName);
        }

        public static T GetPropertyValue<T>(this object @object, string propertyName)
        {
            return (T) @object.GetType().GetProperty(propertyName).GetValue(@object);
        }

        public static object GetPropertyValue(this object @object, Type @interface, string propertyName)
        {
            return GetPropertyValue<object>(@object, @interface, propertyName);
        }

        public static T GetPropertyValue<T>(this object @object, Type @interface, string propertyName)
        {
            return (T) @interface.GetProperty(propertyName).GetValue(@object);
        }

        public static void SetPropertyValue(this object @object, string propertyName, object value)
        {
            @object.GetType().GetProperty(propertyName).SetValue(@object, value);
        }

        public static void SetPropertyValues(this object @object, IEnumerable<Tuple<string, object>> propertyValues)
        {
            foreach (var propertyValue in propertyValues)
            {
                if (propertyValue.Item2 != null)
                    @object.SetPropertyValue(propertyValue.Item1, propertyValue.Item2);
            }
        }

        public static object InvokeMethod(this object @object, string methodName, object[] args = null)
        {
            return InvokeMethod<object>(@object, methodName, args: args);
        }

        public static T InvokeMethod<T>(this object @object, string methodName, object[] args = null,
            Type[] types = null)
        {
            args = args ?? new object[0];

            var method = @object.GetType().GetMethod(methodName);
            if (types != null)
                method = method.MakeGenericMethod(types);

            return (T) method.Invoke(@object, args);
        }

        public static object GetIndexerValue(this object @object, object[] args = null)
        {
            return @object.InvokeMethod("get_Item", args);
        }

        public static T GetIndexerValue<T>(this object @object, object[] args = null)
        {
            return @object.InvokeMethod<T>("get_Item", args: args);
        }

        public static void ValidateCollectionPropertyValues(this object collection, IList<IEnumerable<Tuple<string, object>>> entitiesProperties)
        {
            var responses = ((IEnumerable<object>)collection).ToList();

            responses.Count.Should().Be(entitiesProperties.Count());

            for (int x = 0; x < responses.Count; x++)
            {
                ValidatePropertyValues(responses[x], entitiesProperties[x]);
            }
        }

        public static void ValidatePropertyValues(this object instance, IEnumerable<Tuple<string, object>> propertyValues)
        {
            foreach (var keyValue in propertyValues)
            {
                instance.GetPropertyValue(keyValue.Item1)
                    .Should().Be(keyValue.Item2);
            }
        }
    }
}