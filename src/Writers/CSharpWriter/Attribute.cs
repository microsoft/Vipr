// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Attribute
    {
        public Type Type { get; private set; }

        public IDictionary<string, string> Parameters { get; private set; }

        private Attribute()
        {
            Parameters = new Dictionary<string, string>();
        }

        public static Attribute ForLowerCaseProperty()
        {
            return new Attribute
            {
                Type = new Type(NamesService.GetExtensionTypeName("LowerCasePropertyAttribute"))
            };
        }

        public static Attribute ForMicrosoftOdataClientKey(OdcmEntityClass odcmClass)
        {
            return new Attribute
            {
                Type = new Type(new Identifier("global::Microsoft.OData.Client", "Key")),
                Parameters = odcmClass.Key.ToDictionary<OdcmProperty, string, string>(p => p.Name, p => null)
            };
        }

        public override string ToString()
        {
            var paramString = !Parameters.Keys.Any()
                ? string.Empty
                : string.Format("(\"{0}\")", string.Join("\", \"", Parameters.Keys));

            return string.Format("[{0}{1}]", Type.ToString().Replace("Attribute", string.Empty), paramString);
        }
    }
}
