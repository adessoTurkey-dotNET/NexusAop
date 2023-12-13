using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexusAop.Console.CustomAspect
{
    public class CustomAspectAttribute : NexusAopAttribute
    {
        public override async Task ExecuteAsync(NexusAopContext context)
        {
            // User-defined logic before the target method
            //System.Console.WriteLine("Before invoking the target method.");

            // Proceed with the execution of the target method
            
            var result=await context.NextAsync();
            
            var setResult= await context.ExecuteAndGetResultAsync();

            // User-defined logic after the target method
            //System.Console.WriteLine("After invoking the target method.");
           // return Task.CompletedTask;
        }
    }
}
