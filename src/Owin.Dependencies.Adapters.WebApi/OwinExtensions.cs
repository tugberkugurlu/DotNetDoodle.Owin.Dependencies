using Owin.Dependencies;
using Owin.Dependencies.Adapters.WebApi;
using Owin.Dependencies.Adapters.WebApi.Infrastructure;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Web.Http;

namespace Owin
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class OwinExtensions
    {
        public static IAppBuilder UseWebApiWithContainer(this IAppBuilder app, HttpConfiguration configuration)
        {
            IServiceProvider appContainer = app.GetApplicationContainer();
            configuration.DependencyResolver = new OwinDependencyResolverWebApiAdapter(appContainer);
            HttpServer httpServer = new OwinDependencyScopeHttpServerAdapter(configuration);
            return app.UseWebApi(httpServer);
        }

        public static IAppBuilder UseWebApiWithContainer(this IAppBuilder app, HttpConfiguration configuration, HttpMessageHandler dispatcher)
        {
            IServiceProvider appContainer = app.GetApplicationContainer();
            configuration.DependencyResolver = new OwinDependencyResolverWebApiAdapter(appContainer);
            HttpServer httpServer = new OwinDependencyScopeHttpServerAdapter(configuration, dispatcher);
            return app.UseWebApi(httpServer);
        }
    }
}