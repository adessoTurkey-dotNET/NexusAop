using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NexusAop.Cache;

namespace NexusAop.Proxy
{
    public partial class NexusAopProxy<TDecorated>
    {
        protected virtual Task SetCacheAsync(
            MethodInfo targetMethod,
            object[] args,
            object result)
        {
            if (result == null || !CheckMethodCacheable(targetMethod))
            {
                return Task.CompletedTask;
            }

            var cacheAttribute = targetMethod.GetCustomAttribute<CacheMethodAttribute>();
            var key = GetCacheKey(targetMethod, args);
            _nexusAopCacheService.CacheResult(key, result, cacheAttribute.Ttl);
           
            return Task.CompletedTask;
        }
        
        protected virtual object GetCacheIfAvailable(MethodInfo targetMethod, object[] args)
        {
            if (!CheckMethodCacheable(targetMethod))
            {
                return null;
            }

            var cacheKey = GetCacheKey(targetMethod, args);
            return _nexusAopCacheService.GetResult(cacheKey);
        }

        private bool CheckMethodCacheable(
            MethodInfo targetMethod)
        {
            return targetMethod.ReturnType != typeof(void)
                   && targetMethod.ReturnType != typeof(Task)
                   && targetMethod.CustomAttributes.Any(x => x.AttributeType == typeof(CacheMethodAttribute));
        }
        
        protected virtual string GetCacheKey(
            MethodInfo targetMethod,
            object[] args)
        {
            var stringBuilder = new StringBuilder(targetMethod.Name);
            stringBuilder.AppendJoin(";", targetMethod.ReflectedType.Name);
            stringBuilder.AppendJoin(";", targetMethod.ReturnType.Name);

            if (args != null && args.Any())
            {
                stringBuilder.AppendJoin(";", JsonConvert.SerializeObject(args));
            }

            var hash = SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString()));
            return Convert.ToBase64String(hash);
        }
    }
}