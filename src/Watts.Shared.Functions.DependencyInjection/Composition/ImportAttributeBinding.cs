using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Watts.Shared.Functions.DependencyInjection.Contracts;

namespace Watts.Shared.Functions.DependencyInjection.Composition
{
    internal class ImportAttributeBinding : IBinding
    {
        private readonly ScopedServiceProvider _serviceProvider;
        private readonly Type _parameterType;

        public ImportAttributeBinding(
            ScopedServiceProvider serviceProvider,
            Type parameterType)
        {
            _serviceProvider = serviceProvider
                               ?? throw new ArgumentNullException(nameof(serviceProvider));
            _parameterType = parameterType
                             ?? throw new ArgumentNullException(nameof(parameterType));
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
            => Task.FromResult((IValueProvider) new ImportAttributeValueProvider(value));


        public Task<IValueProvider> BindAsync(BindingContext context)
            => BindAsync(
                _serviceProvider.ResolveFor(context.FunctionInstanceId, _parameterType),
                    context.ValueContext);


        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor()
            {
                Type = _parameterType.ToString(),
                Name = _parameterType.Name,
            };
        }

        public bool FromAttribute => true;
    }
}