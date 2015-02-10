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

        public static OdcmProperty Rename(this OdcmProperty originalProperty, string newName)
        {
            var index = originalProperty.Class.Properties.IndexOf(originalProperty);

            originalProperty.Class.Properties[index] =
                new OdcmProperty(newName)
                {
                    Class = originalProperty.Class,
                    ReadOnly = originalProperty.ReadOnly,
                    Type = originalProperty.Type,
                    ContainsTarget = originalProperty.ContainsTarget,
                    IsCollection = originalProperty.IsCollection,
                    IsLink = originalProperty.IsLink,
                    IsNullable = originalProperty.IsNullable,
                    IsRequired = originalProperty.IsRequired
                };

            if (originalProperty.Class.Key.Contains(originalProperty))
            {
                var keyIndex = originalProperty.Class.Key.IndexOf(originalProperty);
                originalProperty.Class.Key[keyIndex] = originalProperty.Class.Properties[index];
            }

            return originalProperty.Class.Properties[index];
        }

        public static string GetDefaultEntitySetName(this OdcmClass odcmClass)
        {
            return odcmClass.Name + "s";
        }

        public static string GetDefaultEntitySetPath(this OdcmClass odcmClass)
        {
            return "/" + odcmClass.GetDefaultEntitySetName();
        }

        public static string GetDefaultEntityPath(this OdcmClass odcmClass, IEnumerable<Tuple<string, object>> keyValues = null)
        {
            keyValues = keyValues ?? odcmClass.GetSampleKeyArguments().ToArray();
            
            return string.Format("{0}({1})", odcmClass.GetDefaultEntitySetPath(), ODataKeyPredicate.AsString(keyValues.ToArray()));
        }

        public static string GetDefaultEntityPropertyPath(this OdcmClass odcmClass, OdcmProperty property, IEnumerable<Tuple<string, object>> keyValues = null)
        {
            return odcmClass.GetDefaultEntityPropertyPath(property.Name, keyValues);
        }

        public static string GetDefaultEntityPropertyPath(this OdcmClass odcmClass, string propertyName, IEnumerable<Tuple<string, object>> keyValues = null)
        {
            return string.Format("{0}/{1}", odcmClass.GetDefaultEntityPath(keyValues), propertyName);
        }
    }
}
