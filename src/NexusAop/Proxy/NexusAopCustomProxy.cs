using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NexusAop.Proxy
{
    public partial class NexusAopCustomProxy<TDecorated> : DispatchProxy
    {
        private IServiceProvider _serviceProvider;
        private TDecorated _decorated;

        private void SetParameters(
            TDecorated decorated,
            IServiceProvider serviceScope)
        {
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            _logger = serviceScope.GetRequiredService<ILogger<NexusAopCustomProxy<TDecorated>>>();
            _serviceProvider = serviceScope;
        }

        protected override object Invoke(
            MethodInfo targetMethod,
            object[] args)
        {
            var context = new NexusAopContext
            {
                Services = _serviceProvider,
                TargetMethod = targetMethod,
                Target = _decorated,
                TargetMethodsArgs = args
            };
            try
            {
                OnStartAsync(targetMethod, args).GetAwaiter().GetResult();
                var result = new object();
                var attributes = new List<object>();
                var customAspectAttributes = targetMethod.GetCustomAttributes(typeof(NexusAopAttribute), true);

                if (customAspectAttributes.Length > 0)
                {
                    var aspect = (NexusAopAttribute)customAspectAttributes[0];
                    aspect.ExecuteAsync(context).GetAwaiter().GetResult();
                    result = context.Result;
                }

                OnCompletedAsync(targetMethod, args).GetAwaiter().GetResult();
                return result;
            }
            catch (Exception exception)
            {
                OnErrorAsync(targetMethod, args, exception).GetAwaiter().GetResult();
                throw exception.InnerException ?? exception;
            }
        }

        public static TDecorated Create(
            TDecorated decorated,
            IServiceProvider serviceScope
            )
        {
            object proxy = Create<TDecorated, NexusAopCustomProxy<TDecorated>>();
            var nexusAopCustomProxy = (NexusAopCustomProxy<TDecorated>)proxy;
            nexusAopCustomProxy.SetParameters(decorated, serviceScope);
            return (TDecorated)proxy;
        }


    }
}
