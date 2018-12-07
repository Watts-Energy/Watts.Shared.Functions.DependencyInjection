using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace Watts.Shared.Functions.DependencyInjection.Composition
{
    internal class ImportAttributeValueProvider
        : IValueProvider
    {
        private readonly object _value;

        public ImportAttributeValueProvider(object value)
            => _value = value;

        public Task<object> GetValueAsync() 
            => Task.FromResult(_value);


        public string ToInvokeString() 
            => _value.ToString();

        public Type Type => _value.GetType();
    }
}