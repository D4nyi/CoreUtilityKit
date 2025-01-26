namespace CoreUtilityKit.UnitTests.Helpers;

public sealed class ListExtensionTest
{
    #region IsNullOrEmpty
    #region List
    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenListIsNull()
    {
        // Arrange
        List<int>? list = null;

        // Act
        bool isNullOrEmpty = list.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenListIsEmpty()
    {
        // Arrange
        List<int> list = [];

        // Act
        bool isNullOrEmpty = list.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenListHasAtLeastOneElement()
    {
        // Arrange
        List<int> list =
        [
            1
        ];

        // Act
        bool isNullOrEmpty = list.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.Should().BeFalse();
    }
    #endregion

    #region Dictionary
    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenDictionaryIsNull()
    {
        // Arrange
        Dictionary<int, int>? dict = null;

        // Act
        bool isNullOrEmpty = dict.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenDictionaryIsEmpty()
    {
        // Arrange
        Dictionary<int, int> dict = [];

        // Act
        bool isNullOrEmpty = dict.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenDictionaryHasAtLeastOneElement()
    {
        // Arrange
        Dictionary<int, int> dict = new(1)
        {
            { 1, 1 },
        };

        // Act
        bool isNullOrEmpty = dict.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.Should().BeFalse();
    }
    #endregion

    #region Enumerable
    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenEnumerableIsNull()
    {
        // Arrange
        IEnumerable<int>? enumerable = null;

        // Act
        bool isNullOrEmpty = enumerable.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenEnumerableIsEmpty()
    {
        // Arrange
        IEnumerable<int> enumerable = [];

        // Act
        bool isNullOrEmpty = enumerable.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenEnumerableHasAtLeastOneElement()
    {
        // Arrange
        IEnumerable<int> enumerable = [1];

        // Act
        bool isNullOrEmpty = enumerable.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.Should().BeFalse();
    }
    #endregion
    #endregion

    #region EmptyIfNull
    #region List
    [Fact]
    public void EmptyIfNull_ReturnsEmptyList_WhenInputNull()
    {
        // Arrange
        List<int>? list = null;

        // Act
        List<int> emptyIfNull = list.ToEmptyIfNull();

        // Assert
        emptyIfNull.Should().NotBeNull().And.HaveCount(0);
    }

    [Fact]
    public void EmptyIfNull_ReturnTheProvidedList_WhenListIsNotNull()
    {
        // Arrange
        List<int> list = [];

        // Act
        List<int> emptyIfNull = list.ToEmptyIfNull();

        // Assert
        emptyIfNull.Should().BeSameAs(list);
    }
    #endregion

    #region Dictionary
    [Fact]
    public void EmptyIfNull_ReturnsEmptyDictionary_WhenInputNull()
    {
        // Arrange
        Dictionary<int, int>? dict = null;

        // Act
        Dictionary<int, int> emptyIfNull = dict.ToEmptyIfNull();

        // Assert
        emptyIfNull.Should().NotBeNull().And.HaveCount(0);
    }

    [Fact]
    public void EmptyIfNull_ReturnTheProvidedDictionary_WhenDictionaryIsNotNull()
    {
        // Arrange
        Dictionary<int, int> dict = [];

        // Act
        Dictionary<int, int> emptyIfNull = dict.ToEmptyIfNull();

        // Assert
        emptyIfNull.Should().BeSameAs(dict);
    }
    #endregion

    #region Enumerable
    [Fact]
    public void EmptyIfNull_ReturnsEmptyEnumerable_WhenInputNull()
    {
        // Arrange
        IEnumerable<int>? enumerable = null;

        // Act
        IEnumerable<int> emptyIfNull = enumerable.ToEmptyIfNull();

        // Assert
        emptyIfNull.Should().NotBeNull().And.HaveCount(0);
    }

    [Fact]
    public void EmptyIfNull_ReturnTheProvidedEnumerable_WhenEnumerableIsNotNull()
    {
        // Arrange
        IEnumerable<int> enumerable = [];

        // Act
        IEnumerable<int> emptyIfNull = enumerable.ToEmptyIfNull();

        // Assert
        emptyIfNull.Should().BeSameAs(enumerable);
    }
    #endregion
    #endregion

    #region GetOrAdd

    [Fact]
    public void GetOrAdd_AddsIfNotExists()
    {
        // Arrange
        Dictionary<int, int> dict = [];

        // Act
        int addedValue = dict.GetOrAdd(1, 2);

        // Assert
        addedValue.Should().Be(2);
        dict.Should().HaveCount(1);
        dict.Keys.First().Should().Be(1);
    }

    [Fact]
    public void GetOrAdd_GetsIfExists_DoesNotAdd()
    {
        // Arrange
        Dictionary<int, int> dict = new()
        {
            { 2, 3 }
        };

        // Act
        int addedValue = dict.GetOrAdd(2, 4);

        // Assert
        addedValue.Should().Be(3);
        dict.Should().HaveCount(1);
        dict.Keys.First().Should().Be(2);
    }

    [Fact]
    public void GetOrAdd_AddsIfNotExists_ValueFactory()
    {
        // Arrange
        Dictionary<int, int> dict = [];

        // Act
        int addedValue = dict.GetOrAdd(1, static a => a + 2);

        // Assert
        addedValue.Should().Be(3);
        dict.Should().HaveCount(1);
        dict.Keys.First().Should().Be(1);
    }

    [Fact]
    public void GetOrAdd_GetsIfExists_ValueFactory_DoesNotAdd()
    {
        // Arrange
        Dictionary<int, int> dict = new()
        {
            { 2, 3 }
        };

        // Act
        int addedValue = dict.GetOrAdd(2, static a => a + 2);

        // Assert
        addedValue.Should().Be(3);
        dict.Should().HaveCount(1);
        dict.Keys.First().Should().Be(2);
    }

    [Fact]
    public void GetOrAdd_AddsIfNotExists_ValueFactory_WithArgument()
    {
        // Arrange
        Dictionary<int, int> dict = [];

        // Act
        int addedValue = dict.GetOrAdd(1, static (a, b) => a + b + 2, 10);

        // Assert
        addedValue.Should().Be(13);
        dict.Should().HaveCount(1);
        dict.Keys.First().Should().Be(1);
    }

    [Fact]
    public void GetOrAdd_GetsIfExists_ValueFactory_WithArgument_DoesNotAdd()
    {
        // Arrange
        Dictionary<int, int> dict = new()
        {
            { 2, 3 }
        };

        // Act
        int addedValue = dict.GetOrAdd(2, static (a, b) => a + b + 2, 10);

        // Assert
        addedValue.Should().Be(3);
        dict.Should().HaveCount(1);
        dict.Keys.First().Should().Be(2);
    }

    #endregion

    #region GetOrAdd

    [Fact]
    public void TryUpdate_Updates_IfExists()
    {
        // Arrange
        Dictionary<int, int> dict = new()
        {
            { 2, 3 }
        };

        // Act
        bool success = dict.TryUpdate(2, 4);

        // Assert
        success.Should().BeTrue();
        dict.Should().HaveCount(1);
        dict.Keys.First().Should().Be(2);
        dict.Values.First().Should().Be(4);
    }

    [Fact]
    public void TryUpdate_DoesNotUpdates_IfNotExists()
    {
        // Arrange
        Dictionary<int, int> dict = new()
        {
            { 2, 3 }
        };

        // Act
        bool success = dict.TryUpdate(0, 4);

        // Assert
        success.Should().BeFalse();
        dict.Should().HaveCount(1);
    }

    [Fact]
    public void TryUpdate_Updates_IfExists_ValueFactory()
    {
        // Arrange
        Dictionary<int, int> dict = new()
        {
            { 2, 3 }
        };

        // Act
        bool success = dict.TryUpdate(2, static a => a + 2);

        // Assert
        success.Should().BeTrue();
        dict.Should().HaveCount(1);
        dict.Keys.First().Should().Be(2);
        dict.Values.First().Should().Be(4);
    }

    [Fact]
    public void TryUpdate_DoesNotUpdates_IfNotExists_ValueFactory()
    {
        // Arrange
        Dictionary<int, int> dict = new()
        {
            { 2, 3 }
        };

        // Act
        bool success = dict.TryUpdate(0, static a => a + 2);

        // Assert
        success.Should().BeFalse();
        dict.Should().HaveCount(1);
    }

    [Fact]
    public void TryUpdate_Updates_IfExists_ValueFactory_WithArgument()
    {
        // Arrange
        Dictionary<int, int> dict = new()
        {
            { 2, 3 }
        };

        // Act
        bool success = dict.TryUpdate(2, static (a, b) => a + b + 2, 10);

        // Assert
        success.Should().BeTrue();
        dict.Should().HaveCount(1);
        dict.Keys.First().Should().Be(2);
        dict.Values.First().Should().Be(14);
    }

    [Fact]
    public void TryUpdate_DoesNotUpdates_IfNotExists_ValueFactory_WithArgument()
    {
        // Arrange
        Dictionary<int, int> dict = new()
        {
            { 2, 3 }
        };

        // Act
        bool success = dict.TryUpdate(0, static (a, b) => a + b + 2, 10);

        // Assert
        success.Should().BeFalse();
        dict.Should().HaveCount(1);
    }
    #endregion
}
