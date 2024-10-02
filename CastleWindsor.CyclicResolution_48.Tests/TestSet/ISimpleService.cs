namespace CastleWindsor.CyclicResolution_48.Tests.TestSet
{
    public interface ISimpleService
    {
        IComplexService ComplexService { get; set; }
    }
}