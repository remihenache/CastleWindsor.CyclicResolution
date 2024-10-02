namespace CastleWindsor.CyclicResolution.Tests.TestSet
{
    public interface ISuperComplexService
    {
        ISimpleService? SimpleService { get; set; }
        IComplexService? ComplexService { get; set; }
    }
}