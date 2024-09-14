using AutoInject.Attributes;

namespace AutoInject.Test.Library
{
    internal interface SingletonTestInterface
    {
        public string Test();
    }

    [AutoInjectSingleton]
    internal class SingletonTestClass : SingletonTestInterface
    {
        public string Test() => "a";
    }

    [AutoInjectSingleton]
    internal class SingletonTest2Class : SingletonTestInterface
    {
        public string Test() => "b";
    }

    [AutoInjectSingleton]
    internal class SingletonTestClassOnly
    {
        public string TestClassOnly() => "c";
    }
}