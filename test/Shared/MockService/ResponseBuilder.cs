using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Microsoft.MockService
{
    public class ResponseBuilder
    {
        private MockService _mockService;
        private Expression<Func<IOwinContext, bool>> _requestValidator;

        internal ResponseBuilder(MockService mockService, Expression<Func<IOwinContext, bool>> requestValidator)
        {
            _requestValidator = requestValidator;
            _mockService = mockService;
        }

        public MockService RespondWith(Action<IOwinContext> responseConfiguration)
        {
            if (responseConfiguration == null) throw new ArgumentNullException("responseConfiguration");

            Func<IOwinContext, Task> responseFunction = c =>
            {
                responseConfiguration(c);

                return Task.FromResult<object>(null);
            };

            _mockService.Setup(_requestValidator, responseFunction);

            return _mockService;
        }

        public MockService RespondWith(Action<IOwinContext, string> responseConfiguration)
        {
            if (responseConfiguration == null) throw new ArgumentNullException("responseConfiguration");

            Func<IOwinContext, Task> responseFunction = c =>
            {
                responseConfiguration(c, _mockService.GetBaseAddress());

                return Task.FromResult<object>(null);
            };

            _mockService.Setup(_requestValidator, responseFunction);

            return _mockService;
        }
    }
}