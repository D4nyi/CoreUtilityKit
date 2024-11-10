using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CoreUtilityKit.Helpers;

[ExcludeFromCodeCoverage]
internal static class ThrowHelpers
{
    [DoesNotReturn]
    internal static void ThrowFormatInvalidString(int offset, string resource)
    {
        string message = String.Format(
            CultureInfo.InvariantCulture,
            "Input string was not in a correct format. Failure to parse near offset {0}. {1}",
            offset, resource);
        throw new FormatException(message);
    }

    [DoesNotReturn]
    internal static void ThrowFormatIndexOutOfRange()
    {
        throw new FormatException("Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
    }
}

internal static class ExceptionResource
{
    internal const string Format_UnexpectedClosingBrace = "Unexpected closing brace without a corresponding opening brace.";
    internal const string Format_ExpectedAsciiDigit = "Expected an ASCII digit.";
    internal const string Format_UnclosedFormatItem = "Format item ends prematurely.";
}