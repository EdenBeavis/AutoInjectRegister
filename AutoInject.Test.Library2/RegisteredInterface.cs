namespace AutoInject.Test.Library2
{
    internal interface RegisteredInterface
    {
        public int Test();
    }

    [AutoInject(Lifetime.Transient)]
    internal class RegisteredTestClass : RegisteredInterface
    {
        public int Test() => 1;
    }
}