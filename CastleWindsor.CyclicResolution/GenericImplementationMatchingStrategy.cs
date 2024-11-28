using Castle.Core;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Handlers;

namespace CastleWindsor.CyclicResolution;

public class GenericImplementationMatchingStrategy : IGenericImplementationMatchingStrategy
{
    public Type[] GetGenericArguments(ComponentModel model, CreationContext context)
    {
        Type? first = model.Services.FirstOrDefault();
        return first is { IsGenericType: true, IsGenericTypeDefinition: false } ? first.GetGenericArguments() : context.GetGenericArguments();
    }
}