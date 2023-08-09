using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NexusAop.Cache;

namespace NexusAop.Proxy
{
    public partial class NexusAopProxy<TDecorated> : DispatchProxy
    {
        private TDecorated _decorated;
        private INexusAopCacheService _nexusAopCacheService;
        
        private void SetParameters(
            TDecorated decorated,
            IServiceProvider serviceScope)
        {
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            _logger = serviceScope.GetRequiredService<ILogger<NexusAopProxy<TDecorated>>>();
            _nexusAopCacheService = serviceScope.GetRequiredService<INexusAopCacheService>();
        }
        
        protected override object Invoke(
            MethodInfo targetMethod,
            object[] args)
        {
            try
            {
                OnStartAsync(targetMethod, args).GetAwaiter().GetResult();
                var result = GetCacheIfAvailable(targetMethod, args);
                if (result != null)
                {
                    OnCompletedFromCacheAsync(targetMethod, args).GetAwaiter().GetResult();
                    return result;
                }

                result = targetMethod.Invoke(_decorated, args);
                if (result is Task resultTask)
                {
                    resultTask.ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            OnErrorAsync(targetMethod, args, task.Exception).GetAwaiter().GetResult();
                        }

                        if (!resultTask.IsCompletedSuccessfully) return;
                        SetCacheAsync(targetMethod, args, result);
                        OnCompletedAsync(targetMethod,args).GetAwaiter().GetResult();
                    });
                }
                else
                {
                    SetCacheAsync(targetMethod, args, result);
                    OnCompletedAsync(targetMethod,args).GetAwaiter().GetResult();
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
            object proxy = Create<TDecorated, NexusAopProxy<TDecorated>>();
            ((NexusAopProxy<TDecorated>)proxy).SetParameters(decorated, serviceScope);

            return (TDecorated)proxy;
        }
    }
}