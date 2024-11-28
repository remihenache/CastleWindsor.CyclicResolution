using Castle.MicroKernel.Context;
using Castle.Windsor;

namespace CastleWindsor.CyclicResolution
{
    
    public static class CastleExtension
    {
        private const string Genericarguments = "GenericArguments";

        public static IWindsorContainer ResolveCyclicDependencies(this IWindsorContainer container)
        {
            container.Kernel.Resolver.AddSubResolver(new CyclicPropertyResolver(container.Kernel));
            container.Kernel.ComponentModelBuilder.AddContributor(new CyclicActivatorContributor());
            return container;
        }

        internal static void AddGenericArguments(this CreationContext context, Type genericType)
        {
            context.AdditionalArguments.Add(Genericarguments, genericType);
        }
        internal static void CleanGenericArguments(this CreationContext context)
        {
            context.AdditionalArguments.Remove(Genericarguments);
        }
        internal static Type[] GetGenericArguments(this CreationContext context)
        {
            if(context.HasAdditionalArguments && context.AdditionalArguments.Contains(Genericarguments))
            {
                return (context.AdditionalArguments[Genericarguments] as Type).GetGenericArguments();
            }
            return Type.EmptyTypes;
        }
    }
}