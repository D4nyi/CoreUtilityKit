namespace CoreUtilityKit.EnumAttributionCache.UnitTests;

public sealed class EnumAttributeCacheTests
{
    private const EnumAttributeValue AttributeValue = EnumAttributeValue.Description;

    private EnumAttributeCache _cache;
    private readonly Func<Enum, string?> _singleReader;

    public EnumAttributeCacheTests()
    {
        _singleReader = EnumAttributeReaderFactory.GetSingleReader(AttributeValue);

        _cache = new EnumAttributeCache(_singleReader);
    }

    [Fact]
    public void Ctor_ShouldBeEmpty_WhenDefaultIsCalled()
    {
        // Act && Assert
        _cache.Count.ShouldBe(0);
    }

    [Fact]
    public void Ctor_ShouldInitialize_WhenEnumsProvided()
    {
        // Arrange
        Dictionary<Enum, string> dict = EnumAttributeReaderFactory.GenerateDictionary(AttributeValue, [typeof(Color)]);

        // Act
        _cache = new EnumAttributeCache(dict, _singleReader);

        // Assert
        _cache.Count.ShouldBe(4);
    }

    [Fact]
    public void Contains_ShouldReturnTrue_WhenContainsElement()
    {
        // Arrange
        _cache.TryAdd(Color.Red);

        // Act
        bool contains = _cache.ContainsKey(Color.Red);

        // Assert
        contains.ShouldBeTrue();
    }

    [Fact]
    public void Contains_ShouldReturnFalse_WhenDoesNotContainsElement()
    {
        // Act
        bool contains = _cache.ContainsKey(Color.Red);

        // Assert
        contains.ShouldBeFalse();
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrue_WhenContainsElement()
    {
        // Arrange
        _cache.TryAdd(Color.Red);

        // Act
        bool contains = _cache.TryGetValue(Color.Red, out string? description);

        // Assert
        contains.ShouldBeTrue();
        description.ShouldNotBeNull();
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrueButNullDescription_WhenElementDoesNotHaveDescription()
    {
        // Arrange
        _cache.TryAdd(Color.None);

        // Act
        bool contains = _cache.TryGetValue(Color.None, out string? description);

        // Assert
        contains.ShouldBeTrue();
        description.ShouldBeNull();
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalse_WhenDoesNotContainsElement()
    {
        // Act
        bool contains = _cache.TryGetValue(Color.Red, out string? description);

        // Assert
        contains.ShouldBeFalse();
        description.ShouldBeNull();
    }

    [Fact]
    public void TryAdd_ShouldReturnTrue_HappyCase()
    {
        // Act
        bool added = _cache.TryAdd(Color.Red);

        // Assert
        added.ShouldBeTrue();
        _cache.ContainsKey(Color.Red).ShouldBeTrue();
    }

    [Fact]
    public void TryAdd_ShouldReturnTrue_WhenNoDescription()
    {
        // Act
        bool added = _cache.TryAdd(Color.Black);

        // Assert
        added.ShouldBeTrue();
        _cache.ContainsKey(Color.Black).ShouldBeTrue();
    }

    [Fact]
    public void TryAdd_ShouldReturnFalse_WhenAlreadyExists()
    {
        // Act
        bool added1 = _cache.TryAdd(Color.Red);
        bool added2 = _cache.TryAdd(Color.Red);

        // Assert
        added1.ShouldBeTrue();
        added2.ShouldBeFalse();
        _cache.ContainsKey(Color.Red).ShouldBeTrue();
    }

    [Fact]
    public void GetOrAdd_ShouldReturnTrue_HappyCase()
    {
        // Act
        string? description1 = _cache.GetOrAdd(Color.Red);
        string? description2 = _cache.GetOrAdd(Color.Red);
        string? description3 = _cache.GetOrAdd(Color.None);

        // Assert
        description1.ShouldNotBeNull();
        description2.ShouldNotBeNull();
        description3.ShouldBeNull();
    }
}
