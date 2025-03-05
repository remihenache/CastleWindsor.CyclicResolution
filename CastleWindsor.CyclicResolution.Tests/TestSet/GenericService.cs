namespace CastleWindsor.CyclicResolution.Tests.TestSet;

public class GenericService<T> : IGenericService<T>
{
    public T? Value { get; set; }
}