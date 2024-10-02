using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ComponentActivator;
using Castle.MicroKernel.Context;

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

    }
}