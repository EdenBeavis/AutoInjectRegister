using AutoInject.Attributes;

namespace AutoInject.Test.Library
{
    internal interface ScopedTestInterface
    {
        public string Test();
    }

    [AutoInjectScoped]
    internal class ScopedTestClass : ScopedTestInterface
    {
        public string Test() => "a";
    }

    [AutoInjectScoped]
    internal class ScopedTest2Class : ScopedTestInterface
    {
        public string Test() => "b";
    }

    [AutoInjectScoped]
    internal class ScopedTestClassOnly
    {
        public string TestClassOnly() => "c";
    }
}