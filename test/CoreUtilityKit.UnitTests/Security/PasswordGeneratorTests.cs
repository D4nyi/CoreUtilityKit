using CoreUtilityKit.Security;

namespace CoreUtilityKit.UnitTests.Security;

public sealed class PasswordGeneratorTests
{
    #region Guard clause tests
    [Theory]
    [InlineData(7, 4)]
    [InlineData(0, 4)]
    [InlineData(-8, 4)]
    [InlineData(129, 4)]
    [InlineData(182, 4)]
    public void GeneratePassword_ShouldThrow_IfRequestedLengthIsOutOfRange(int length, int specialChars)
    {
        // Act
        Action action = () => PasswordGenerator.GeneratePassword(length, specialChars);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(8, 8)]
    [InlineData(8, 9)]
    [InlineData(8, 0)]
    [InlineData(8, -1)]
    [InlineData(8, -8)]
    public void GeneratePassword_ShouldThrow_IfRequestedSpecialCharsIsOutOfRange(int length, int specialChars)
    {
        // Act
        Action action = () => PasswordGenerator.GeneratePassword(length, specialChars);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Deterministic tests

    [Theory]
    [InlineData(8, 4)]
    [InlineData(10, 5)]
    [InlineData(20, 10)]
    public void GeneratePassword_ShouldGeneratePassword_IfInputsAreValid(int length, int specialChars)
    {
        // Act
        string password = PasswordGenerator.GeneratePassword(length, specialChars);

        // Assert
        AssertPassword(password, length);
    }

    [Theory]
    [InlineData(8, 4)]
    [InlineData(10, 5)]
    [InlineData(20, 10)]
    public void GeneratePassword_ShouldAllMatch(int length, int specialChars)
    {
        // Arrange
        const int minSampleCount = 50_000;

        // Act
        for (int i = 0; i < minSampleCount; i++)
        {
            // Assert
            AssertPassword(PasswordGenerator.GeneratePassword(length, specialChars), length);
        }

        Assert.True(true);
    }
    #endregion

    private static void AssertPassword(string password, int length)
    {
        password.ShouldNotBeNullOrEmpty();
        password.ShouldMatch(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}");

        password.Length.ShouldBeInRange(length, length + 4);
    }
}
