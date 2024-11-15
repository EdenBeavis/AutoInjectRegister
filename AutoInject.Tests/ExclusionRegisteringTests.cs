using AutoInject.Test.Library;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Tests
{
    [Collection("ServiceTests")]
    public class ExclusionRegisteringTests
    {
        private IServiceCollection _serviceCollection;

        public ExclusionRegisteringTests()
        {
            _serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void ServiceProviderWillExcludeInterfaceService()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(new AutoInjectorOptions { TypesToExclude = [typeof(ExclusionTestInterface)] });
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            //Get no interface
            var testInterface = serviceProvider.GetService<ExclusionTestInterface>();
            Assert.Null(testInterface);
        }

        [Fact]
        public void ServiceProviderWillExcludeAClassService()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(new AutoInjectorOptions { TypesToExclude = [typeof(ExclusionTest2Class)] });
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            //Getting interface should return something
            var testInterface = serviceProvider.GetService<ExclusionTestInterface>();
            Assert.NotNull(testInterface);
            Assert.Equal("a", testInterface.Test());

            //Getting class should return nothing
            var testClass = serviceProvider.GetService<ExclusionTest2Class>();
            Assert.Null(testClass);
        }

        [Fact]
        public void ServiceProviderWillExcludeTwoClassServices()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(new AutoInjectorOptions { TypesToExclude = [typeof(ExclusionTestClass), typeof(ExclusionTest2Class)] });
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            //Getting interface should return nothing
            var testInterface = serviceProvider.GetService<ExclusionTestInterface>();
            Assert.Null(testInterface);

            //Getting class should return nothing
            var testClass = serviceProvider.GetService<ExclusionTestClass>();
            Assert.Null(testClass);
            var testClass2 = serviceProvider.GetService<ExclusionTest2Class>();
            Assert.Null(testClass2);
        }

        [Fact]
        public void ServiceProviderWillExcludeInterfaceCanGetAnotherInterfaceService()
        {
            _serviceCollection = _serviceCollection.AutoInjectRegisterServices(new AutoInjectorOptions { TypesToExclude = [typeof(ExclusionTestInterface)] });
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            //Get no interface
            var testInterface = serviceProvider.GetService<ExclusionTestInterface>();
            Assert.Null(testInterface);

            //Get an interface
            var testInterface2 = serviceProvider.GetService<TransientTestInterface>();
            Assert.NotNull(testInterface2);
            Assert.Equal("b", testInterface2.Test());
        }
    }
}