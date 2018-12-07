using Microsoft.Azure.WebJobs.Host.Config;

namespace Watts.Shared.Functions.DependencyInjection.Composition.Configuration
{
    internal class ImportExtensionProvider : IExtensionConfigProvider
    {
        private readonly ImportAttributeBindingProvider _bindingProvider;

        public ImportExtensionProvider(ImportAttributeBindingProvider bindingProvider)
        {
            _bindingProvider = bindingProvider;
        }

        public void Initialize(ExtensionConfigContext context) =>
            context.AddBindingRule<ImportAttribute>().Bind(_bindingProvider);
    }
}