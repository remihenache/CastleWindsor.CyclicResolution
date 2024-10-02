namespace CastleWindsor.CyclicResolution_48.Tests.TestSet
{
    public interface IComplexService
    {
        ISimpleService SimpleService { get; set; }
    }
}