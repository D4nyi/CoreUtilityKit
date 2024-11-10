using System.Text.RegularExpressions;

namespace CoreUtilityKit.EnumAttributionCache.UnitTests;

public sealed class ReadonlyEnumAttributeCacheTests
{
    private const EnumAttributeValue AttributeValue = EnumAttributeValue.Description;

    private readonly ReadonlyEnumAttributeCache _cache;

    public ReadonlyEnumAttributeCacheTests()
    {
        Dictionary<Enum, string> dict = EnumAttributeReaderFactory.GenerateDictionary(AttributeValue, [typeof(Color)]);

        _cache = new ReadonlyEnumAttributeCache(dict);
    }

    [Fact]
    public void ShouldHaveCount4()
    {
        _cache.Count.Should().Be(4);
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenNullProvided()
    {
        // Act
        Action action = () => _ = new ReadonlyEnumAttributeCache(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenNoAttributeDataIsFound()
    {
        // Act
        Action action = () => _ = new ReadonlyEnumAttributeCache([]);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Readonly cache must not be empty! (Parameter 'dict')\r\nActual value was 0.")
            .WithParameterName("dict");
    }

    [Fact]
    public void Contains_ShouldReturnTrue_WhenContainsElement()
    {
        // Act
        bool contains = _cache.ContainsKey(Color.Red);

        // Assert
        contains.Should().BeTrue();
    }

    [Fact]
    public void Contains_ShouldReturnFalse_WhenDoesNotContainsElement()
    {
        // Act
        bool contains = _cache.ContainsKey(Color.None);

        // Assert
        contains.Should().BeFalse();
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalse_WhenDoesNotContainsElement()
    {
        // Act
        bool contains = _cache.TryGetValue(RegexOptions.None, out string? description);

        // Assert
        contains.Should().BeFalse();
        description.Should().BeNull();
    }
}
