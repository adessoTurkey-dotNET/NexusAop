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
        
        public void  Next()
        {
            TargetMethod.Invoke(Target, TargetMethodsArgs);
        }
    }

    public abstract class NexusAopAttribute : Attribute
    {
        public abstract Task ExecuteAsync(NexusAopContext context);
    }
}
