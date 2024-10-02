namespace CastleWindsor.CyclicResolution.Tests.TestSet
{
    public class ComplexService : IComplexService
    {
        public ISimpleService? SimpleService { get; set; }
    }
}