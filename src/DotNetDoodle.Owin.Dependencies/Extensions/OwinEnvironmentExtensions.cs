using Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DotNetDoodle.Owin.Dependencies
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class OwinEnvironmentExtensions
    {
        public static IDisposable SetRequestContainer(this IDictionary<string, object> environment, IAppBuilder app)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            IServiceProvider appContainer = app.GetApplicationContainer();
            if (appContainer == null)
            {
                throw new InvalidOperationException("There is no application container registered to resolve a request container");
            }

            IServiceProvider requestContainer = appContainer.GetService(typeof(IServiceProvider)) as IServiceProvider;
            if (!(requestContainer is IDisposable))
            {
                throw new NotSupportedException("An IServiceProvider implementation which doesn't implement IDisposable is not supported");
            }

            // Set request container
            environment[Constants.OwinRequestContainerEnvironmentKey] = requestContainer;

            return requestContainer as IDisposable;
        }

        public static IServiceProvider GetRequestContainer(this IDictionary<string, object> environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            return environment[Constants.OwinRequestContainerEnvironmentKey] as IServiceProvider;
        }
    }
}
