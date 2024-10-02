using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CastleWindsor.CyclicResolution.Tests.TestSet;

namespace CastleWindsor.CyclicResolution.Tests
{
    public class CyclicResolutionUnresolvedTest
    {
        private readonly WindsorContainer _container;

        public CyclicResolutionUnresolvedTest()
        {
            _container = new WindsorContainer();
        }
        
        [Fact]
        public void SimpleCyclicDependency()
        {
            _container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
            _container.Register(Component.For<IComplexService>().ImplementedBy<ComplexService>());
            Assert.Null(_container.Resolve<ISimpleService>().ComplexService?.SimpleService);
        }

        [Fact]
        public void ComplexCyclicDependency()
        {
            _container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
            _container.Register(Component.For<IComplexService>().ImplementedBy<ComplexService>());
            _container.Register(Component.For<ISuperComplexService>().ImplementedBy<SuperComplexService>());
            Assert.Null(_container.Resolve<ISuperComplexService>().ComplexService?.SimpleService);
        }

    }
}