using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.MockService.Middleware;
using Microsoft.Owin;
using Owin;

namespace Microsoft.MockService.SelfHost
{
    class MockStartup
    {
        private MockService _service;
        public void Configuration(IAppBuilder appBuilder)
        {
            var portNumber =
                Int32.Parse(
                    (string)((IList<IDictionary<string, object>>)appBuilder.Properties["host.Addresses"])[0]["port"]);

            _service = MockServiceRepository.GetScenario(portNumber) as MockService;

            appBuilder.Use(typeof (LoggingMiddleware));

            appBuilder.Run(Invoke);
        }

        public async Task Invoke(IOwinContext context)
        {
            await _service.Invoke(context);
        }
    }
}
