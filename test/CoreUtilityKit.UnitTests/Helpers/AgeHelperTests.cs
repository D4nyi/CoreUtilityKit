using CoreUtilityKit.UnitTests.DataGenerators;

namespace CoreUtilityKit.UnitTests.Helpers;

public sealed class AgeHelperTests
{
    private readonly AgeHelper _ageHelper = new(Constants.TimeProvider);

    [Fact]
    public void Ctor_ShouldThrow_WhenTimeProviderIsNull()
    {
        // Arrange
        Action action = () => _ = new AgeHelper(null!);

        // Act && Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(AgeHelperDateTimeGenerator))]
    public void CalculateAge_DateTime_HappyCase(DateTime dateTime, int expectedAge)
    {
        // Act
        int age = _ageHelper.CalculateAge(dateTime);

        // Assert
        age.Should().Be(expectedAge);
    }

    [Fact]
    public void CalculateAge_DateTime_LeapDay()
    {
        // Arrange
        AgeHelper ageHelper = new(Constants.CreateTimeProviderForLeapDay());

        // Act
        int ageBeforeLeapDay = ageHelper.CalculateAge(Constants.DateTimeUtcLeapDay);
        int ageAfterLeapDay = ageHelper.CalculateAge(Constants.DateTimeUtcLeapDay);

        // Assert
        ageBeforeLeapDay.Should().Be(Constants.Age);
        ageAfterLeapDay.Should().Be(Constants.AgeLeapDay);
    }

    [Theory]
    [ClassData(typeof(AgeHelperDateTimeOffsetGenerator))]
    public void CalculateAge_DateTimeOffset_HappyCase(DateTime dateTimeOff, int expectedAge)
    {
        // Act
        int age = _ageHelper.CalculateAge(dateTimeOff);

        // Assert
        age.Should().Be(expectedAge);
    }

    [Fact]
    public void CalculateAge_DateTimeOffset_LeapDay()
    {
        // Arrange
        AgeHelper ageHelper = new(Constants.CreateTimeProviderForLeapDay());

        // Act
        int ageBeforeLeapDay = ageHelper.CalculateAge(Constants.DateTimeOffsetUtcLeapDay);
        int ageAfterLeapDay = ageHelper.CalculateAge(Constants.DateTimeOffsetUtcLeapDay);

        // Assert
        ageBeforeLeapDay.Should().Be(Constants.Age);
        ageAfterLeapDay.Should().Be(Constants.AgeLeapDay);
    }

    [Theory]
    [ClassData(typeof(AgeHelperDateOnlyGenerator))]
    public void CalculateAge_DateOnly_HappyCase(DateOnly dateOnly, int expectedAge)
    {
        // Act
        int age = _ageHelper.CalculateAge(dateOnly);

        // Assert
        age.Should().Be(expectedAge);
    }

    [Fact]
    public void CalculateAge_DateOnly_LeapDay()
    {
        // Arrange
        AgeHelper ageHelper = new(Constants.CreateTimeProviderForLeapDay());

        DateOnly dateOnly = DateOnly.FromDateTime(Constants.DateTimeUtcLeapDay);

        // Act
        int ageBeforeLeapDay = ageHelper.CalculateAge(dateOnly);
        int ageAfterLeapDay = ageHelper.CalculateAge(dateOnly);

        // Assert
        ageBeforeLeapDay.Should().Be(Constants.Age);
        ageAfterLeapDay.Should().Be(Constants.AgeLeapDay);
    }
}
