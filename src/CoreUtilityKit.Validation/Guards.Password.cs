using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CoreUtilityKit.Validation;

public static partial class Guards
{
    /// <summary>
    /// Validates a password to be a secure, strong password.
    /// </summary>
    /// <param name="password">To be validated.</param>
    /// <returns><see langword="true" /> if the email password is matches the format requirements; otherwise <see langword="false" /></returns>
    /// <remarks>
    /// Format:
    /// <list type="bullet">
    /// <item>length &gt; 8</item>
    /// <item>no whitespaces</item>
    /// <item>one lowercase letter</item>
    /// <item>one uppercase letter</item>
    /// <item>one digit</item>
    /// </list>
    /// </remarks>
    public static bool ValidatePassword([NotNullWhen(true)] string? password)
    {
        try
        {
            return
                ValidateStringConstraints(password) &&
                PasswordRegex().IsMatch(password);
        }
        catch // hard to test
        {
            return false;
        }
    }

    private static bool ValidateStringConstraints([NotNullWhen(true)] string? password)
    {
        if (password is null || password.Length < 8 || password.Length > 128)
        {
            return false;
        }

        foreach (char c in password)
        {
            if (Char.IsWhiteSpace(c))
            {
                return false;
            }
        }

        return true;
    }

    [GeneratedRegex(PasswordValidation, Options, TimeoutMilliseconds)]
    public static partial Regex PasswordRegex();
}