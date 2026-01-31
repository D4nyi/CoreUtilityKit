using CoreUtilityKit.EnumAttributionCache.Abstraction;

using Microsoft.Extensions.DependencyInjection;

namespace CoreUtilityKit.EnumAttributionCache;

/// <summary>
/// Extensions for adding enum caches to the <see cref="IServiceCollection"/>.
/// </summary>
public static class EnumAttributeCacheExtensions
{
    /// <summary>
    /// Adds a <see cref="IReadonlyEnumAttributeCache"/> as a singleton to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="attributeValue"><see cref="EnumAttributeValue"/> value describing which attribute type should be read from the enum values.</param>
    /// <param name="enumTypes">An array of enum types from which the cache will be built.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddReadonlyEnumAttributeCache(this IServiceCollection services, EnumAttributeValue attributeValue, Type[] enumTypes)
    {
        Dictionary<Enum, string> dict = EnumAttributeReaderFactory.GenerateDictionary(attributeValue, enumTypes);

        ReadonlyEnumAttributeCache cache = new(dict);

        return services.AddSingleton<IReadonlyEnumAttributeCache>(cache);
    }

    /// <summary>
    /// Adds an empty <see cref="IEnumAttributeCache"/> as a singleton to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="attributeValue"><see cref="EnumAttributeValue"/> value describing which attribute type should be read from the enum values.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddEnumAttributeCache(this IServiceCollection services, EnumAttributeValue attributeValue)
    {
        Func<Enum, string?> singleReader = EnumAttributeReaderFactory.GetSingleReader(attributeValue);

        EnumAttributeCache cache = new(singleReader);

        return services.AddSingleton<IEnumAttributeCache>(cache);
    }

    /// <summary>
    /// Adds a preinstalled <see cref="IEnumAttributeCache"/> as a singleton to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="attributeValue"><see cref="EnumAttributeValue"/> value describing which attribute type should be read from the enum values.</param>
    /// <param name="enumTypes">An array of enum types from which the cache will be installed.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddEnumAttributeCache(this IServiceCollection services, EnumAttributeValue attributeValue, Type[] enumTypes)
    {
        Dictionary<Enum, string> dict = EnumAttributeReaderFactory.GenerateDictionary(attributeValue, enumTypes);

        Func<Enum, string?> singleReader = EnumAttributeReaderFactory.GetSingleReader(attributeValue);

        EnumAttributeCache cache = new(dict, singleReader);

        return services.AddSingleton<IEnumAttributeCache>(cache);
    }
}
