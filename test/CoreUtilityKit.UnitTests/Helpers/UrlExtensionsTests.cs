using CoreUtilityKit.UnitTests.DataGenerators;

namespace CoreUtilityKit.UnitTests.Helpers;

public sealed class UrlExtensionsTests
{
    #region 2 Param

    [Theory]
    [ClassData(typeof(UrlHelpers2ParamThrowsGenerator))]
    public void Combine_2Params_Throws(string path1, string path2)
    {
        // Arrange
        Action action = () => UrlExtensions.Combine(path1, path2);

        // Act & Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(UrlHelpers2ParamGenerator))]
    public void Combine_2Params_HappyCase(string path1, string path2)
    {
        // Act
        string combined = UrlExtensions.Combine(path1, path2);

        // Assert
        combined.ShouldBe("part0/part1");
    }

    [Fact]
    public void Combine_2Params_ReturnsEmptyString_WhenParametersEmpty()
    {
        // Act
        string combined = UrlExtensions.Combine("", "");

        // Assert
        combined.ShouldBe("");
    }

    [Theory]
    [InlineData("", "/v1/users", "/v1/users")]
    [InlineData("https://example.com", "", "https://example.com")]
    public void Combine_2Params_ReturnEitherPart_WhenOtherIsEmpty(string path1, string path2, string expected)
    {
        // Act
        string combined = UrlExtensions.Combine(path1, path2);

        // Assert
        combined.ShouldBe(expected);
    }

    #endregion

    #region 3 Param

    [Theory]
    [ClassData(typeof(UrlHelpers3ParamThrowsGenerator))]
    public void Combine_3Params_Throws(string path1, string path2, string path3)
    {
        // Arrange
        Action action = () => UrlExtensions.Combine(path1, path2, path3);

        // Act & Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(UrlHelpers3ParamGenerator))]
    public void Combine_3Params_HappyCase(string path1, string path2, string path3)
    {
        // Act
        string combined = UrlExtensions.Combine(path1, path2, path3);

        // Assert
        combined.ShouldBe("part0/part1/part2");
    }

    [Theory]
    [InlineData("", "/v1", "users", "/v1/users")]
    [InlineData("https://example.com", "", "users", "https://example.com/users")]
    [InlineData("https://example.com", "v1", "", "https://example.com/v1")]
    [InlineData("", "", "users", "users")]
    [InlineData("", "v1", "", "v1")]
    [InlineData("https://example.com", "", "", "https://example.com")]
    [InlineData("", "", "", "")]
    public void Combine_3Params_ReturnEitherPart_WhenOtherIsEmpty(string path1, string path2, string path3, string expected)
    {
        // Act
        string combined = UrlExtensions.Combine(path1, path2, path3);

        // Assert
        combined.ShouldBe(expected);
    }

    #endregion

    #region 4 Param

    [Theory]
    [ClassData(typeof(UrlHelpers4ParamThrowsGenerator))]
    public void Combine_4Params_Throws(string path1, string path2, string path3, string path4)
    {
        // Arrange
        Action action = () => UrlExtensions.Combine(path1, path2, path3, path4);

        // Act & Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(UrlHelpers4ParamGenerator))]
    public void Combine_4Params_HappyCase(string path1, string path2, string path3, string path4)
    {
        // Act
        string combined = UrlExtensions.Combine(path1, path2, path3, path4);

        // Assert
        combined.ShouldBe("part0/part1/part2/part3");
    }

    [Theory]
    [InlineData("", "api", "v1", "users", "api/v1/users")]
    [InlineData("https://example.com", "", "v1", "users", "https://example.com/v1/users")]
    [InlineData("https://example.com", "api", "", "users", "https://example.com/api/users")]
    [InlineData("https://example.com", "api", "v1", "", "https://example.com/api/v1")]
    [InlineData("", "", "v1", "users", "v1/users")]
    [InlineData("", "api", "", "users", "api/users")]
    [InlineData("", "api", "v1", "", "api/v1")]
    [InlineData("https://example.com", "", "", "users", "https://example.com/users")]
    [InlineData("https://example.com", "", "v1", "", "https://example.com/v1")]
    [InlineData("https://example.com", "api", "", "", "https://example.com/api")]
    [InlineData("", "", "", "users", "users")]
    [InlineData("", "", "v1", "", "v1")]
    [InlineData("", "api", "", "", "api")]
    [InlineData("https://example.com", "", "", "", "https://example.com")]
    [InlineData("", "", "", "", "")]
    public void Combine_4Params_ReturnEitherPart_WhenOtherIsEmpty(string path1, string path2, string path3, string path4, string expected)
    {
        // Act
        string combined = UrlExtensions.Combine(path1, path2, path3, path4);

        // Assert
        combined.ShouldBe(expected);
    }

    #endregion

    #region N Param

    [Theory]
    [ClassData(typeof(UrlHelpersNParamThrowsGenerator))]
    public void Combine_NParams_Throws(string[] paths)
    {
        // Arrange
        Action action = () => UrlExtensions.Combine(paths);

        // Act & Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(UrlHelpersNParamGenerator))]
    public void Combine_NParams_HappyCase(string[] paths)
    {
        // Act
        string combined = UrlExtensions.Combine(paths);

        // Assert
        combined.ShouldBe("part0/part1/part2/part3/part4");
    }

    [Theory]
    [InlineData("", "api", "v1", "users", "123", "api/v1/users/123")]
    [InlineData("https://example.com", "", "v1", "users", "123", "https://example.com/v1/users/123")]
    [InlineData("https://example.com", "api", "", "users", "123", "https://example.com/api/users/123")]
    [InlineData("https://example.com", "api", "v1", "", "123", "https://example.com/api/v1/123")]
    [InlineData("https://example.com", "api", "v1", "users", "", "https://example.com/api/v1/users")]
    [InlineData("", "", "v1", "users", "123", "v1/users/123")]
    [InlineData("", "api", "", "users", "123", "api/users/123")]
    [InlineData("", "api", "v1", "", "123", "api/v1/123")]
    [InlineData("", "api", "v1", "users", "", "api/v1/users")]
    [InlineData("https://example.com", "", "", "users", "123", "https://example.com/users/123")]
    [InlineData("https://example.com", "", "v1", "", "123", "https://example.com/v1/123")]
    [InlineData("https://example.com", "", "v1", "users", "", "https://example.com/v1/users")]
    [InlineData("https://example.com", "api", "", "", "123", "https://example.com/api/123")]
    [InlineData("https://example.com", "api", "", "users", "", "https://example.com/api/users")]
    [InlineData("https://example.com", "api", "v1", "", "", "https://example.com/api/v1")]
    [InlineData("", "", "", "users", "123", "users/123")]
    [InlineData("", "", "v1", "", "123", "v1/123")]
    [InlineData("", "", "v1", "users", "", "v1/users")]
    [InlineData("", "api", "", "users", "", "api/users")]
    [InlineData("", "api", "v1", "", "", "api/v1")]
    [InlineData("https://example.com", "", "v1", "", "", "https://example.com/v1")]
    [InlineData("https://example.com", "api", "", "", "", "https://example.com/api")]
    [InlineData("", "", "", "", "123", "123")]
    [InlineData("", "", "", "users", "", "users")]
    [InlineData("", "", "v1", "", "", "v1")]
    [InlineData("", "api", "", "", "", "api")]
    [InlineData("https://example.com", "", "", "", "", "https://example.com")]
    [InlineData("", "", "", "", "", "")]
    public void Combine_NParams_ReturnEitherPart_WhenOtherIsEmpty(string path1, string path2, string path3, string path4, string path5, string expected)
    {
        // Act
        string combined = UrlExtensions.Combine(path1, path2, path3, path4, path5);

        // Assert
        combined.ShouldBe(expected);
    }

    #endregion
}
