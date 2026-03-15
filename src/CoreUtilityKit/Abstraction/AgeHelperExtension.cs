using CoreUtilityKit.Helpers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreUtilityKit.Abstraction;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> to register <see cref="IAgeHelper"/>.
/// </summary>
public static class AgeHelperExtension
{
    /// <summary>
    /// Registers <see cref="IAgeHelper"/> with <see cref="AgeHelper"/> implementation and <see cref="TimeProvider.System"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddAgeHelperWithTimeProvider(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);

        return services.AddSingleton<IAgeHelper, AgeHelper>();
    }

    /// <summary>
    /// Registers <see cref="IAgeHelper"/> with <see cref="AgeHelper"/> implementation using <see cref="TimeProvider.System"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddAgeHelper(this IServiceCollection services)
    {
        return services.AddSingleton<IAgeHelper>(new AgeHelper(TimeProvider.System));
    }
}
