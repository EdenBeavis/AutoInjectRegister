using AutoInject.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes;

public class AutoInjectTransientAttribute() : AutoInjectAttribute(ServiceLifetime.Transient, AddType.Add)
{
}