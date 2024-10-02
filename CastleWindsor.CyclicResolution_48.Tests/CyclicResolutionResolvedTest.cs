using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CastleWindsor.CyclicResolution_48.Tests.TestSet;
using CastleWindsor.CyclicResolution_net48;
using Xunit;

namespace CastleWindsor.CyclicResolution_48.Tests
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