using System.Collections.Concurrent;

using CoreUtilityKit.EnumAttributionCache.Abstraction;

namespace CoreUtilityKit.EnumAttributionCache;

internal sealed class EnumAttributeCache : IEnumAttributeCache
{
    private readonly Func<Enum, string?> _singleReader;
    private readonly ConcurrentDictionary<Enum, string?> _dict;

    public int Count => _dict.Count;

    internal EnumAttributeCache(Dictionary<Enum, string> dict, Func<Enum, string?> singleReader)
    {
        _singleReader = singleReader;
        _dict = new(dict!);
    }

    internal EnumAttributeCache(Func<Enum, string?> singleReader)
    {
        _singleReader = singleReader;
        _dict = new();
    }

    public bool ContainsKey(Enum key) => _dict.ContainsKey(key);

    public bool TryGetValue(Enum key, out string? description) => _dict.TryGetValue(key, out description);

    public bool TryAdd(Enum key) => _dict.TryAdd(key, _singleReader(key));

    public string? GetOrAdd(Enum key) => _dict.GetOrAdd(key, _singleReader);
}
