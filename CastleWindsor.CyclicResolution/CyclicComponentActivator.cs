using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ComponentActivator;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Handlers;

namespace CastleWindsor.CyclicResolution
{
    internal class CyclicComponentActivator : DefaultComponentActivator
    {
        public CyclicComponentActivator(ComponentModel model, IKernelInternal kernel, ComponentInstanceDelegate onCreation, ComponentInstanceDelegate onDestruction) : base(model, kernel, onCreation, onDestruction)
        {
        }

        protected override object? Instantiate(CreationContext context)
        {
            var instance = base.Instantiate(context);
            if(instance == null)
                return null;
            
            context.SetContextualProperty(Model.Name, instance);
            foreach (var service in Model.Services)
            {
                context.SetContextualProperty(service.FullName, instance);
            }
            return instance;
        }
        
        
        protected override object[] CreateConstructorArguments(
            ConstructorCandidate constructor,
            CreationContext context)
        {
            object?[] constructorArguments = base.CreateConstructorArguments(constructor, context);
            if(constructorArguments == null)
                return constructorArguments;
            if (constructorArguments.All(c => c != null))
                return constructorArguments;
            try
            {
                for (int index = 0; index < constructorArguments.Length; ++index)
                {
                    context.AddGenericArguments(constructor.Dependencies[index].TargetType);
                    constructorArguments[index] ??= TryResolve(constructor, context, index);
                    context.CleanGenericArguments();
                }
                return constructorArguments;
            }
            catch
            {
                foreach (object instance in constructorArguments)
                    this.Kernel.ReleaseComponent(instance);
                throw;
            }
        }

        private object? TryResolve(ConstructorCandidate constructor, CreationContext context, int index)
        {
            return !Kernel.HasComponent(constructor.Dependencies[index].TargetType) ? null : Kernel.GetHandler(constructor.Dependencies[index].TargetType)?.TryResolve(context);
        }
    }
    
    public class GenericImplementationMatchingStrategy : IGenericImplementationMatchingStrategy
    {
        public Type[] GetGenericArguments(ComponentModel model, CreationContext context)
        {
            var first = model.Services.First();
            if (first.IsGenericType && !first.IsGenericTypeDefinition)
            {
                return first.GetGenericArguments();
            }

            return context.GetGenericArguments();
        }
    }
}