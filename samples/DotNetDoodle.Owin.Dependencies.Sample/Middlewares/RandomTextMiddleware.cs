using Microsoft.Owin;
using DotNetDoodle.Owin.Dependencies.Sample.Repositories;
using System;
using System.Threading.Tasks;

namespace DotNetDoodle.Owin.Dependencies.Sample.Middlewares
{
    public class RandomTextMiddleware : OwinMiddleware
    {
        public RandomTextMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            IServiceProvider requestContainer = context.Environment.GetRequestContainer();
            IRepository repository = requestContainer.GetService(typeof(IRepository)) as IRepository;

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
}