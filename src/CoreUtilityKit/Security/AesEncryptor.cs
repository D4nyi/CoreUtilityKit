using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreUtilityKit.Security;

[UnsupportedOSPlatform("browser")]
[UnsupportedOSPlatform("ios")]
[UnsupportedOSPlatform("tvos")]
public static class AesEncryptor
{
    private const string SerializationUnreferencedCodeMessage = "JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.";
    private const string SerializationRequiresDynamicCodeMessage = "JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.";

    private const int SaltSize  = 32;
    private const int TagSize   = 16;
    private const int NonceSize = 12;

    private static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
    {
        // Web defaults don't use the relax JSON escaping encoder.
        // Because these options are for producing content that is written directly to the request
        // (and not embedded in an HTML page, for example), we can use UnsafeRelaxedJsonEscaping.
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        MaxDepth = 32,
        PropertyNameCaseInsensitive = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        NumberHandling = JsonNumberHandling.Strict | JsonNumberHandling.AllowNamedFloatingPointLiterals
    };

    /// <summary>
    /// Serializes the given <typeparamref name="T"/> type to JSON then encrypts it with the given <paramref name="password"/>
    /// </summary>
    /// <typeparam name="T">Generic type parameter for the data parameter</typeparam>
    /// <param name="data">An object that should be encrypted</param>
    /// <param name="password">The private key that will be used to encrypt the data</param>
    /// <param name="options">JSON serializer options</param>
    /// <returns>The hashed <paramref name="data"/></returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="CryptographicException" />
    /// <exception cref="NotSupportedException" />
    [RequiresUnreferencedCode(SerializationUnreferencedCodeMessage)]
    [RequiresDynamicCode(SerializationRequiresDynamicCodeMessage)]
    public static string EncryptAsJson<T>(T? data, string? password, JsonSerializerOptions? options = null)
    {
        if (data is null)
        {
            return String.Empty;
        }

        ArgumentException.ThrowIfNullOrEmpty(password);

        byte[] json = JsonSerializer.SerializeToUtf8Bytes(data, options ?? _serializerOptions);

        return EncryptCore(json, password);
    }

    /// <summary>
    /// Encrypts the given <paramref name="data"/> with the given <paramref name="password"/> and returns the hash
    /// </summary>
    /// <param name="data">A string that should be encrypted</param>
    /// <param name="password">The private key that will be used to encrypt the data</param>
    /// <returns>The hashed <paramref name="data"/></returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="CryptographicException" />
    public static string Encrypt(string? data, string? password)
    {
        if (String.IsNullOrEmpty(data))
        {
            return String.Empty;
        }

        ArgumentException.ThrowIfNullOrEmpty(password);

        return EncryptCore(Encoding.UTF8.GetBytes(data), password);
    }

    private static string EncryptCore(byte[] plaintext, string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] key = DeriveKey(password, salt);

        Span<byte> tag        = stackalloc byte[TagSize];
        Span<byte> nonce      = stackalloc byte[NonceSize];
        Span<byte> ciphertext = stackalloc byte[plaintext.Length];

        RandomNumberGenerator.Fill(nonce);

        using (AesGcm aes = new(key, TagSize))
        {
            aes.Encrypt(nonce, plaintext.AsSpan(), ciphertext, tag);
        }

        int start = 0;
        Span<byte> result = stackalloc byte[SaltSize + NonceSize + TagSize + ciphertext.Length];

        salt.CopyTo(result.Slice(start, SaltSize));
        start += SaltSize;

        ciphertext.CopyTo(result.Slice(start, ciphertext.Length));
        start += ciphertext.Length;

        nonce.CopyTo(result.Slice(start, NonceSize));
        start += NonceSize;

        tag.CopyTo(result[start..]);

        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Decrypts a <paramref name="hash"/> that originally was a JSON string and maps it to the provided <typeparamref name="T"/> type
    /// </summary>
    /// <typeparam name="T">A type to which the decrypted JSON should be mapped</typeparam>
    /// <param name="hash">A string that should be decrypted</param>
    /// <param name="password">The private key that used to encrypt the data</param>
    /// <param name="options">JSON serializer options</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="CryptographicException" />
    /// <exception cref="FormatException" />
    /// <exception cref="JsonException" />
    /// <exception cref="NotSupportedException" />
    [RequiresUnreferencedCode(SerializationUnreferencedCodeMessage)]
    [RequiresDynamicCode(SerializationRequiresDynamicCodeMessage)]
    public static T? DecryptFromJson<T>(string? hash, string? password, JsonSerializerOptions? options = null)
    {
        if (String.IsNullOrEmpty(hash))
        {
            return default;
        }

        ArgumentException.ThrowIfNullOrEmpty(password);

        ReadOnlySpan<byte> hashBytes = Convert.FromBase64String(hash);
        int ciphertextLength = CalcCipherTextLength(hashBytes.Length);
        Span<byte> json = stackalloc byte[ciphertextLength];

        DecryptCore(hashBytes, password, json);

        return JsonSerializer.Deserialize<T>(json, options ?? _serializerOptions);
    }

    /// <summary>
    /// Decrypts a <paramref name="hash"/> with the given <paramref name="password"/> and returns the decrypted text.
    /// </summary>
    /// <param name="hash">A string that should be decrypted</param>
    /// <param name="password">The private key that used to encrypt the data</param>
    /// <returns>The decrypted text</returns>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="ArgumentOutOfRangeException" />
    /// <exception cref="CryptographicException" />
    /// <exception cref="FormatException" />
    /// <exception cref="ObjectDisposedException" />
    /// <exception cref="InvalidOperationException" />
    public static string Decrypt(string? hash, string? password)
    {
        if (String.IsNullOrEmpty(hash))
        {
            return String.Empty;
        }

        ArgumentException.ThrowIfNullOrEmpty(password);

        ReadOnlySpan<byte> hashBytes = Convert.FromBase64String(hash);
        int ciphertextLength = CalcCipherTextLength(hashBytes.Length);
        Span<byte> plaintext = stackalloc byte[ciphertextLength];

        DecryptCore(hashBytes, password, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }

    private static void DecryptCore(ReadOnlySpan<byte> hashBytes, string password, Span<byte> plaintext)
    {
        byte[] salt = hashBytes[..SaltSize].ToArray();
        byte[] key = DeriveKey(password, salt);

        int start = SaltSize;

        ReadOnlySpan<byte> ciphertext = hashBytes.Slice(SaltSize, plaintext.Length);
        start += plaintext.Length;

        ReadOnlySpan<byte> nonce = hashBytes.Slice(start, NonceSize);
        start += NonceSize;

        ReadOnlySpan<byte> tag = hashBytes[start..];

        using (AesGcm aes = new(key, TagSize))
        {
            aes.Decrypt(nonce, ciphertext, tag, plaintext);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalcCipherTextLength(int hashBytesLength) => hashBytesLength - (SaltSize + NonceSize + TagSize);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte[] DeriveKey(string password, byte[] salt)
    {
        const int iterations = 100_000;
        const int outputLength = 256 / 8;

        return Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, outputLength);
    }
}
