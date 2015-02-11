using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.MockService.SelfHost;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Linq;
using ODataV4TestService.SelfHost;

namespace Microsoft.MockService
{
    public class MockService : IDisposable
    {
        private readonly int _portNumber;
        private IDisposable _host;
        private readonly List<Tuple<Expression<Func<IOwinContext, bool>>, Func<IOwinContext, Task>>> _handlers;
        private readonly IList<Expression<Func<IOwinContext, bool>>> _unusedHandlers;
        private readonly bool _ignoreUnusedHandlers;

        public MockService(bool ignoreUnusedHandlers = false)
        {
            _portNumber = PortRepository.GetFreePortNumber();
            _handlers = new List<Tuple<Expression<Func<IOwinContext, bool>>, Func<IOwinContext, Task>>>();
            _unusedHandlers = new List<Expression<Func<IOwinContext, bool>>>();
            _ignoreUnusedHandlers = ignoreUnusedHandlers;

            MockServiceRepository.Register(_portNumber, this);
        }

        public MockService Setup(Expression<Func<IOwinContext, bool>> condition, Func<IOwinContext, Task> response)
        {
            _handlers.Add(new Tuple<Expression<Func<IOwinContext, bool>>, Func<IOwinContext, Task>>(condition, response));
            _unusedHandlers.Add(condition);

            Debug.WriteLine(new ConstantMemberEvaluationVisitor().Visit(condition));

            return this;
        }

        public MockService Setup(Expression<Func<IOwinContext, bool>> condition, Action<IOwinContext> response)
        {
            Setup(condition, context =>
            {
                response(context);
                return Task.FromResult<object>(null);
            });

            return this;
        }

        public MockService Setup(Expression<Func<IOwinContext, bool>> condition, Action<string, IOwinContext> response)
        {
            Setup(condition, context =>
            {
                response(this.GetBaseAddress(), context);
                return Task.FromResult<object>(null);
            });

            return this;
        }

        public Task Invoke(IOwinContext context)
        {
            try
            {
                foreach (var handler in _handlers)
                {
                    if (!handler.Item1.Compile().Invoke(context)) continue;

                    _unusedHandlers.Remove(handler.Item1);
                    return handler.Item2(context);
                }

                context.Response.StatusCode = 400;
                Debug.WriteLine("No handler for request\n\r{0} {1}", context.Request.Method, context.Request.Path);
                return Task.FromResult<object>(null);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(JObject.FromObject(e).ToString());
                return Task.FromResult<object>(null);
            }
        }

        public string GetBaseAddress()
        {
            return String.Format("http://localhost:{0}/", _portNumber);
        }

        public MockService Start()
        {
            _host = WebApp.Start<MockStartup>(GetBaseAddress());

            return this;
        }

        public void Dispose()
        {
            _host.Dispose();

            if (!_ignoreUnusedHandlers && _unusedHandlers.Any())
                throw new InvalidOperationException(
                    String.Format("Mock Server {0} expected requests \r\n\r\n {1} \r\n\r\n but they were not made.",
                        GetBaseAddress(),
                        _unusedHandlers.Select(h => new ConstantMemberEvaluationVisitor().Visit(h).ToString())
                            .Aggregate((c, n) => c + "\r\n" + n)));
        }
    }
}