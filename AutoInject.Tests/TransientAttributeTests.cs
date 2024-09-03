using AutoInject.Test.Library;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Tests
{
    public class TransientAttributeTests
    {
        private readonly ServiceProvider _serviceProvider;

        public TransientAttributeTests()
        {
            var serviceCollection = new ServiceCollection()
                .AutoInjectRegisterServices();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void ServiceProviderHasTransientInterface()
        {
            var testInterface = _serviceProvider.GetService<TransientTestInterface>();

            Assert.NotNull(testInterface);
        }

        [Fact]
        public void ServiceProviderHasTransientInterface_AndUsesLastImplementation()
        {
            var testInterface = _serviceProvider.GetService<TransientTestInterface>();

            Assert.NotNull(testInterface);

            // Will be transienttestclass2 implementation so should be b
            Assert.Equal("b", testInterface.Test());
        }

        [Fact]
        public void RetrievingSameImplementationTwiceShouldProduceSameResult()
        {
            var testInterface = _serviceProvider.GetService<TransientTestInterface>();

            Assert.NotNull(testInterface);

            // Will be transienttestclass2 implementation so should be b
            Assert.Equal("b", testInterface.Test());

            // Retreiving twice should produce the same result
            var testInterface2 = _serviceProvider.GetService<TransientTestInterface>();
            Assert.NotNull(testInterface2);
            Assert.Equal("b", testInterface2.Test());
        }

        [Fact]
        public void ServiceProviderHasClassImplementation()
        {
            var testClass = _serviceProvider.GetService<TransientTestClassOnly>();

            Assert.NotNull(testClass);
            Assert.Equal("c", testClass?.TestClassOnly());
        }

        [Fact]
        public void ServiceProviderShouldNotHaveClassImplmentationOfInterfaceServices()
        {
            var testClass = _serviceProvider.GetService<TransientTestClass>();
            var testClass2 = _serviceProvider.GetService<TransientTest2Class>();

            Assert.Null(testClass);
            Assert.Null(testClass2);
        }
    }
}