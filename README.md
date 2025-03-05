# Castle Windsor Extensions for resolving Cyclic Dependencies

A cyclic dependency is a situation where two or more classes depend on each other. This is a common problem in object-oriented programming and can lead to a variety of issues, such as infinite loops, stack overflows, and other runtime errors.
This is a simple implementation of Castle Windsor class that can be used to resolve cyclic dependencies in C#.

You should of course try to avoid cyclic dependencies in your codebase, but sometimes they are unavoidable, specially when working with old codebase. This extension can help you to resolve them when they do occur.
# Install

To use these extensions, install the nuget package for your C# project and ensure that the appropriate namespaces are
referenced. Make sure that you have the necessary dependencies and target framework version set correctly.

```shell
dotnet add package CastleWindsor.CyclicResolution
```

# Usages

Simply use the following extension method to resolve the cyclic dependencies in your project.

```csharp
public class IocConfiguration
{
    public IWindsorContainer Configure() 
    {
        var container = new WindsorContainer();
        container.ResolveCyclicDependencies();
        // container.Register(...);
        // ...
        return container;
    }
}
``` 

# Pros and cons

## Pros
Resolve cyclic dependencies in your project without having to refactor your code. This extension is a simple and effective.

## Cons
Can't be used with Constructor Injection. You may need to refactor your code to use Property Injection instead.
At least cyclic dependencies must be properties in order to be resolved.
Is not a built-in feature of Castle Windsor. You need to add the extension to your project. And the project may not be ready for production usage.

# More about

You can find more details about the implementation
here: [Castle Windsor Extensions for resolving Cyclic Dependencies]()

Feel free to explore and leverage these extensions.

### Note: This code snippet is a standalone C# implementation and can be integrated into your project to extend Castle Windsor functionality.