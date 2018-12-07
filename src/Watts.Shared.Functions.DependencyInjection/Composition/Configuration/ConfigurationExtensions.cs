using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Watts.Shared.Functions.DependencyInjection.Contracts;
using Watts.Shared.Functions.DependencyInjection.Utils;

namespace Watts.Shared.Functions.DependencyInjection.Composition.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IWebJobsBuilder UseServiceProvider<T>(this IWebJobsBuilder builder)
            where T : IScopedServiceProvider
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddSingleton(typeof(IScopedServiceProvider), typeof(T));;

            builder.Services.AddSingleton(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var serviceProvider = provider.GetRequiredService<IScopedServiceProvider>();
                return new ScopedServiceProvider(serviceProvider.CurrentFor(configuration));
            });

            builder.AddExtension<ImportExtensionProvider>();
            builder.Services.AddSingleton<ImportAttributeBindingProvider>();
            builder.Services.AddServiceDescriptor<IFunctionFilter, FunctionScopeCleanupAttribute>();

            return builder;
        }

        internal static IServiceCollection AddServiceDescriptor<TService, TImplementation>(this IServiceCollection serviceCollection)
            where TService : class 
            where TImplementation : class, TService
        {
            serviceCollection.TryAddEnumerable(
                ServiceDescriptor.Singleton<TService, TImplementation>());
            return serviceCollection;
        }
    }
}