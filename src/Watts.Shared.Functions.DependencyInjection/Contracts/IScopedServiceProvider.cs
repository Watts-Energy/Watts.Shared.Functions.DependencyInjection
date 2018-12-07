using System;
using Microsoft.Extensions.Configuration;

namespace Watts.Shared.Functions.DependencyInjection.Contracts
{
    public interface IScopedServiceProvider
    {
        IServiceProvider CurrentFor(IConfiguration configuration);
    }
}