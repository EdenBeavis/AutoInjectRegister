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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ServiceProviderWillExcludeInterfaceService(bool useActionToCreateOption)
        {
            AutoRegister([typeof(ExclusionTestInterface)], useActionToCreateOption);
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            //Get no interface
            var testInterface = serviceProvider.GetService<ExclusionTestInterface>();
            Assert.Null(testInterface);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ServiceProviderWillExcludeAClassService(bool useActionToCreateOption)
        {
            AutoRegister([typeof(ExclusionTest2Class)], useActionToCreateOption);
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            //Getting interface should return something
            var testInterface = serviceProvider.GetService<ExclusionTestInterface>();
            Assert.NotNull(testInterface);
            Assert.Equal("a", testInterface.Test());

            //Getting class should return nothing
            var testClass = serviceProvider.GetService<ExclusionTest2Class>();
            Assert.Null(testClass);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ServiceProviderWillExcludeTwoClassServices(bool useActionToCreateOption)
        {
            AutoRegister([typeof(ExclusionTestClass), typeof(ExclusionTest2Class)], useActionToCreateOption);
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ServiceProviderWillExcludeInterfaceCanGetAnotherInterfaceService(bool useActionToCreateOption)
        {
            AutoRegister([typeof(ExclusionTestInterface)], useActionToCreateOption);
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            //Get no interface
            var testInterface = serviceProvider.GetService<ExclusionTestInterface>();
            Assert.Null(testInterface);

            //Get an interface
            var testInterface2 = serviceProvider.GetService<TransientTestInterface>();
            Assert.NotNull(testInterface2);
            Assert.Equal("b", testInterface2.Test());
        }

        private void AutoRegister(Type[] typesToExclude, bool useActionToCreateOption)
        {
            if (useActionToCreateOption)
                _serviceCollection = _serviceCollection.AutoInjectRegisterServices(options =>
                {
                    options.TypesToExclude = typesToExclude;
                });
            else
                _serviceCollection = _serviceCollection.AutoInjectRegisterServices(new AutoInjectorOptions { TypesToExclude = typesToExclude });
        }
    }
}