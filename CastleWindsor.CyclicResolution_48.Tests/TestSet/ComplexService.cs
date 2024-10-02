namespace CastleWindsor.CyclicResolution_48.Tests.TestSet
{
    public class ComplexService : IComplexService
    {
        public ISimpleService SimpleService { get; set; }
    }
}