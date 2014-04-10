using System;
using System.ComponentModel;
using DotNetDoodle.Owin.Dependencies;

namespace Owin
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class AppBuilderExtensions
    {
        public static void SetApplicationContainer(this IAppBuilder app, IServiceProvider container)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            app.Properties[Constants.OwinApplicationContainerKey] = container;
        }

        public static IServiceProvider GetApplicationContainer(this IAppBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            return app.Properties[Constants.OwinApplicationContainerKey] as IServiceProvider;
        }
    }
}