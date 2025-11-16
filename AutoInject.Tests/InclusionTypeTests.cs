using AutoInject.Enums;
using AutoInject.SecondAssemblyTest.Library;
using AutoInject.SecondAssemblyTest.Library.FolderTest;
using AutoInject.Test.Library;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Tests
{
    [Collection("ServiceTests")]
    public class InclusionTypeTests
    {
        private IServiceCollection _serviceCollection;

        public InclusionTypeTests()
        {
            _serviceCollection = new ServiceCollection();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ServiceProviderWillExcludeInterfacesNotInScanListService(bool useActionToCreateOption)
        {
            AutoRegister([typeof(TransientTestInterface)], InclusionType.TypesToScanOnly, useActionToCreateOption);
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            // Get included interface
            var testInterface = serviceProvider.GetService<TransientTestInterface>();
            Assert.NotNull(testInterface);

            // Class not added so don't included
            var classInterface = serviceProvider.GetService<TransientTestClassOnly>();
            Assert.Null(classInterface);

            // Other not added interface
            var testInterface2 = serviceProvider.GetService<SingletonTestInterface>();
            Assert.Null(testInterface2);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ServiceProviderWillExcludeInterfacesNotInScanListWithMultipleTypesToScanService(bool useActionToCreateOption)
        {
            AutoRegister([typeof(TransientTestInterface), typeof(RegisteredInterface)], InclusionType.TypesToScanOnly, useActionToCreateOption);
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            // Get included interface
            var testInterface = serviceProvider.GetService<TransientTestInterface>();
            Assert.NotNull(testInterface);

            // Get included interface
            var testInterface2 = serviceProvider.GetService<RegisteredInterface>();
            Assert.NotNull(testInterface2);

            // Other not added interface
            var testInterface3 = serviceProvider.GetService<ScopedTestInterface>();
            Assert.Null(testInterface3);

            // Other not added interface
            var testInterface4 = serviceProvider.GetService<RegisteredInAFolderInterface>();
            Assert.Null(testInterface4);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ServiceProviderWillIncludeAllInterfacesService(bool useActionToCreateOption)
        {
            AutoRegister([typeof(TransientTestInterface)], InclusionType.AllAutoAttributes, useActionToCreateOption);
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            //Get included interface
            var testInterface = serviceProvider.GetService<TransientTestInterface>();
            Assert.NotNull(testInterface);

            // Class not added but should still register
            var classInterface = serviceProvider.GetService<TransientTestClassOnly>();
            Assert.NotNull(classInterface);

            // Other not added interface, but still added
            var testInterface2 = serviceProvider.GetService<SingletonTestInterface>();
            Assert.NotNull(testInterface2);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ServiceProviderWillDoItAllForMeScoped(bool useActionToCreateOption)
        {
            AutoRegister([typeof(TransientTestInterface)], InclusionType.NoAttributeRegister, useActionToCreateOption, ServiceLifetime.Scoped);
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            // Ensure interface is still transisent if it had an attribute
            var testInterface = _serviceCollection.FirstOrDefault(service => service.ServiceType == typeof(TransientTestInterface) && service.Lifetime == ServiceLifetime.Transient);
            Assert.NotNull(testInterface);

            // Ensure class is still transisent if had no attribute
            var classInterface = _serviceCollection.FirstOrDefault(service => service.ServiceType == typeof(TransientTestClassOnly) && service.Lifetime == ServiceLifetime.Transient);
            Assert.NotNull(classInterface);

            // Other not added interface, but still added
            var testInterface2 = _serviceCollection.FirstOrDefault(service => service.ServiceType == typeof(NoAttributeTestInterface) && service.Lifetime == ServiceLifetime.Scoped);
            Assert.NotNull(testInterface2);

            foreach (ServiceLifetime noLifetime in ((ServiceLifetime[])Enum.GetValues(typeof(ServiceLifetime))).Where(l => l != ServiceLifetime.Scoped))
            {
                var noDescriptor = _serviceCollection.FirstOrDefault(service => service.ServiceType == typeof(NoAttributeTestInterface) && service.Lifetime == noLifetime);
                Assert.Null(noDescriptor);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ServiceProviderWillDoItAllForMeDefault(bool useActionToCreateOption)
        {
            AutoRegister([typeof(TransientTestInterface)], InclusionType.NoAttributeRegister, useActionToCreateOption);
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            // Other not added interface, but still added
            var testInterface2 = _serviceCollection.FirstOrDefault(service => service.ServiceType == typeof(NoAttributeTestInterface) && service.Lifetime == ServiceLifetime.Transient);
            Assert.NotNull(testInterface2);

            foreach (ServiceLifetime noLifetime in ((ServiceLifetime[])Enum.GetValues(typeof(ServiceLifetime))).Where(l => l != ServiceLifetime.Transient))
            {
                var noDescriptor = _serviceCollection.FirstOrDefault(service => service.ServiceType == typeof(NoAttributeTestInterface) && service.Lifetime == noLifetime);
                Assert.Null(noDescriptor);
            }
        }

        private void AutoRegister(Type[] typesToScan, InclusionType inclusionType, bool useActionToCreateOption, ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
        {
            if (useActionToCreateOption)
                _serviceCollection = _serviceCollection.AutoInjectRegisterServices(options =>
                {
                    options.TypesToScan = typesToScan;
                    options.InclusionType = inclusionType;
                    options.DefaultLifetime = defaultLifetime;
                });
            else
                _serviceCollection = _serviceCollection.AutoInjectRegisterServices(
                    new AutoInjectorOptions
                    {
                        TypesToScan = typesToScan,
                        InclusionType = inclusionType,
                        DefaultLifetime = defaultLifetime
                    });
        }
    }
}