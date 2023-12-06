using System.Threading.Tasks;
using NexusAop.Console.CustomAspect;

namespace NexusAop.Console.Service
{
    public interface ITestService
    {       
        [CustomAspect("Property1", "Value1", "Property2", 42, "Property3", true)]
        void MyVoidMethod();

        [CustomAspect]
        string MyStringMethod();

        [CustomAspect("Property1", "Value1", "Property2", 42, "Property3", true)]
        Task MyMethodTask();

        [CustomAspect("Property1", "Value1", "Property2", 42, "Property3", true)]
        Task<string> MyMethodTaskReturnString();

    }
}