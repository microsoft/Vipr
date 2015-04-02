// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp
{
    public static class WriterExtensions
    {
        public static string ToParametersString(this IEnumerable<Parameter> parameters)
        {
            if (parameters == null)
                return String.Empty;

            var sb = new List<string>();

            foreach (var parameter in parameters)
            {
                var defaultValue = parameter.DefaultValue == null ? String.Empty : " = " + parameter.DefaultValue;

                sb.Add(string.Format("{0} {1}{2}", parameter.Type, parameter.Name, defaultValue));
            }

            return String.Join(", ", sb);
        }

        public static string ToArgumentString(this IEnumerable<Parameter> parameters)
        {
            if (parameters == null)
                return String.Empty;

            var sb = new List<string>();

            foreach (var parameter in parameters)
            {
                sb.Add(parameter.Name);
            }

            return String.Join(", ", sb);
        }

        public static string ToEquivalenceString(this IDictionary<Parameter, OdcmProperty> parameterToPropertyMap, string instanceName)
        {
            if (parameterToPropertyMap == null)
                return "true";

            var sb = new List<string>();

            foreach (var parameter in parameterToPropertyMap.Keys)
            {
                sb.Add(String.Format("{0}.{1} == {2}", instanceName,
                    NamesService.GetPropertyName(parameterToPropertyMap[parameter]), parameter.Name));
            }

            return String.Join(" && ", sb);
        }
    }
}
