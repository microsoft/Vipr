// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

namespace Vipr.Core.CodeModel
{
    public abstract class OdcmObject
    {
        public string Name { get; private set; }

        public OdcmObject(string name)
        {
            Name = name;
        }

        public virtual string CanonicalName()
        {
            return Name;
        }

        public static string MakeCanonicalName(string name, string @namespace)
        {
            return string.Format("{1}.{0}", name, @namespace);
        }

        public static string MakeCanonicalName(string name, string @namespace, params OdcmType[] parameters)
        {
            return string.Format("{0}<{1}>",
                MakeCanonicalName(name, @namespace),
                string.Join(",", from parameter in parameters select parameter.CanonicalName()));
        }

        public static string MakeCanonicalName(string name, OdcmObject returnType, params OdcmParameter[] parameters)
        {
            return string.Format("{0}<{1}>:{2}",
                name,
                string.Join(",", from parameter in parameters select parameter.Type.CanonicalName()),
                returnType == null ? "void" : returnType.CanonicalName());
        }
    }
}
