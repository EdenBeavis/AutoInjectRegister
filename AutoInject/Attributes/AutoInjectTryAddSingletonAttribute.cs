using AutoInject.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes;

public class AutoInjectTryAddSingletonAttribute() : AutoInjectAttribute(ServiceLifetime.Singleton, AddType.TryAdd)
{
}