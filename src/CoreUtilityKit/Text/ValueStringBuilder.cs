using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CoreUtilityKit.Text;

/// <summary>
/// Provides a stack-allocated or pooled builder for creating strings.
/// </summary>
public ref partial struct ValueStringBuilder
{
    private char[]? _arrayToReturnToPool;
    private Span<char> _chars;
    private int _pos;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueStringBuilder"/> struct with an initial buffer.
    /// </summary>
    /// <param name="initialBuffer">The initial buffer to use.</param>
    public ValueStringBuilder(Span<char> initialBuffer)
    {
        _arrayToReturnToPool = null;
        _chars = initialBuffer;
        _pos = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueStringBuilder"/> struct with an initial capacity.
    /// </summary>
    /// <param name="initialCapacity">The initial capacity of the builder.</param>
    public ValueStringBuilder(int initialCapacity)
    {
        _arrayToReturnToPool = ArrayPool<char>.Shared.Rent(initialCapacity);
        _chars = _arrayToReturnToPool;
        _pos = 0;
    }

    /// <summary>
    /// Gets or sets the length of the string builder.
    /// </summary>
    public int Length
    {
        readonly get => _pos;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, _chars.Length);

            _pos = value;
        }
    }

    /// <summary>
    /// Gets the total capacity of the builder.
    /// </summary>
    public readonly int Capacity => _chars.Length;

    /// <summary>
    /// Gets a reference to the character at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the character.</param>
    /// <returns>A reference to the character at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is out of range.</exception>
    public ref char this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(index, _pos);

            return ref _chars[index];
        }
    }

    /// <summary>Returns the underlying storage of the builder.</summary>
    public readonly Span<char> RawChars => _chars;

    /// <summary>
    /// Returns a read-only span around the contents of the builder.
    /// </summary>
    /// <returns>A read-only span of characters.</returns>
    public readonly ReadOnlySpan<char> AsSpan() => _chars[.._pos];

    /// <summary>
    /// Returns a read-only span around the contents of the builder starting at the specified index.
    /// </summary>
    /// <param name="start">The starting index.</param>
    /// <returns>A read-only span of characters.</returns>
    public readonly ReadOnlySpan<char> AsSpan(int start) => _chars[start.._pos];

    /// <summary>
    /// Returns a read-only span around a slice of the builder's contents.
    /// </summary>
    /// <param name="start">The starting index.</param>
    /// <param name="length">The length of the slice.</param>
    /// <returns>A read-only span of characters.</returns>
    public readonly ReadOnlySpan<char> AsSpan(int start, int length) => _chars.Slice(start, length);

    /// <summary>
    /// Returns a span around the contents of the builder.
    /// </summary>
    /// <param name="terminate">Ensures that the builder has a null char after <see cref="Length"/></param>
    /// <returns>A read-only span of characters.</returns>
    public ReadOnlySpan<char> AsSpan(bool terminate)
    {
        if (terminate)
        {
            EnsureCapacity(Length + 1);
            _chars[Length] = '\0';
        }

        return _chars[.._pos];
    }

    /// <summary>
    /// Ensures that the builder has at least the specified capacity.
    /// </summary>
    /// <param name="capacity">The minimum capacity required.</param>
    public void EnsureCapacity(int capacity)
    {
        // This is not expected to be called this with negative capacity
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);

        // If the caller has a bug and calls this with negative capacity, make sure to call Grow to throw an exception.
        if ((uint)capacity > (uint)_chars.Length)
            Grow(capacity - _pos);
    }

    /// <summary>
    /// Get a pinnable reference to the builder.
    /// Does not ensure there is a null char after <see cref="Length"/>
    /// This overload is pattern matched in the C# 7.3+ compiler so you can omit
    /// the explicit method call, and write eg "fixed (char* c = builder)"
    /// </summary>
    /// <returns>A reference to the first character in the builder.</returns>
    public readonly ref char GetPinnableReference()
    {
        return ref MemoryMarshal.GetReference(_chars);
    }

    /// <summary>
    /// Get a pinnable reference to the builder.
    /// </summary>
    /// <param name="terminate">Ensures that the builder has a null char after <see cref="Length"/></param>
    /// <returns>A reference to the first character in the builder.</returns>
    public ref char GetPinnableReference(bool terminate)
    {
        if (terminate)
        {
            EnsureCapacity(Length + 1);
            _chars[Length] = '\0';
        }

        return ref MemoryMarshal.GetReference(_chars);
    }

    /// <summary>
    /// Returns the contents of the builder as a string and disposes of the internal buffer.
    /// </summary>
    /// <returns>A string representation of the builder's contents.</returns>
    public override string ToString()
    {
        string s = _chars[.._pos].ToString();
        Dispose();
        return s;
    }

    /// <summary>
    /// Attempts to copy the contents of the builder to the specified destination span.
    /// </summary>
    /// <param name="destination">The destination span.</param>
    /// <param name="charsWritten">When this method returns, contains the number of characters written to the destination.</param>
    /// <returns><see langword="true"/> if the copy was successful; otherwise, <see langword="false"/>.</returns>
    public bool TryCopyTo(Span<char> destination, out int charsWritten)
    {
        if (_chars[.._pos].TryCopyTo(destination))
        {
            charsWritten = _pos;
            Dispose();
            return true;
        }
        else
        {
            charsWritten = 0;
            Dispose();
            return false;
        }
    }

    /// <summary>
    /// Removes the specified range of characters from the builder.
    /// </summary>
    /// <param name="startIndex">The starting index of the range to remove.</param>
    /// <param name="length">The number of characters to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the range is out of bounds.</exception>
    public void Remove(int startIndex, int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, _pos);

        if (length > Length - startIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Index was out of range. Must be non-negative and less than or equal to the size of the collection.");
        }

        if (length == 0)
        {
            return;
        }

        if (Length == length && startIndex == 0)
        {
            Length = 0;
            return;
        }

        if ((startIndex + length) == Length)
        {
            Length = startIndex;
            return;
        }

        for (int i = startIndex; i < _pos - length; i++)
        {
            _chars[i] = _chars[i + length];
        }

        Length -= length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Dispose()
    {
        char[]? toReturn = _arrayToReturnToPool;
        this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowAndAppend(char c)
    {
        Grow(1);
        Append(c);
    }

    /// <summary>
    /// Resize the internal buffer either by doubling current buffer size or
    /// by adding <paramref name="additionalCapacityBeyondPos"/> to
    /// <see cref="_pos"/> whichever is greater.
    /// </summary>
    /// <param name="additionalCapacityBeyondPos">
    /// Number of chars requested beyond current position.
    /// </param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow(int additionalCapacityBeyondPos)
    {
        Debug.Assert(additionalCapacityBeyondPos > 0);
        Debug.Assert(_pos > _chars.Length - additionalCapacityBeyondPos, "Grow called incorrectly, no resize is needed.");

        const uint ArrayMaxLength = 0x7FFFFFC7U; // same as Array.MaxLength

        // Increase to at least the required size (_pos + additionalCapacityBeyondPos), but try
        // to double the size if possible, bounding the doubling to not go beyond the max array length.
        int newCapacity = (int)Math.Max(
                (uint)(_pos + additionalCapacityBeyondPos),
                Math.Min((uint)_chars.Length * 2, ArrayMaxLength));

        // Make sure to let Rent throw an exception if the caller has a bug and the desired capacity is negative.
        // This could also go negative if the actual required length wraps around.
        char[] poolArray = ArrayPool<char>.Shared.Rent(newCapacity);

        _chars[.._pos].CopyTo(poolArray);

        char[]? toReturn = _arrayToReturnToPool;
        _chars = _arrayToReturnToPool = poolArray;
        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }
}
