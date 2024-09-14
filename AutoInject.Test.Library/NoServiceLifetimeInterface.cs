using AutoInject.Attributes;

namespace AutoInject.Test.Library
{
    internal interface NoServiceLifetimeTestInterface
    {
        public string Test();
    }

    [AutoInject]
    internal class NoServiceLifetimeTestClass : NoServiceLifetimeTestInterface
    {
        public string Test() => "a";
    }

    [AutoInject]
    internal class NoServiceLifetimeTest2Class : NoServiceLifetimeTestInterface
    {
        public string Test() => "b";
    }

    [AutoInject]
    internal class NoServiceLifetimeTestClassOnly
    {
        public string TestClassOnly() => "c";
    }
}