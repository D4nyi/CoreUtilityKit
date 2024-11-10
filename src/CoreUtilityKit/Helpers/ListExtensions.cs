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
}
