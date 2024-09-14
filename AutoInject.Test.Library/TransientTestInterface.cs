using AutoInject.Attributes;

namespace AutoInject.Test.Library
{
    internal interface TransientTestInterface
    {
        public string Test();
    }

    [AutoInjectTransient]
    internal class TransientTestClass : TransientTestInterface
    {
        public string Test() => "a";
    }

    [AutoInjectTransient]
    internal class TransientTest2Class : TransientTestInterface
    {
        public string Test() => "b";
    }

    [AutoInjectTransient]
    internal class TransientTestClassOnly
    {
        public string TestClassOnly() => "c";
    }
}