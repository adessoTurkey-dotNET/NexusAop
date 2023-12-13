using NexusAop.Console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
