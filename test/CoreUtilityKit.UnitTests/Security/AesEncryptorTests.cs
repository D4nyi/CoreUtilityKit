using CoreUtilityKit.Security;
using CoreUtilityKit.UnitTests.DataGenerators.Models;

namespace CoreUtilityKit.UnitTests.Security;

public sealed class AesEncryptorTests
{
    private const string Password = "SecretPassword123!";
    private const string StringToHash = "SomeRandomTextThatWillBeHashed";
    private static readonly TestData _objectToHash = new()
    {
        Id = 5,
        Name = "John Doe"
    };

    #region Encrypt Tests
    [Fact]
    public void Encrypt_ShouldHashAString_WhenStringIsValid()
    {
        // Act
        string hash = AesEncryptor.Encrypt(StringToHash, Password);

        // Assert
        _ = hash.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Encrypt_ShouldCreateDifferentHashes_WhenTheInputsAreTheSame()
    {
        // Arrange
        const int expectedCount = 10;

        HashSet<string> hashes = new(expectedCount);

        // Act
        for (int i = 0; i < expectedCount; i++)
        {
            string hash = AesEncryptor.Encrypt(StringToHash, Password);
            // Assert
            _ = hashes.Add(hash).Should().BeTrue();
        }

        // Assert
        hashes.Should().HaveCount(expectedCount);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Encrypt_ShouldReturnEmptyString_WhenInputIsNullOrEmpty(string? data)
    {
        // Act
        Action action = () => AesEncryptor.Encrypt(data, Password).Should().BeEmpty();

        // Assert
        _ = action.Should().NotThrow();
    }

    [Fact]
    public void Encrypt_ShouldThrow_WhenPasswordIsNull()
    {
        // Act
        Action action = () => AesEncryptor.Encrypt(StringToHash, null);

        // Assert
        _ = action.Should().Throw<ArgumentNullException>();
    }
    #endregion

    #region EncryptAsJson Tests
    [Fact]
    public void EncryptAsJson_ShouldHashAObject_WhenObjectIsValid()
    {
        // Act
        string hash = AesEncryptor.EncryptAsJson(_objectToHash, Password);

        // Assert
        _ = hash.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void EncryptAsJson_ShouldCreateDifferentHashes_WhenTheInputsAreTheSame()
    {
        // Arrange
        const int expectedCount = 10;

        HashSet<string> hashes = new(expectedCount);

        // Act
        for (int i = 0; i < 10; i++)
        {
            string hash = AesEncryptor.EncryptAsJson(_objectToHash, Password);
            _ = hashes.Add(hash).Should().BeTrue();
        }

        // Assert
        hashes.Should().HaveCount(expectedCount);
    }

    [Fact]
    public void EncryptAsJson_ShouldReturnEmptyString_WhenInputIsNull()
    {
        // Act
        Action action = () => AesEncryptor.EncryptAsJson<TestData>(null, Password).Should().BeEmpty();

        // Assert
        _ = action.Should().NotThrow();
    }

    [Fact]
    public void EncryptAsJson_ShouldThrow_WhenPasswordIsNull()
    {
        // Act
        Action action = () => AesEncryptor.EncryptAsJson(StringToHash, null);

        // Assert
        _ = action.Should().Throw<ArgumentNullException>();
    }
    #endregion

    #region Decrypt Tests
    [Fact]
    public void Decrypt_ShouldHashAString_WhenStringIsValid()
    {
        // Arrange
        string hash = AesEncryptor.Encrypt(StringToHash, Password);

        // Act
        string stringThatWasHashed = AesEncryptor.Decrypt(hash, Password);

        // Assert
        _ = stringThatWasHashed.Should().Be(StringToHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Decrypt_ShouldReturnEmptyString_WhenInputIsNullOrEmpty(string? hash)
    {
        // Act
        Action action = () => AesEncryptor.Decrypt(hash, Password).Should().BeEmpty();

        // Assert
        _ = action.Should().NotThrow();
    }

    [Fact]
    public void Decrypt_ShouldThrow_WhenPasswordIsNull()
    {
        // Act
        Action action = () => AesEncryptor.Decrypt(StringToHash, null);

        // Assert
        _ = action.Should().Throw<ArgumentNullException>();
    }
    #endregion

    #region DecryptFromJson Tests
    [Fact]
    public void DecryptFromJson_ShouldHashAString_WhenStringIsValid()
    {
        // Arrange
        string hash = AesEncryptor.EncryptAsJson(_objectToHash, Password);

        // Act
        TestData? obj = AesEncryptor.DecryptFromJson<TestData>(hash, Password);

        // Assert
        _ = obj.Should().NotBeNull().And.BeEquivalentTo(_objectToHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void DecryptFromJson_ShouldReturnNullForClass_WhenInputIsNullOrEmpty(string? hash)
    {
        // Act
        Action action = () =>
             AesEncryptor.DecryptFromJson<TestData>(hash, Password).Should().BeNull();

        // Assert
        _ = action.Should().NotThrow();
    }

    [Fact]
    public void DecryptFromJson_ShouldThrow_WhenPasswordIsNull()
    {
        // Arrange
        string hash = AesEncryptor.EncryptAsJson(_objectToHash, Password);

        // Act
        Func<TestData?> action = () =>
             AesEncryptor.DecryptFromJson<TestData>(hash, null);

        // Assert
        _ = action.Should().Throw<ArgumentNullException>();
    }
    #endregion
}
