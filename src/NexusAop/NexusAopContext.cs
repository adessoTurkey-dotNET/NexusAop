using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NexusAop
{
    public class NexusAopContext
    {
        public IServiceProvider Services { get; set; }
        public MethodInfo TargetMethod { get; set; }
        public object Target { get; set; }
        public object[] TargetMethodsArgs { get; set; }
        public object Result { get; set; }


        public async Task<object> NextAsync()
        {
            if (TargetMethod.ReturnType == typeof(void))
            {
                // void 
                TargetMethod.Invoke(Target, TargetMethodsArgs);
                return null;
            }
            else if (TargetMethod.ReturnType == typeof(Task))
            {
                // task 
                var task = Task.Factory.StartNew(() => TargetMethod.Invoke(Target, TargetMethodsArgs));
                var result = await task;
                return result;
            }
            else if (TargetMethod.ReturnTypeCustomAttributes is not null)
            {
                // task<t> or t
                var result = await Task.Factory.StartNew(() => (object)TargetMethod.Invoke(Target, TargetMethodsArgs));
                return result;
            }
            else return null;
        }
        public async Task<object> SetResultAsync(object result)
        {
            if (result == null)
            {
                this.Result = new object();
                return this.Result;
            }

            if (IsTask(result))
            {
                dynamic task = result;

                var genricArgs = result.GetType().GenericTypeArguments;

                if (genricArgs.Length > 0)
                {
                    if (genricArgs.First().Name.Contains("VoidTask"))
                    {
                        //Task
                        this.Result = Task.CompletedTask;
                    }
                    else
                    {
                        //Task<T>
                        this.Result = task;
                    }
                }
            }
            else
            {
                // The result is not a Task, return it directly
                this.Result = result;
            }
            return this.Result;
        }
        public static bool IsTask(object obj)
        {
            return obj.GetType().IsGenericType;
        }

        public static bool IsGenericTask(object obj)
        {
            return obj.GetType().GetGenericTypeDefinition() == typeof(Task<>);
        }
    }

    public abstract class NexusAopAttribute : Attribute
    {
        public abstract Task ExecuteAsync(NexusAopContext context);
    }
}
