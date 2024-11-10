using System.Diagnostics;

using CoreUtilityKit.EnumAttributionCache.UnitTests.DataGenerators;

namespace CoreUtilityKit.EnumAttributionCache.UnitTests;

public sealed class EnumAttributeReaderFactoryTests
{
    public static readonly TheoryData<EnumAttributeValue> Values = new(Enum.GetValues<EnumAttributeValue>().Where(x => x != EnumAttributeValue.None));

    public static readonly TheoryData<Color> Colors = new(Color.Red, Color.None);

    #region Factory methods

    [Theory]
    [MemberData(nameof(Values))]
    public void GetReader_HappyCase(EnumAttributeValue value)
    {
        // Arrange
        Func<Dictionary<Enum, string>> action = () => EnumAttributeReaderFactory.GenerateDictionary(value, [typeof(Color)]);

        // Act && Assert
        action
            .Should().NotThrow()
            .Subject.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(EnumAttributeValue.None)]
    [InlineData((EnumAttributeValue)32)]
    public void GetReader_ShouldThrow_WhenUnsupportedValueUsed(EnumAttributeValue value)
    {
        // Arrange
        Func<Dictionary<Enum, string>> action = () => EnumAttributeReaderFactory.GenerateDictionary(value, [typeof(Color)]);

        // Act && Assert
        action.Should().Throw<UnreachableException>()
            .WithMessage($"Value {value} is not supported!");
    }

    [Theory]
    [MemberData(nameof(Values))]
    public void GetSingleReader_HappyCase(EnumAttributeValue value)
    {
        // Arrange
        Func<Func<Enum, string?>> action = () => EnumAttributeReaderFactory.GetSingleReader(value);

        // Act && Assert
        action
            .Should().NotThrow()
            .Subject.Should().NotBeNull();
    }

    [Theory]
    [InlineData(EnumAttributeValue.None)]
    [InlineData((EnumAttributeValue)32)]
    public void GetSingleReader_ShouldThrow_WhenUnsupportedValueUsed(EnumAttributeValue value)
    {
        // Arrange
        Func<Func<Enum, string?>> action = () => EnumAttributeReaderFactory.GetSingleReader(value);

        // Act && Assert
        action.Should().Throw<UnreachableException>()
            .WithMessage($"Value {value} is not supported!");
    }

    #endregion

    #region Readers

    [Theory]
    [MemberData(nameof(Values))]
    public void GenerateDictionary_HappyCase(EnumAttributeValue attributeValue)
    {
        // Arrange
        Dictionary<Enum, string> dict = EnumAttributeReaderFactory.GenerateDictionary(attributeValue, [typeof(Color)]);

        // Assert
        dict.Should().HaveCount(4);
        dict.Should().BeEquivalentTo(ColorNames.Lookup);
    }

    [Theory]
    [ClassData(typeof(EnumAttributeAndTypesData))]
    public void GenerateDictionary_ShouldThrow_WhenInvalidArgument(EnumAttributeValue attributeValue, Type[]? types)
    {
        // Arrange
        Action action = () => _ = EnumAttributeReaderFactory.GenerateDictionary(attributeValue, types!);

        // Act && Assert
        if (types is null)
        {
            action.Should().Throw<ArgumentNullException>();
        }
        else
        {
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }

    #endregion

    #region SingleReaders

    [Theory]
    [MemberData(nameof(Colors))]
    public void GetDescription_HappyCase(Color color)
    {
        // Arrange
        Func<Enum, string?> reader = EnumAttributeReaderFactory.GetSingleReader(EnumAttributeValue.Description);

        TestSingleReader(reader, color);
    }

    [Fact]
    public void GetDescription_ShouldThrow_WhenInvalidArgument()
    {
        // Arrange
        Func<Enum, string?> reader = EnumAttributeReaderFactory.GetSingleReader(EnumAttributeValue.Description);

        TestSingleReaderThrows(reader);
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void GetEnumMemberValue_HappyCase(Color color)
    {
        // Arrange
        Func<Enum, string?> reader = EnumAttributeReaderFactory.GetSingleReader(EnumAttributeValue.EnumMemberValue);

        TestSingleReader(reader, color);
    }

    [Fact]
    public void GetEnumMemberValue_ShouldThrow_WhenInvalidArgument()
    {
        // Arrange
        Func<Enum, string?> reader = EnumAttributeReaderFactory.GetSingleReader(EnumAttributeValue.EnumMemberValue);

        TestSingleReaderThrows(reader);
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void GetDisplayName_HappyCase(Color color)
    {
        // Arrange
        Func<Enum, string?> reader = EnumAttributeReaderFactory.GetSingleReader(EnumAttributeValue.DisplayName);

        TestSingleReader(reader, color);
    }

    [Fact]
    public void GetDisplayName_ShouldThrow_WhenInvalidArgument()
    {
        // Arrange
        Func<Enum, string?> reader = EnumAttributeReaderFactory.GetSingleReader(EnumAttributeValue.DisplayName);

        TestSingleReaderThrows(reader);
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void GetDisplayDescription_HappyCase(Color color)
    {
        // Arrange
        Func<Enum, string?> reader = EnumAttributeReaderFactory.GetSingleReader(EnumAttributeValue.DisplayDescription);

        TestSingleReader(reader, color);
    }

    [Fact]
    public void GetDisplayDescription_ShouldThrow_WhenInvalidArgument()
    {
        // Arrange
        Func<Enum, string?> reader = EnumAttributeReaderFactory.GetSingleReader(EnumAttributeValue.DisplayDescription);

        TestSingleReaderThrows(reader);
    }

    #endregion

    private static void TestSingleReader(Func<Enum, string?> singleReader, Color color)
    {
        // Act
        string? result = singleReader(color);

        // Assert
        if (color != Color.None)
        {
            result.Should().Be(ColorNames.Lookup[color]);
        }
        else
        {
            result.Should().BeNull();
        }
    }

    private static void TestSingleReaderThrows(Func<Enum, string?> singleReader)
    {
        Action action = () => _ = singleReader(null!);

        // Act && Assert
        action.Should().Throw<ArgumentNullException>();
    }
}