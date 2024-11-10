using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoreUtilityKit.Text;

public static class StringUtils
{
    private const char Replacer = '_';
    private const uint LowerCaseA = 'a';
    private const uint ZADifference = 'z' - 'a';
    private const int LoweringMask = 0x20;
    private const int UpperingMask = 0x5F;

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
                        ? (char)(byte)(runeChar.Value |
                                       LoweringMask) // on x86, extending BYTE -> DWORD is more efficient than WORD -> DWORD
                        : (char)runeChar.Value;
                }
            }
        });
    }

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
            else if(runeChar.Value != Replacer && !previousIsReplacer)
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