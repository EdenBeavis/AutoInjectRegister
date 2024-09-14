using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AutoInjectAttribute(ServiceLifetime lifetime = ServiceLifetime.Transient) : Attribute
    {
        public ServiceLifetime Lifetime { get; private set; } = lifetime;
    }
}