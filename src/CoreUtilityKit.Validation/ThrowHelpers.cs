using System.Diagnostics.CodeAnalysis;

namespace CoreUtilityKit.Validation;

[ExcludeFromCodeCoverage]
internal static class ThrowHelpers
{
    [DoesNotReturn]
    internal static void ArgumentOutOfRange(string? paramName, object actualValue)
    {
        throw new ArgumentOutOfRangeException(paramName, actualValue, "The specified parameter is outside the processable range!");
    }

    [DoesNotReturn]
    internal static void ArgumentOutOfRangeCollection(string? paramName, int actualValue)
    {
        throw new ArgumentOutOfRangeException(paramName, actualValue, "The collections length is out of range!");
    }
}