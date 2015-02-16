using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Microsoft.MockService.Extensions.ODataV4
{
    public static class OwinRequestExtensions
    {
        public static bool InvokesMethodWithParameters(this IOwinRequest request, string entityMethodPath,
            TestReadableStringCollection uriArguments = null)
        {
            var actualPath = request.Path.Value;

            var methodPathStart = entityMethodPath + "(";

            if (!actualPath.StartsWith(methodPathStart))
                return false;

            var argumentsStartIndex = methodPathStart.Length;

            var argumentsEndIndex = actualPath.IndexOf(")", argumentsStartIndex, System.StringComparison.Ordinal);

            if (argumentsEndIndex < 0)
                return false;

            var argumentsString = actualPath.Substring(argumentsStartIndex, argumentsEndIndex - argumentsStartIndex);

            var argumentsArray = argumentsString.Split(',');

            if (argumentsArray.Length == 1)
            {
                if (uriArguments.Count() == 1 &&
                    uriArguments.First().Value.Count() == 1 &&
                    uriArguments.First().Value[0] == argumentsArray[0])

                    return true;
            }

            var argumentsCollection = new TestReadableStringCollection(argumentsArray.Select(s =>
            {
                var kv = s.Split('=');
                return new Tuple<string, string>(kv[0], kv[1]);
            }).ToDictionary(k => k.Item1, k => new[] {k.Item2.Trim('\'')}));

            return argumentsCollection.Equals(uriArguments);
        }
    }
}
