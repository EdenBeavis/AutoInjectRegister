using AutoInject.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Test.Library
{
    internal interface TryAddTestInterface
    {
        public string Test();
    }

    [AutoInjectSingleton(AddType.TryAdd)] // This should be the only implemented interface
    [AutoInjectScoped(AddType.TryAdd)]
    [AutoInjectTransient(AddType.TryAdd)]
    [AutoInject(ServiceLifetime.Scoped, AddType.TryAdd)]
    internal class TryAddTestClass : TryAddTestInterface
    {
        public string Test() => "a";
    }

    // This should no be implemented at all
    [AutoInjectScoped(AddType.TryAdd)]
    [AutoInjectTransient(AddType.TryAdd)]
    [AutoInject(ServiceLifetime.Scoped, AddType.TryAdd)]
    [AutoInjectSingleton(AddType.TryAdd)]
    internal class TryAddTest2Class : TryAddTestInterface
    {
        public string Test() => "b";
    }

    [AutoInjectTransient(AddType.TryAdd)]
    [AutoInject(ServiceLifetime.Scoped, AddType.TryAdd)] // Due to ordering, this should be the only implemented interface
    [AutoInjectScoped(AddType.TryAdd)]
    internal class TryAddTestClassOnly
    {
        public string TestClassOnly() => "c";
    }
}