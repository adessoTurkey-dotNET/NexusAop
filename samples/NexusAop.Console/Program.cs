// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NexusAop.Cache;
using NexusAop.Console.CustomAspect;
using NexusAop.Console.Repository;
using NexusAop.Console.Service;
using NexusAop.Extensions;
using NexusAop.Proxy;

namespace NexusAop.Console
{
    public static class Program
    {
        public static async Task Main(
            string[] arg)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddNexusAop();
            //serviceCollection.AddSingletonWithAop<ITestService, TestService>();
            serviceCollection.AddSingletonWithCustomAop<ITestService, TestService>();
            serviceCollection.AddSingleton<IFooRepository, FooRepository>();
            serviceCollection.AddSingleton<ICustomAspectService, CustomAspectService>();

            serviceCollection.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.SetMinimumLevel(LogLevel.Information);
            });
            var provider = serviceCollection.BuildServiceProvider();

            var svc = provider.GetRequiredService<ITestService>();
            svc.MyMethod();


            // Setup DI
            //var serviceProvider = new ServiceCollection()
            //    .AddScopedeWithCustomAop<ITestService, TestService>()
            //    .AddSingleton<IFooRepository, FooRepository>()
            //    .AddScoped<ICustomAspectService, CustomAspectService>()
            //    .AddLogging(configure =>
            //    {
            //        configure.AddConsole();
            //        configure.SetMinimumLevel(LogLevel.Information);
            //    })
            //    .BuildServiceProvider();

            //var myService = provider.GetRequiredService<ITestService>();

            //var proxy = NexusAopCustomProxy<ITestService>.Create(
            //    svc,
            //    provider,
            //    context =>
            //    {
            //        // User-defined logic before the target method
            //        System.Console.WriteLine($"Custom logic before {context.TargetMethod.Name}");
            //    },
            //    context =>
            //    {
            //        // User-defined logic after the target method
            //        System.Console.WriteLine($"Custom logic after {context.TargetMethod.Name}");
            //    });


            //proxy.MyMethod();

            //MyTestAop aoptets = new MyTestAop();
            //aoptets.ExecuteAsync(new NexusAopContext() { });


            //for (var i = 0; i < 3; i++)
            //{
            //    var isGoodNumber = i % 2 == 0;
            //    var stopWatch = Stopwatch.StartNew();
            //    var result = await svc.GetFooAsync(isGoodNumber, CancellationToken.None);
            //    stopWatch.Stop();
            //    System.Console.WriteLine($"[Loop {i}] Method result is {result} with parameter {isGoodNumber} & total execution is {stopWatch.ElapsedMilliseconds} ms");
            //    await Task.Delay(TimeSpan.FromSeconds(2));
            //}


            //TestService myClass = new TestService(new FooRepository());
            //Type type = myClass.GetType();
            //var methodInfo = type.GetMethod("MyMethod");

            //var customAspectAttributes = methodInfo.GetCustomAttributes(typeof(CustomAspectAttribute), true);

            //if (customAspectAttributes.Length > 0)
            //{
            //    CustomAspectAttribute aspect = (CustomAspectAttribute)customAspectAttributes[0];

            //    foreach (var property in aspect.Properties)
            //    {
            //        System.Console.WriteLine($"{property.Key}: {property.Value}");
            //    }
            //}

        }
    }
}