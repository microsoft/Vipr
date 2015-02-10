using System;
using System.Linq;

namespace CSharpWriterUnitTests
{
    public static class ODataKeyPredicate
    {
        public static string AsString(Tuple<string, object>[] keyValues)
        {
            return keyValues.Count() == 1
                ? keyValues.First().Item2 as string
                : keyValues.Select(k => string.Format("{0}='{1}'", k.Item1, k.Item2))
                    .Aggregate((c, n) => c + "," + n);
        }
    }
}