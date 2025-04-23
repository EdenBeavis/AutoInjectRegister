using AutoInject.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes;

public class AutoInjectScopedAttribute() : AutoInjectAttribute(ServiceLifetime.Scoped, AddType.Add)
{
}