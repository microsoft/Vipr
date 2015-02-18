using Its.Configuration;
using Vipr.Core;

namespace Vipr
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private static IConfigurationProvider _instance;

        private static IConfigurationProvider Instance
        {
            get { return _instance ?? (_instance = new ConfigurationProvider()); }
        }

        public T GetConfiguration<T>()
        {
            return Settings.Get<T>();
        }

        public static void SetConfigurationOn(object target)
        {
            var configurable = target as IConfigurable;

            if (configurable != null)
            {
                configurable.SetConfigurationProvider(Instance);
            }
        }
    }
}