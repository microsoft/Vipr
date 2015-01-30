// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using Vipr.Core;

namespace CSharpWriter
{
    public static class ConfigurationService
    {
        private static IConfigurationProvider s_configurationProvider;

        public static void Initialize(IConfigurationProvider configurationProvider)
        {
            s_configurationProvider = configurationProvider;
        }

        public static IImmutableDictionary<string, string> OdcmNamespaceToProxyNamespace
        {
            get
            {
                return s_configurationProvider != null && s_configurationProvider.OdcmNamespaceToProxyNamespace != null
                    ? s_configurationProvider.OdcmNamespaceToProxyNamespace
                    : ImmutableDictionary<string, string>.Empty;
            }
        }

        public static IImmutableDictionary<string, IImmutableDictionary<string, string>> OdcmClassNameToProxyClassName
        {
            get
            {
                return s_configurationProvider != null && s_configurationProvider.OdcmClassNameToProxyClassName != null
                    ? s_configurationProvider.OdcmClassNameToProxyClassName
                    : ImmutableDictionary<string, IImmutableDictionary<string, string>>.Empty;
            }
        }

        public static string NamespacePrefix
        {
            get
            {
                return s_configurationProvider != null
                       && !String.IsNullOrWhiteSpace(s_configurationProvider.NamespacePrefix)
                    ? s_configurationProvider.NamespacePrefix
                    : null;
            }
        }

        public static bool OmitFetcherUpcastMethods
        {
            get
            {
                return s_configurationProvider != null && s_configurationProvider.OmitFetcherCastMethods;
            }
        }

        public static bool ForcePropertyPascalCasing
        {
            get
            {
                return s_configurationProvider != null && s_configurationProvider.ForcePropertyPascalCasing;
            }
        }
    }
}
