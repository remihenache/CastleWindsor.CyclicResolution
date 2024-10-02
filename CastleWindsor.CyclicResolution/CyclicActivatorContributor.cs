using Castle.Core;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;

namespace CastleWindsor.CyclicResolution
{
    internal class CyclicActivatorContributor : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            if(model.Implementation == typeof(LateBoundComponent))
                return;
            if(model.Implementation.GetInterfaces().Any(i => i == typeof(IProxyTargetAccessor)))
                return;
            model.CustomComponentActivator = typeof(CyclicComponentActivator);
        }
    }
}