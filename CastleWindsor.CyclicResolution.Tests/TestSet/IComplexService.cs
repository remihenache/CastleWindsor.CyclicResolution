namespace CastleWindsor.CyclicResolution.Tests.TestSet
{
    public interface IComplexService
    {
        ISimpleService? SimpleService { get; set; }
    }
}