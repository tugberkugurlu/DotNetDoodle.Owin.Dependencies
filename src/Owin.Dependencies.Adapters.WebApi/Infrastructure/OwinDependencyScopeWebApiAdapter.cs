using System;
using System.Collections.Generic;
using System.Security;
using System.Web.Http.Dependencies;

namespace Owin.Dependencies.Adapters.WebApi.Infrastructure
{
    internal class OwinDependencyScopeWebApiAdapter : IDependencyScope
    {
        private bool _disposed;
        private readonly IServiceProvider _requestContainer;

        [SecuritySafeCritical]
        ~OwinDependencyScopeWebApiAdapter()
        {
            Dispose(false);
        }

        public OwinDependencyScopeWebApiAdapter(IServiceProvider requestContainer)
        {
            _requestContainer = requestContainer;
        }

        [SecuritySafeCritical]
        public object GetService(Type serviceType)
        {
            return _requestContainer.GetService(serviceType);
        }

        [SecuritySafeCritical]
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _requestContainer.GetService(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
        }

        [SecuritySafeCritical]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_requestContainer != null && _requestContainer is IDisposable)
                    {
                        (_requestContainer as IDisposable).Dispose();
                    }
                }
                _disposed = true;
            }
        }
    }
}