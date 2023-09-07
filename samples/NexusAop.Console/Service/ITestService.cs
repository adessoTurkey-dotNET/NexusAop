using System.Threading;
using System.Threading.Tasks;
using NexusAop.Cache;
using NexusAop.Console.Models;

namespace NexusAop.Console.Service
{
    public interface ITestService
    {
        void SayHello(string name);
        
        [CacheMethod(10)]
        int GetBalance(
            int id);
        
        [CacheMethod(20)]
        Task<FooEntity> GetFooAsync(
            bool isGoodNumber,
            CancellationToken cancellationToken = default);
    }
}