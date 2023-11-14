using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NexusAop.Cache;
using NexusAop.CustomAspect;
using NexusAop.Customization;
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
        private ICustomAspectService _nexusAopCustomService;

        private void SetParameters(
            TDecorated decorated,
            IServiceProvider serviceScope)
        {
            _logger = serviceScope.GetRequiredService<ILogger<NexusAopCustomProxy<TDecorated>>>();
            _nexusAopCustomService = serviceScope.GetRequiredService<ICustomAspectService>();
        }

        protected override object Invoke(
            MethodInfo targetMethod,
            object[] args)
        {
            try
            {
                OnStartAsync(targetMethod, args).GetAwaiter().GetResult();
                var result = CheckMethod(targetMethod);
                if (result)
                {
                   var attributes = GetAttributeKeys(targetMethod, args);
                    _nexusAopCustomService.Start(attributes);
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
            IServiceProvider serviceScope)
        {
            object proxy = Create<TDecorated, NexusAopCustomProxy<TDecorated>>();
            ((NexusAopCustomProxy<TDecorated>)proxy).SetParameters(decorated, serviceScope);

            return (TDecorated)proxy;
        }


    }
}
