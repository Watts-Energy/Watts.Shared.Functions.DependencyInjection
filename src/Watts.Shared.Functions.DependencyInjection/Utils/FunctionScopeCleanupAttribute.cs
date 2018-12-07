using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Watts.Shared.Functions.DependencyInjection.Contracts;

namespace Watts.Shared.Functions.DependencyInjection.Utils
{
    internal class FunctionScopeCleanupAttribute
        : IFunctionExceptionFilter, IFunctionInvocationFilter
    {
        private readonly ScopedServiceProvider _serviceProvider;

        public FunctionScopeCleanupAttribute(ScopedServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task OnExecutingAsync(
            FunctionExecutingContext executingContext,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task OnExecutedAsync(
            FunctionExecutedContext executedContext, 
            CancellationToken cancellationToken)
        {
            _serviceProvider.DisposeFor(executedContext.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExceptionAsync(
            FunctionExceptionContext exceptionContext,
            CancellationToken cancellationToken)
        {
            _serviceProvider.DisposeFor(exceptionContext.FunctionInstanceId);
            return Task.CompletedTask;
        }
    }
}