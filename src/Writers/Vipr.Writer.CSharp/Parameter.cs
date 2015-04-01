// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Parameter
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public Type Type { get; private set; }

        public string DefaultValue { get; private set; }

        private Parameter()
        {
        }

        public Parameter(Type type, string name, string defaultValue = null)
        {
            Type = type;
            Name = name;
            DefaultValue = defaultValue;
        }

        public static IEnumerable<Parameter> Empty { get { return new List<Parameter>(); } }

        public static Parameter FromProperty(OdcmProperty arg)
        {
            return new Parameter
            {
                Name = arg.Name.ToLowerCamelCase(),
                Type = new Type(NamesService.GetConcreteTypeName(arg.Type))
            };
        }

        public static Parameter FromOdcmParameter(OdcmParameter odcmParameter)
        {
            return new Parameter
            {
                Name = odcmParameter.Name,
                Description = odcmParameter.Description,
                Type = odcmParameter.IsCollection
                    ? new Type(new Identifier("System.Collections.Generic", "ICollection"), new Type(NamesService.GetConcreteTypeName(odcmParameter.Type)))
                    : TypeService.GetParameterType(odcmParameter)
            };
        }
    }
}