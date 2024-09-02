using AutoInject.Tests.TestClasses;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Tests
{
    public class ScopedAttributeTests
    {
        private readonly ServiceProvider _serviceProvider;

        public ScopedAttributeTests()
        {
            var serviceCollection = new ServiceCollection()
                .AutoInjectRegisterServices();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void ServiceProviderHasScopedInterface()
        {
            var testInterface = _serviceProvider.GetService<ScopedTestInterface>();

            Assert.NotNull(testInterface);
        }

        [Fact]
        public void ServiceProviderHasScopedInterface_AndUsesLastImplementation()
        {
            var testInterface = _serviceProvider.GetService<ScopedTestInterface>();

            Assert.NotNull(testInterface);

            // Will be scopedtestclass2 implementation so should be 2
            Assert.Equal(2, testInterface.Test());
        }

        [Fact]
        public void RetrievingSameImplementationTwiceShouldProduceSameResult()
        {
            var testInterface = _serviceProvider.GetService<ScopedTestInterface>();

            Assert.NotNull(testInterface);

            // Will be scopedtestclass2 implementation so should be 2
            Assert.Equal(2, testInterface.Test());

            // Retreieving twice should produce the same result
            var testInterface2 = _serviceProvider.GetService<ScopedTestInterface>();
            Assert.NotNull(testInterface2);
            Assert.Equal(2, testInterface2.Test());
        }

        [Fact]
        public void ServiceProviderHasClassImplementation()
        {
            var testClass = _serviceProvider.GetService<ScopedTestClassOnly>();

            Assert.NotNull(testClass);
            Assert.Equal(3, testClass?.TestClassOnly());
        }

        [Fact]
        public void ServiceProviderShouldNotHaveClassImplmentationOfInterfaceServices()
        {
            var testClass = _serviceProvider.GetService<ScopedTestClass>();
            var testClass2 = _serviceProvider.GetService<ScopedTest2Class>();

            Assert.Null(testClass);
            Assert.Null(testClass2);
        }
    }
}