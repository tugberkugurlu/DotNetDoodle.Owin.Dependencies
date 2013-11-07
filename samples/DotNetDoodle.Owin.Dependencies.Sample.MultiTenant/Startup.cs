using Autofac;
using Autofac.Extras.Multitenant;
using Autofac.Integration.WebApi;
using DotNetDoodle.Owin.Dependencies.Autofac;
using DotNetDoodle.Owin.Dependencies.Sample.MultiTenant.Middlewares;
using DotNetDoodle.Owin.Dependencies.Sample.MultiTenant.Repositories;
using Microsoft.Owin.Security.Basic;
using Owin;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Web.Http;

namespace DotNetDoodle.Owin.Dependencies.Sample.MultiTenant
{
    public class Startup
    {
        public static ConcurrentDictionary<Type, ConcurrentBag<string>> TypeOperations = new ConcurrentDictionary<Type, ConcurrentBag<string>>();

        public void Configuration(IAppBuilder app)
        {
            IContainer container = RegisterServices();
            PrincipalTenantIdentificationStrategy tenantStrategy = new PrincipalTenantIdentificationStrategy();
            MultitenantContainer mtc = new MultitenantContainer(tenantStrategy, container);
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultHttpRoute", "api/{controller}");

            app.UseAutofacContainer(mtc)
               .Use<RandomTextMiddleware>()
               .UseWebApiWithContainer(config);
        }

        public IContainer RegisterServices()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterOwinApplicationContainer();

            builder.RegisterType<Repository>()
                   .As<IRepository>()
                   .InstancePerLifetimeScope();

            return builder.Build();
        }

        public class PrincipalTenantIdentificationStrategy : ITenantIdentificationStrategy
        {
            public bool TryIdentifyTenant(out object tenantId)
            {
                throw new NotImplementedException();
            }
        }
    }
}