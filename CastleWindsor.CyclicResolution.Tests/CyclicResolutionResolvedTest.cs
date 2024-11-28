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
        public void SimpleGenericCyclicDependency()
        {
            _container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
            _container.Register(Component.For<IComplexService>().ImplementedBy<ComplexService>());
            _container.Register(Component.For(typeof(IGenericService<>)).ImplementedBy(typeof(GenericService<>)));
            var complexService = _container.Resolve<IGenericService<ISimpleService>>().Service;
            Assert.NotNull(complexService);
            Assert.NotNull(complexService.ComplexService);
        }

        [Fact]
        public void GenericWithConstructorCyclicDependency()
        {
            _container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
            _container.Register(Component.For<IComplexService>().ImplementedBy<ComplexService>());
            _container.Register(Component.For(typeof(IGenericService<>)).ImplementedBy(typeof(GenericService<>)));
            _container.Register(Component.For(typeof(ConstructorGenericService<>)).ImplementedBy(typeof(ConstructorGenericService<>)));
            var complexService = _container.Resolve<ConstructorGenericService<ISimpleService>>().Service;
            Assert.NotNull(complexService);
            Assert.NotNull(complexService.Service);
            Assert.NotNull(complexService.Service.ComplexService);
        }
        [Fact]
        public void GenericWithCustomCyclicDependency()
        {
            _container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
            _container.Register(Component.For<IComplexService>().ImplementedBy<ComplexService>());
            _container.Register(Component.For(typeof(IGenericService<>)).ImplementedBy(typeof(GenericService<>), new GenericImplementationMatchingStrategy()));
            _container.Register(Component.For<AGenericService>().ImplementedBy<AGenericService>());
            var complexService = _container.Resolve<AGenericService>();
            Assert.NotNull(complexService);
            Assert.NotNull(complexService.Service);
            Assert.NotNull(complexService.Service.ComplexService);
            Assert.NotNull(complexService.GenericService);
            Assert.NotNull(complexService.GenericService.Service);
            Assert.Equal(complexService.Service.ComplexService, complexService.GenericService.Service);
            Assert.Equal(complexService.Service, complexService.GenericService.Service.SimpleService);
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