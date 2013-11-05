using Microsoft.Owin.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;

namespace DotNetDoodle.Owin.Dependencies.Middlewares
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class ContainerMiddleware
    {
        private readonly AppFunc _nextFunc;
        private readonly ILogger _logger;
        private readonly IAppBuilder _app;

        public ContainerMiddleware(AppFunc nextFunc, IAppBuilder app)
        {
            _nextFunc = nextFunc;
            _app = app;
            _logger = app.CreateLogger<ContainerMiddleware>();
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            using (IDisposable scope = env.SetRequestContainer(_app))
            {
                await _nextFunc(env);
            }
        }
    }
}