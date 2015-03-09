using System;
using System.IO;
using FluentAssertions;
using Its.Configuration;
using Microsoft.Its.Recipes;
using Newtonsoft.Json;
using Vipr;
using ViprUnitTests;
using Xunit;

namespace ViprCliUnitTests
{
    public class Given_a_ConfigurationProvider
    {
        private string _workingDirectory;

        public Given_a_ConfigurationProvider()
        {
            _workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        [Fact]
        public void It_loads_configuration_from_the_dotConfig_directory()
        {
            ValidateLoadingConfiguraiton<TestSettings>(Any.TestSettings());
        }

        [Fact]
        public void When_executed_from_a_different_directory_It_loads_configuration_from_the_dotConfig_directory()
        {
            _workingDirectory = Path.Combine(Environment.CurrentDirectory, Any.Word());

            Directory.CreateDirectory(_workingDirectory);

            var currentDirectory = Environment.CurrentDirectory;

            try
            {
                Environment.CurrentDirectory = _workingDirectory;

                ValidateLoadingConfiguraiton<TestSettings2>(Any.TestSettings2());
            }
            finally
            {
                Environment.CurrentDirectory = currentDirectory;

                Directory.Delete(_workingDirectory, true);

                Settings.SettingsDirectory = Path.Combine(currentDirectory, ".config");
            }

        }

        private void ValidateLoadingConfiguraiton<T>(T testSettings) where T : TestSettings, new()
        {
            var configDirectory = Path.Combine(_workingDirectory, ".config");

            try
            {
                WriteTestSettingsJson(configDirectory, testSettings);

                var configurable = new TestConfigurable();

                ConfigurationProvider.SetConfigurationOn(configurable);

                configurable.ConfigurationProvider.Should()
                    .NotBeNull("Because the ConfigurationProvider should call SetConfigurationProviderOn");

                var providedSettings = configurable.ConfigurationProvider.GetConfiguration<T>();

                providedSettings.Should().Be(testSettings);
            }
            finally
            {
                Directory.Delete(configDirectory, true);
            }
        }


        private void WriteTestSettingsJson(string configDirectory, TestSettings testSettings)
        {
            if (!Directory.Exists(configDirectory))
                Directory.CreateDirectory(configDirectory);

            var configFilePath = Path.Combine(configDirectory, String.Format("{0}.json", testSettings.GetType().Name));

            if (File.Exists(configFilePath)) File.Delete(configFilePath);

            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var settingsJson = JsonConvert.SerializeObject(testSettings, serializerSettings);

            File.WriteAllText(configFilePath, settingsJson);
        }
    }
}
