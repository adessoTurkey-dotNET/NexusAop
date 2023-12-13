using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NexusAop.Console.Models;

namespace NexusAop.Console.Cache
{
    public class CacheMethodAttribute : NexusAopAttribute
    {
        public CacheMethodAttribute(
                int ttlAsSecond)
        {
            Ttl = TimeSpan.FromSeconds(ttlAsSecond);
        }

        public CacheMethodAttribute()
        {
            Ttl = null;
        }

        public TimeSpan? Ttl { get; set; }

        public override async Task ExecuteAsync(NexusAopContext context)
        {
            if (!CheckMethodCacheable(context.TargetMethod))
            {
                return;
            }
            var cacheKey = GetCacheKey(context.TargetMethod, context.TargetMethodsArgs);
            var result = GetResult(cacheKey);

            // read from cache
            if (result != null)
            {
                context.Result= result;
                return;
            }

            result = await context.ExecuteAndGetResultAsync();
            await SetCacheAsync(context.TargetMethod, context.TargetMethodsArgs,result);
        }

        public void CacheResult(
           string key,
           object value,
           TimeSpan? ttl = null)
        {
            DateTime? expDate = ttl.HasValue ? DateTime.Now.Add(ttl.Value) : null;
            CacheResultModel.Store.TryAdd(key, new Tuple<object, DateTime?>(value, expDate));
        }

        public object GetResult(
            string key)
        {
            if (!CacheResultModel.Store.ContainsKey(key)) return null;
            var value = CacheResultModel.Store[key];
            if (!value.Item2.HasValue || value.Item2.Value >= DateTime.Now)
            {
                return value.Item1;
            }

            CacheResultModel.Store.Remove(key, out _);
            return null;
        }


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
            CacheResult(key, result, cacheAttribute.Ttl);

            return Task.CompletedTask;
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
