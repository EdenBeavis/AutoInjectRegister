using AutoInject.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Test.Library
{
    internal interface MultipleLifetimesTestInterface
    {
        public string Test();
    }

    [AutoInjectSingleton] // What should be implemented
    [AutoInjectScoped]
    [AutoInjectTransient]
    [AutoInject(ServiceLifetime.Scoped)]
    internal class MultipleLifetimesTestClass : MultipleLifetimesTestInterface
    {
        public string Test() => "a";
    }

    [AutoInjectScoped]
    [AutoInjectTransient]
    [AutoInject(ServiceLifetime.Scoped)]
    [AutoInjectSingleton] // What should be implemented
    internal class MultipleLifetimesTest2Class : MultipleLifetimesTestInterface
    {
        public string Test() => "b";
    }

    [AutoInjectTransient]
    [AutoInject(ServiceLifetime.Scoped)] // What should be implemented
    [AutoInjectScoped]
    internal class MultipleLifetimesTestClassOnly
    {
        public string TestClassOnly() => "c";
    }
}