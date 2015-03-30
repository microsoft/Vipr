using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            _service = MockServiceRepository.GetServiceMockForPort(portNumber);

            if (Debugger.IsAttached) appBuilder.Use(typeof (LoggingMiddleware));

            appBuilder.Use(typeof (BufferedBodyMiddleware));

            appBuilder.Run(Invoke);
        }

        public async Task Invoke(IOwinContext context)
        {
            await _service.Invoke(context);
        }
    }
}
