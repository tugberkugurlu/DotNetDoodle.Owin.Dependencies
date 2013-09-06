using Autofac;
using Autofac.Integration.WebApi;
using Owin.Dependencies.Autofac;
using Owin.Dependencies.Sample.Middlewares;
using Owin.Dependencies.Sample.Repositories;
using System.Reflection;
using System.Web.Http;

namespace Owin.Dependencies.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IContainer container = RegisterServices();
            AutofacOwinDependencyResolver resolver = new AutofacOwinDependencyResolver(container);

            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultHttpRoute", "api/{controller}");

            app.UseDependencyResolver(resolver)
               .Use<RandomTextMiddleware>()
               .UseWebApiWithOwinDependencyResolver(resolver, config);
        }

        public IContainer RegisterServices()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<Repository>()
                   .As<IRepository>()
                   .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}