using AutoInject.Test.Library;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Tests
{
    [Collection("ServiceTests")]
    public class TryAddTests
    {
        private readonly IServiceCollection _services;
        private readonly ServiceProvider _serviceProvider;

        public TryAddTests()
        {
            _services = new ServiceCollection()
                .AutoInjectRegisterServices();

            _serviceProvider = _services.BuildServiceProvider();
        }

        [Fact]
        public void ServiceProviderHasServiceOfLifetimeInterface()
        {
            var testInterface = _serviceProvider.GetService<TryAddTestInterface>();
            var serviceDescriptorSingleton = _services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestInterface) && service.Lifetime == ServiceLifetime.Singleton);
            var serviceDescriptorScoped = _services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestInterface) && service.Lifetime == ServiceLifetime.Scoped);
            var serviceDescriptorTransient = _services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestInterface) && service.Lifetime == ServiceLifetime.Transient);

            Assert.NotNull(testInterface);
            Assert.NotNull(serviceDescriptorSingleton);
            Assert.Equal(ServiceLifetime.Singleton, serviceDescriptorSingleton.Lifetime);

            Assert.Null(serviceDescriptorScoped);
            Assert.Null(serviceDescriptorTransient);
        }

        [Fact]
        public void ServiceProviderHasServiceOfLifetimeInterface_AndUsesFirstImplementation()
        {
            var testInterface = _serviceProvider.GetService<TryAddTestInterface>();
            var serviceDescriptor = _services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestInterface) && service.Lifetime == ServiceLifetime.Singleton);

            Assert.NotNull(testInterface);
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
            Assert.Equal("a", testInterface.Test());
        }

        [Fact]
        public void ServiceProviderHasClassImplementation_AndUsesScopedImplementation()
        {
            var testClass = _serviceProvider.GetService<TryAddTestClassOnly>();
            var serviceDescriptorScoped = _services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestClassOnly) && service.Lifetime == ServiceLifetime.Scoped);
            var serviceDescriptorSingleton = _services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestClassOnly) && service.Lifetime == ServiceLifetime.Singleton);
            var serviceDescriptorTransient = _services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestClassOnly) && service.Lifetime == ServiceLifetime.Transient);

            Assert.NotNull(testClass);
            Assert.NotNull(serviceDescriptorScoped);
            Assert.Equal(ServiceLifetime.Scoped, serviceDescriptorScoped.Lifetime);
            Assert.Equal("c", testClass.TestClassOnly());

            Assert.Null(serviceDescriptorSingleton);
            Assert.Null(serviceDescriptorTransient);
        }
    }
}