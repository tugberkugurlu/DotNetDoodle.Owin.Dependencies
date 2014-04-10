using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Scanning;
using System;

namespace DotNetDoodle.Owin.Dependencies.Autofac
{
    using RuntimeRegistrationDelegate = Action
        <
            Func
                <
                    IServiceProvider, 
                    Tuple<Type, object
                >
            >
        >;

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterOwinApplicationContainer(this ContainerBuilder builder)
        {
            return builder.Register(ctx =>
            {
                ILifetimeScope scope = ctx.Resolve<ILifetimeScope>().BeginLifetimeScope();
                IComponentRegistry registry = scope.ComponentRegistry;
                ContainerBuilder newBuilder = new ContainerBuilder();
                newBuilder.Register(c => new RuntimeRegistrationDelegate(func =>
                {
                    Tuple<Type, object> registerResult = func(scope as IServiceProvider);
                    ContainerBuilder innerNewBuilder = new ContainerBuilder();
                    innerNewBuilder.Register(_ => registerResult.Item2)
                        .As(registerResult.Item1)
                        .ExternallyOwned();

                    innerNewBuilder.Update(registry);

                })).As<RuntimeRegistrationDelegate>().InstancePerLifetimeScope();

                newBuilder.Update(registry);

                return scope as IServiceProvider;

            }).As<IServiceProvider>() as IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>;
        }
    }
}