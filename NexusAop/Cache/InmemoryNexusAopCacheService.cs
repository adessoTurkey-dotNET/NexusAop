using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NexusAop.Cache
{
    internal class InmemoryNexusAopCacheService : INexusAopCacheService
    {
        private readonly ConcurrentDictionary<string, Tuple<object, DateTime?>> _store;

        public InmemoryNexusAopCacheService()
        {
            _store = new ConcurrentDictionary<string, Tuple<object, DateTime?>>();
        }

        public void CacheResult(
            string key,
            object value,
            TimeSpan? ttl = null)
        {
            DateTime? expDate = ttl.HasValue ? DateTime.Now.Add(ttl.Value) : null;
            _store.TryAdd(key, new Tuple<object, DateTime?>(value, expDate));
        }

        public object GetResult(
            string key)
        {
            if (!_store.ContainsKey(key)) return null;
            var value = _store[key];
            if (!value.Item2.HasValue || value.Item2.Value >= DateTime.Now)
            {
                return value.Item1;
            }

            _store.TryRemove(key, out _);

            return null;
        }
    }
}