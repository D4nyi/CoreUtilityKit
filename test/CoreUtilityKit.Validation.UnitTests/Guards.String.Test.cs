using CoreUtilityKit.Validation.UnitTests.DataGenerators;

namespace CoreUtilityKit.Validation.UnitTests;

public sealed class GuardsStringTests
{
    [Theory]
    [ClassData(typeof(MinLengthGenerator))]
    public void MinLength_HappyCase(string? value, int minLength, bool expectedResult)
    {
        // Act
        bool success = Guards.MinLength(value, minLength);

        // Assert
        success.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(MaxLengthGenerator))]
    public void MaxLength_HappyCase(string? value, int maxLength, bool expectedResult)
    {
        // Act
        bool success = Guards.MaxLength(value, maxLength);

        // Assert
        success.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(BetweenLengthBaseGenerator))]
    public void BetweenLength_HappyCase(string? value, int minLength, bool expectedResult)
    {
        // Act
        bool success = Guards.Between(value, minLength, LengthBaseGenerator.MaxLength);

        // Assert
        success.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(ExactLengthBaseGenerator))]
    public void ExactLength_HappyCase(string? value, int length, bool expectedResult)
    {
        // Act
        bool success = Guards.ExactLength(value, length);

        // Assert
        success.Should().Be(expectedResult);
    }
}