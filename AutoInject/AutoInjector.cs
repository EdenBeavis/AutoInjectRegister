using AutoInject.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Concurrent;
using System.Reflection;

namespace AutoInject
{
    internal class AutoInjector
    {
        private readonly IServiceCollection _services;
        private readonly ConcurrentDictionary<string, Assembly> _assemblies = [];

        public AutoInjector(IServiceCollection services, Type[]? typesToScan = null)
        {
            _services = services;
            UpdateAssemblies(typesToScan);
        }

        internal void Register()
        {
            var implementingClasses = _assemblies.Values
                .SelectMany(s => s.GetTypes())
                .Where(HasAutoAttributes);

            var interfaceTypes = implementingClasses
                .SelectMany(t => t.GetInterfaces())
                .Distinct();

            foreach (ServiceLifetime lifetime in (ServiceLifetime[])Enum.GetValues(typeof(ServiceLifetime)))
            {
                var classesOfLifeTime = GetClassesWithLifeTime(implementingClasses, lifetime);
                AddAllServicesOfLifeTime(classesOfLifeTime, lifetime);

                implementingClasses = implementingClasses.Except(classesOfLifeTime);
            }
        }

        private void AddAllServicesOfLifeTime(IEnumerable<Type> lifeTimeClasses, ServiceLifetime lifetime)
        {
            foreach (var lifeTimeClass in lifeTimeClasses)
            {
                if (lifeTimeClass.GetInterfaces().Length == 0)
                    AddAutoService(null, lifeTimeClass, lifetime);
                else
                    foreach (var interfaceType in lifeTimeClass.GetInterfaces())
                        AddAutoService(interfaceType, lifeTimeClass, lifetime);
            }
        }

        private void AddAutoService(Type? interfaceType, Type implementingClass, ServiceLifetime lifetime)
        {
            var serviceToAdd = interfaceType is null ?
                new ServiceDescriptor(implementingClass, implementingClass, lifetime) :
                new ServiceDescriptor(interfaceType, implementingClass, lifetime);

            if (HasAutoAttributeAndTryAdd(implementingClass, lifetime))
                _services.TryAdd(serviceToAdd);
            else
                _services.Add(serviceToAdd);
        }

        private void UpdateAssemblies(Type[]? typesToScan = null)
        {
            var assembliesToScan = (typesToScan == null || typesToScan.Length == 0) ?
                AppDomain.CurrentDomain.GetAssemblies() :
                typesToScan.Select(t => t.GetTypeInfo().Assembly);

            foreach (var assembly in assembliesToScan)
                _assemblies.TryAdd(assembly.FullName!, assembly);

            // Only continue if no types were add as a parameter
            if (typesToScan?.Length > 0) return;

            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (path is null) return;

                foreach (string dll in Directory.GetFiles(path, "*.dll"))
                {
                    var assembly = Assembly.LoadFile(dll);

                    if (assembly is not null && assembly.FullName is not null)
                        _assemblies.TryAdd(assembly.FullName!, assembly);
                }
            }
            catch (Exception)
            {
                // no need to do throw anything, we need to exit safely if we are trying to access something we can't
                return;
            }
        }

        private IEnumerable<Type> GetClassesWithLifeTime(IEnumerable<Type> implementingClasses, ServiceLifetime lifetime) =>
            implementingClasses.Where(t => HasAutoAttributeAndLifeTime(t, lifetime));

        private static AutoInjectAttribute[]? GetAutoAttributes(Type t) =>
            Attribute.GetCustomAttributes(t, typeof(AutoInjectAttribute)) as AutoInjectAttribute[];

        private static bool HasAutoAttributes(Type t)
        {
            var attributes = GetAutoAttributes(t);
            return attributes != null && attributes.Length > 0;
        }

        private static bool HasAutoAttributeAndLifeTime(Type t, ServiceLifetime lifetime)
        {
            var attributes = GetAutoAttributes(t);
            var attribute = attributes?.FirstOrDefault(a => a.Lifetime == lifetime);

            return attribute != null;
        }

        private static bool HasAutoAttributeAndTryAdd(Type t, ServiceLifetime lifetime)
        {
            var attributes = GetAutoAttributes(t);
            var attribute = attributes?.FirstOrDefault(a => a.Lifetime == lifetime && a.AddType == AddType.TryAdd);

            return attribute != null;
        }
    }
}