using AutoInject.Attributes;
using AutoInject.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace AutoInject;

internal class AutoInjector
{
    private readonly IServiceCollection _services;
    private readonly AutoInjectorOptions _options;

    internal AutoInjector(IServiceCollection services, AutoInjectorOptions? options = null)
    {
        _services = services;
        _options = options ?? new AutoInjectorOptions();
    }

    internal void Register()
    {
        var assemblies = GetAssemblies();
        var implementingClasses = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(HasAutoAttributes)
            .ToList();

        foreach (ServiceLifetime lifetime in Enum.GetValues<ServiceLifetime>())
            AddAllServicesOfLifeTime(implementingClasses, lifetime);
    }

    private void AddAllServicesOfLifeTime(List<Type> implementingClasses, ServiceLifetime lifetime)
    {
        foreach (var lifeTimeClass in GetClassesWithLifeTime(implementingClasses, lifetime))
        {
            var interfaceTypes = lifeTimeClass.GetInterfaces();

            if (interfaceTypes.Length != 0)
                foreach (var interfaceType in interfaceTypes)
                    AddAutoService(interfaceType, lifeTimeClass, lifetime);
            else
                AddAutoService(null, lifeTimeClass, lifetime);
        }
    }

    private void AddAutoService(Type? interfaceType, Type implementingClass, ServiceLifetime lifetime)
    {
        if (ShouldBeExcluded(interfaceType, implementingClass))
            return;

        var serviceToAdd = interfaceType is null ?
            new ServiceDescriptor(implementingClass, implementingClass, lifetime) :
            new ServiceDescriptor(interfaceType, implementingClass, lifetime);

        if (HasAutoAttributeAndTryAdd(implementingClass, lifetime))
            _services.TryAdd(serviceToAdd);
        else
            _services.Add(serviceToAdd);
    }

    private IEnumerable<Assembly> GetAssemblies()
    {
        var assemblies = new Dictionary<string, Assembly>();

        var assembliesToScan = (_options.TypesToScan is null || _options.TypesToScan.Any() == false) ?
            AppDomain.CurrentDomain.GetAssemblies() :
            _options.TypesToScan.Select(t => t.GetTypeInfo().Assembly);

        foreach (var assembly in assembliesToScan)
            assemblies.TryAdd(assembly.FullName!, assembly);

        // Only continue if no types were add as a parameter
        if (_options.TypesToScan?.Any() == true)
            return assemblies.Values.AsEnumerable();

        try
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (path is null || string.IsNullOrEmpty(path))
                return assemblies.Values.AsEnumerable();

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
            {
                var assembly = Assembly.LoadFile(dll);

                if (assembly is not null && assembly.FullName is not null)
                    assemblies.TryAdd(assembly.FullName!, assembly);
            }
        }
        catch (Exception)
        {
            // No need to do throw anything, we need to exit safely if we are trying to access something we can't
        }

        return assemblies.Values.AsEnumerable();
    }

    private static IEnumerable<Type> GetClassesWithLifeTime(List<Type> implementingClasses, ServiceLifetime lifetime)
    {
        var length = implementingClasses.Count - 1;

        for (var i = 0; i <= length; i++)
        {
            if (HasAutoAttributeAndLifeTime(implementingClasses[i], lifetime))
            {
                yield return implementingClasses[i];

                // Remove service that is now added
                implementingClasses.RemoveAt(i);
                i--;
            }

            length = implementingClasses.Count - 1;
        }
    }

    private static AutoInjectAttribute[] GetAutoAttributes(Type t) =>
        Attribute.GetCustomAttributes(t, typeof(AutoInjectAttribute)) as AutoInjectAttribute[] ?? [];

    private static bool HasAutoAttributes(Type t)
    {
        var attributes = GetAutoAttributes(t);
        return attributes.Length > 0;
    }

    private static bool HasAutoAttributeAndLifeTime(Type t, ServiceLifetime lifetime)
    {
        var attributes = GetAutoAttributes(t);
        var attribute = attributes.FirstOrDefault(a => a.Lifetime == lifetime);

        return attribute is not null;
    }

    private static bool HasAutoAttributeAndTryAdd(Type t, ServiceLifetime lifetime)
    {
        var attributes = GetAutoAttributes(t);
        var attribute = attributes.FirstOrDefault(a => a.Lifetime == lifetime && a.AddType is AddType.TryAdd);

        return attribute is not null;
    }

    private bool ShouldBeExcluded(Type? interfaceType, Type implementingClass) =>
        IsTypeToScanOnlyAndNotInScanList(interfaceType, implementingClass) || IsInExclusion(interfaceType, implementingClass);

    private bool IsTypeToScanOnlyAndNotInScanList(Type? interfaceType, Type implementingClass) =>
        _options.InclusionType is InclusionType.TypesToScanOnly &&
        !((interfaceType is not null && _options.TypesToScan?.Contains(interfaceType) == true) || _options.TypesToScan?.Contains(implementingClass) == true);

    private bool IsInExclusion(Type? interfaceType, Type implementingClass) =>
        (interfaceType is not null && _options.TypesToExclude?.Contains(interfaceType) == true) || _options.TypesToExclude?.Contains(implementingClass) == true;
}