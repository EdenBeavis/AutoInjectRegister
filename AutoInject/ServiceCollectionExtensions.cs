using Microsoft.Extensions.DependencyInjection;

namespace AutoInject
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AutoInjectRegisterServices(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var implementingClasses = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(HasAutoAttribute);

            var interfaceTypes = implementingClasses
                .SelectMany(t => t.GetInterfaces())
                .Distinct();

            services.AddAllTransient(interfaceTypes, implementingClasses);
            services.AddAllScoped(interfaceTypes, implementingClasses);
            services.AddAllSingletons(interfaceTypes, implementingClasses);

            return services;
        }

        private static IServiceCollection AddAllTransient(this IServiceCollection services, IEnumerable<Type> interfaceTypes, IEnumerable<Type> implementingClasses)
        {
            const Lifetime lifetime = Lifetime.Transient;
            services.AddAllClassServicesOfLifeTime(implementingClasses, lifetime);
            services.AddAllInterfaceServicesOfLifeTime(interfaceTypes, implementingClasses, lifetime);
            return services;
        }

        private static IServiceCollection AddAllScoped(this IServiceCollection services, IEnumerable<Type> interfaceTypes, IEnumerable<Type> implementingClasses)
        {
            const Lifetime lifetime = Lifetime.Scoped;
            services.AddAllClassServicesOfLifeTime(implementingClasses, lifetime);
            services.AddAllInterfaceServicesOfLifeTime(interfaceTypes, implementingClasses, lifetime);
            return services;
        }

        private static IServiceCollection AddAllSingletons(this IServiceCollection services, IEnumerable<Type> interfaceTypes, IEnumerable<Type> implementingClasses)
        {
            const Lifetime lifetime = Lifetime.Singleton;
            services.AddAllClassServicesOfLifeTime(implementingClasses, lifetime);
            services.AddAllInterfaceServicesOfLifeTime(interfaceTypes, implementingClasses, lifetime);
            return services;
        }

        private static IServiceCollection AddAllInterfaceServicesOfLifeTime(this IServiceCollection services, IEnumerable<Type> interfaceTypes, IEnumerable<Type> implementingClasses, Lifetime lifetime)
        {
            foreach (var interfaceType in interfaceTypes)
            {
                var lifeTimeClasses = GetClassesWithLifeTime(implementingClasses, interfaceType, lifetime);

                foreach (var lifeTimeClass in lifeTimeClasses)
                    services.AddAutoService(lifeTimeClass, lifetime, interfaceType);
            }

            return services;
        }

        private static IServiceCollection AddAllClassServicesOfLifeTime(this IServiceCollection services, IEnumerable<Type> implementingClasses, Lifetime lifetime)
        {
            var classOnlyServices = implementingClasses
               .Where(t => t.GetInterfaces().Any() == false)
               .Where(t => HasAutoAttributeAndLifeTime(t, lifetime));

            foreach (var classOnly in classOnlyServices)
                services.AddAutoService(classOnly, lifetime);

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

        private static IEnumerable<Type> GetClassesWithLifeTime(IEnumerable<Type> implementingClasses, Type interfaceType, Lifetime lifetime) => implementingClasses
            .Where(t => interfaceType.IsAssignableFrom(t) && HasAutoAttributeAndLifeTime(t, lifetime));

        private static AutoInjectAttribute? GetAutoAttribute(Type t) => Attribute.GetCustomAttribute(t, typeof(AutoInjectAttribute)) as AutoInjectAttribute;

        private static bool HasAutoAttribute(Type t) => GetAutoAttribute(t) != null;

        private static bool HasAutoAttributeAndLifeTime(Type t, Lifetime lifetime) => HasAutoAttribute(t) && GetAutoAttribute(t)?.Lifetime == lifetime;
    }
}