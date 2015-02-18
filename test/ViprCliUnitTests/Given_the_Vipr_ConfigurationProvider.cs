using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Newtonsoft.Json;
using Vipr;
using Xunit;

namespace ViprUnitTests
{
    public class Given_the_Vipr_ConfigurationProvider
    {
        private readonly string _workingDirectory;

        public Given_the_Vipr_ConfigurationProvider()
        {
            _workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        [Fact]
        public void It_loads_configuration_from_the_dotConfig_directory()
        {
            var configDirectory = Path.Combine(_workingDirectory, ".config");

            try
            {
                var testSettings = Any.TestSettings();

                WriteTestSettingsJson(configDirectory, testSettings);

                var configurable = new TestConfigurable();

                ConfigurationProvider.SetConfigurationOn(configurable);

                configurable.ConfigurationProvider.Should()
                    .NotBeNull("Because the ConfigurationProvider should call SetConfigurationProviderOn");

                var providedSettings = configurable.ConfigurationProvider.GetConfiguration<TestSettings>();

                providedSettings.Should().Be(testSettings);

            }
            finally
            {
                Directory.Delete(configDirectory, true);
            }
        }

        private string WriteTestSettingsJson(string configDirectory, TestSettings testSettings)
        {
            if (!Directory.Exists(configDirectory))
                Directory.CreateDirectory(configDirectory);

            var configFilePath = Path.Combine(configDirectory, "TestSettings.json");

            if (File.Exists(configFilePath)) File.Delete(configFilePath);

            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(testSettings, serializerSettings));
            return configDirectory;
        }
    }
}
