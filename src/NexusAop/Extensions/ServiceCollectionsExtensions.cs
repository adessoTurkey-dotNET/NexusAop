using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NexusAop.Cache;
using NexusAop.CustomAspect;
using NexusAop.Proxy;

namespace NexusAop.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddNexusAop(
            this IServiceCollection services)
        {
            services.AddSingleton<INexusAopCacheService, InmemoryNexusAopCacheService>();
            services.AddSingleton<ICustomAspectService, CustomAspectService>();

            return services;
        }
        
        public static IServiceCollection AddSingletonWithAop<TInterface, TService>(
            this IServiceCollection services)
            where TInterface: class
            where TService : class, TInterface
        {
            return services.AddSingleton<TInterface, TService>()
                .DecorateWithDispatchProxy<TInterface, NexusAopProxy<TInterface>>();
        }

        public static IServiceCollection AddScopedWithAop<TInterface, TService>(
            this IServiceCollection services)
            where TInterface: class
            where TService : class, TInterface
        {
            return services.AddScoped<TInterface, TService>()
                .DecorateWithDispatchProxy<TInterface, NexusAopProxy<TInterface>>();
        }

        public static IServiceCollection AddTransientWithAop<TInterface, TService>(
            this IServiceCollection services)
            where TInterface: class
            where TService : class, TInterface
        {
            return services.AddTransient<TInterface, TService>()
                .DecorateWithDispatchProxy<TInterface, NexusAopProxy<TInterface>>();
        }

        public static IServiceCollection AddSingletonWithCustomAop<TInterface, TService>(
            this IServiceCollection services)
            where TInterface : class
            where TService : class, TInterface
        {
            return services.AddSingleton<TInterface, TService>()
                .DecorateWithDispatchProxy<TInterface, NexusAopCustomProxy<TInterface>>();
        }
        public static IServiceCollection AddScopedeWithCustomAop<TInterface, TService>(
            this IServiceCollection services)
            where TInterface : class
            where TService : class, TInterface
        {
            return services.AddScoped<TInterface, TService>()
                .DecorateWithDispatchProxy<TInterface, NexusAopCustomProxy<TInterface>>();
        }

        #region Private Methods

        private static IServiceCollection DecorateWithDispatchProxy<TInterface, TProxy>(
            this IServiceCollection services)
            where TInterface : class
            where TProxy : DispatchProxy
        {
            var createMethod = typeof(TProxy)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(info => !info.IsGenericMethod && info.ReturnType == typeof(TInterface));

            var methodArgs = createMethod.GetParameters();

            var descriptorsToDecorate = services
                .Where(s => s.ServiceType == typeof(TInterface))
                .ToList();

            if (descriptorsToDecorate.Count == 0)
            {
                throw new InvalidOperationException($"Attempted to Decorate services of type {typeof(TInterface)}, " +
                                                    "but no such services are present in ServiceCollection");
            }

            foreach (var descriptor in descriptorsToDecorate)
            {
                var decorated = ServiceDescriptor.Describe(
                    typeof(TInterface),
                    sp =>
                    {
                        var decoratorInstance = createMethod.Invoke(null,
                            methodArgs.Select(
                                    info => info.ParameterType ==
                                            (descriptor.ServiceType)
                                        ? sp.CreateInstance(descriptor)
                                        : sp.GetRequiredService(info.ParameterType))
                                .ToArray());

                        return (TInterface)decoratorInstance;
                    },
                    descriptor.Lifetime);

                services.Remove(descriptor);
                services.Add(decorated);
            }

            return services;
        }

        private static object CreateInstance(
            this IServiceProvider services,
            ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }

            return descriptor.ImplementationFactory != null 
                ? descriptor.ImplementationFactory(services) 
                : ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
        }
        
        #endregion
    }
}