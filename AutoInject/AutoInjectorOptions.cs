using AutoInject.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject;

public class AutoInjectorOptions
{
    public IEnumerable<Type> TypesToScan { get; set; } = [];
    public IEnumerable<Type> TypesToExclude { get; set; } = [];
    public InclusionType InclusionType { get; set; } = InclusionType.All;
    public ServiceLifetime DefaultLifetime { get; set; } = ServiceLifetime.Transient;
}