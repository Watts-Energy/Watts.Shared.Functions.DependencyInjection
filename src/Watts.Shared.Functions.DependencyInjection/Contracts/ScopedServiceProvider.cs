using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Watts.Shared.Functions.DependencyInjection.Contracts
{
    internal class ScopedServiceProvider
    {
        private readonly ConcurrentDictionary<Guid, IServiceScope> _scopes =
            new ConcurrentDictionary<Guid, IServiceScope>();

        private readonly IServiceProvider _serviceProvider;

        public ScopedServiceProvider(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ??
                               throw new ArgumentNullException(nameof(serviceProvider));
        }

        internal object ResolveFor(Guid functionId, Type resolvable)
        {
            var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();

            if (scopeFactory == null)
            {
                return _serviceProvider.GetRequiredService(resolvable);
            }

            var serviceScope = _scopes.GetOrAdd(functionId, _ => scopeFactory.CreateScope());

            return serviceScope != null
                ? serviceScope.ServiceProvider.GetRequiredService(resolvable)
                : _serviceProvider.GetRequiredService(resolvable);            
        }

        internal void DisposeFor(Guid functionId)
        {
            if (_scopes.Any() && _scopes.ContainsKey(functionId))
            {
                _scopes[functionId]?.Dispose();
            }
        }
    }
}