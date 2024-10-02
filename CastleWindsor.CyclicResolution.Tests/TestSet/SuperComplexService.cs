namespace CastleWindsor.CyclicResolution.Tests.TestSet
{
    public class SuperComplexService : ISuperComplexService
    {
        public ISimpleService? SimpleService { get; set; }
        public IComplexService? ComplexService { get; set; }
    }
}