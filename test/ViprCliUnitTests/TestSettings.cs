using System.Collections.Generic;
using System.Linq;

namespace ViprUnitTests
{
    public class TestSettings
    {
        public TestSettings()
        {
        }

        public IDictionary<string, string> StringDictionary { get; set; }
        
        public string StringValue { get; set; }

        public bool BoolValue { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as TestSettings;

            if (other == null)
                return base.Equals(obj);

            return (other.BoolValue == this.BoolValue &&
                    other.StringValue == this.StringValue &&
                    ((this.StringDictionary == null && other.StringDictionary == null) ||
                     ((this.StringDictionary != null && other.StringDictionary != null) &&
                      (other.StringDictionary.Count == this.StringDictionary.Count &&
                       !other.StringDictionary.Except(this.StringDictionary).Any()))));

        }
    }
}
