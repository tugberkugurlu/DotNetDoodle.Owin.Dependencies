using Owin.Dependencies;
using Owin.Dependencies.Middlewares;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Owin
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class OwinExtensions
    {
        public static IAppBuilder UseContainer(this IAppBuilder app, IServiceProvider appContainer)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            if (appContainer == null)
            {
                throw new ArgumentNullException("appContainer");
            }

            app.SetApplicationContainer(appContainer);

            return app.Use(new Func<AppFunc, AppFunc>(nextApp => new ContainerMiddleware(nextApp, app).Invoke));
        }
    }
}