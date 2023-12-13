// See https://aka.ms/new-console-template for more information

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NexusAop.Console.Repository;
using NexusAop.Console.Service;
using NexusAop.Extensions;

namespace NexusAop.Console
{
    public static class Program
    {
        public static async Task Main(
            string[] arg)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IFooRepository, FooRepository>();
            serviceCollection.AddSingletonWithCustomAop<ITestService, TestService>();

            serviceCollection.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.SetMinimumLevel(LogLevel.Information);
            });
            var provider = serviceCollection.BuildServiceProvider();

            var svc = provider.GetRequiredService<ITestService>();

            // Id=xxx
            var result= await svc.GetFooAsync(true);

            // Id=xxx
            var resultWithCache = await svc.GetFooAsync(true);
            Thread.Sleep(20000);

            // Id=yyy
            var resultAfterTimeoutCache = await svc.GetFooAsync(true);

            svc.MyStringMethod();

            svc.MyVoidMethod();

            await svc.MyMethodTask();

            await svc.MyMethodTaskReturnString();


        }
    }
}