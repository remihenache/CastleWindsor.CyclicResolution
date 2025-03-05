using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CastleWindsor.CyclicResolution.Tests.TestSet;
using Component = Castle.MicroKernel.Registration.Component;

namespace CastleWindsor.CyclicResolution.Tests
{
    public class CyclicResolutionResolvedTest
    {
        private readonly WindsorContainer _container;

        public CyclicResolutionResolvedTest()
        {
            _container = new WindsorContainer();
            _container.ResolveCyclicDependencies();
        }
        
        [Fact]
        public void OpenGenericCyclicDependency()
        {
            _container.Register(Component.For(typeof(IGenericService<>)).ImplementedBy(typeof(GenericService<>)));
            _container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
            _container.Register(Component.For<IComplexService>().ImplementedBy<ComplexService>());
            var complexService = _container.Resolve<IGenericService<ISimpleService>>();
            Assert.NotNull(complexService);
            Assert.NotNull(complexService.Value);
        }
        [Fact]
        public void SimpleCyclicDependency()
        {
            _container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
            _container.Register(Component.For<IComplexService>().ImplementedBy<ComplexService>());
            var complexService = _container.Resolve<ISimpleService>().ComplexService;
            Assert.NotNull(complexService);
            Assert.NotNull(complexService.SimpleService);
        }

        [Fact]
        public void ComplexCyclicDependency()
        {
            _container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
            _container.Register(Component.For<IComplexService>().ImplementedBy<ComplexService>());
            _container.Register(Component.For<ISuperComplexService>().ImplementedBy<SuperComplexService>());
            var complexService = _container.Resolve<ISuperComplexService>();
            Assert.NotNull(complexService.SimpleService);
            Assert.NotNull(complexService.ComplexService);
            Assert.NotNull(complexService.SimpleService.ComplexService);
            Assert.NotNull(complexService.ComplexService.SimpleService);
            Assert.Equal(complexService.SimpleService, complexService.ComplexService.SimpleService);
            Assert.Equal(complexService.ComplexService, complexService.SimpleService.ComplexService);
        }

        [Fact]
        public void NamedComplexCyclicDependency()
        {
            _container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
            _container.Register(Component.For<IComplexService>().ImplementedBy<ComplexService>());
            _container.Register(Component.For<IComplexService>().ImplementedBy<ComplexService>().Named("ComplexService"));
            _container.Register(Component.For<ISuperComplexService>().ImplementedBy<SuperComplexService>().DependsOn(Dependency.OnComponent("ComplexService", "ComplexService")));
            var complexService = _container.Resolve<ISuperComplexService>();
            Assert.NotNull(complexService.SimpleService);
            Assert.NotNull(complexService.ComplexService);
            Assert.NotNull(complexService.SimpleService.ComplexService);
            Assert.NotNull(complexService.ComplexService.SimpleService);
            Assert.Equal(complexService.SimpleService, complexService.ComplexService.SimpleService);
            Assert.NotEqual(complexService.ComplexService, complexService.SimpleService.ComplexService);
        }
    }
}