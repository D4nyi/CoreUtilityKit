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
        _cache.Count.Should().Be(0);
    }

    [Fact]
    public void Ctor_ShouldInitialize_WhenEnumsProvided()
    {
        // Arrange
        Dictionary<Enum, string> dict = EnumAttributeReaderFactory.GenerateDictionary(AttributeValue, [typeof(Color)]);

        // Act
        _cache = new EnumAttributeCache(dict, _singleReader);

        // Assert
        _cache.Count.Should().Be(4);
    }

    [Fact]
    public void Contains_ShouldReturnTrue_WhenContainsElement()
    {
        // Arrange
        _cache.TryAdd(Color.Red);

        // Act
        bool contains = _cache.ContainsKey(Color.Red);

        // Assert
        contains.Should().BeTrue();
    }

    [Fact]
    public void Contains_ShouldReturnFalse_WhenDoesNotContainsElement()
    {
        // Act
        bool contains = _cache.ContainsKey(Color.Red);

        // Assert
        contains.Should().BeFalse();
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrue_WhenContainsElement()
    {
        // Arrange
        _cache.TryAdd(Color.Red);

        // Act
        bool contains = _cache.TryGetValue(Color.Red, out string? description);

        // Assert
        contains.Should().BeTrue();
        description.Should().NotBeNull();
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrueButNullDescription_WhenElementDoesNotHaveDescription()
    {
        // Arrange
        _cache.TryAdd(Color.None);

        // Act
        bool contains = _cache.TryGetValue(Color.None, out string? description);

        // Assert
        contains.Should().BeTrue();
        description.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalse_WhenDoesNotContainsElement()
    {
        // Act
        bool contains = _cache.TryGetValue(Color.Red, out string? description);

        // Assert
        contains.Should().BeFalse();
        description.Should().BeNull();
    }

    [Fact]
    public void TryAdd_ShouldReturnTrue_HappyCase()
    {
        // Act
        bool added = _cache.TryAdd(Color.Red);

        // Assert
        added.Should().BeTrue();
        _cache.ContainsKey(Color.Red).Should().BeTrue();
    }

    [Fact]
    public void TryAdd_ShouldReturnTrue_WhenNoDescription()
    {
        // Act
        bool added = _cache.TryAdd(Color.Black);

        // Assert
        added.Should().BeTrue();
        _cache.ContainsKey(Color.Black).Should().BeTrue();
    }

    [Fact]
    public void TryAdd_ShouldReturnFalse_WhenAlreadyExists()
    {
        // Act
        bool added1 = _cache.TryAdd(Color.Red);
        bool added2 = _cache.TryAdd(Color.Red);

        // Assert
        added1.Should().BeTrue();
        added2.Should().BeFalse();
        _cache.ContainsKey(Color.Red).Should().BeTrue();
    }

    [Fact]
    public void GetOrAdd_ShouldReturnTrue_HappyCase()
    {
        // Act
        string? description1 = _cache.GetOrAdd(Color.Red);
        string? description2 = _cache.GetOrAdd(Color.Red);
        string? description3 = _cache.GetOrAdd(Color.None);

        // Assert
        description1.Should().NotBeNull();
        description2.Should().NotBeNull();
        description3.Should().BeNull();
    }
}
