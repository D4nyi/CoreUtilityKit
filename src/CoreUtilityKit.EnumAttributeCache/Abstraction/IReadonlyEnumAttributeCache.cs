namespace CoreUtilityKit.EnumAttributionCache.Abstraction;

/// <summary>
/// Immutable enum attribution cache.
/// </summary>
public interface IReadonlyEnumAttributeCache
{
    int Count { get; }

    /// <summary>Determines whether the cache contains the specified key.</summary>
    /// <param name="key">The key to locate in the cache.</param>
    /// <returns><see langword="true"/> if the cache contains an element with the specified key; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a <see langword="null"/> reference.</exception>
    bool ContainsKey(Enum key);

    /// <summary>Attempts to get the value associated with the specified key from the cache.</summary>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="value">
    /// When this method returns, <paramref name="value"/> will be populated from the cache
    /// if the specified key exists or <see langword="null"/>, if the operation failed.
    /// </param>
    /// <returns><see langword="true"/> if the key was found in the cache; otherwise, <see langword="false"/>.</returns>
    bool TryGetValue(Enum key, out string? value);
}