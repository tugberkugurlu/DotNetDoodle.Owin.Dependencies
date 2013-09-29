using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using System;

namespace Owin.Dependencies.Autofac
{
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterOwinApplicationContainer(this ContainerBuilder builder)
        {
            return builder.Register(ctx => ctx.Resolve<ILifetimeScope>().BeginLifetimeScope() as IServiceProvider).As<IServiceProvider>() as IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>;
        }
    }
}