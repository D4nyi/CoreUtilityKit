using System.Collections.Frozen;

using CoreUtilityKit.EnumAttributionCache.Abstraction;

namespace CoreUtilityKit.EnumAttributionCache;

internal sealed class ReadonlyEnumAttributeCache : IReadonlyEnumAttributeCache
{
    private readonly FrozenDictionary<Enum, string> _dict;

    public int Count => _dict.Count;

    internal ReadonlyEnumAttributeCache(Dictionary<Enum, string> dict)
    {
        ArgumentNullException.ThrowIfNull(dict);
        if (dict.Count == 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(dict),
                dict.Count,
                "Readonly cache must not be empty!");
        }

        _dict = dict
            .ToFrozenDictionary(
            static kvp => kvp.Key,
            static kvp => kvp.Value
            );
    }

    public bool ContainsKey(Enum key) => _dict.ContainsKey(key);

    public bool TryGetValue(Enum key, out string? description) => _dict.TryGetValue(key, out description);
}
