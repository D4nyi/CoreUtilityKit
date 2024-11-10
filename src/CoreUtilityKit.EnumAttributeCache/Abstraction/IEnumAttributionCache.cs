namespace CoreUtilityKit.EnumAttributionCache.Abstraction;

/// <summary>
/// Mutable enum description cache.
/// </summary>
public interface IEnumAttributeCache : IReadonlyEnumAttributeCache
{
    /// <summary>Adds a new key/value pair to the cache if the key does not already exist.</summary>
    /// <param name="key">The key of the element to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="OverflowException">The cache contains too many elements.</exception>
    /// <returns>
    /// The value for the key.
    /// This will be either the existing value for the key if the
    /// key is already in the cache, or the newly added value for the key.
    /// </returns>
    string? GetOrAdd(Enum key);

    /// <summary>Attempts to add the specified key and value to the cache.</summary>
    /// <param name="key">The key of the element to add.</param>
    /// <returns><see langword="true"/> if the key/value pair was added to the cache successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="OverflowException">The cache contains too many elements.</exception>
    bool TryAdd(Enum key);
}