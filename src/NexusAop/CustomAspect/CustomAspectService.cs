using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusAop.CustomAspect
{
    internal class CustomAspectService : ICustomAspectService
    {
        public void Start(Dictionary<string, object> attributes)
        {
            foreach (var item in attributes)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }
    }
}
