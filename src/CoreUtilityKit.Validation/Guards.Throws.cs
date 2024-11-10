using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CoreUtilityKit.Validation;

public static partial class Guards
{
    /// <summary>
    /// Tests the given number with the provided predicate.
    /// </summary>
    /// <typeparam name="T">Number type parameter</typeparam>
    /// <param name="value">Number to be validated</param>
    /// <param name="predicate">Provides the validation logic</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="predicate"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the <paramref name="predicate"/> returns <see langword="true"/>.</exception>
    public static void ThrowNumberOutOfRangeIf<T>([NotNull] this T value, Func<T, bool> predicate, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(predicate);
        if (predicate(value))
            ThrowHelpers.ArgumentOutOfRange(paramName, value);
    }

    /// <summary>
    /// Tests the given collection with the provided predicate.
    /// </summary>
    /// <typeparam name="T">Collection type parameter</typeparam>
    /// <param name="value">Collection to be validated</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the <paramref name="value"/> is empty.</exception>
    public static void ThrowIfEmpty<T>([NotNullWhen(true)] this T? value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : ICollection
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        if (value.Count == 0)
            ThrowHelpers.ArgumentOutOfRangeCollection(paramName, value.Count);
    }

    /// <summary>
    /// Tests the given collection with the provided predicate.
    /// </summary>
    /// <typeparam name="T">Collection type parameter</typeparam>
    /// <param name="value">Collection to be validated</param>
    /// <param name="count">Value that the collection count will be matched against</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the <paramref name="value"/> is empty.</exception>
    public static void ThrowIfCountIsGreaterThen<T>([NotNullWhen(true)] this T? value, int count, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : ICollection
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        if (value.Count > count)
            ThrowHelpers.ArgumentOutOfRangeCollection(paramName, value.Count);
    }

    /// <summary>
    /// Tests the given collection with the provided predicate.
    /// </summary>
    /// <typeparam name="T">Collection type parameter</typeparam>
    /// <param name="value">Collection to be validated</param>
    /// <param name="count">Value that the collection count will be matched against</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the <paramref name="value"/> is empty.</exception>
    public static void ThrowIfCountEqualTo<T>([NotNullWhen(true)] this T? value, int count, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : ICollection
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        if (value.Count == count)
            ThrowHelpers.ArgumentOutOfRangeCollection(paramName, value.Count);
    }

    /// <summary>
    /// Tests the given collection with the provided predicate.
    /// </summary>
    /// <typeparam name="T">Collection type parameter</typeparam>
    /// <param name="value">Collection to be validated</param>
    /// <param name="count">Value that the collection count will be matched against</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the <paramref name="value"/> is empty.</exception>
    public static void ThrowIfCountNotEqualTo<T>([NotNullWhen(true)] this T? value, int count, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : ICollection
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        if (value.Count != count)
            ThrowHelpers.ArgumentOutOfRangeCollection(paramName, value.Count);
    }

    /// <summary>
    /// Tests the given collection with the provided predicate.
    /// </summary>
    /// <typeparam name="T">Collection type parameter</typeparam>
    /// <param name="value">Collection to be validated</param>
    /// <param name="count">Value that the collection count will be matched against</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the <paramref name="value"/> is empty.</exception>
    public static void ThrowIfCountIsGreaterThenOrEqualTo<T>([NotNullWhen(true)] this T? value, int count, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : ICollection
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        if (value.Count >= count)
            ThrowHelpers.ArgumentOutOfRangeCollection(paramName, value.Count);
    }

    /// <summary>
    /// Tests the given collection with the provided predicate.
    /// </summary>
    /// <typeparam name="T">Collection type parameter</typeparam>
    /// <param name="value">Collection to be validated</param>
    /// <param name="count">Value that the collection count will be matched against</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the <paramref name="value"/> is empty.</exception>
    public static void ThrowIfCountIsLessThen<T>([NotNullWhen(true)] this T? value, int count, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : ICollection
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        if (value.Count < count)
            ThrowHelpers.ArgumentOutOfRangeCollection(paramName, value.Count);
    }

    /// <summary>
    /// Tests the given collection with the provided predicate.
    /// </summary>
    /// <typeparam name="T">Collection type parameter</typeparam>
    /// <param name="value">Collection to be validated</param>
    /// <param name="count">Value that the collection count will be matched against</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the <paramref name="value"/> is empty.</exception>
    public static void ThrowIfCountIsLessThenOrEqualTo<T>([NotNullWhen(true)] this T? value, int count, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : ICollection
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        if (value.Count <= count)
            ThrowHelpers.ArgumentOutOfRangeCollection(paramName, value.Count);
    }
}
