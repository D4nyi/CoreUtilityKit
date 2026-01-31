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
        hash.ShouldNotBeNullOrWhiteSpace();
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
            hashes.Add(hash).ShouldBeTrue();
        }

        // Assert
        hashes.Count.ShouldBe(expectedCount);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Encrypt_ShouldReturnEmptyString_WhenInputIsNullOrEmpty(string? data)
    {
        // Act
        Action action = () => AesEncryptor.Encrypt(data, Password).ShouldBeEmpty();

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void Encrypt_ShouldThrow_WhenPasswordIsNull()
    {
        // Act
        Action action = () => AesEncryptor.Encrypt(StringToHash, null);

        // Assert
        action.ShouldThrow<ArgumentNullException>();
    }
    #endregion

    #region EncryptAsJson Tests
    [Fact]
    public void EncryptAsJson_ShouldHashAObject_WhenObjectIsValid()
    {
        // Act
        string hash = AesEncryptor.EncryptAsJson(_objectToHash, Password);

        // Assert
        hash.ShouldNotBeNullOrWhiteSpace();
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
            hashes.Add(hash).ShouldBeTrue();
        }

        // Assert
        hashes.Count.ShouldBe(expectedCount);
    }

    [Fact]
    public void EncryptAsJson_ShouldReturnEmptyString_WhenInputIsNull()
    {
        // Act
        Action action = () => AesEncryptor.EncryptAsJson<TestData>(null, Password).ShouldBeEmpty();

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void EncryptAsJson_ShouldThrow_WhenPasswordIsNull()
    {
        // Act
        Action action = () => AesEncryptor.EncryptAsJson(StringToHash, null);

        // Assert
        action.ShouldThrow<ArgumentNullException>();
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
        stringThatWasHashed.ShouldBe(StringToHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Decrypt_ShouldReturnEmptyString_WhenInputIsNullOrEmpty(string? hash)
    {
        // Act
        Action action = () => AesEncryptor.Decrypt(hash, Password).ShouldBeEmpty();

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void Decrypt_ShouldThrow_WhenPasswordIsNull()
    {
        // Act
        Action action = () => AesEncryptor.Decrypt(StringToHash, null);

        // Assert
        action.ShouldThrow<ArgumentNullException>();
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
        obj.ShouldNotBeNull().ShouldBeEquivalentTo(_objectToHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void DecryptFromJson_ShouldReturnNullForClass_WhenInputIsNullOrEmpty(string? hash)
    {
        // Act
        Action action = () =>
             AesEncryptor.DecryptFromJson<TestData>(hash, Password).ShouldBeNull();

        // Assert
        action.ShouldNotThrow();
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
        action.ShouldThrow<ArgumentNullException>();
    }
    #endregion
}
