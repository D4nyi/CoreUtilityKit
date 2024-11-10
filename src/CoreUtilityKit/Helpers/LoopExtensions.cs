using System.Diagnostics.CodeAnalysis;

namespace CoreUtilityKit.Helpers;

public static class LoopExtensions
{
    public static CustomIntEnumerator GetEnumerator(this Range range) =>
        new(range);

    public static CustomIntEnumerator GetEnumerator(this int number) =>
        new(new Range(0, number));
}

public ref struct CustomIntEnumerator
{
    private int _current;
    private readonly int _end;

    public readonly int Current => _current;

    public CustomIntEnumerator(Range range)
    {
        if (range.End.IsFromEnd)
            NotSupported();

        _current = range.Start.Value - 1;
        _end = range.End.Value;
    }

    public bool MoveNext() => ++_current <= _end;

    [DoesNotReturn]
    private static void NotSupported()
    {
        throw new NotSupportedException("Infinite loops are not allowed!");
    }
}
