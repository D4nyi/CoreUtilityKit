using CoreUtilityKit.Validation.UnitTests.DataGenerators;

namespace CoreUtilityKit.Validation.UnitTests;

public sealed class GuardsPasswordTests
{
    [Fact]
    public void ValidatePassword_ShouldReturnTrue_WhenInputIsValid()
    {
        // Arrange
        const string password = "Password123!";

        // Act
        bool success = Guards.ValidatePassword(password);

        // Assert

        success.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(InvalidPasswordGenerator))]
    public void ValidatePassword_ShouldReturnFalse_WhenInputIsAnInvalidPassword(string? password)
    {
        // Act
        bool success = Guards.ValidatePassword(password);

        // Assert
        success.Should().BeFalse();
    }
}
