using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;

namespace CastleWindsor.CyclicResolution
{
    internal class CyclicPropertyResolver : ISubDependencyResolver
    {
        private readonly IKernel _kernel;

        public CyclicPropertyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandler, 
            ComponentModel model, DependencyModel dependency)
        {
            return true;
        }

        public object? Resolve(CreationContext context, ISubDependencyResolver contextHandler, 
            ComponentModel model, DependencyModel dependency)
        {
            return string.IsNullOrEmpty(dependency.ReferencedComponentName) ? ResolveType(context, dependency) : ResolveNamedDependency(context, dependency);
        }

        private object? ResolveType(CreationContext context, DependencyModel dependency)
        {
            if(context.TryGetContextualInstance(dependency.TargetType.FullName!, out var instance))
                return instance;
            return dependency.TargetType.IsGenericType ? ResolveGenericType(context, dependency) : TryResolve(context, dependency);
        }

        private object? ResolveNamedDependency(CreationContext context, DependencyModel dependency)
        {
            if(context.TryGetContextualInstance(dependency.ReferencedComponentName, out var instance))
                return instance;
            return _kernel.HasComponent(dependency.ReferencedComponentName) ? _kernel.GetHandler(dependency.ReferencedComponentName)?.TryResolve(context) : null;
        }

        private object? ResolveGenericType(CreationContext context, DependencyModel dependency)
        {
            context.AddGenericArguments(dependency.TargetType);
            var resolved = TryResolve(context, dependency);
            context.CleanGenericArguments();
            return resolved;
        }

        private object? TryResolve(CreationContext context, DependencyModel dependency)
        {
            return _kernel.HasComponent(dependency.TargetType)
                ? _kernel.GetHandler(dependency.TargetType)?.TryResolve(context)
                : null;
        }
    }
}