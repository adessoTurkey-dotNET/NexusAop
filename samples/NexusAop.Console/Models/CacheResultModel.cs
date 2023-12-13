using System;
using System.Collections.Generic;

namespace NexusAop.Console.Models
{
    public class CacheResultModel
    {
        public static Dictionary<string, Tuple<object, DateTime?>> Store { get; set; } = new Dictionary<string, Tuple<object, DateTime?>>();
    }
}
