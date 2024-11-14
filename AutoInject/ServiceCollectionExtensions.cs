using AutoInject.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace AutoInject
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ConcurrentDictionary<string, Assembly> assemblies = [];

        public static IServiceCollection AutoInjectRegisterServices(this IServiceCollection services)
        {
            UpdateAssemblies();
            services.Register();
            return services;
        }

        public static IServiceCollection AutoInjectRegisterServices(this IServiceCollection services, params Type[] typesToScan)
        {
<<<<<<< Updated upstream
            UpdateAssemblies(typesToScan);
            services.Register();
=======
            new AutoInjector(services, new AutoInjectorOptions { TypesToScan = typesToScan }).Register();
            return services;
        }

        /// <summary>
        /// Auto register services in the assemblies specified via options and exclude certain types via options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">Options class containing TypesToScan and TypesToExclude</param>
        /// <returns></returns>
        public static IServiceCollection AutoInjectRegisterServices(this IServiceCollection services, AutoInjectorOptions options)
        {
            new AutoInjector(services, options).Register();
>>>>>>> Stashed changes
            return services;
        }

        private static IServiceCollection Register(this IServiceCollection services)
        {
            var implementingClasses = assemblies.Values
                .SelectMany(s => s.GetTypes())
                .Where(HasAutoAttributes);

            var interfaceTypes = implementingClasses
                .SelectMany(t => t.GetInterfaces())
                .Distinct();

            foreach (ServiceLifetime lifetime in (ServiceLifetime[])Enum.GetValues(typeof(ServiceLifetime)))
            {
                var classesOfLifeTime = GetClassesWithLifeTime(implementingClasses, lifetime);
                services.AddAllServicesOfLifeTime(classesOfLifeTime, lifetime);

                implementingClasses = implementingClasses.Except(classesOfLifeTime);
            }

            assemblies.Clear();

            return services;
        }

        private static IServiceCollection AddAllServicesOfLifeTime(this IServiceCollection services, IEnumerable<Type> lifeTimeClasses, ServiceLifetime lifetime)
        {
            foreach (var lifeTimeClass in lifeTimeClasses)
            {
                if (lifeTimeClass.GetInterfaces().Length == 0)
                    services.AddAutoService(null, lifeTimeClass, lifetime);
                else
                    foreach (var interfaceType in lifeTimeClass.GetInterfaces())
                        services.AddAutoService(interfaceType, lifeTimeClass, lifetime);
            }

            return services;
        }

        private static IServiceCollection AddAutoService(this IServiceCollection services, Type? interfaceType, Type implementingClass, ServiceLifetime lifetime)
        {
            var serviceToAdd = interfaceType is null ?
                new ServiceDescriptor(implementingClass, implementingClass, lifetime) :
                new ServiceDescriptor(interfaceType, implementingClass, lifetime);

            services.Add(serviceToAdd);

            return services;
        }

        private static IEnumerable<Type> GetClassesWithLifeTime(IEnumerable<Type> implementingClasses, ServiceLifetime lifetime) =>
            implementingClasses.Where(t => HasAutoAttributeAndLifeTime(t, lifetime));

        private static AutoInjectAttribute[]? GetAutoAttributes(Type t) =>
            Attribute.GetCustomAttributes(t, typeof(AutoInjectAttribute)) as AutoInjectAttribute[];

        private static bool HasAutoAttributes(Type t)
        {
            var attributes = GetAutoAttributes(t);
            return attributes != null && attributes.Length > 0;
        }

        private static bool HasAutoAttributeAndLifeTime(Type t, ServiceLifetime lifetime) =>
            HasAutoAttributes(t) && GetAutoAttributes(t)?.FirstOrDefault(a => a.Lifetime == lifetime) != null;

        private static void UpdateAssemblies(Type[]? typesToScan = null)
        {
            var assembliesToScan = (typesToScan == null || typesToScan.Length == 0) ?
                AppDomain.CurrentDomain.GetAssemblies() :
                typesToScan.Select(t => t.GetTypeInfo().Assembly);

            foreach (var assembly in assembliesToScan)
                assemblies.TryAdd(assembly.FullName!, assembly);

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
                        assemblies.TryAdd(assembly.FullName!, assembly);
                }
            }
            catch (Exception)
            {
                // no need to do throw anything, we need to exit safely if we are trying to access something we can't
                return;
            }
        }
    }
}