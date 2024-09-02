namespace AutoInject.Tests.TestClasses
{
    internal interface ScopedTestInterface
    {
        public int Test();
    }

    [AutoInject(Lifetime.Scoped)]
    internal class ScopedTestClass : ScopedTestInterface
    {
        public int Test() => 1;
    }

    [AutoInject(Lifetime.Scoped)]
    internal class ScopedTest2Class : ScopedTestInterface
    {
        public int Test() => 2;
    }

    [AutoInject(Lifetime.Scoped)]
    internal class ScopedTestClassOnly
    {
        public int TestClassOnly() => 3;
    }
}