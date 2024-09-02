namespace AutoInject
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AutoInjectAttribute(Lifetime lifetime) : Attribute
    {
        public Lifetime Lifetime { get; private set; } = lifetime;
    }
}