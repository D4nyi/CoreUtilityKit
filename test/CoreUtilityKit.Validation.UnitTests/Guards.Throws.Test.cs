namespace CoreUtilityKit.Validation.UnitTests;

public sealed class GuardsThrowsTest
{
    private static readonly string[] _data = [ "test", "data" ];

    #region ThrowNumberOutOfRangeIf
    [Fact]
    public void ThrowNumberOutOfRangeIf_ShouldNotThrow_WhenDataIsValidAndPredicateIsFalse()
    {
        // Arrange
        const int number = 10;

        // Act
        Action action = () => number.ThrowNumberOutOfRangeIf(x => x % 2 != 0);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void ThrowNumberOutOfRangeIf_ShouldThrow_WhenPredicateIsTrue()
    {
        // Arrange
        const int number = 10;

        // Act
        Action action = () => number.ThrowNumberOutOfRangeIf(x => x % 2 == 0);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"The specified parameter is outside the processable range! (Parameter 'number')\r\nActual value was {number}.")
            .WithParameterName(nameof(number));
    }

    [Fact]
    public void ThrowNumberOutOfRangeIf_ShouldThrow_WhenDataIsNull()
    {
        // Arrange
        const int number = 10;

        // Act
        Action action = () => number.ThrowNumberOutOfRangeIf(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("predicate");
    }
    #endregion

    #region ThrowIf Collection Count
    [Fact]
    public void ThrowIfEmpty_ShouldNotThrow_WhenNotEmpty()
    {
        // Act
        Action action = () => _data.ThrowIfEmpty();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void ThrowIfEmpty_ShouldThrow_WhenEmpty()
    {
        // Arrange
        List<int> list = [];

        // Act
        Action action = () => list.ThrowIfEmpty();

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("The collections length is out of range! (Parameter 'list')\r\nActual value was 0.")
            .WithParameterName(nameof(list));
    }

    [Fact]
    public void ThrowIfCountEqualTo_ShouldNotThrow_WhenNotEqual()
    {
        // Act
        Action action = () => _data.ThrowIfCountEqualTo(0);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void ThrowIfCountEqualTo_ShouldThrow_WhenEqual()
    {
        // Act
        Action action = () => _data.ThrowIfCountEqualTo(_data.Length);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"The collections length is out of range! (Parameter '_data')\r\nActual value was {_data.Length}.")
            .WithParameterName(nameof(_data));
    }

    [Fact]
    public void ThrowIfCountNotEqualTo_ShouldNotThrow_WhenNotEqual()
    {
        // Act
        Action action = () => _data.ThrowIfCountNotEqualTo(_data.Length);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void ThrowIfCountNotEqualTo_ShouldThrow_WhenEqualTo()
    {
        // Act
        Action action = () => _data.ThrowIfCountNotEqualTo(0);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"The collections length is out of range! (Parameter '_data')\r\nActual value was {_data.Length}.")
            .WithParameterName(nameof(_data));
    }

    [Fact]
    public void ThrowIfCountIsGreaterThen_ShouldNotThrow_WhenIsNotGreaterThen()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsGreaterThen(2);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void ThrowIfCountIsGreaterThen_ShouldThrow_WhenIsGreaterThen()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsGreaterThen(1);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"The collections length is out of range! (Parameter '_data')\r\nActual value was {_data.Length}.")
            .WithParameterName(nameof(_data));
    }

    [Fact]
    public void ThrowIfCountIsLessThen_ShouldNotThrow_WhenIsNotLessThen()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsLessThen(2);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void ThrowIfCountIsLessThen_ShouldThrow_WhenIsLessThen()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsLessThen(3);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"The collections length is out of range! (Parameter '_data')\r\nActual value was {_data.Length}.")
            .WithParameterName(nameof(_data));
    }

    [Fact]
    public void ThrowIfCountIsGreaterThenOrEqualTo_ShouldNotThrow_WhenIsNotGreaterThenOrEqualTo()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsGreaterThenOrEqualTo(3);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void ThrowIfCountIsGreaterThenOrEqualTo_ShouldThrow_WhenIsGreaterThenOrEqualTo()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsGreaterThenOrEqualTo(2);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"The collections length is out of range! (Parameter '_data')\r\nActual value was {_data.Length}.")
            .WithParameterName(nameof(_data));
    }

    [Fact]
    public void ThrowIfCountIsLessThenOrEqualTo_ShouldNotThrow_WhenIsNotLessThenOrEqualTo()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsLessThenOrEqualTo(1);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void ThrowIfCountIsLessThenOrEqualTo_ShouldThrow_WhenIsLessThenOrEqualTo()
    {
        // Act
        Action action = () => _data.ThrowIfCountIsLessThenOrEqualTo(3);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"The collections length is out of range! (Parameter '_data')\r\nActual value was {_data.Length}.")
            .WithParameterName(nameof(_data));
    }
    #endregion
}
