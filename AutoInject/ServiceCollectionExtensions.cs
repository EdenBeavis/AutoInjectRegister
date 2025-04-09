using Microsoft.Extensions.DependencyInjection;

namespace AutoInject;

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
        return services;
    }

    /// <summary>
    /// Auto register services in the assemblies specified via options using an action to invoke the options.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionOptions">Options action to create AutoInjectorOptions</param>
    /// <returns></returns>
    public static IServiceCollection AutoInjectRegisterServices(this IServiceCollection services, Action<AutoInjectorOptions> actionOptions)
    {
        var options = new AutoInjectorOptions();
        actionOptions.Invoke(options);

        return AutoInjectRegisterServices(services, options);
    }
}