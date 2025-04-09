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
            AutoRegister([typeof(TransientTestInterface)], InclusionType.All, useActionToCreateOption);
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

        private void AutoRegister(Type[] typesToScan, InclusionType inclusionType, bool useActionToCreateOption)
        {
            if (useActionToCreateOption)
                _serviceCollection = _serviceCollection.AutoInjectRegisterServices(options =>
                {
                    options.TypesToScan = typesToScan;
                    options.InclusionType = inclusionType;
                });
            else
                _serviceCollection = _serviceCollection.AutoInjectRegisterServices(new AutoInjectorOptions { TypesToScan = typesToScan, InclusionType = inclusionType });
        }
    }
}