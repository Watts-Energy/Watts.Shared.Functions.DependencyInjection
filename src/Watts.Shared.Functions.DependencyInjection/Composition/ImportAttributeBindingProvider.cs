using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Watts.Shared.Functions.DependencyInjection.Contracts;

namespace Watts.Shared.Functions.DependencyInjection.Composition
{
    internal class ImportAttributeBindingProvider : IBindingProvider
    {
        private readonly ScopedServiceProvider _serviceProvider;

        public ImportAttributeBindingProvider(ScopedServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            return Task.FromResult(
                (IBinding) new ImportAttributeBinding(
                    _serviceProvider, 
                    context.Parameter.ParameterType));
        }
    }
}