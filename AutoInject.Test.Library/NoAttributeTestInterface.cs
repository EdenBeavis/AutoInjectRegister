namespace AutoInject.Test.Library
{
    internal interface NoAttributeTestInterface
    {
        public string Test();
    }

    internal class NoAttributeTestClass : NoAttributeTestInterface
    {
        public string Test() => "a";
    }

    internal class NoAttributeTest2Class : NoAttributeTestInterface
    {
        public string Test() => "b";
    }

    internal class NoAttributeTestClassOnly
    {
        public string TestClassOnly() => "c";
    }
}