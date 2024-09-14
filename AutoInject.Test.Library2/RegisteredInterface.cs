using AutoInject.Attributes;

namespace AutoInject.SecondAssemblyTest.Library
{
    internal interface RegisteredInterface
    {
        public int Test();
    }

    [AutoInjectTransient]
    internal class RegisteredTestClass : RegisteredInterface
    {
        public int Test() => 1;
    }
}