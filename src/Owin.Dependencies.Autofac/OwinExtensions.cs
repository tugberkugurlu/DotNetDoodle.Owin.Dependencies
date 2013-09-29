using Autofac;
using System;

namespace Owin
{
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class OwinExtensions
    {
        public static IAppBuilder UseAutofacContainer(this IAppBuilder app, IContainer container) 
        {
            IServiceProvider appContainer = container as IServiceProvider;
            if (appContainer == null)
            {
                throw new NotSupportedException("An IContainer implementation which doesn't implement IServiceProvider is not supported");
            }

            return app.UseContainer(appContainer);
        }
    }
}