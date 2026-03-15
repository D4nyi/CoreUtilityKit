using System.Diagnostics.CodeAnalysis;

namespace CoreUtilityKit.Helpers;

/// <summary>
/// Provides extension methods for creating custom enumerators for loops.
/// </summary>
public static class LoopExtensions
{
    /// <summary>
    /// Gets an enumerator for a <see cref="Range"/>.
    /// </summary>
    /// <param name="range">The range to enumerate.</param>
    /// <returns>A <see cref="CustomIntEnumerator"/> for the specified range.</returns>
    public static CustomIntEnumerator GetEnumerator(this Range range) =>
        new(range);

    /// <summary>
    /// Gets an enumerator that iterates from 0 to the specified number.
    /// </summary>
    /// <param name="number">The end of the range (inclusive).</param>
    /// <returns>A <see cref="CustomIntEnumerator"/> for the range [0, number].</returns>
    public static CustomIntEnumerator GetEnumerator(this int number) =>
        new(new Range(0, number));
}

/// <summary>
/// A custom enumerator for iterating over integers.
/// </summary>
public ref struct CustomIntEnumerator
{
    private int _current;
    private readonly int _end;

    /// <summary>
    /// Gets the current element of the enumerator.
    /// </summary>
    public readonly int Current => _current;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomIntEnumerator"/> struct.
    /// </summary>
    /// <param name="range">The range to iterate over.</param>
    /// <exception cref="NotSupportedException">Thrown if the range end is from the end (infinite loops are not allowed).</exception>
    public CustomIntEnumerator(Range range)
    {
        if (range.End.IsFromEnd)
            NotSupported();

        _current = range.Start.Value - 1;
        _end = range.End.Value;
    }

    /// <summary>
    /// Advances the enumerator to the next element of the collection.
    /// </summary>
    /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the collection.</returns>
    public bool MoveNext() => ++_current <= _end;

    [DoesNotReturn]
    private static void NotSupported()
    {
        throw new NotSupportedException("Infinite loops are not allowed!");
    }
}
