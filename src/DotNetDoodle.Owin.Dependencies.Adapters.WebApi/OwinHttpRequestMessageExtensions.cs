using DotNetDoodle.Owin.Dependencies.Adapters.WebApi.Infrastructure;
using System.Web.Http.Dependencies;
using Owin;

namespace System.Net.Http
{
    internal static class OwinHttpRequestMessageExtensions
    {
        internal static IDependencyScope GetOwinDependencyScope(this HttpRequestMessage request)
        {
            IServiceProvider requestContainer = request.GetOwinContext().Environment.GetRequestContainer();
            return new OwinDependencyScopeWebApiAdapter(requestContainer);
        }
    }
}