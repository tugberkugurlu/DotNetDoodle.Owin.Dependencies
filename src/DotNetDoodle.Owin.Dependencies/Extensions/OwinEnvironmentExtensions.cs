using System;
using System.Collections.Generic;
using System.ComponentModel;
using DotNetDoodle.Owin.Dependencies;

namespace Owin
{
    using PerRequestContainerTuple = Tuple
        <
            IServiceProvider, // Per-request Container
            Action<Func<IServiceProvider, Tuple<Type, object>>> // Runtime Registration Delegate
        >;

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

            IServiceProvider requestContainer = 
                appContainer.GetService(typeof(IServiceProvider)) as IServiceProvider;

            if (!(requestContainer is IDisposable))
            {
                throw new NotSupportedException("An IServiceProvider implementation which doesn't implement IDisposable is not supported");
            }

            Action<Func<IServiceProvider, Tuple<Type, object>>> runtimeRegistrationDelegate =
                requestContainer.GetService(typeof(Action<Func<IServiceProvider, Tuple<Type, object>>>))
                    as Action<Func<IServiceProvider, Tuple<Type, object>>>;

            if (runtimeRegistrationDelegate == null)
            {
                throw new NotSupportedException("Could not resolve an instance of 'Action<IServiceProvider>' for the runtime registration delegate.");
            }

            // Set request container
            environment[Constants.OwinRequestContainerEnvironmentKey] =
                Tuple.Create<IServiceProvider, Action<Func<IServiceProvider, Tuple<Type, object>>>>(
                    requestContainer, 
                    runtimeRegistrationDelegate);

            return requestContainer as IDisposable;
        }

        public static IServiceProvider GetRequestContainer(
            this IDictionary<string, object> environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            PerRequestContainerTuple perRequestContainerTuple = 
                environment[Constants.OwinRequestContainerEnvironmentKey] as PerRequestContainerTuple;

            return perRequestContainerTuple != null
                ? perRequestContainerTuple.Item1
                : null;
        }

        public static Action<Func<IServiceProvider, Tuple<Type, object>>> GetRuntimeRegistrationDelegate(
            this IDictionary<string, object> environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            PerRequestContainerTuple perRequestContainerTuple =
                environment[Constants.OwinRequestContainerEnvironmentKey] as PerRequestContainerTuple;

            return perRequestContainerTuple != null
                ? perRequestContainerTuple.Item2
                : null;
        }
    }
}
