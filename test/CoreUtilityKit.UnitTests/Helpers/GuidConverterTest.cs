namespace CoreUtilityKit.UnitTests.Helpers;

public sealed class GuidConverterTest
{
    private static readonly Guid _guid = Guid.ParseExact("67ffbeefebe54408878333bb82dc8180", "N");

    #region Parse
    [Fact]
    public void Parse_ShouldParse_WhenInputIsValid()
    {
        // Arrange
        string base64 = ToBase64StringHelper(_guid);
        Guid expected = ParseHelper(base64);

        // Act
        Guid result = GuidConverter.Parse(base64);

        // Assert
        expected.ShouldBe(_guid);
        result.ShouldBe(_guid);
    }

    [Fact]
    public void Parse_ShouldReturnEmptyGuid_WhenInputIsEmptySpan()
    {
        // Arrange
        ReadOnlySpan<char> base64 = [];

        // Act
        Guid result = GuidConverter.Parse(base64);

        // Assert
        result.ShouldBe(Guid.Empty);
    }

    [Fact]
    public void Parse_ShouldReturnEmptyGuid_WhenInputIsANullString()
    {
        // Arrange
        string? base64 = null;

        // Act
        Guid result = GuidConverter.Parse(base64);

        // Assert
        result.ShouldBe(Guid.Empty);
    }

    [Theory]
    [InlineData("")]
    [InlineData("AAAAAAAAAAAAAAAAAAAAA")]   // 21
    [InlineData("AAAAAAAAAAAAAAAAAAAAAAA")] // 23
    public void Parse_ShouldReturnEmptyGuid_WhenInputHasNotHaveTheCorrectLength(string base64)
    {
        // Act
        Guid result = GuidConverter.Parse(base64);

        // Assert
        result.ShouldBe(Guid.Empty);
    }
    #endregion

    #region ToBase64String
    [Fact]
    public void ToBase64String_ShouldReturnBase64String_WhenInputIsValid()
    {
        // Arrange
        string expectedBase64 = ToBase64StringHelper(_guid);

        // Act
        string result = GuidConverter.ToBase64String(_guid);

        // Assert
        result.ShouldBe(expectedBase64);
    }

    [Fact]
    public void Parse_ShouldReturn22AString_WhenInputIsAnEmptyGuid()
    {
        // Arrange
        string expectedBase64 = ToBase64StringHelper(Guid.Empty);

        // Act
        string result = GuidConverter.ToBase64String(Guid.Empty);

        // Assert
        result.ShouldBe(expectedBase64);
    }
    #endregion

    private static Guid ParseHelper(string id)
    {
        byte[] base64 = Convert.FromBase64String(id
            .Replace('-', '/')
            .Replace('_', '+') + "==");

        return new(base64);
    }

    private static string ToBase64StringHelper(Guid id)
    {
        return Convert.ToBase64String(id.ToByteArray())
            .Replace('/', '-')
            .Replace('+', '_')
            .Replace("=", String.Empty, StringComparison.Ordinal);
    }
}
