using System.Collections.Generic;

namespace NexusAop.Console.CustomAspect
{
    public class CustomAspectService : ICustomAspectService
    {
        public void Start(List<object> attributes)
        {
            foreach (var item in attributes)
            {
                System.Console.WriteLine($"{item.ToString()}");
            }
        }
    }
}
