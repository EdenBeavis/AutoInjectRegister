namespace AutoInject.Tests.TestClasses
{
    internal interface SingletonTestInterface
    {
        public bool Test();
    }

    [AutoInject(Lifetime.Singleton)]
    internal class SingletonTestClass : SingletonTestInterface
    {
        public bool Test() => true;
    }

    [AutoInject(Lifetime.Singleton)]
    internal class SingletonTest2Class : SingletonTestInterface
    {
        public bool Test() => false;
    }

    [AutoInject(Lifetime.Singleton)]
    internal class SingletonTestClassOnly
    {
        public bool TestClassOnly() => true;
    }
}