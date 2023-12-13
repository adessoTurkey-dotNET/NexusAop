using NexusAop.Console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NexusAop.Console.Repository.FooRepository;

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
