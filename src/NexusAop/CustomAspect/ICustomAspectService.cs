using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusAop.CustomAspect
{
    public interface ICustomAspectService
    {
        void Start(List<object> attributes);

    }
}
