using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Vipr.Core;

namespace Vipr
{
    internal class ConfigurationProvider : Core.IConfigurationProvider
    {
        private readonly IConfiguration Configuration;
        private ConfigurationProvider()
        {
            var builder = new ConfigurationBuilder();
            if (!string.IsNullOrWhiteSpace(Environment.CurrentDirectory))
                builder.AddXmlFile(Path.Combine(Environment.CurrentDirectory, ".config"), true);

            Configuration = builder.AddEnvironmentVariables().Build();
        }

        public T GetConfiguration<T>()
        {
            return Configuration.Get<T>();
        }

        public static void SetConfigurationOn(object target)
        {
            var configurable = target as IConfigurable;

            if (configurable != null)
            {
                configurable.SetConfigurationProvider(new ConfigurationProvider());
            }
        }
    }
}