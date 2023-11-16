using System.Threading.Tasks;

namespace NexusAop.Console
{
    public class MyTestAop : NexusAopAttribute
    {
        public override Task ExecuteAsync(NexusAopContext context)
        {

            // User-defined logic before the target method
            System.Console.WriteLine("Before invoking the target method.");

            // Proceed with the execution of the target method
            context.Next();

            // User-defined logic after the target method
            System.Console.WriteLine("After invoking the target method.");

            return Task.CompletedTask;
        }
    }
}