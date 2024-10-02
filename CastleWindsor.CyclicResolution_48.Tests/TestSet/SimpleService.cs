namespace CastleWindsor.CyclicResolution_48.Tests.TestSet
{
    public class SimpleService : ISimpleService
    {
        public IComplexService ComplexService { get; set; }
    }
}