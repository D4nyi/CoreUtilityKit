using System.Diagnostics.CodeAnalysis;

namespace CoreUtilityKit.Validation;

public static partial class Guards
{
    public static bool MinLength([NotNullWhen(true)] string? value, int minLength)
    {
        return !String.IsNullOrWhiteSpace(value) && value.Length >= minLength;
    }

    public static bool MaxLength([NotNullWhen(true)] string? value, int maxLength)
    {
        return !String.IsNullOrWhiteSpace(value) && value.Length <= maxLength;
    }

    public static bool Between([NotNullWhen(true)] string? value, int minLength, int maxLength)
    {
        return !String.IsNullOrWhiteSpace(value) && value.Length >= minLength && value.Length <= maxLength;
    }

    public static bool ExactLength([NotNullWhen(true)] string? value, int length)
    {
        return !String.IsNullOrWhiteSpace(value) && value.Length == length;
    }
}