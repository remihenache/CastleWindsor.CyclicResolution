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
            if(context.AdditionalArguments.Contains(Genericarguments))
                (context.AdditionalArguments[Genericarguments] as Stack<Type>).Push(genericType);
            else
            {
                Stack<Type> types = new Stack<Type>();
                types.Push(genericType);
                context.AdditionalArguments.Add(Genericarguments, types);
            }
        }
        internal static void CleanGenericArguments(this CreationContext context)
        {
            if (!context.AdditionalArguments.Contains(Genericarguments))
                return;
            (context.AdditionalArguments[Genericarguments] as Stack<Type>).Pop();
        }
        internal static Type[] GetGenericArguments(this CreationContext context)
        {
            if (context.HasAdditionalArguments && context.AdditionalArguments.Contains(Genericarguments))
            {
                return (context.AdditionalArguments[Genericarguments] as Stack<Type>).Peek().GetGenericArguments();
            }
            return Type.EmptyTypes;
        }
    }
}