using CoreUtilityKit.Validation.UnitTests.DataGenerators;

namespace CoreUtilityKit.Validation.UnitTests;

public sealed class GuardsEmailTests
{
    [Theory]
    [ClassData(typeof(ValidEmailGenerator))]
    public void ValidateEmail_ShouldReturnTrue_WhenInputIsValid(string email)
    {
        // Act
        bool success = Guards.ValidateEmail(email);

        // Assert
        success.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(InvalidEmailGenerator))]
    public void ValidateEmail_ShouldReturnFalse_WhenInputIsInvalid(string? email)
    {
        // Act
        bool success = Guards.ValidateEmail(email);

        // Assert
        success.ShouldBeFalse();
    }

    [Fact]
    public void ValidateEmail_ShouldHandleArgumentExceptionThrow_WhenInputContainsNonIDNAChars()
    {
        // Arrange
        const string email = "email@example..com";
        Func<bool> action = () => Guards.ValidateEmail(email);

        // Act, Assert
        action.ShouldNotThrow();
    }
}
