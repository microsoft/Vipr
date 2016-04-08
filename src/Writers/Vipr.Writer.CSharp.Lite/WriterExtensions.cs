// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Vipr.Writer.CSharp.Lite
{
    public static class WriterExtensions
    {
        private static IDictionary<string, string> _wellKnownCapabilityNames = new Dictionary<string, string>()
        {
            [TermNames.Update] = "Upd",
            [TermNames.UpdateLink] = "Ulk",
            [TermNames.Delete] = "Del",
            [TermNames.DeleteLink] = "Dlk",
            [TermNames.Insert] = "Ins",
//            [TermNames.OdcmInsertLinkTerm] = "Ilk",    // not used by the writer
            [TermNames.Expand] = "Exp",
//            [TermNames.OdcmExpandLinkTerm] = "Elk",    // not used by the writer
        };

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

        public static OdcmBooleanCapability SetBooleanCapability(this OdcmProjection projection, string term, bool value)
        {
            var capability = projection.Capabilities
                                    .SingleOrDefault(c => c.TermName == term)
                                    as OdcmBooleanCapability;

            if (capability != null)
            {
                capability.Value = value;
                return capability;
            }

            capability = new OdcmBooleanCapability(value, term);

            if (!value)
            {
                projection.Capabilities.Add(capability);
            }

            return capability;
        }

        public static string GetProjectionShortForm(this OdcmProjection projection)
        {
            var result = string.Empty;

            var capabilities = projection.WellKnownCapabilities
                                                .OrderBy(c => c.GetShortName());

            foreach (OdcmBooleanCapability capability in capabilities)
            {
                if (capability.Value == true)
                {
                    result = result + "_" + capability.GetShortName();
                }
            }

            return result.Trim('_');
        }

        public static IEnumerable<OdcmProjection> DistinctProjections(this OdcmType odcmType)
        {
            return odcmType.Projections;
        }

        public static string GetShortName(this OdcmCapability capability)
        {
            return _wellKnownCapabilityNames[capability.TermName];
        }
    }
}
