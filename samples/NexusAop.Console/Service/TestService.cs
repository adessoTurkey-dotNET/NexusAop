using System.Threading.Tasks;

namespace NexusAop.Console.Service
{
    public class TestService : ITestService
    {  
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