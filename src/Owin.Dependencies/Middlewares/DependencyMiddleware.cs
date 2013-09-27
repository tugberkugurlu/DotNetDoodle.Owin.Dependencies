using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Owin.Dependencies.Middlewares
{
    using Microsoft.Owin.Logging;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class DependencyMiddleware
    {
        private readonly AppFunc _nextFunc;
        private readonly IOwinDependencyResolver _resolver;
        private readonly ILogger _logger;

        public DependencyMiddleware(AppFunc nextFunc, IAppBuilder app, IOwinDependencyResolver resolver)
        {
            _nextFunc = nextFunc;
            _resolver = resolver;
            _logger = app.CreateLogger<DependencyMiddleware>();
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            using (IOwinDependencyScope scope = _resolver.BeginScope())
            {
                env.Add(Constants.OwinDependencyScopeEnvironmentKey, scope);
                await _nextFunc(env);
            }
        }
    }
}