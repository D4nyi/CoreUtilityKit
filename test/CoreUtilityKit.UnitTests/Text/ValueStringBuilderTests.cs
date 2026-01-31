using System.Globalization;
using System.Text;

using CoreUtilityKit.Text;

namespace CoreUtilityKit.UnitTests.Text;

public sealed class ValueStringBuilderTests
{
    [Fact]
    public void CtorInitBuffer_HappyCase()
    {
        // Arrange
        const int capacity = 2;
        Span<char> initBuffer = stackalloc char[capacity];

        // Act
        ValueStringBuilder sb = new(initBuffer);

        // Assert
        sb.Capacity.ShouldBe(capacity);
    }

    [Fact]
    public void CtorInitCapacity_HappyCase()
    {
        // Arrange
        const int capacity = 2;

        // Act
        ValueStringBuilder sb = new(capacity);

        // Assert
        sb.Capacity.ShouldBeGreaterThanOrEqualTo(capacity);
    }

    [Fact]
    public void LengthSet_ShouldSetPosition_HappyCase()
    {
        // Arrange
        const int capacity = 2;
        Span<char> initBuffer = stackalloc char[capacity];
        ValueStringBuilder sb = new(initBuffer);

        // Act
        sb.Length++;

        // Assert
        sb.Length.ShouldBe(1);
        sb.Capacity.ShouldBe(capacity);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void LengthSet_ShouldThrow_WhenOutOfRange(int length)
    {
        // Arrange
        Action action = () =>
        {
            _ = new ValueStringBuilder { Length = length };
        };

        // Act && Assert
        action
            .ShouldThrow<ArgumentOutOfRangeException>()
            .ParamName.ShouldBe("value");
    }

    [Fact]
    public void Indexer_HappyCase()
    {
        // Arrange
        const int capacity = 3;

        ValueStringBuilder sb = new(capacity);
        sb.Append('a');
        sb.Append('b');
        sb.Append('c');

        // Act && Assert
        sb[1].ShouldBe('b');
    }

    [Fact]
    public void Indexer_ShouldThrow_When()
    {
        // Arrange
        Action action = () =>
        {
            ValueStringBuilder sb = new();
            _ = sb[1];
        };

        // Act && Assert
        action
            .ShouldThrow<ArgumentOutOfRangeException>()
            .ParamName.ShouldBe("index");
    }

    [Theory]
    [InlineData(2, false)]
    [InlineData(20, true)]
    public void EnsureCapacity_HappyCase(int capacity, bool sizeShouldChange)
    {
        // Arrange
        const int initCapacity = 2;
        ValueStringBuilder sb = new(initCapacity);

        int beforeEnsureCapacity = sb.Capacity;

        // Act
        sb.EnsureCapacity(capacity);

        // Assert
        sb.Capacity.ShouldBeGreaterThanOrEqualTo(capacity);
        if (sizeShouldChange)
        {
            sb.Capacity.ShouldBeGreaterThan(beforeEnsureCapacity);
        }
    }

    [Fact]
    public void EnsureCapacity_ShouldThrow_WhenNegative()
    {
        // Arrange
        Action action = () =>
        {
            ValueStringBuilder sb = new();
            sb.EnsureCapacity(-1);
        };

        // Act && Assert
        action
            .ShouldThrow<ArgumentOutOfRangeException>()
            .ParamName.ShouldBe("capacity");
    }

    [Fact]
    public void GetPinnableReference_HappyCase()
    {
        // Arrange
        ValueStringBuilder sb = new(2);
        sb.Append('a');

        // Act && Assert
        char ch = sb.GetPinnableReference();
        ch.ShouldBe('a');
    }

    [Fact]
    public void GetPinnableReferenceTerminate_HappyCase()
    {
        // Arrange
        ValueStringBuilder sb = new(2);
        sb.Length = sb.Capacity;

        int beforeAct = sb.Capacity;

        // Act
        _ = sb.GetPinnableReference(true);

        // Assert
        sb.Capacity.ShouldBeGreaterThan(beforeAct);
    }

    [Fact]
    public void ToString_HappyCase()
    {
        // Arrange
        const int capacity = 3;

        ValueStringBuilder sb = new(capacity);
        sb.Append('a');
        sb.Append('b');
        sb.Append('c');

        // Act && Assert
        sb.ToString().ShouldBe("abc");
    }

    [Fact]
    public void RawChars_HappyCase()
    {
        // Arrange
        Span<char> initBuf = stackalloc char[1];
        ValueStringBuilder sb = new(initBuf);
        sb.Append('a');

        // Act
        Span<char> raw = sb.RawChars;

        // Assert
        raw.Length.ShouldBe(sb.Length);
        raw[0].ShouldBe(sb[0]);
    }

    [Fact]
    public void AsSpan_HappyCase()
    {
        // Arrange
        Span<char> initBuf = stackalloc char[5];
        ValueStringBuilder sb = new(initBuf);
        sb.Append('a');
        sb.Append('b');
        sb.Append('c');
        sb.Append('d');
        sb.Append('e');
        sb.Length--;

        // Act
        ReadOnlySpan<char> span1 = sb.AsSpan();
        ReadOnlySpan<char> span2 = sb.AsSpan(1);
        ReadOnlySpan<char> span3 = sb.AsSpan(2, 2);
        ReadOnlySpan<char> span4 = sb.AsSpan(true);
        sb.Length++;

        // Assert
        span1.Length.ShouldBe(sb.Length - 1);
        span2.Length.ShouldBe(sb.Length - 2);
        span3.Length.ShouldBe(2);
        span4.Length.ShouldBe(sb.Length - 1);
    }

    [Fact]
    public void TryCopyTo_HappyCase()
    {
        // Arrange
        const int capacity = 3;
        Span<char> init = stackalloc char[capacity];

        ValueStringBuilder sb = new(init);
        sb.Append('a');
        sb.Append('b');
        sb.Append('c');

        Span<char> dest = stackalloc char[capacity];

        // Act
        bool success = sb.TryCopyTo(dest, out int charsWritten);

        // Assert
        success.ShouldBeTrue();
        charsWritten.ShouldBe(capacity);
    }

    [Fact]
    public void TryCopyTo_ReturnsFalse()
    {
        // Arrange
        const int capacity = 3;
        Span<char> init = stackalloc char[capacity];

        ValueStringBuilder sb = new(init);
        sb.Append('a');
        sb.Append('b');
        sb.Append('c');

        Span<char> dest = stackalloc char[2];

        // Act
        bool success = sb.TryCopyTo(dest, out int charsWritten);

        // Assert
        success.ShouldBeFalse();
        charsWritten.ShouldBe(0);
    }

    [Fact]
    public void InsertChar_HappyCase()
    {
        // Arrange
        const int capacity = 2;
        Span<char> init = stackalloc char[capacity];

        ValueStringBuilder sb = new(init);
        sb.Append('b');
        sb.Append('c');

        // Act
        sb.Insert(0, 'a', 1);

        // Assert
        sb.Length.ShouldBeGreaterThan(capacity);
        sb[0].ShouldBe('a');
    }

    [Theory]
    [InlineData(null)]
    [InlineData("ab")]
    public void InsertString_HappyCase(string? s)
    {
        // Arrange
        const int capacity = 1;
        Span<char> init = stackalloc char[capacity];

        ValueStringBuilder sb = new(init);
        sb.Append('c');

        // Act
        sb.Insert(0, s);

        // Assert
        if (s is null)
        {
            sb.Capacity.ShouldBe(capacity);
        }
        else
        {
            sb.Length.ShouldBe(3);
            sb[0].ShouldBe('a');
            sb[1].ShouldBe('b');
        }
    }

    [Fact]
    public void AppendChar_HappyCase()
    {
        // Arrange
        const int capacity = 2;
        Span<char> init = stackalloc char[capacity];

        ValueStringBuilder sb = new(init);
        sb.Append('a');
        sb.Append('b');

        // Act
        sb.Append('c');

        // Assert
        sb.Length.ShouldBeGreaterThan(capacity);
        sb[^1].ShouldBe('c');
    }

    [Fact]
    public void AppendCharCount_HappyCase()
    {
        // Arrange
        const int capacity = 2;
        Span<char> init = stackalloc char[capacity];

        ValueStringBuilder sb = new(init);
        sb.Append('a');
        sb.Append('b');

        // Act
        sb.Append('c', 3);

        // Assert
        sb.Length.ShouldBeGreaterThan(capacity);
        sb[^3].ShouldBe('c');
        sb[^2].ShouldBe('c');
        sb[^1].ShouldBe('c');
    }

    [Theory]
    [InlineData(null)]
    [InlineData("b")]
    [InlineData("bc")]
    public void AppendString_HappyCase(string? s)
    {
        // Arrange
        const int capacity = 2;
        Span<char> init = stackalloc char[capacity];

        ValueStringBuilder sb = new(init);
        sb.Append('a');

        // Act
        sb.Append(s);

        // Assert
        if (s is null)
        {
            sb.Capacity.ShouldBe(capacity);
        }
        else if (s.Length == 1)
        {
            sb.Length.ShouldBe(capacity);
            sb[^1].ShouldBe('b');
        }
        else
        {
            sb.Length.ShouldBeGreaterThan(capacity);
            sb[^2].ShouldBe('b');
            sb[^1].ShouldBe('c');
        }
    }

    [Fact]
    public void AppendReadOnlySpan_HappyCase()
    {
        // Arrange
        const int capacity = 2;
        Span<char> init = stackalloc char[capacity];

        ValueStringBuilder sb = new(init);
        sb.Append('a');
        sb.Append('b');

        ReadOnlySpan<char> s = "cd";

        // Act
        sb.Append(s);

        // Assert
        sb.Length.ShouldBeGreaterThan(capacity);
        sb[^1].ShouldBe('d');
    }

    [Fact]
    public void AppendSpan_HappyCase()
    {
        // Arrange
        const int capacity = 2;
        Span<char> init = stackalloc char[capacity];

        ValueStringBuilder sb = new(init);
        sb.Append('a');
        sb.Append('b');

        // Act
        Span<char> span = sb.AppendSpan(capacity);

        // Assert
        span.Length.ShouldBe(capacity);
        sb.Length.ShouldBeGreaterThanOrEqualTo(capacity * 2);
    }

    [Fact]
    public void AppendSpanFormattable_HappyCase()
    {
        // Arrange
        ValueStringBuilder sb = new();

        // Act
        sb.AppendSpanFormattable(10L);
        sb.AppendSpanFormattable(10L, "d");
        sb.AppendSpanFormattable(10L, "d", CultureInfo.InvariantCulture);

        sb.AppendSpanFormattable(10);
        sb.AppendSpanFormattable(10, "d");
        sb.AppendSpanFormattable(10, "d", CultureInfo.InvariantCulture);

        // Assert
        sb.ToString().ShouldBe("101010101010");
    }

    [Fact]
    public void AppendRune_HappyCase()
    {
        // Arrange
        Span<char> buff = stackalloc char[3];
        ValueStringBuilder sb = new(buff);

        // Act
        sb.Append(new Rune('é'));
        sb.Append(new Rune(65536));

        sb.Append(' ');
        int f = sb.Capacity - sb.Length - 1;
        for (int i = 0; i < f; i++)
        {
            sb.Append(' ');
        }

        sb.Append(new Rune('á'));
        sb.Append(new Rune(65536));

        // Assert
        sb.ToString().ShouldBe("é\ud800\udc00            á\ud800\udc00");
    }

    [Theory]
    [InlineData(1, 1, "ac")]
    [InlineData(0, 0, "abc")]
    [InlineData(0, 3, "")]
    [InlineData(1, 2, "a")]
    public void Remove_HappyCase(int startIndex, int length, string expected)
    {
        // Arrange
        Span<char> buff = stackalloc char[3];
        ValueStringBuilder sb = new(buff);
        sb.Append('a');
        sb.Append('b');
        sb.Append('c');

        // Act
        sb.Remove(startIndex, length);

        // Assert
        string s = sb.ToString();
        s.ShouldBe(expected);
    }

    [Theory]
    [InlineData(-1, 0, "startIndex")]
    [InlineData(0, -1, "length")]
    [InlineData(0, 4, "length")]
    public void Remove_Throw(int startIndex, int length, string paramName)
    {
        // Arrange
        Action action = () =>
        {
            Span<char> buf = stackalloc char[2];
            new ValueStringBuilder(buf).Remove(startIndex, length);
        };

        // Act && Assert
        action
            .ShouldThrow<ArgumentOutOfRangeException>()
            .ParamName.ShouldBe(paramName);
    }
}
