using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes
{
    public class AutoInjectSingletonAttribute : AutoInjectAttribute
    {
        public AutoInjectSingletonAttribute() : base(ServiceLifetime.Singleton)
        {
        }
    }
}