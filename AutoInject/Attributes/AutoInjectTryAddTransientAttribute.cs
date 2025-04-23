using AutoInject.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes;

public class AutoInjectTryAddTransientAttribute() : AutoInjectAttribute(ServiceLifetime.Transient, AddType.TryAdd)
{
}