namespace CastleWindsor.CyclicResolution.Tests.TestSet
{
    public interface ISimpleService
    {
        IComplexService? ComplexService { get; set; }
    }
}