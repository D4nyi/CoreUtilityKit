using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;

namespace CoreUtilityKit.Security;

/// <summary>
/// A helper class that generates localization safe and strong password for new users and forgotten password scenarios
/// </summary>
public static class PasswordGenerator
{
    private const int Zero   = '0';
    private const int UpperA = 'A';
    private const int LowerA = 'a';

    private const int PunctuationCount = 11;

    /// <summary>
    /// Used to add special characters to the password
    /// </summary>
    private static readonly char[] _punctuations = ['!', '?', '#', '&', '@', '$', '*', '^', '%', '-', '_'];

    static PasswordGenerator()
    {
        Debug.Assert(
            PunctuationCount == _punctuations.Length,
            $"{nameof(PunctuationCount)} != {nameof(_punctuations)}.Length",
            $"Synchronize the {nameof(PunctuationCount)} with the update length of {nameof(_punctuations)}"
        );
    }

    /// <summary>
    /// Generates a localization safe and strong password with a given length
    /// </summary>
    /// <param name="length">The generated passwords length (8-128)</param>
    /// <param name="specialChars"></param>
    /// <returns>The created safe and strong password</returns>
    /// <remarks>
    /// <b>Recommended specialChars count is:</b> <c>Math.Ceiling(length / 10) + 1</c><br/>
    /// <list>
    /// <item>Length: 9 => SpecialChars: 2</item>
    /// <item>Length: 32 => SpecialChars: 5</item>
    /// </list>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Throws when the length is out of range or <paramref name="specialChars"/> is larger or equal to <paramref name="length"/></exception>
    /// <exception cref="CryptographicException">Throws if the cryptographic service cannot be initialized</exception>
    [UnsupportedOSPlatform("browser")]
    public static string GeneratePassword(int length, int specialChars)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(length, 8);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, 128);

        // -3 => so a lower and upper case letter and a number can be added
        ArgumentOutOfRangeException.ThrowIfLessThan(specialChars, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(specialChars, length - 3);

        Span<char> password = stackalloc char[length];

        // *2 to correctly fill the password buffer
        Span<byte> buf = stackalloc byte[length * 2];
        RandomNumberGenerator.Fill(buf);

        Span<byte> specialCharPos = stackalloc byte[specialChars];
        RandomNumberGenerator.Fill(specialCharPos);

        _ = RandomNumberGenerator.GetInt32(10);

        int numbers  = 0;
        int lowers   = 0;
        int uppers   = 0;

        bool needsSpecials     = true;
        byte iter              = 0;
        int  nextEmptyPosition = 0;
        while (nextEmptyPosition != length)
        {
            int pos = needsSpecials
                ? RandomNumberGenerator.GetInt32(length)
                : nextEmptyPosition;

            ref char currentPasswordChar = ref password[pos];
            if (currentPasswordChar != Char.MinValue)
            {
                nextEmptyPosition++;
                continue;
            }

            if (needsSpecials)
            {
                currentPasswordChar = _punctuations[RandomNumberGenerator.GetInt32(_punctuations.Length)];
                needsSpecials = --specialChars == 0;
                continue;
            }

            if (iter >= buf.Length)
            {
                // rewrite the buffer
                RandomNumberGenerator.Fill(buf);
                iter = 0;
            }

            int i = buf[iter] % 87;
            switch (i)
            {
                case < 10:
                    numbers++;
                    currentPasswordChar = (char)(Zero + i);
                    break;
                case < 36:
                    uppers++;
                    currentPasswordChar = (char)(UpperA + i - 10);
                    break;
                case < 62:
                    lowers++;
                    currentPasswordChar = (char)(LowerA + i - 36);
                    break;
                default:
                    // skip the incrementation of 'nextEmptyPosition'
                    iter++;
                    continue;
            }

            iter++;
            nextEmptyPosition++;
        }

        Change change = WhatToChange(numbers, uppers, lowers);
        if (change != Change.None)
        {
            if (numbers == 0)
            {
                ChangeRandomChar(password, length, Zero, 10, change);
                numbers++;
                change = WhatToChange(numbers, uppers, lowers);
            }

            if (uppers == 0)
            {
                ChangeRandomChar(password, length, UpperA, 26, change);
                uppers++;
                change = WhatToChange(numbers, uppers, lowers);
            }

            if (lowers == 0)
            {
                ChangeRandomChar(password, length, LowerA, 26, change);
            }
        }

        return new string(password);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ChangeRandomChar(Span<char> password, int length, int start, int offset, Change change)
    {
        Func<char, bool> comparer = change switch
        {
            Change.Numbers => Char.IsAsciiDigit,
            Change.Uppers => Char.IsAsciiLetterUpper,
            Change.Lowers => Char.IsAsciiLetterLower,
            _ => throw new UnreachableException("This case should never execute!")
        };

        int k;
        do
        {
            k = RandomNumberGenerator.GetInt32(length);
        } while (!comparer(password[k]));

        password[k] = (char)(start + RandomNumberGenerator.GetInt32(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Change WhatToChange(int numbers, int uppers, int lowers)
    {
        if (numbers == 0)
        {
            return uppers > lowers ? Change.Uppers : Change.Lowers;
        }

        if (uppers == 0)
        {
            return numbers > lowers ? Change.Numbers : Change.Lowers;
        }

        if (lowers == 0)
        {
            return numbers > uppers ? Change.Numbers : Change.Uppers;
        }

        return Change.None;
    }

    private enum Change
    {
        None = 0,
        Numbers,
        Uppers,
        Lowers,
    }
}