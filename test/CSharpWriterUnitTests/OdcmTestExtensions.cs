using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Its.Recipes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriterUnitTests
{
    public static class OdcmTestExtensions
    {
        public static IEnumerable<OdcmProperty> GetProperties(this OdcmModel model)
        {
            return
                model.Namespaces.SelectMany(n => n.Classes.SelectMany(c => c.Properties));
        }

        public static IEnumerable<Tuple<string, object>> GetSampleKeyArguments(this OdcmClass entityClass)
        {
            return entityClass.Key.Select(p => new Tuple<string, object>(p.Name, Any.CSharpIdentifier(1)));
        }

        public static string AsJson(this Type type, string baseUri, IEnumerable<Tuple<string, object>> propertyValues)
        {
            var instance = Activator.CreateInstance(type);

            instance.SetPropertyValues(propertyValues);

            var jo = JObject.FromObject(instance);
            jo.AddFirst(new JProperty("@odata.context", baseUri + "$metadata#" + type.Name + "s/$entity"));
            jo.Remove("ChangedProperties");
            return jo.ToString();
        }
    }
}
