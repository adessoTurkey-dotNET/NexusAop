using NexusAop.Console.Models;
using System.Threading;
using System;
using System.Threading.Tasks;
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
        public string MyStringMethod()
        {
            //System.Console.WriteLine("My string method.");
            return "My string method.";
        }
        public void MyVoidMethod()
        {
            //System.Console.WriteLine("My void method.");
        }
        public async Task MyMethodTask()
        {
            //System.Console.WriteLine("MyMethod returns task.");
            await Task.CompletedTask;
        }

        public async Task<string> MyMethodTaskReturnString()
        {
            //System.Console.WriteLine("MyMethod returns Task<string>.");
            return "thats ok";            
        }
    }
}