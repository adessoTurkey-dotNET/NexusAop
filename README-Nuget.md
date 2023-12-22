<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![NuGet][nuget-shield]][nuget-url]
[![MIT License][license-shield]][license-url]

NexusAop is a powerful and flexible library for reflection and aspect-oriented programming (AOP) in .NET 5.0. This library enables developers to easily apply cross-cutting concerns to their applications by utilizing custom attributes. With NexusAop, you can interrupt method executions, perform specific actions, and retrieve results seamlessly.


# Table Of Content
1. [Features](#features)
2. [Get It Started](#get-it-started)
3. [Cache Attribute Example](#cache-example)
4. [Contributions](#contributions)
5. [License](#license)
      
# Features
1. Aspect-Oriented Programming (AOP): 
NexusAop empowers developers to embrace the principles of AOP by providing a straightforward mechanism for applying cross-cutting concerns using custom attributes.
2. Method Interruption: 
Leverage the NextAsync() method to interrupt the execution of a method and perform specific actions before allowing the method to continue. This allows for dynamic and context-aware behavior in your applications.
3. Result Retrieval: 
Utilize the ExecuteAndGetResultAsync() method to retrieve the result of the related method. This feature is particularly useful when you need to capture and manipulate the output of a method in a controlled manner. 
4. Custom Attributes: 
Easily create and apply custom attributes to your methods, enabling a clean and declarative way to define aspects. Custom attributes in NexusAop serve as the building blocks for weaving cross-cutting concerns into your application.
5. .NET 5.0 Compatibility: 
NexusAop is designed to seamlessly integrate with .NET 5.0.

# Get It Started 
To start using NexusAop in your .NET 5.0 project, follow these simple steps:

1. Install the Package:

` dotnet add package NexusAop `
2. Service Implementation:

```csharp
serviceCollection.AddSingletonWithCustomAop<ITestService, TestService>();
```
3. Apply Custom Attributes:

Decorate your methods with custom attributes to define the desired cross-cutting concerns.
```csharp
[CustomAspect]
public async Task<int> MyMethodAsync()
{
    // Your method implementation
}
```
4. Integrate Aspect-Oriented Behavior: 

Use the provided methods such as NextAsync() and ExecuteAndGetResultAsync() within your custom aspects to influence the method execution flow.

```csharp
public class CustomAspectAttribute : NexusAopAttribute
{
    public override async Task ExecuteAsync(NexusAopContext context)
    {
        // Perform actions before the method execution

        // Proceed with the execution of the target method
        var result = await context.NextAsync();

        // User-defined logic after the target method

        // Get the result if you needed
        var setResult= await context.ExecuteAndGetResultAsync();

        return result;
    }
}
```
5. Build and Run: 

Build your project, and NexusAop will seamlessly weave the specified aspects into your methods during runtime.
# Cache Attribute Example
```csharp
public class CacheMethodAttribute : NexusAopAttribute
    {
        public CacheMethodAttribute(
                int ttlAsSecond)
        {
            Ttl = TimeSpan.FromSeconds(ttlAsSecond);
        }

        public CacheMethodAttribute()
        {
            Ttl = null;
        }

        public TimeSpan? Ttl { get; set; }

        public override async Task ExecuteAsync(NexusAopContext context)
        {
            if (!CheckMethodCacheable(context.TargetMethod))
            {
                return;
            }
            var cacheKey = GetCacheKey(context.TargetMethod, context.TargetMethodsArgs);
            var result = GetResult(cacheKey);

            if (result != null)
            {
                context.Result= result;
                return;
            }

            result = await context.ExecuteAndGetResultAsync();
            await SetCacheAsync(context.TargetMethod, context.TargetMethodsArgs,result);
        }

        // ...
        // see CacheMethodAttribute.cs in /Samples/Cache for other logics
        // ...
  }
```
# Contributions
Contributions are welcome! If you encounter any issues or have suggestions for improvements, please feel free to create an issue or submit a pull request.

# License
This project is licensed under the BSD 3-Clause License.

<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/adessoTurkey-dotNET/NexusAop.svg?style=for-the-badge
[contributors-url]: https://github.com/adessoTurkey-dotNET/NexusAop/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/adessoTurkey-dotNET/NexusAop.svg?style=for-the-badge
[forks-url]: https://github.com/adessoTurkey-dotNET/NexusAop/network/members
[stars-shield]: https://img.shields.io/github/stars/adessoTurkey-dotNET/NexusAop.svg?style=for-the-badge
[stars-url]: https://github.com/adessoTurkey-dotNET/NexusAop/stargazers
[issues-shield]: https://img.shields.io/github/issues/adessoTurkey-dotNET/NexusAop.svg?style=for-the-badge
[issues-url]: https://github.com/adessoTurkey-dotNET/NexusAop/issues
[license-shield]: https://img.shields.io/github/license/adessoTurkey-dotNET/NexusAop.svg?style=for-the-badge
[license-url]: https://github.com/adessoTurkey-dotNET/NexusAop/blob/main/LICENSE
[.Net]: https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[.Net-shield]: https://img.shields.io/badge/.NET-5C2D91?
[nuget-shield]: https://img.shields.io/nuget/v/NexusAop?style=for-the-badge
[nuget-url]: https://www.nuget.org/packages/NexusAop
