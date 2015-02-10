using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ODataV4TestService.SelfHost
{
    public class MockScenario : ScenarioBase<MockScenario>, IDisposable
    {
        private readonly List<Tuple<Expression<Func<IOwinContext, bool>>, Func<IOwinContext, Task>>> _handlers;
        private readonly IList<Expression<Func<IOwinContext, bool>>> _unusedHandlers;
        private readonly bool _ignoreUnusedHandlers;

        public MockScenario(bool ignoreUnusedHandlers = false)
        {
            _handlers = new List<Tuple<Expression<Func<IOwinContext, bool>>, Func<IOwinContext, Task>>>();
            _unusedHandlers = new List<Expression<Func<IOwinContext, bool>>>();
            _ignoreUnusedHandlers = ignoreUnusedHandlers;
        }

        public MockScenario Setup(Expression<Func<IOwinContext, bool>> condition, Func<IOwinContext, Task> response)
        {
            _handlers.Add(new Tuple<Expression<Func<IOwinContext, bool>>, Func<IOwinContext, Task>>(condition, response));
            _unusedHandlers.Add(condition);

            Debug.WriteLine(new ConstantMemberEvaluationVisitor().Visit(condition));

            return this;
        }

        public MockScenario Setup(Expression<Func<IOwinContext, bool>> condition, Action<IOwinContext> response)
        {
            Setup(condition, context =>
            {
                response(context);
                return Task.FromResult<object>(null);
            });

            return this;
        }

        public MockScenario Setup(Expression<Func<IOwinContext, bool>> condition, Action<string, IOwinContext> response)
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

        public new MockScenario Start()
        {
            base.Start<MockStartup>();

            return this;
        }

        public new void Dispose()
        {
            if (!_ignoreUnusedHandlers && _unusedHandlers.Any())
                throw new InvalidOperationException(
                    String.Format("Mock Server {0} expected requests \r\n\r\n {1} \r\n\r\n but they were not made.",
                        GetBaseAddress(),
                        _unusedHandlers.Select(h => new ConstantMemberEvaluationVisitor().Visit(h).ToString())
                            .Aggregate((c, n) => c + "\r\n" + n)));

            base.Dispose();
        }
    }
}