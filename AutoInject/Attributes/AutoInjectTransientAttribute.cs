using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes
{
    public class AutoInjectTransientAttribute : AutoInjectAttribute
    {
        public AutoInjectTransientAttribute() : base(ServiceLifetime.Transient)
        {
        }
    }
}