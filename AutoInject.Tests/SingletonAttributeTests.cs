using AutoInject.Tests.TestClasses;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Tests
{
    public class SingletonAttributeTests
    {
        private readonly ServiceProvider _serviceProvider;

        public SingletonAttributeTests()
        {
            var serviceCollection = new ServiceCollection()
                .AutoInjectRegisterServices();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void ServiceProviderHasSingletonInterface()
        {
            var testInterface = _serviceProvider.GetService<SingletonTestInterface>();

            Assert.NotNull(testInterface);
        }

        [Fact]
        public void ServiceProviderHasSingletonInterface_AndClassImplementation()
        {
            var testInterface = _serviceProvider.GetService<SingletonTestInterface>();

            Assert.NotNull(testInterface);

            //Should be last implementation
            Assert.False(testInterface.Test());
        }

        [Fact]
        public void RetrievingSameImplementationTwiceShouldProduceSameResult()
        {
            var testInterface = _serviceProvider.GetService<SingletonTestInterface>();

            Assert.NotNull(testInterface);

            // Will be scopedtestclass2 implementation so should be false
            Assert.False(testInterface.Test());

            // Retreieving twice should produce the same result
            var testInterface2 = _serviceProvider.GetService<SingletonTestInterface>();
            Assert.NotNull(testInterface2);
            Assert.False(testInterface2.Test());
        }

        [Fact]
        public void ServiceProviderHasClassImplementation()
        {
            var testClass = _serviceProvider.GetService<SingletonTestClassOnly>();

            Assert.NotNull(testClass);
            Assert.True(testClass?.TestClassOnly());
        }

        [Fact]
        public void ServiceProviderShouldNotHaveClassImplmentationOfInterfaceServices()
        {
            var testClass = _serviceProvider.GetService<SingletonTestClass>();
            var testClass2 = _serviceProvider.GetService<SingletonTest2Class>();

            Assert.Null(testClass);
            Assert.Null(testClass2);
        }
    }
}