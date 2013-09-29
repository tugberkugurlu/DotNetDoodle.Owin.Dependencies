using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owin.Dependencies
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