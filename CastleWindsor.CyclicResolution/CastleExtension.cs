using Castle.Windsor;

namespace CastleWindsor.CyclicResolution
{
    
    public static class CastleExtension
    {
        public static IWindsorContainer ResolveCyclicDependencies(this IWindsorContainer container)
        {
            container.Kernel.Resolver.AddSubResolver(new CyclicPropertyResolver(container.Kernel));
            container.Kernel.ComponentModelBuilder.AddContributor(new CyclicActivatorContributor());
            return container;
        }
    }
}