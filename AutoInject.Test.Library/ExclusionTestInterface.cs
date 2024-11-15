using AutoInject.Attributes;

namespace AutoInject.Test.Library
{
    internal interface ExclusionTestInterface
    {
        public string Test();
    }

    [AutoInjectTransient]
    internal class ExclusionTestClass : ExclusionTestInterface
    {
        public string Test() => "a";
    }

    [AutoInjectTransient]
    internal class ExclusionTest2Class : ExclusionTestInterface
    {
        public string Test() => "b";
    }

    [AutoInjectTransient]
    internal class ExclusionTestClassOnly
    {
        public string TestClassOnly() => "c";
    }
}