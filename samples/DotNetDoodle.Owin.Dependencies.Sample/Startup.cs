using System.Collections.Generic;
using Autofac;
using Autofac.Integration.WebApi;
using DotNetDoodle.Owin.Dependencies.Autofac;
using DotNetDoodle.Owin.Dependencies.Sample.Middlewares;
using DotNetDoodle.Owin.Dependencies.Sample.Repositories;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Web.Http;

namespace DotNetDoodle.Owin.Dependencies.Sample
{
    public class Startup
    {
        public static ConcurrentDictionary<Type, ConcurrentBag<string>> TypeOperations = new ConcurrentDictionary<Type, ConcurrentBag<string>>();

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

            builder.RegisterType<RequestHandler>()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
    }

    public class RequestHandler : IDisposable
    {
        public RequestHandler(IDictionary<string, object> owinEnvironment)
        {
            Console.WriteLine("RequestHandler ctor");
            OwinRequest req = new OwinRequest(owinEnvironment);
            Console.WriteLine(req.Uri.AbsoluteUri);
        }

        public void Dispose()
        {
            Console.WriteLine("RequestHandler dispose");
        }
    }
}