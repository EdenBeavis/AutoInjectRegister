using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes;

public class AutoInjectTransientAttribute(AddType addType = AddType.Add) : AutoInjectAttribute(ServiceLifetime.Transient, addType)
{
}