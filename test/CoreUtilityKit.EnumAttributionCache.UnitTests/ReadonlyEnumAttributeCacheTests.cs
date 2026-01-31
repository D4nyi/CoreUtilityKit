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
        _cache.Count.ShouldBe(4);
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenNullProvided()
    {
        // Act
        Action action = () => _ = new ReadonlyEnumAttributeCache(null!);

        // Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenNoAttributeDataIsFound()
    {
        // Act
        Action action = () => _ = new ReadonlyEnumAttributeCache([]);

        // Assert
        var ex = action.ShouldThrow<ArgumentOutOfRangeException>();
        ex.Message.ShouldBe($"Readonly cache must not be empty! (Parameter 'dict'){Environment.NewLine}Actual value was 0.");
        ex.ParamName.ShouldBe("dict");
    }

    [Fact]
    public void Contains_ShouldReturnTrue_WhenContainsElement()
    {
        // Act
        bool contains = _cache.ContainsKey(Color.Red);

        // Assert
        contains.ShouldBeTrue();
    }

    [Fact]
    public void Contains_ShouldReturnFalse_WhenDoesNotContainsElement()
    {
        // Act
        bool contains = _cache.ContainsKey(Color.None);

        // Assert
        contains.ShouldBeFalse();
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalse_WhenDoesNotContainsElement()
    {
        // Act
        bool contains = _cache.TryGetValue(RegexOptions.None, out string? description);

        // Assert
        contains.ShouldBeFalse();
        description.ShouldBeNull();
    }
}
