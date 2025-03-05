namespace CastleWindsor.CyclicResolution.Tests.TestSet;

public interface IGenericService<T>
{
    T? Value { get; set; }
}