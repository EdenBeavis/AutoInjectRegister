using AutoInject.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.SecondAssemblyTest.Library.FolderTest
{
    internal interface RegisteredInAFolderInterface
    {
        public bool Test();
    }

    [AutoInject(ServiceLifetime.Singleton)]
    internal class RegisteredInAFolderClass : RegisteredInAFolderInterface
    {
        public bool Test() => true;
    }
}