using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AutoInject
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AutoInjectRegisterServices(this IServiceCollection services)
        {
            services.Register(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }

        public static IServiceCollection AutoInjectRegisterServices(this IServiceCollection services, params Type[] typesToScan)
        {
            var assembliesToScan = (typesToScan == null || typesToScan.Length == 0) ?
                AppDomain.CurrentDomain.GetAssemblies() :
                typesToScan.Select(t => t.GetTypeInfo().Assembly);

            services.Register(assembliesToScan);
            return services;
        }

        private static IServiceCollection Register(this IServiceCollection services, IEnumerable<Assembly> assembliesToScan)
        {
            var implementingClasses = assembliesToScan
                .SelectMany(s => s.GetTypes())
                .Where(HasAutoAttribute);

            var interfaceTypes = implementingClasses
                .SelectMany(t => t.GetInterfaces())
                .Distinct();

            foreach (Lifetime lifetime in (Lifetime[])Enum.GetValues(typeof(Lifetime)))
                services.AddAllServicesOfLifeTime(implementingClasses, lifetime);

            return services;
        }

        private static IServiceCollection AddAllServicesOfLifeTime(this IServiceCollection services, IEnumerable<Type> implementingClasses, Lifetime lifetime)
        {
            var lifeTimeClasses = GetClassesWithLifeTime(implementingClasses, lifetime);

            foreach (var lifeTimeClass in lifeTimeClasses)
            {
                if (lifeTimeClass.GetInterfaces().Length == 0)
                    services.AddAutoService(lifeTimeClass, lifetime);
                else
                    foreach (var interfaceType in lifeTimeClass.GetInterfaces())
                        services.AddAutoService(lifeTimeClass, lifetime, interfaceType);
            }
            return services;
        }

        private static IServiceCollection AddAutoService(this IServiceCollection services, Type implementingClass, Lifetime lifetime, Type? interfaceType = null)
        {
            _ = lifetime switch
            {
                Lifetime.Transient => interfaceType is null ? services.AddTransient(implementingClass) : services.AddTransient(interfaceType, implementingClass),
                Lifetime.Scoped => interfaceType is null ? services.AddScoped(implementingClass) : services.AddScoped(interfaceType, implementingClass),
                Lifetime.Singleton => interfaceType is null ? services.AddSingleton(implementingClass) : services.AddSingleton(interfaceType, implementingClass),
                _ => throw new ArgumentOutOfRangeException("Lifetime must be within lifetime enum range.")
            };

            return services;
        }

        private static IEnumerable<Type> GetClassesWithLifeTime(IEnumerable<Type> implementingClasses, Lifetime lifetime) => implementingClasses
            .Where(t => HasAutoAttributeAndLifeTime(t, lifetime));

        private static AutoInjectAttribute? GetAutoAttribute(Type t) => Attribute.GetCustomAttribute(t, typeof(AutoInjectAttribute)) as AutoInjectAttribute;

        private static bool HasAutoAttribute(Type t) => GetAutoAttribute(t) != null;

        private static bool HasAutoAttributeAndLifeTime(Type t, Lifetime lifetime) => HasAutoAttribute(t) && GetAutoAttribute(t)?.Lifetime == lifetime;
    }
}