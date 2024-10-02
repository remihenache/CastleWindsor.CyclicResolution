namespace CastleWindsor.CyclicResolution.Tests.TestSet
{
    public class SimpleService : ISimpleService
    {
        public IComplexService? ComplexService { get; set; }
    }
}