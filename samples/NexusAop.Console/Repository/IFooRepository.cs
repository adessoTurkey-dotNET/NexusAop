using System.Threading;
using System.Threading.Tasks;
using NexusAop.Console.Models;

namespace NexusAop.Console.Repository
{
    public interface IFooRepository
    {
        FooEntity GetById(int id);

        Task<FooEntity> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}