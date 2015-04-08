// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core;
using Vipr.Writer.CSharp.Lite.Settings;

namespace Vipr.Writer.CSharp.Lite
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
