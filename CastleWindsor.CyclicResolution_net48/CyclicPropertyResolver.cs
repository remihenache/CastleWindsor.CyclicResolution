using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;

namespace CastleWindsor.CyclicResolution_net48
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

        public object Resolve(CreationContext context, ISubDependencyResolver contextHandler, 
            ComponentModel model, DependencyModel dependency)
        {
            if (string.IsNullOrEmpty(dependency.ReferencedComponentName))
            {
                var alreadyResolvedType = context.GetContextualProperty(dependency.TargetType.FullName);
                if(alreadyResolvedType != null)
                    return alreadyResolvedType;
                return !_kernel.HasComponent(dependency.TargetType) ? null : _kernel.GetHandler(dependency.TargetType)?.TryResolve(context);
            }
            var alreadyResolvedName = context.GetContextualProperty(dependency.ReferencedComponentName);
            if(alreadyResolvedName != null)
                return alreadyResolvedName;
            return !_kernel.HasComponent(dependency.ReferencedComponentName) ? null : _kernel.GetHandler(dependency.ReferencedComponentName)?.TryResolve(context);
        }
    }
}