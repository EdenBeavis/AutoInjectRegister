using AutoInject.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes;

public class AutoInjectTryAddScopedAttribute() : AutoInjectAttribute(ServiceLifetime.Scoped, AddType.TryAdd)
{
}