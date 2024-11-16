using AutoInject.Test.Library;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Tests
{
    [Collection("ServiceTests")]
    public class AttributeTests
    {
        private readonly IServiceCollection _services;
        private readonly ServiceProvider _serviceProvider;

        public AttributeTests()
        {
            _services = new ServiceCollection()
                .AutoInjectRegisterServices();

            _serviceProvider = _services.BuildServiceProvider();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient, typeof(TransientTestInterface))]
        [InlineData(ServiceLifetime.Scoped, typeof(ScopedTestInterface))]
        [InlineData(ServiceLifetime.Singleton, typeof(SingletonTestInterface))]
        [InlineData(ServiceLifetime.Transient, typeof(NoServiceLifetimeTestInterface))]
        [InlineData(ServiceLifetime.Singleton, typeof(MultipleLifetimesTestInterface))]
        public void ServiceProviderHasServiceOfLifetimeInterface(ServiceLifetime lifetime, Type type)
        {
            var testInterface = _serviceProvider.GetService(type);
            var serviceDescriptor = _services.FirstOrDefault(service => service.ServiceType == type && service.Lifetime == lifetime);

            Assert.NotNull(testInterface);
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(lifetime, serviceDescriptor.Lifetime);

            foreach (ServiceLifetime noLifetime in ((ServiceLifetime[])Enum.GetValues(typeof(ServiceLifetime))).Where(l => l != lifetime))
            {
                var noDescriptor = _services.FirstOrDefault(service => service.ServiceType == type && service.Lifetime == noLifetime);
                Assert.Null(noDescriptor);
            }
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient, typeof(TransientTestInterface))]
        [InlineData(ServiceLifetime.Scoped, typeof(ScopedTestInterface))]
        [InlineData(ServiceLifetime.Singleton, typeof(SingletonTestInterface))]
        [InlineData(ServiceLifetime.Transient, typeof(NoServiceLifetimeTestInterface))]
        [InlineData(ServiceLifetime.Singleton, typeof(MultipleLifetimesTestInterface))]
        public void ServiceProviderHasServiceOfLifetimeInterface_AndUsesLastImplementation(ServiceLifetime lifetime, Type type)
        {
            var testInterface = _serviceProvider.GetService(type);
            var serviceDescriptor = _services.FirstOrDefault(service => service.ServiceType == type && service.Lifetime == lifetime);
            var methodInfo = type.GetMethod("Test");

            Assert.NotNull(testInterface);
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(lifetime, serviceDescriptor.Lifetime);
            Assert.Equal("b", methodInfo?.Invoke(testInterface, []));

            foreach (ServiceLifetime noLifetime in ((ServiceLifetime[])Enum.GetValues(typeof(ServiceLifetime))).Where(l => l != lifetime))
            {
                var noDescriptor = _services.FirstOrDefault(service => service.ServiceType == type && service.Lifetime == noLifetime);
                Assert.Null(noDescriptor);
            }
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient, typeof(TransientTestInterface))]
        [InlineData(ServiceLifetime.Scoped, typeof(ScopedTestInterface))]
        [InlineData(ServiceLifetime.Singleton, typeof(SingletonTestInterface))]
        [InlineData(ServiceLifetime.Transient, typeof(NoServiceLifetimeTestInterface))]
        [InlineData(ServiceLifetime.Singleton, typeof(MultipleLifetimesTestInterface))]
        public void RetrievingSameImplementationTwiceShouldProduceSameResult(ServiceLifetime lifetime, Type type)
        {
            var testInterface = _serviceProvider.GetService(type);
            var serviceDescriptor = _services.FirstOrDefault(service => service.ServiceType == type);
            var methodInfo = type.GetMethod("Test");

            Assert.NotNull(testInterface);
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(lifetime, serviceDescriptor.Lifetime);
            Assert.Equal("b", methodInfo?.Invoke(testInterface, []));

            // Retreieving twice should produce the same result
            var testInterface2 = _serviceProvider.GetService(type);
            var serviceDescriptor2 = _services.FirstOrDefault(service => service.ServiceType == type && service.Lifetime == lifetime);
            var methodInfo2 = type.GetMethod("Test");

            Assert.NotNull(testInterface2);
            Assert.NotNull(serviceDescriptor2);
            Assert.Equal(lifetime, serviceDescriptor2.Lifetime);
            Assert.Equal("b", methodInfo2?.Invoke(testInterface2, []));

            foreach (ServiceLifetime noLifetime in ((ServiceLifetime[])Enum.GetValues(typeof(ServiceLifetime))).Where(l => l != lifetime))
            {
                var noDescriptor = _services.FirstOrDefault(service => service.ServiceType == type && service.Lifetime == noLifetime);
                Assert.Null(noDescriptor);
            }
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient, typeof(TransientTestClassOnly))]
        [InlineData(ServiceLifetime.Scoped, typeof(ScopedTestClassOnly))]
        [InlineData(ServiceLifetime.Singleton, typeof(SingletonTestClassOnly))]
        [InlineData(ServiceLifetime.Transient, typeof(NoServiceLifetimeTestClassOnly))]
        [InlineData(ServiceLifetime.Scoped, typeof(MultipleLifetimesTestClassOnly))]
        public void ServiceProviderHasClassImplementation(ServiceLifetime lifetime, Type type)
        {
            var testClass = _serviceProvider.GetService(type);
            var serviceDescriptor = _services.FirstOrDefault(service => service.ServiceType == type && service.Lifetime == lifetime);
            var methodInfo = type.GetMethod("TestClassOnly");

            Assert.NotNull(testClass);
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(lifetime, serviceDescriptor.Lifetime);
            Assert.Equal("c", methodInfo?.Invoke(testClass, []));

            foreach (ServiceLifetime noLifetime in ((ServiceLifetime[])Enum.GetValues(typeof(ServiceLifetime))).Where(l => l != lifetime))
            {
                var noDescriptor = _services.FirstOrDefault(service => service.ServiceType == type && service.Lifetime == noLifetime);
                Assert.Null(noDescriptor);
            }
        }

        [Theory]
        [InlineData(typeof(TransientTestClass), typeof(TransientTest2Class))]
        [InlineData(typeof(ScopedTestClass), typeof(ScopedTest2Class))]
        [InlineData(typeof(SingletonTestClass), typeof(SingletonTest2Class))]
        [InlineData(typeof(NoServiceLifetimeTestClass), typeof(NoServiceLifetimeTest2Class))]
        [InlineData(typeof(MultipleLifetimesTest2Class), typeof(MultipleLifetimesTest2Class))]
        public void ServiceProviderShouldNotHaveClassImplmentationOfInterfaceServices(Type type, Type type2)
        {
            var testClass = _serviceProvider.GetService(type);
            var testClass2 = _serviceProvider.GetService(type2);

            Assert.Null(testClass);
            Assert.Null(testClass2);
        }
    }
}