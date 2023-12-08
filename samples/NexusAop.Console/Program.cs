// See https://aka.ms/new-console-template for more information

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            serviceCollection.AddSingletonWithCustomAop<ITestService, TestService>();

            serviceCollection.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.SetMinimumLevel(LogLevel.Information);
            });
            var provider = serviceCollection.BuildServiceProvider();

            var svc = provider.GetRequiredService<ITestService>();


            svc.MyStringMethod();

            svc.MyVoidMethod();

            await svc.MyMethodTask();

            await svc.MyMethodTaskReturnString();


        }
    }
}