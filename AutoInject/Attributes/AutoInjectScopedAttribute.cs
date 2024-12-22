using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes;

public class AutoInjectScopedAttribute(AddType addType = AddType.Add) : AutoInjectAttribute(ServiceLifetime.Scoped, addType)
{
}