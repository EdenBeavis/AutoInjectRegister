using AutoInject.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class AutoInjectAttribute(ServiceLifetime lifetime = ServiceLifetime.Transient, AddType addType = AddType.Add) : Attribute
{
    public ServiceLifetime Lifetime { get; private set; } = lifetime;
    public AddType AddType { get; private set; } = addType;
}