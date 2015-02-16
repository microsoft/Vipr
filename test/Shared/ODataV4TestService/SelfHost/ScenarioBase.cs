// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData.Builder;

namespace ODataV4TestService.SelfHost
{
    public abstract class ScenarioBase<T> : IStartedScenario where T : ScenarioBase<T>
    {
        private int _portNumber;
        private IDisposable _host;
        private readonly HttpConfiguration _httpConfiguration;
        internal readonly UnityContainer Container;
        private bool _started = false;

        public HttpConfiguration GetHttpConfiguration()
        {
            if (IsConfigurable) throw new InvalidOperationException("Scenario not started.");

            return _httpConfiguration;
        }

        internal ScenarioBase()
        {
            _portNumber = PortRepository.GetFreePortNumber();

            ScenarioRepository.Register(_portNumber, this);

            Container = new UnityContainer();

            _httpConfiguration = new HttpConfiguration
            {
                DependencyResolver = new UnityDependencyResolver(Container)
            };
        }

        public virtual void Start()
        {
            Start<WebApiStartup>();
        }

        protected void Start<TStartup>()
        {
            _started = true;

            WebApiConfig.Register(_httpConfiguration);

            _host = WebApp.Start<TStartup>(GetBaseAddress());
        }

        public string GetBaseAddress()
        {
            return String.Format("http://localhost:{0}/", _portNumber);
        }

        public void Dispose()
        {
            _host.Dispose();

            ScenarioRepository.Unregister(_portNumber);
        }

        public T WithCustomPort(int portNumber)
        {
            ScenarioRepository.Unregister(_portNumber);

            _portNumber = portNumber;

            ScenarioRepository.Register(_portNumber, this);

            return (T)this;
        }

        internal bool IsConfigurable
        {
            get { return !_started; }
        }

        public HttpClient GetHttpClient()
        {
            return new HttpClient { BaseAddress = new Uri(GetBaseAddress()) };
        }
    }
}