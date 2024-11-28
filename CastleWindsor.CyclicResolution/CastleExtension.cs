using Castle.MicroKernel.Context;
using Castle.Windsor;

namespace CastleWindsor.CyclicResolution
{
    
    public static class CastleExtension
    {
        private const string GenericArguments = "GenericArguments";

        public static IWindsorContainer ResolveCyclicDependencies(this IWindsorContainer container)
        {
            container.Kernel.Resolver.AddSubResolver(new CyclicPropertyResolver(container.Kernel));
            container.Kernel.ComponentModelBuilder.AddContributor(new CyclicActivatorContributor());
            return container;
        }


        internal static void AddGenericArguments(this CreationContext context, Type genericType)
        {
            if(context.AdditionalArguments.Contains(GenericArguments))
                (context.AdditionalArguments[GenericArguments] as Stack<Type>)!.Push(genericType);
            else
            {
                Stack<Type> types = new ();
                types.Push(genericType);
                context.AdditionalArguments.Add(GenericArguments, types);
            }
        }
        internal static void CleanGenericArguments(this CreationContext context)
        {
            if (!context.AdditionalArguments.Contains(GenericArguments))
                return;
            (context.AdditionalArguments[GenericArguments] as Stack<Type>)!.Pop();
        }
        internal static Type[] GetGenericArguments(this CreationContext context)
        {
            if (context.HasAdditionalArguments && context.AdditionalArguments.Contains(GenericArguments))
            {
                return (context.AdditionalArguments[GenericArguments] as Stack<Type>)!.Peek().GetGenericArguments();
            }
            return Type.EmptyTypes;
        }
    }
}