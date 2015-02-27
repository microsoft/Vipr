using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core;

namespace ViprUnitTests
{
    public class TestConfigurable : IConfigurable
    {
        public IConfigurationProvider ConfigurationProvider { get; private set; }

        public void SetConfigurationProvider(IConfigurationProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;
        }
    }
}
