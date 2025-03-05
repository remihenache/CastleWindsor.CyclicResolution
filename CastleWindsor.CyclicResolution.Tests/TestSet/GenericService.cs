namespace CastleWindsor.CyclicResolution.Tests.TestSet
{
    public interface IGenericService<T>
    {
        T? Service { get; set; }
    }
    public class GenericService<T> : IGenericService<T>
    {
        public T? Service { get; set; }
    }

    public class AGenericService
    {
        public AGenericService(IGenericService<ISimpleService> service)
        {
            Service = service?.Service;
        }

        public ISimpleService Service { get; protected set; }
        
        public IGenericService<IComplexService> GenericService { get; set; }
    }

    public class ConstructorGenericService<T> 
    {
        public ConstructorGenericService(IGenericService<T> service)
        {
            Service = service;
        }

        public IGenericService<T> Service { get; protected set; }
    }

}