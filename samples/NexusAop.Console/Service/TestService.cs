using System;
using System.Threading;
using System.Threading.Tasks;
using NexusAop.Console.Models;
using NexusAop.Console.Repository;

namespace NexusAop.Console.Service
{
    public class TestService : ITestService
    {
        private readonly IFooRepository _fooRepository;

        public TestService(
            IFooRepository fooRepository)
        {
            _fooRepository = fooRepository;
        }

        public void SayHello(
            string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }
            
            System.Console.WriteLine($"Hi {name}");
            
        }
        
        public int GetBalance(
            int id)
        {
            System.Console.WriteLine($"Checking id for {id}!");
            var balance = new Random().Next(11111);
            Task.Delay(balance).GetAwaiter().GetResult();
            return balance;
        }

        public async Task<FooEntity> GetFooAsync(
            bool isGoodNumber,
            CancellationToken cancellationToken = default)
        {
            var number = new Random().Next(9000);
            await Task.Delay(number, cancellationToken);
            var foo = await _fooRepository.GetByIdAsync(2, cancellationToken);
            return new FooEntity()
            {
                Id = number,
                Text = DateTime.Now.ToLongDateString()
            };
        }
        public void MyMethod()
        {
            System.Console.WriteLine("MyMethod has custom aspect properties.");
        }
    }
}