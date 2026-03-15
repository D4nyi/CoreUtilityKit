using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoreUtilityKit.Text;

/// <summary>
/// Provides extension methods for string manipulation, including normalization, slug creation, and sanitizing file names.
/// </summary>
public static class StringUtils
{
    private const char Replacer = '_';
    private const uint LowerCaseA = 'a';
    private const uint ZADifference = 'z' - 'a';
    private const int LoweringMask = 0x20;
    private const int UpperingMask = 0x5F;

    /// <summary>
    /// Normalizes the specified string to uppercase and removes non-spacing marks (accents).
    /// </summary>
    /// <param name="value">The string to normalize.</param>
    /// <returns>The normalized uppercase string, or <see cref="String.Empty"/> if the input is null or whitespace.</returns>
    public static string NormalizeToUpper(this string? value)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            return String.Empty;
        }

        value = value.Normalize(NormalizationForm.FormD);

        return String.Create(value.Length, value.EnumerateRunes(), static (span, runes) =>
        {
            int i = 0;
            foreach (Rune runeChar in runes)
            {
                if (Rune.GetUnicodeCategory(runeChar) != UnicodeCategory.NonSpacingMark)
                {
                    span[i++] = IsLowerCaseAsciiLetter(runeChar.Value)
                        ? (char)(runeChar.Value & UpperingMask) // = low 7 bits of ~0x20
                        : (char)runeChar.Value;
                }
            }
        });
    }

    /// <summary>
    /// Normalizes the specified string to the lowercase and removes non-spacing marks (accents).
    /// </summary>
    /// <param name="value">The string to normalize.</param>
    /// <returns>The normalized lowercase string, or <see cref="String.Empty"/> if the input is null or whitespace.</returns>
    public static string NormalizeToLower(this string? value)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            return String.Empty;
        }

        return String.Create(value.Length, value.EnumerateRunes(), static (span, runes) =>
        {
            int i = 0;
            foreach (Rune runeChar in runes)
            {
                if (Rune.GetUnicodeCategory(runeChar) != UnicodeCategory.NonSpacingMark)
                {
                    span[i++] = IsLowerCaseAsciiLetter(runeChar.Value)
                        ? (char)(byte)(runeChar.Value | LoweringMask) // on x86, extending BYTE -> DWORD is more efficient than WORD -> DWORD
                        : (char)runeChar.Value;
                }
            }
        });
    }

    /// <summary>
    /// Creates a URL-friendly slug from the specified string.
    /// </summary>
    /// <param name="value">The string to convert to a slug.</param>
    /// <returns>A slug representation of the string, or <see cref="String.Empty"/> if the input is null or whitespace.</returns>
    public static string CreateSlug(this string? value)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            return String.Empty;
        }

        int originalLength = value.Length;

        value = value.Normalize(NormalizationForm.FormD);

        Span<char> slug = stackalloc char[originalLength];

        int charsWritten = SlugNormalize(value, slug);

        charsWritten = ClampEnd(slug, charsWritten);

        return new string(slug[..charsWritten]);
    }

    /// <summary>
    /// Replaces invalid file name characters in the specified string with an underscore.
    /// </summary>
    /// <param name="value">The string to sanitize.</param>
    /// <returns>A sanitized string suitable for a file name, or <see cref="String.Empty"/> if the input is null or whitespace.</returns>
    public static string ReplaceInvalidFileChars(this string? value)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            return String.Empty;
        }

        return String.Create(value.Length, value, (span, s) =>
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            int i = 0;

            while (i < s.Length)
            {
                int index = Array.IndexOf(invalidFileNameChars, s[i]);
                span[i] = index == -1
                    ? s[i]
                    : Replacer;
                i++;
            }
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsLowerCaseAsciiLetter(int value) => (uint)(value | LoweringMask) - LowerCaseA <= ZADifference;

    #region CreateSlug Helpers

    private static int SlugNormalize(string phrase, Span<char> slug)
    {
        bool isStartOfString = true;
        bool previousIsReplacer = false;
        int i = 0;

        foreach (Rune runeChar in phrase.EnumerateRunes())
        {
            if (isStartOfString && (Rune.IsWhiteSpace(runeChar) || runeChar.Value == Replacer))
            {
                continue;
            }

            isStartOfString = false;

            if (Rune.GetUnicodeCategory(runeChar) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            if (Rune.IsWhiteSpace(runeChar) && !previousIsReplacer)
            {
                slug[i++] = Replacer;
                previousIsReplacer = true;
            }
            else if (Rune.IsLower(runeChar) || Rune.IsDigit(runeChar))
            {
                slug[i++] = (char)runeChar.Value;
                previousIsReplacer = false;
            }
            else if (Rune.IsUpper(runeChar))
            {
                slug[i++] = (char)Rune.ToLowerInvariant(runeChar).Value;
                previousIsReplacer = false;
            }
            else if (runeChar.Value != Replacer && !previousIsReplacer)
            {
                slug[i++] = Replacer;
                previousIsReplacer = true;
            }
        }

        return i;
    }

    private static int ClampEnd(Span<char> span, int end)
    {
        end--;

        for (; end >= 0; end--)
        {
            char c = span[end];
            if (c != Replacer && c != Char.MinValue)
            {
                break;
            }
        }

        return end + 1;
    }

    #endregion
}
