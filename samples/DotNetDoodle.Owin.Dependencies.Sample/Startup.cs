using Autofac;
using Autofac.Integration.WebApi;
using DotNetDoodle.Owin.Dependencies.Autofac;
using DotNetDoodle.Owin.Dependencies.Sample.Middlewares;
using DotNetDoodle.Owin.Dependencies.Sample.Repositories;
using Owin;
using System.Reflection;
using System.Web.Http;

namespace DotNetDoodle.Owin.Dependencies.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IContainer container = RegisterServices();
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultHttpRoute", "api/{controller}");

            app.UseAutofacContainer(container)
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
    }
}