using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NexusAop.Proxy
{
    public partial class NexusAopProxy<TDecorated>
    {
        private ILogger<NexusAopProxy<TDecorated>> _logger;

        protected virtual Task OnStartAsync(
            MethodInfo targetMethod,
            object[] args)
        {
            _logger.LogInformation("The invocation of {DecoratedClass}.{MethodName} started!",
                typeof(TDecorated), targetMethod.Name);

            return Task.CompletedTask;
        }

        protected virtual Task OnCompletedAsync(
            MethodInfo targetMethod,
            object[] args)
        {
            _logger.LogInformation("The invocation of {DecoratedClass}.{MethodName} completed!",
                typeof(TDecorated), targetMethod.Name);

            return Task.CompletedTask;
        }

        protected virtual Task OnCompletedFromCacheAsync(
            MethodInfo targetMethod,
            object[] args)
        {
            _logger.LogInformation("The invocation of {DecoratedClass}.{MethodName} completed & the result returned from the cache", 
                typeof(TDecorated), targetMethod.Name);
            
            return Task.CompletedTask;
        }
        
        protected virtual Task OnErrorAsync(
            MethodInfo targetMethod,
            object[] args,
            System.Exception exception)
        {
            _logger.LogError(exception.InnerException ?? exception,
                "Error during invocation of {DecoratedClass}.{MethodName}",
                typeof(TDecorated), targetMethod.Name);
            return Task.CompletedTask;
        }
    }
}