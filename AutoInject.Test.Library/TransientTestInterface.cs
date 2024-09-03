namespace AutoInject.Test.Library
{
    internal interface TransientTestInterface
    {
        public string Test();
    }

    [AutoInject(Lifetime.Transient)]
    internal class TransientTestClass : TransientTestInterface
    {
        public string Test() => "a";
    }

    [AutoInject(Lifetime.Transient)]
    internal class TransientTest2Class : TransientTestInterface
    {
        public string Test() => "b";
    }

    [AutoInject(Lifetime.Transient)]
    internal class TransientTestClassOnly
    {
        public string TestClassOnly() => "c";
    }
}