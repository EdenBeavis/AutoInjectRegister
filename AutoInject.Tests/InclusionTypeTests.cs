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

        [Fact]
        public void ServiceProviderWillExcludeInterfacesNotInScanListService()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(new AutoInjectorOptions { TypesToScan = [typeof(TransientTestInterface)], InclusionType = InclusionType.TypesToScanOnly });
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

        [Fact]
        public void ServiceProviderWillExcludeInterfacesNotInScanListWithMultipleTypesToScanService()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(new AutoInjectorOptions { TypesToScan = [typeof(TransientTestInterface), typeof(RegisteredInterface)], InclusionType = InclusionType.TypesToScanOnly });
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

        [Fact]
        public void ServiceProviderWillIncludeAllInterfacesService()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(new AutoInjectorOptions { TypesToScan = [typeof(TransientTestInterface)], InclusionType = InclusionType.All });
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            //Get included interface
            var testInterface = serviceProvider.GetService<TransientTestInterface>();
            Assert.NotNull(testInterface);

            // Class not added so don't included
            var classInterface = serviceProvider.GetService<TransientTestClassOnly>();
            Assert.NotNull(classInterface);

            // Other not added interface
            var testInterface2 = serviceProvider.GetService<SingletonTestInterface>();
            Assert.NotNull(testInterface2);
        }
    }
}