namespace CastleWindsor.CyclicResolution_48.Tests.TestSet
{
    public interface ISuperComplexService
    {
        ISimpleService SimpleService { get; set; }
        IComplexService ComplexService { get; set; }
    }
}