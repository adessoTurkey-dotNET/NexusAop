using System;

namespace NexusAop.Cache
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheMethodAttribute : System.Attribute
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
    }
}