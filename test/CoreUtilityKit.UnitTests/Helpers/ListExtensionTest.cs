using CoreUtilityKit.UnitTests.DataGenerators;

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
        isNullOrEmpty.ShouldBeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenListIsEmpty()
    {
        // Arrange
        List<int> list = [];

        // Act
        bool isNullOrEmpty = list.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.ShouldBeTrue();
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
        isNullOrEmpty.ShouldBeFalse();
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
        isNullOrEmpty.ShouldBeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenDictionaryIsEmpty()
    {
        // Arrange
        Dictionary<int, int> dict = [];

        // Act
        bool isNullOrEmpty = dict.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.ShouldBeTrue();
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
        isNullOrEmpty.ShouldBeFalse();
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
        isNullOrEmpty.ShouldBeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenEnumerableIsEmpty()
    {
        // Arrange
        IEnumerable<int> enumerable = [];

        // Act
        bool isNullOrEmpty = enumerable.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.ShouldBeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenEnumerableHasAtLeastOneElement()
    {
        // Arrange
        IEnumerable<int> enumerable = [1];

        // Act
        bool isNullOrEmpty = enumerable.IsNullOrEmpty();

        // Assert
        isNullOrEmpty.ShouldBeFalse();
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
        emptyIfNull.ShouldNotBeNull().ShouldBeEmpty();
    }

    [Fact]
    public void EmptyIfNull_ReturnTheProvidedList_WhenListIsNotNull()
    {
        // Arrange
        List<int> list = [];

        // Act
        List<int> emptyIfNull = list.ToEmptyIfNull();

        // Assert
        emptyIfNull.ShouldBeSameAs(list);
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
        emptyIfNull.ShouldNotBeNull().ShouldBeEmpty();
    }

    [Fact]
    public void EmptyIfNull_ReturnTheProvidedDictionary_WhenDictionaryIsNotNull()
    {
        // Arrange
        Dictionary<int, int> dict = [];

        // Act
        Dictionary<int, int> emptyIfNull = dict.ToEmptyIfNull();

        // Assert
        emptyIfNull.ShouldBeSameAs(dict);
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
        emptyIfNull.ShouldNotBeNull().ShouldBeEmpty();
    }

    [Fact]
    public void EmptyIfNull_ReturnTheProvidedEnumerable_WhenEnumerableIsNotNull()
    {
        // Arrange
        IEnumerable<int> enumerable = [];

        // Act
        IEnumerable<int> emptyIfNull = enumerable.ToEmptyIfNull();

        // Assert
        emptyIfNull.ShouldBeSameAs(enumerable);
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
        addedValue.ShouldBe(2);
        dict.ShouldHaveSingleItem();
        dict.Keys.First().ShouldBe(1);
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
        addedValue.ShouldBe(3);
        dict.ShouldHaveSingleItem();
        dict.Keys.First().ShouldBe(2);
    }

    [Fact]
    public void GetOrAdd_AddsIfNotExists_ValueFactory()
    {
        // Arrange
        Dictionary<int, int> dict = [];

        // Act
        int addedValue = dict.GetOrAdd(1, static a => a + 2);

        // Assert
        addedValue.ShouldBe(3);
        dict.ShouldHaveSingleItem();
        dict.Keys.First().ShouldBe(1);
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
        addedValue.ShouldBe(3);
        dict.ShouldHaveSingleItem();
        dict.Keys.First().ShouldBe(2);
    }

    [Fact]
    public void GetOrAdd_AddsIfNotExists_ValueFactory_WithArgument()
    {
        // Arrange
        Dictionary<int, int> dict = [];

        // Act
        int addedValue = dict.GetOrAdd(1, static (a, b) => a + b + 2, 10);

        // Assert
        addedValue.ShouldBe(13);
        dict.ShouldHaveSingleItem();
        dict.Keys.First().ShouldBe(1);
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
        addedValue.ShouldBe(3);
        dict.ShouldHaveSingleItem();
        dict.Keys.First().ShouldBe(2);
    }

    #endregion

    #region TryUpdate

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
        success.ShouldBeTrue();
        dict.ShouldHaveSingleItem();
        dict.Keys.First().ShouldBe(2);
        dict.Values.First().ShouldBe(4);
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
        success.ShouldBeFalse();
        dict.ShouldHaveSingleItem();
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
        success.ShouldBeTrue();
        dict.ShouldHaveSingleItem();
        dict.Keys.First().ShouldBe(2);
        dict.Values.First().ShouldBe(4);
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
        success.ShouldBeFalse();
        dict.ShouldHaveSingleItem();
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
        success.ShouldBeTrue();
        dict.ShouldHaveSingleItem();
        dict.Keys.First().ShouldBe(2);
        dict.Values.First().ShouldBe(14);
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
        success.ShouldBeFalse();
        dict.ShouldHaveSingleItem();
    }
    #endregion

    #region EquivalentTo

    [Theory]
    [ClassData(typeof(DictionaryEqualGenerator))]
    public void DictionaryEqual_HappyCase(Dictionary<int, int> first, Dictionary<int, int> second, bool expected)
    {
        // Act
        bool success = first.EquivalentTo(second);

        // Assert
        success.ShouldBe(expected);
    }

    #endregion
}
