using CoreUtilityKit.Helpers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreUtilityKit.Abstraction;

public static class AgeHelperExtension
{
    public static IServiceCollection AddAgeHelperWithTimeProvider(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);

        return services.AddSingleton<IAgeHelper, AgeHelper>();
    }

    public static IServiceCollection AddAgeHelper(this IServiceCollection services)
    {
        return services.AddSingleton<IAgeHelper>(new AgeHelper(TimeProvider.System));
    }
}
