using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusAop.CustomAspect
{
    public class CustomAspectService : ICustomAspectService
    {
        public void Start(List<object> attributes)
        {
            foreach (var item in attributes)
            {
                Console.WriteLine($"{item.ToString()}");
            }
        }
    }
}
