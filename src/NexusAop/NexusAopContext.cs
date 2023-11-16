using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace NexusAop
{
    public class NexusAopContext
    {
        public IServiceProvider Services { get; set; }
        public MethodInfo TargetMethod { get; set; }
        public object Target { get; set; }
        public object[] TargetMethodsArgs { get; set; }
        public object Result { get; set; }
        public Task<object> NextAsync()
        {
            //void döbderse
            //task dönderse
            //task<t> dönderse:  await t didkat
            
             var result = TargetMethod.Invoke(Target, TargetMethodsArgs);
             this.Result = result;
             return result;
        }

        public void SetTaskResult(object result)
        {
            //if result is null
            //if methods is async

            this.Result = Task.FromResult(result);
        }
        
        public void SetResult(object result)
        {
            //if result is null
            //if methods is async
            
            this.Result = result
        }
    }

    public abstract class NexusAopAttribute : Attribute
    {
        public abstract Task ExecuteAsync(NexusAopContext context);
    }
}
