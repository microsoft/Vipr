using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace Microsoft.MockService.Extensions.ODataV4
{
    public static class ArgumentOfTupleExtensions
    {
        public static JObject ToJObject(this IEnumerable<Tuple<string, object>> propertyValues)
        {
            if (propertyValues == null || !propertyValues.Any())
                return null;

            var retVal = new JObject();

            foreach (var propertyValue in propertyValues)
            {
                retVal.Add(propertyValue.Item1, new JValue(propertyValue.Item2));
            }

            return retVal;
        }

        public static TestReadableStringCollection ToTestReadableStringCollection(
            this IEnumerable<Tuple<string, object>> propertyValues)
        {
            return new TestReadableStringCollection(propertyValues
                .ToDictionary(p => p.Item1, p => new[] {p.Item2.ToString()}));
        }
    }
}
