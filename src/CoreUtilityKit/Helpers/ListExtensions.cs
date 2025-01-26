using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CoreUtilityKit.Helpers;

/// <summary>
/// Useful collection extensions
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Determines whether the <see cref="IEnumerable{TSource}"/> is <see langword="null"/> or contains any element.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check for null and emptiness.</param>
    /// <returns><see langword="true"/> if the source sequence contains any elements; otherwise, <see langword="false"/>.</returns>
    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource>? source) =>
        source is null || !source.Any();

    /// <summary>
    /// Determines whether the <see cref="List{TSource}"/> is <see langword="null"/> or contains any element.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The <see cref="List{TSource}"/> to check for null and emptiness.</param>
    /// <returns><see langword="true"/> if the source sequence contains any elements; otherwise, <see langword="false"/>.</returns>
    public static bool IsNullOrEmpty<TSource>(this List<TSource>? source) =>
        source is null || source.Count == 0;

    /// <summary>
    /// Determines whether the <see cref="Dictionary{TKey, TValue}"/> is <see langword="null"/> or contains any element.
    /// </summary>
    /// <typeparam name="TKey">The type to assign to the key type parameter of the returned generic <see cref="Dictionary{TKey, TValue}"/>.</typeparam>
    /// <typeparam name="TValue">The type to assign to the value type parameter of the returned generic <see cref="Dictionary{TKey, TValue}"/>.</typeparam>
    /// <param name="source">The <see cref="Dictionary{TKey, TValue}"/> to check for null and emptiness.</param>
    /// <returns><see langword="true"/> if the source sequence contains any elements; otherwise, <see langword="false"/>.</returns>
    public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue>? source) where TKey : notnull =>
        source is null || source.Count == 0;

    /// <summary>
    /// Returns <see cref="IEnumerable{TSource}"/> self or an empty <see cref="IEnumerable{TSource}"/> if the <paramref name="source"/> is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TSource">The type to assign to the type parameter of the returned generic <see cref="IEnumerable{TResult}"/>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check for null.</param>
    /// <returns><see cref="IEnumerable{TSource}"/> self or an empty <see cref="IEnumerable{TSource}"/></returns>
    public static IEnumerable<TSource> ToEmptyIfNull<TSource>(this IEnumerable<TSource>? source) =>
        source ?? [];

    /// <summary>
    /// Returns <see cref="List{TSource}"/> self or an empty <see cref="List{TSource}"/> if the <paramref name="source"/> is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TSource">The type to assign to the type parameter of the returned generic <see cref="List{TResult}"/>.</typeparam>
    /// <param name="source">The <see cref="List{TSource}"/> to check for null.</param>
    /// <returns><see cref="List{TSource}"/> self or an empty <see cref="List{TSource}"/></returns>
#pragma warning disable CA1002
    public static List<TSource> ToEmptyIfNull<TSource>(this List<TSource>? source) =>
#pragma warning restore CA1002
        source ?? [];

    /// <summary>
    /// Returns <see cref="Dictionary{TKey,TValue}"/> self or an empty <see cref="Dictionary{TKey,TValue}"/> if the <paramref name="source"/> is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TKey">The type to assign to the key type parameter of the returned generic <see cref="Dictionary{TKey, TValue}"/>.</typeparam>
    /// <typeparam name="TValue">The type to assign to the value type parameter of the returned generic <see cref="Dictionary{TKey, TValue}"/>.</typeparam>
    /// <param name="source">The <see cref="Dictionary{TKey,TValue}"/> to check for null.</param>
    /// <returns><see cref="Dictionary{TKey,TValue}"/> self or an empty <see cref="Dictionary{TKey,TValue}"/></returns>
    public static Dictionary<TKey, TValue> ToEmptyIfNull<TKey, TValue>(this Dictionary<TKey, TValue>? source) where TKey : notnull =>
        source ?? [];

    /// <summary>Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/> if the key does not already exist.</summary>
    /// <param name="dict">The dictionary from which to read a value or add to it.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value to be added, if the key does not already exist.</param>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dict"/> is a null reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference.</exception>
    public static TValue? GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue? value) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dict);
        ArgumentNullException.ThrowIfNull(key);

        ref TValue? val = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out bool exists);
        if (exists)
        {
            return val;
        }

        val = value;
        return value;
    }

    /// <summary>Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/> if the key does not already exist.</summary>
    /// <param name="dict">The dictionary from which to read a value or add to it.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="valueFactory">The function used to generate a value for the key.</param>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dict"/> is a null reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a null reference.</exception>
    public static TValue? GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFactory) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        return GetOrAdd(dict, key, valueFactory(key));
    }

    /// <summary>Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/> if the key does not already exist.</summary>
    /// <param name="dict">The dictionary from which to read a value or add to it.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="valueFactory">The function used to generate a value for the key.</param>
    /// <param name="factoryArgument">An argument value to pass into <paramref name="valueFactory"/>.</param>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <typeparam name="TArg">The type of argument to pass into valueFactory.</typeparam>
    /// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dict"/> is a null reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a null reference.</exception>
    public static TValue? GetOrAdd<TKey, TValue, TArg>(this Dictionary<TKey, TValue> dict, TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        return GetOrAdd(dict, key, valueFactory(key, factoryArgument));
    }

    /// <summary>Updates the value associated with <paramref name="key"/> to <paramref name="newValue"/> in the <see cref="Dictionary{TKey,TValue}"/>.</summary>
    /// <param name="dict">The dictionary in which to update the value by the given key.</param>
    /// <param name="key">The key whose value will possibly be replaced.</param>
    /// <param name="newValue">The value that replaces the value of the element with <paramref name="key"/> if the comparison results in equality.</param>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <returns><see langword="true"/> if the value with <paramref name="key"/> replaced with <paramref name="newValue"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dict"/> is a null reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference.</exception>
    public static bool TryUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue newValue) where TKey : notnull
    {
        ref TValue val = ref CollectionsMarshal.GetValueRefOrNullRef(dict, key);
        if (Unsafe.IsNullRef(ref val))
        {
            return false;
        }

        val = newValue;
        return true;
    }

    /// <summary>Updates the value associated with <paramref name="key"/> to a newly generated value by <paramref name="valueFactory"/> in the <see cref="Dictionary{TKey,TValue}"/>.</summary>
    /// <param name="dict">The dictionary in which to update the value by the given key.</param>
    /// <param name="key">The key whose value will possibly be replaced.</param>
    /// <param name="valueFactory">The function used to generate a value for the key.</param>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <returns><see langword="true"/> if the value with <paramref name="key"/> replaced with a new value generated by <paramref name="valueFactory"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dict"/> is a null reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a null reference.</exception>
    public static bool TryUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFactory) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        return TryUpdate(dict, key, valueFactory(key));
    }

    /// <summary>Updates the value associated with <paramref name="key"/> to a newly generated value by <paramref name="valueFactory"/> in the <see cref="Dictionary{TKey,TValue}"/>.</summary>
    /// <param name="dict">The dictionary in which to update the value by the given key.</param>
    /// <param name="key">The key whose value will possibly be replaced.</param>
    /// <param name="valueFactory">The function used to generate a value for the key.</param>
    /// <param name="factoryArgument">An argument value to pass into <paramref name="valueFactory"/>.</param>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <typeparam name="TArg">The type of argument to pass into valueFactory.</typeparam>
    /// <returns><see langword="true"/> if the value with <paramref name="key"/> replaced with a new value generated by <paramref name="valueFactory"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dict"/> is a null reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a null reference.</exception>
    public static bool TryUpdate<TKey, TValue, TArg>(this Dictionary<TKey, TValue> dict, TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        return TryUpdate(dict, key, valueFactory(key, factoryArgument));
    }
}
