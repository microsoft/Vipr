using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;

namespace Microsoft.MockService.Extensions.ODataV4
{
    public class TestReadableStringCollection :ReadableStringCollection
    {
        public TestReadableStringCollection(IDictionary<string, string[]> store) : base(store)
        {
        }

        public bool Equals(IReadableStringCollection other)
        {
            if (this.Count() != other.Count())
                return false;

            foreach (var kv in this)
            {
                var values1 = GetValues(kv.Key);
                var values2 = other.GetValues(kv.Key);

                var missing1 = values1.Except(values2).Any();
                var missing2 = values2.Except(values1).Any();

                if (missing1 || missing2)
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            if (!this.Any())
                return string.Empty;

            return "?" + this.SelectMany(k => k.Value.Select(v => k.Key + "=" + v)).Aggregate((c, n) => c + "&" + n);
        }
    }
}
