using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace CoreUtilityKit.Helpers;

/// <summary>
/// Provides methods for converting between <see cref="Guid"/> and its Base64 string representation.
/// </summary>
public static class GuidConverter
{
    private const char Equal             = '=';
    private const char Hyphen            = '-';
    private const char Underscore        = '_';
    private const char Slash             = '/';
    private const char Plus              = '+';
    private const byte SlashByte         = (byte)'/';  // 47
    private const byte PlusByte          = (byte)'+';  // 43
    private const string EmptyGuidBase64 = "AAAAAAAAAAAAAAAAAAAAAA";

    /// <summary>
    /// Parses a Base64 string into a <see cref="Guid"/>.
    /// </summary>
    /// <param name="id">The Base64 string representation of a GUID.</param>
    /// <returns>A <see cref="Guid"/> parsed from the string, or <see cref="Guid.Empty"/> if the string is null or empty.</returns>
    public static Guid Parse(string? id)
    {
        return String.IsNullOrWhiteSpace(id)
            ? Guid.Empty
            : Parse(id.AsSpan());
    }

    /// <summary>
    /// Parses a Base64 character span into a <see cref="Guid"/>.
    /// </summary>
    /// <param name="id">The Base64 character span representation of a GUID.</param>
    /// <returns>A <see cref="Guid"/> parsed from the span, or <see cref="Guid.Empty"/> if the span length is not 22.</returns>
    public static Guid Parse(ReadOnlySpan<char> id)
    {
        if (id.Length != 22)
        {
            return Guid.Empty;
        }

        Span<char> base64Chars = stackalloc char[24];

        for (int i = 0; i < 22; i++)
        {
            char c = id[i];
            base64Chars[i] = c switch
            {
                Hyphen => Slash,
                Underscore => Plus,
                _ => c
            };
        }

        base64Chars[22] = Equal;
        base64Chars[23] = Equal;

        Span<byte> bytes = stackalloc byte[16];

        _ = Convert.TryFromBase64Chars(base64Chars, bytes, out _);

        return new Guid(bytes);
    }

    /// <summary>
    /// Converts a <see cref="Guid"/> to its Base64 string representation.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> to convert.</param>
    /// <returns>A Base64 string representation of the <see cref="Guid"/>.</returns>
    public static string ToBase64String(Guid id)
    {
        if (id == Guid.Empty)
        {
            return EmptyGuidBase64;
        }

        Span<byte> idBytes = stackalloc byte[16];
        Span<byte> base64Bytes = stackalloc byte[24];

        _ = MemoryMarshal.TryWrite(idBytes, in id);

        _ = Base64.EncodeToUtf8(idBytes, base64Bytes, out _, out _);

        Span<char> chars = stackalloc char[22];

        for (int i = 0; i < 22; i++)
        {
            byte b = base64Bytes[i];
            chars[i] = b switch
            {
                PlusByte => Underscore,
                SlashByte => Hyphen,
                _ => (char)b
            };
        }

        return new string(chars);
    }
}
