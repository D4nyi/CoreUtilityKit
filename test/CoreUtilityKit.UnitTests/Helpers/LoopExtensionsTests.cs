namespace CoreUtilityKit.UnitTests.Helpers;

public sealed class LoopExtensionsTests
{
    [Theory]
    [InlineData(0, 10, 55)]
    [InlineData(5, 10, 45)]
    public void LoopExtension_ShouldGenerateTheCorrectNumbers(int start, int end, int expectedSum)
    {
        // Act
        int sum = 0;
        foreach (int i in start..end)
        {
            sum += i;
        }

        // Assert
        sum.Should().Be(expectedSum);
    }

    [Theory]
    [InlineData(10, 55)]
    [InlineData(5, 15)]
    public void LoopExtension_ShouldGenerateTheCorrectNumbers_WhenStartNotSpecified(int end, int expectedSum)
    {
        // Act
        int sum = 0;
        foreach (int i in ..end)
        {
            sum += i;
        }

        // Assert
        sum.Should().Be(expectedSum);
    }

    [Theory]
    [InlineData(10, 55)]
    [InlineData(5, 15)]
    public void LoopExtension_ShouldGenerateTheCorrectNumbers_WhenOnlyEndSpecified(int end, int expectedSum)
    {
        // Act
        int sum = 0;
        foreach (int i in end)
        {
            sum += i;
        }

        // Assert
        sum.Should().Be(expectedSum);
    }

    [Fact]
    public void LoopExtension_ShouldThrow_WhenInfiniteLoopDetected()
    {
        // Act
        Func<int> action = () =>
        {
            int sum = 0;
            foreach (int i in 0..)
            {
                sum += i;
            }

            return sum;
        };

        // Assert
        _ = action.Should()
            .Throw<NotSupportedException>()
            .WithMessage("Infinite loops are not allowed!");
    }
}
