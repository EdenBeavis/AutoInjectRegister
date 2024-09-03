namespace AutoInject.Test.Library2.FolderTest
{
    internal interface RegisteredInAFolderInterface
    {
        public bool Test();
    }

    [AutoInject(Lifetime.Singleton)]
    internal class RegisteredInAFolderClass : RegisteredInAFolderInterface
    {
        public bool Test() => true;
    }
}