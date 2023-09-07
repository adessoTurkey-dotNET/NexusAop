using System.Threading;
using System.Threading.Tasks;
using NexusAop.Console.Models;

namespace NexusAop.Console.Repository
{
    public class FooRepository : IFooRepository
    {
        public FooEntity GetById(
            int id)
        {
            return new FooEntity()
            {
                Id = id,
                Text = "text"
            };
        }

        public Task<FooEntity> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new FooEntity()
            {
                Id = id,
                Text = "Async Text"
            });
        }
    }
}