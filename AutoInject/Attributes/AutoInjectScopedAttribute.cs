using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes
{
    public class AutoInjectScopedAttribute : AutoInjectAttribute
    {
        public AutoInjectScopedAttribute() : base(ServiceLifetime.Scoped)
        {
        }
    }
}