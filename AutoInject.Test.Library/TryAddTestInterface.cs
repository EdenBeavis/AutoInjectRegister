using AutoInject.Attributes;
using AutoInject.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Test.Library
{
    internal interface TryAddTestInterface
    {
        public string Test();
    }

    [AutoInjectTryAddSingleton] // This should be the only implemented interface
    [AutoInjectTryAddScoped]
    [AutoInjectTryAddTransient]
    [AutoInject(ServiceLifetime.Scoped, AddType.TryAdd)]
    internal class TryAddTestClass : TryAddTestInterface
    {
        public string Test() => "a";
    }

    // This should no be implemented at all
    [AutoInjectTryAddScoped]
    [AutoInjectTryAddTransient]
    [AutoInject(ServiceLifetime.Scoped, AddType.TryAdd)]
    [AutoInjectTryAddSingleton]
    internal class TryAddTest2Class : TryAddTestInterface
    {
        public string Test() => "b";
    }

    [AutoInjectTryAddTransient]
    [AutoInject(ServiceLifetime.Scoped, AddType.TryAdd)] // Due to ordering, this should be the only implemented interface
    [AutoInjectTryAddScoped]
    internal class TryAddTestClassOnly
    {
        public string TestClassOnly() => "c";
    }
}