using CoreUtilityKit.Text;

namespace CoreUtilityKit.UnitTests.Text;

public sealed class UrlHelperTests
{
    public static readonly TheoryData<Dictionary<string, string>?> StringValueData = [ [], null ];

    public static readonly TheoryData<Dictionary<string, int>?> NumberValueData = [ [], null ];

    [Fact]
    public void ToQueryString_ShouldCreateCorrectQueryString_HappyCase()
    {
        // Arrange
        Dictionary<string, string> dict = new()
        {
            { "test", "value" },
            { "random", "true" }
        };

        const string expectedResult = "test=value&random=true";

        // Act
        string queryString = UrlHelper.ToQueryString(dict);

        // Assert
        queryString.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(StringValueData))]
    public void ToQueryString_ShouldReturnEmptyString_WhenDictIsEmpty(Dictionary<string, string>? dict)
    {
        // Act
        string queryString = UrlHelper.ToQueryString(dict);

        // Assert
        queryString.Should().BeEmpty();
    }

    [Fact]
    public void ToQueryStringGeneric_ShouldCreateCorrectQueryString_HappyCase()
    {
        // Arrange
        Dictionary<string, int> dict = new()
        {
            { "test", 10 },
            { "random", 234 }
        };

        const string expectedResult = "test=10&random=234";

        // Act
        string queryString = UrlHelper.ToQueryString(dict);

        // Assert
        queryString.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(NumberValueData))]
    public void ToQueryStringGeneric_ShouldReturnEmptyString_WhenDictIsEmpty(Dictionary<string, int>? dict)
    {
        // Act
        string queryString = UrlHelper.ToQueryString(dict);

        // Assert
        queryString.Should().BeEmpty();
    }
}
