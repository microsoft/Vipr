using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace ODataV4TestService.SelfHost
{
    class MockStartup
    {
        private MockScenario _scenario;
        public void Configuration(IAppBuilder appBuilder)
        {
            var portNumber =
                Int32.Parse(
                    (string)((IList<IDictionary<string, object>>)appBuilder.Properties["host.Addresses"])[0]["port"]);

            _scenario = ScenarioRepository.GetScenario(portNumber) as MockScenario;

            appBuilder.Use(typeof (LoggingMiddleware));

            appBuilder.Run(Invoke);
        }

        public async Task Invoke(IOwinContext context)
        {
            await _scenario.Invoke(context);
        }
    }
}
