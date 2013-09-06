Owin.Dependencies
=================
An IoC container adapter into OWIN pipeline

## Install Through NuGet

Core package which includes the interfaces and common extensions:

    PM> Install-Package Owin.Dependencies -pre

Autofac IoC container implementation:

    PM> Install-Package Owin.Dependencies.Autofac -pre

ASP.NET Web API adapter:

    PM> Install-Package Owin.Dependencies.Adapters.WebApi -pre

## Sample Snippet

Install the `Owin.Dependencies.Adapters.WebApi`, `Owin.Dependencies.Autofac` and `Autofac.WebApi5` NuGet packages.

### Startup.cs

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

### RandomTextMiddleware.cs

    public class RandomTextMiddleware : OwinMiddleware
    {
        public RandomTextMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            IOwinDependencyScope dependencyScope = context.GetRequestDependencyScope();
            IRepository repository = dependencyScope.GetService(typeof(IRepository)) as IRepository;

            if (context.Request.Path == "/random")
            {
                await context.Response.WriteAsync(repository.GetRandomText());
            }
            else
            {
                context.Response.Headers.Add("X-Random-Sentence", new[] { repository.GetRandomText() });
                await Next.Invoke(context);
            }
        }
    }

### TextsController.cs

    public class TextsController : ApiController
    {
        private readonly IRepository _repo;

        public TextsController(IRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<string> Get()
        {
            return _repo.GetTexts();
        }
    }