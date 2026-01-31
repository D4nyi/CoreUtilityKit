namespace CoreUtilityKit.Validation.UnitTests;

public sealed class GuardsThrowsTest
{
    private static readonly string[] _data = ["test", "data"];

    #region ThrowNumberOutOfRangeIf

    [Fact]
    public void ThrowNumberOutOfRangeIf_ShouldNotThrow_WhenDataIsValidAndPredicateIsFalse()
    {
        // Arrange
        const int number = 10;

        // Act
        Action action = () => number.ThrowNumberOutOfRangeIf(x => x % 2 != 0);

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void ThrowNumberOutOfRangeIf_ShouldThrow_WhenPredicateIsTrue()
    {
        // Arrange
        const int number = 10;

        // Act
        Action action = () => number.ThrowNumberOutOfRangeIf(x => x % 2 == 0);

        // Assert
        var ex = action.ShouldThrow<ArgumentOutOfRangeException>();
        ex.Message.ShouldBe($"The specified parameter is outside the processable range! (Parameter 'number'){Environment.NewLine}Actual value was {number}.");
        ex.ParamName.ShouldBe(nameof(number));
    }

    [Fact]
    public void ThrowNumberOutOfRangeIf_ShouldThrow_WhenDataIsNull()
    {
        // Arrange
        const int number = 10;

        // Act
        Action action = () => number.ThrowNumberOutOfRangeIf(null!);

        // Assert
        action
            .ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("predicate");
    }

    #endregion

    #region ThrowIf Collection Count

    [Fact]
    public void ThrowIfEmpty_ShouldNotThrow_WhenNotEmpty()
    {
        // Act
        Action action = () => _data.ThrowIfEmpty();

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void ThrowIfEmpty_ShouldThrow_WhenEmpty()
    {
        // Arrange
        List<int> list = [];

        // Act
        Action action = () => list.ThrowIfEmpty();

        // Assert
        var ex = action.ShouldThrow<ArgumentOutOfRangeException>();
        ex.Message.ShouldBe($"The collections length is out of range! (Parameter 'list'){Environment.NewLine}Actual value was 0.");
        ex.ParamName.ShouldBe(nameof(list));
    }

    [Fact]
    public void ThrowIfCountEqualTo_ShouldNotThrow_WhenNotEqual()
    {
        // Act
        Action action = () => _data.ThrowIfCountEqualTo(0);

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void ThrowIfCountEqualTo_ShouldThrow_WhenEqual()
    {
        // Act
        Action action = () => _data.ThrowIfCountEqualTo(_data.Length);

        // Assert
        var ex = action.ShouldThrow<ArgumentOutOfRangeException>();
        ex.Message.ShouldBe($"The collections length is out of range! (Parameter '_data'){Environment.NewLine}Actual value was {_data.Length}.");
        ex.ParamName.ShouldBe(nameof(_data));
    }

    [Fact]
    public void ThrowIfCountNotEqualTo_ShouldNotThrow_WhenNotEqual()
    {
        // Act
        Action action = () => _data.ThrowIfCountNotEqualTo(_data.Length);

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void ThrowIfCountNotEqualTo_ShouldThrow_WhenEqualTo()
    {
        // Act
        Action action = () => _data.ThrowIfCountNotEqualTo(0);

        // Assert
        var ex = action.ShouldThrow<ArgumentOutOfRangeException>();
        ex.Message.ShouldBe($"The collections length is out of range! (Parameter '_data'){Environment.NewLine}Actual value was {_data.Length}.");
        ex.ParamName.ShouldBe(nameof(_data));
    }

    [Fact]
    public void ThrowIfCountIsGreaterThen_ShouldNotThrow_WhenIsNotGreaterThen()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsGreaterThen(2);

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void ThrowIfCountIsGreaterThen_ShouldThrow_WhenIsGreaterThen()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsGreaterThen(1);

        // Assert
        var ex = action.ShouldThrow<ArgumentOutOfRangeException>();
        ex.Message.ShouldBe($"The collections length is out of range! (Parameter '_data'){Environment.NewLine}Actual value was {_data.Length}.");
        ex.ParamName.ShouldBe(nameof(_data));
    }

    [Fact]
    public void ThrowIfCountIsLessThen_ShouldNotThrow_WhenIsNotLessThen()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsLessThen(2);

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void ThrowIfCountIsLessThen_ShouldThrow_WhenIsLessThen()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsLessThen(3);

        // Assert
        var ex = action.ShouldThrow<ArgumentOutOfRangeException>();
        ex.Message.ShouldBe($"The collections length is out of range! (Parameter '_data'){Environment.NewLine}Actual value was {_data.Length}.");
        ex.ParamName.ShouldBe(nameof(_data));
    }

    [Fact]
    public void ThrowIfCountIsGreaterThenOrEqualTo_ShouldNotThrow_WhenIsNotGreaterThenOrEqualTo()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsGreaterThenOrEqualTo(3);

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void ThrowIfCountIsGreaterThenOrEqualTo_ShouldThrow_WhenIsGreaterThenOrEqualTo()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsGreaterThenOrEqualTo(2);

        // Assert
        var ex = action.ShouldThrow<ArgumentOutOfRangeException>();
        ex.Message.ShouldBe($"The collections length is out of range! (Parameter '_data'){Environment.NewLine}Actual value was {_data.Length}.");
        ex.ParamName.ShouldBe(nameof(_data));
    }

    [Fact]
    public void ThrowIfCountIsLessThenOrEqualTo_ShouldNotThrow_WhenIsNotLessThenOrEqualTo()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsLessThenOrEqualTo(1);

        // Assert
        action.ShouldNotThrow();
    }

    [Fact]
    public void ThrowIfCountIsLessThenOrEqualTo_ShouldThrow_WhenIsLessThenOrEqualTo()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsLessThenOrEqualTo(3);

        // Assert
        var ex = action.ShouldThrow<ArgumentOutOfRangeException>();
        ex.Message.ShouldBe($"The collections length is out of range! (Parameter '_data'){Environment.NewLine}Actual value was {_data.Length}.");
        ex.ParamName.ShouldBe(nameof(_data));
    }

    #endregion
}
