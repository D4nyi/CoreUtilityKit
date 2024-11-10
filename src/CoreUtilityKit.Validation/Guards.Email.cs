using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreUtilityKit.Validation;

public static partial class Guards
{
    /// <summary>
    /// Validates the email address to be in a valid format.
    /// </summary>
    /// <param name="email">Email address to be validated</param>
    /// <returns><see langword="true" /> if the <paramref name="email"/> address has a valid format and character set; otherwise <see langword="false" />.</returns>
    public static bool ValidateEmail([NotNullWhen(true)] string? email)
    {
        if (!ValidateEmailParts(email, out int atPos))
        {
            return false;
        }

        try
        {
            atPos++; // to include the '@' symbol in the result

            // Normalize the domain
            // Pull out and process domain name (throws ArgumentException on invalid)
            string domainName = _idn.GetAscii(email, atPos);
            Span<char> result = stackalloc char[atPos + domainName.Length];
            email.AsSpan(0, atPos).CopyTo(result);
            domainName.AsSpan().CopyTo(result[atPos..]);

            return EmailRegex().IsMatch(result);
        }
        catch // hard to test
        {
            return false;
        }
    }

    /// <summary>
    /// Validates the given emails and its parts length according to RFC 5321
    /// </summary>
    /// <param name="email">Email which length will be validated</param>
    /// <param name="atPos">
    /// When this method returns, contains the index of the 'at' (@) symbol
    /// if the validation succeeded, or zero if the validation failed.
    /// </param>
    /// <returns><see langword="true" /> if the email address matches the RFC format; otherwise <see langword="false" /></returns>
    private static bool ValidateEmailParts([NotNullWhen(true)] string? email, out int atPos)
    {
        ReadOnlySpan<char> emailSpan = email.AsSpan();

        atPos = emailSpan.IndexOf('@');

        // must contain an '@' symbol and has at least one char before it
        if (atPos < 1)
        {
            atPos = default;
            return false;
        }

        // validate domain with tld
        int lastDot = emailSpan.LastIndexOf('.');
        if (lastDot <= 1) // no one has a tld domain email address, or a domain with no tld
        {
            atPos = default;
            return false;
        }

        // https://stackoverflow.com/a/386302/9730811
        // Note:
        // in the linked post the user does not refer to the unit of measurement
        // but in the RFC 5321 [4.5.3. Sizes and Timeouts] clearly states
        // that it is measured in octets (bytes)
        // https://www.rfc-editor.org/rfc/rfc5321#section-4.5.3
        int emailByteCount = Encoding.UTF8.GetByteCount(emailSpan);
        if (emailByteCount > 320)
        {
            atPos = default;
            return false;
        }

        ReadOnlySpan<char> local = emailSpan[..atPos];

        // validate parts (local and domain) lengths according to RFC
        int localPartByteCount = Encoding.UTF8.GetByteCount(local);
        int domainPartByteCount = emailByteCount - localPartByteCount; // contains the '@' symbol
        if (localPartByteCount > 64 ||     // < 1 case tested above by searching for '@'
            domainPartByteCount > 255 + 1) // + 1 for the @ symbol; it is not included in the domain part by the RFC
        {
            atPos = default;
            return false;
        }

        return true;
    }

    [GeneratedRegex(EmailValidation2, Options, TimeoutMilliseconds)]
    public static partial Regex EmailRegex();
}