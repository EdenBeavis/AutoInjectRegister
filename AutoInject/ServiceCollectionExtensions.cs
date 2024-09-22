using Microsoft.Extensions.DependencyInjection;

namespace AutoInject
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Auto register services that have the autoinject attribute attached to them.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AutoInjectRegisterServices(this IServiceCollection services)
        {
            new AutoInjector(services).Register();
            return services;
        }

        /// <summary>
        /// Auto register services in the assemblies specified from parameter.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typesToScan">A classes in the library to scan</param>
        /// <returns></returns>
        public static IServiceCollection AutoInjectRegisterServices(this IServiceCollection services, params Type[] typesToScan)
        {
            new AutoInjector(services, typesToScan).Register();
            return services;
        }
    }
}