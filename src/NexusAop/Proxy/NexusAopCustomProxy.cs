using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NexusAop.Cache;
using NexusAop.CustomAspect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                var result = CheckMethod(targetMethod);
                if (result)
                {
                    var attributes = new List<object>();
                    var customAspectAttributes = targetMethod.GetCustomAttributes(typeof(NexusAopAttribute), true);

                    if (customAspectAttributes.Length > 0)
                    {
                        var aspect = (NexusAopAttribute)customAspectAttributes[0];
                      
                        aspect.ExecuteAsync(context);
                    }

                    OnCompletedAsync(targetMethod, args).GetAwaiter().GetResult();
                    return result;
                }
                else
                {
                    OnInvalidMethod(targetMethod, args).GetAwaiter().GetResult();
                }

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
