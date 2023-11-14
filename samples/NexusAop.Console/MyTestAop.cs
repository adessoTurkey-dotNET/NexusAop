using System.Threading.Tasks;

namespace NexusAop.Console
{
    public class MyTestAop : NexusAopAttribute
    {
        public override Task ExecuteAsync(NexusAopContext context)
        {
            
            context.Next();
            return Task.CompletedTask;
        }
    }
}