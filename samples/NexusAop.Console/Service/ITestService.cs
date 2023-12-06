using System.Threading.Tasks;
using NexusAop.Console.CustomAspect;

namespace NexusAop.Console.Service
{
    public interface ITestService
    {       
        [CustomAspect]
        void MyVoidMethod();

        [CustomAspect]
        string MyStringMethod();

        [CustomAspect]
        Task MyMethodTask();

        [CustomAspect]
        Task<string> MyMethodTaskReturnString();

    }
}