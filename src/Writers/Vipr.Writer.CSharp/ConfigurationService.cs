// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CSharpWriter.Settings;
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

        public static CSharpWriterSettings Settings 
        { 
            get
            {
                return s_configurationProvider != null
                    ? s_configurationProvider.GetConfiguration<CSharpWriterSettings>()
                    : new CSharpWriterSettings();
            } 
        }
    }
}
