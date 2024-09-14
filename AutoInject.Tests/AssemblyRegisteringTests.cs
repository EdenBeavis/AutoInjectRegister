using AutoInject.SecondAssemblyTest.Library;
using AutoInject.SecondAssemblyTest.Library.FolderTest;
using AutoInject.Test.Library;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Tests
{
    [Collection("ServiceTests")]
    public class AssemblyRegisteringTests
    {
        private IServiceCollection _serviceCollection;

        public AssemblyRegisteringTests()
        {
            _serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void ServiceProviderWillHaveRegisteredAssemblyService()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(typeof(ScopedTestInterface));
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            var testInterface = serviceProvider.GetService<ScopedTestInterface>();
            Assert.NotNull(testInterface);
        }

        [Fact]
        public void ServiceProviderWontHaveUnregisteredAssemblyService()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(typeof(ScopedTestInterface));
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            var testInterface = serviceProvider.GetService<RegisteredInterface>();
            Assert.Null(testInterface);
        }

        [Fact]
        public void ServiceProviderWontHaveUnregisteredAssemblyServiceButWillHaveRegisteredInterface()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(typeof(ScopedTestInterface));
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            var testInterface = serviceProvider.GetService<ScopedTestInterface>();
            Assert.NotNull(testInterface);
            var testInterface2 = serviceProvider.GetService<RegisteredInterface>();
            Assert.Null(testInterface2);
        }

        [Fact]
        public void ServiceProviderWillHaveInterfacesFromTwoAssemblies()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(typeof(ScopedTestInterface), typeof(RegisteredInterface));
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            var testInterface = serviceProvider.GetService<ScopedTestInterface>();
            Assert.NotNull(testInterface);
            var testInterface2 = serviceProvider.GetService<RegisteredInterface>();
            Assert.NotNull(testInterface2);
        }

        [Fact]
        public void ServiceProviderWillHaveMultipleInterfacesFromAnAssembly()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(typeof(ScopedTestInterface));
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            var testInterface = serviceProvider.GetService<ScopedTestInterface>();
            Assert.NotNull(testInterface);
            var testInterface2 = serviceProvider.GetService<TransientTestInterface>();
            Assert.NotNull(testInterface2);
            var testInterface3 = serviceProvider.GetService<SingletonTestInterface>();
            Assert.NotNull(testInterface3);
        }

        [Fact]
        public void ServiceProviderWillHaveInterfacesInFolders()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(typeof(RegisteredInterface));
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            var testInterface = serviceProvider.GetService<RegisteredInAFolderInterface>();
            Assert.NotNull(testInterface);
            Assert.True(testInterface.Test());
        }
    }
}