using AutoInject.Test.Library;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Tests
{
    [Collection("ServiceTests")]
    public class TryAddTests
    {
        [Fact]
        public void ServiceProviderHasServiceOfLifetimeInterface()
        {
            var services = new ServiceCollection().AutoInjectRegisterServices();
            var serviceProvider = services.BuildServiceProvider();

            var testInterface = serviceProvider.GetService<TryAddTestInterface>();
            var serviceDescriptorSingleton = services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestInterface) && service.Lifetime == ServiceLifetime.Singleton);
            var serviceDescriptorScoped = services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestInterface) && service.Lifetime == ServiceLifetime.Scoped);
            var serviceDescriptorTransient = services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestInterface) && service.Lifetime == ServiceLifetime.Transient);

            Assert.NotNull(testInterface);
            Assert.NotNull(serviceDescriptorSingleton);
            Assert.Equal(ServiceLifetime.Singleton, serviceDescriptorSingleton.Lifetime);

            Assert.Null(serviceDescriptorScoped);
            Assert.Null(serviceDescriptorTransient);
        }

        [Fact]
        public void ServiceProviderHasServiceOfLifetimeInterface_AndUsesFirstImplementation()
        {
            var services = new ServiceCollection().AutoInjectRegisterServices();
            var serviceProvider = services.BuildServiceProvider();

            var testInterface = serviceProvider.GetService<TryAddTestInterface>();
            var serviceDescriptor = services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestInterface) && service.Lifetime == ServiceLifetime.Singleton);

            Assert.NotNull(testInterface);
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
            Assert.Equal("a", testInterface.Test());
        }

        [Fact]
        public void ServiceProviderHasClassImplementation_AndUsesScopedImplementation()
        {
            var services = new ServiceCollection().AutoInjectRegisterServices();
            var serviceProvider = services.BuildServiceProvider();

            var testClass = serviceProvider.GetService<TryAddTestClassOnly>();
            var serviceDescriptorScoped = services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestClassOnly) && service.Lifetime == ServiceLifetime.Scoped);
            var serviceDescriptorSingleton = services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestClassOnly) && service.Lifetime == ServiceLifetime.Singleton);
            var serviceDescriptorTransient = services.FirstOrDefault(service => service.ServiceType == typeof(TryAddTestClassOnly) && service.Lifetime == ServiceLifetime.Transient);

            Assert.NotNull(testClass);
            Assert.NotNull(serviceDescriptorScoped);
            Assert.Equal(ServiceLifetime.Scoped, serviceDescriptorScoped.Lifetime);
            Assert.Equal("c", testClass.TestClassOnly());

            Assert.Null(serviceDescriptorSingleton);
            Assert.Null(serviceDescriptorTransient);
        }
    }
}