using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NexusAop.Proxy
{
    public partial class NexusAopCustomProxy<TDecorated>
    {
        private ILogger<NexusAopCustomProxy<TDecorated>> _logger;

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
        protected virtual Task OnInvalidMethod(
            MethodInfo targetMethod,
            object[] args)
        {
            _logger.LogInformation("The invocation of {DecoratedClass}.{MethodName} has no custom attribute!",
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
