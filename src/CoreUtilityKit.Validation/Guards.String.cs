using System.Diagnostics.CodeAnalysis;

namespace CoreUtilityKit.Validation;

/// <summary>
/// Provides utility methods for validating various conditions and constraints.
/// </summary>
/// <remarks>
/// This class is static and provides a collection of validation guards for strings, passwords,
/// emails, and other general validation scenarios. The methods in this class are designed
/// to return boolean values indicating whether the specified conditions satisfy the given
/// constraints.
/// </remarks>
public static partial class Guards
{
    /// <summary>
    /// Validates that a string has at least the specified minimum length.
    /// </summary>
    /// <param name="value">The string to validate. Can be null.</param>
    /// <param name="minLength">The minimum length required (must be non-negative).</param>
    /// <returns>
    /// <c>true</c> if <paramref name="value"/> is not null and has at least
    /// <paramref name="minLength"/> characters; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="minLength"/> is negative.
    /// </exception>
    public static bool MinLength([NotNullWhen(true)] string? value, int minLength)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(minLength);

        return value is not null && value.Length >= minLength;
    }

    /// <summary>
    /// Validates that a string does not exceed the specified maximum length.
    /// </summary>
    /// <param name="value">The string to validate. Can be null.</param>
    /// <param name="maxLength">The maximum length allowed (must be non-negative).</param>
    /// <returns>
    /// <c>true</c> if <paramref name="value"/> is not null and has at most
    /// <paramref name="maxLength"/> characters; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="maxLength"/> is negative.
    /// </exception>
    public static bool MaxLength([NotNullWhen(true)] string? value, int maxLength)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxLength);

        return value is not null && value.Length <= maxLength;
    }

    /// <summary>
    /// Validates that a string's length falls within the specified minimum and maximum bounds, inclusive.
    /// </summary>
    /// <param name="value">The string to validate. Can be null.</param>
    /// <param name="minLength">The minimum length allowed (must be non-negative).</param>
    /// <param name="maxLength">The maximum length allowed (must be greater than or equal to <paramref name="minLength"/>).</param>
    /// <returns>
    /// <c>true</c> if <paramref name="value"/> is not null and its length is between
    /// <paramref name="minLength"/> and <paramref name="maxLength"/>, inclusive; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="minLength"/> is negative or
    /// <paramref name="maxLength"/> is less than <paramref name="minLength"/>.
    /// </exception>
    public static bool Between([NotNullWhen(true)] string? value, int minLength, int maxLength)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(minLength);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxLength, minLength);

        return value is not null && value.Length >= minLength && value.Length <= maxLength;
    }

    /// <summary>
    /// Validates that a string has exactly the specified length.
    /// </summary>
    /// <param name="value">The string to validate. Can be null.</param>
    /// <param name="length">The exact length required (must be non-negative).</param>
    /// <returns>
    /// <c>true</c> if <paramref name="value"/> is not null and has exactly
    /// <paramref name="length"/> characters; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="length"/> is negative.
    /// </exception>
    public static bool ExactLength([NotNullWhen(true)] string? value, int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        return value is not null && value.Length == length;
    }
}
