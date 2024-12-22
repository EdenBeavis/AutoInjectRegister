using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes;

public class AutoInjectSingletonAttribute(AddType addType = AddType.Add) : AutoInjectAttribute(ServiceLifetime.Singleton, addType)
{
}