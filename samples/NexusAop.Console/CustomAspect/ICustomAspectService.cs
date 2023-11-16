using System.Collections.Generic;

namespace NexusAop.Console.CustomAspect
{
    public interface ICustomAspectService
    {
        void Start(List<object> attributes);

    }
}
