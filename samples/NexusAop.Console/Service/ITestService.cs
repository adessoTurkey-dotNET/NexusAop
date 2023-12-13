using System.Threading;
using System.Threading.Tasks;
using NexusAop.Console.Cache;
using NexusAop.Console.CustomAspect;
using NexusAop.Console.Models;

namespace NexusAop.Console.Service
{
    public interface ITestService
    {
        [CacheMethod(10)]
        int GetBalance(
            int id);

        [CacheMethod(20)]
        Task<FooEntity> GetFooAsync(
            bool isGoodNumber,
            CancellationToken cancellationToken = default);

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