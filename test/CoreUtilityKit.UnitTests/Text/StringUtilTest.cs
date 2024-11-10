using System.Text;

using CoreUtilityKit.Text;

namespace CoreUtilityKit.UnitTests.Text;

public sealed class StringUtilTest
{
    private static readonly string[] _invalidChars = Path.GetInvalidFileNameChars().Select(x => x.ToString()).ToArray();

    #region InlineData
    [Theory]
    [InlineData("StringToBeNormalized")]
    [InlineData("StringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalized")]
    #endregion
    public void NormalizeToUpper_ShouldNormalize_WhenInputIsValid(string value)
    {
        // Act
        string normalizedString = value.NormalizeToUpper();
        bool onlyAscii = Encoding.UTF8.GetByteCount(normalizedString) == normalizedString.Length;
        bool onlyUpper = normalizedString.All(Char.IsUpper);

        // Assert
        onlyAscii.Should().BeTrue();
        onlyUpper.Should().BeTrue();
        _ = normalizedString.Should().HaveLength(value.Length);
    }

    [Fact]
    public void NormalizeToUpper_ShouldNormalize_EmailCorrectly()
    {
        // Arrange
        const string email = "test@email.com";
        const string expectedOutput = "TEST@EMAIL.COM";

        // Act
        string normalizedString = email.NormalizeToUpper();

        // Assert
        normalizedString.Should().Be(expectedOutput);
    }

    #region InlineData
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t")]
    #endregion
    public void NormalizeToUpper_ShouldReturnEmptyString_WhenInvalidInput(string? input)
    {
        // Arrange && Act
        Action act = () => input.NormalizeToUpper().Should().BeEmpty();

        // Assert
        act.Should().NotThrow();
    }

    #region InlineData
    [Theory]
    [InlineData("StringToBeNormalized")]
    [InlineData("StringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalized")]
    #endregion
    public void NormalizeToLower_ShouldNormalize_WhenInputIsValid(string value)
    {
        // Act
        string normalizedString = value.NormalizeToLower();
        bool onlyAscii = Encoding.UTF8.GetByteCount(normalizedString) == normalizedString.Length;
        bool onlyLower = normalizedString.All(Char.IsLower);

        // Assert
        onlyAscii.Should().BeTrue();
        onlyLower.Should().BeTrue();
        _ = normalizedString.Should().HaveLength(value.Length);
    }

    [Fact]
    public void NormalizeToLower_ShouldNormalize_EmailCorrectly()
    {
        // Arrange
        const string email= "TEST@EMAIL.COM";
        const string expectedOutput = "test@email.com";

        // Act
        string normalizedString = email.NormalizeToLower();

        // Assert
        normalizedString.Should().Be(expectedOutput);
    }

    #region InlineData
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t")]
    #endregion
    public void NormalizeToLower_ShouldReturnEmptyString_WhenInvalidInput(string? input)
    {
        // Arrange && Act
        Action act = () => input.NormalizeToLower().Should().BeEmpty();

        // Assert
        act.Should().NotThrow();
    }

    #region InlineData
    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData(" \t", "")]
    [InlineData(" Test Data ÁÉŰ ", "test_data_aeu")]
    [InlineData("  Test Data ÁÉŰ ", "test_data_aeu")]
    [InlineData(" _Test Data ÁÉŰ ", "test_data_aeu")]
    [InlineData(" Test Data ÁÉŰ  ", "test_data_aeu")]
    [InlineData(" Test  Data_ ÁÉŰ ", "test_data_aeu")]
    [InlineData("  Test  Data  ÁÉŰ_ ", "test_data_aeu")]
    [InlineData("Test::Data::ÁÉŰ", "test_data_aeu")]
    #endregion
    public void CreateSlug_HappyCase(string? value, string expected)
    {
        // Act
        string slug = value.CreateSlug();

        // Assert
        slug.Should().Be(expected);
    }

    [Fact]
    public void ReplaceInvalidFileChars_ShouldReplace()
    {
        // Arrange
        const string value = "path?which*has:invalid|chars";

        // Act
        string safePath = value.ReplaceInvalidFileChars();

        // Assert
        safePath.Should().NotContainAny(_invalidChars);
    }

    #region InlineData
    [Theory]
    [InlineData("path?which*has:invalid|chars")]
    [InlineData("PATH?WHICH*HAS:INVALID|CHARS")]
    [InlineData("path?WHICH*has:INVALID|chars")]
    [InlineData("stringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalizedStringToBeNormalized")]
    #endregion
    public void ReplaceInvalidFileChars_ShouldReturnEmptyString_WhenInputIsEmpty(string input)
    {
        // Act
        string safePath = input.ReplaceInvalidFileChars();

        bool onlyAscii = Encoding.UTF8.GetByteCount(safePath) == safePath.Length;

        // Assert
        onlyAscii.Should().BeTrue();
        safePath.Should().NotContainAny(_invalidChars);
    }

    #region InlineData
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t")]
    #endregion
    public void ReplaceInvalidFileChars_ShouldReturnEmptyString_WhenInvalidInput(string? input)
    {
        // Arrange && Act
        Action act = () => input.ReplaceInvalidFileChars().Should().BeEmpty();

        // Assert
        act.Should().NotThrow();
    }
}
