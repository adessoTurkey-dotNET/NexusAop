using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NexusAop.Console.Models
{
    public class FooEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    
}
}
