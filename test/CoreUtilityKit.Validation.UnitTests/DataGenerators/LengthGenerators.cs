namespace CoreUtilityKit.Validation.UnitTests.DataGenerators;

internal abstract class LengthBaseGenerator : TheoryData<string?, int, bool>
{
    protected const int MinLength = 3;
    public const int MaxLength = 5;

    private static readonly string?[] _invalid =
    [
        null,
        "",
        " ",
        "\t \r"
    ];

    private static readonly string[] _valid =
    [
        "te",
        "tes",
        "test",
        "tests",
        "testss"
    ];

    protected LengthBaseGenerator(Func<int, bool> expectedResultFunc)
    {
        foreach (string? data in _invalid)
        {
            Add(data, MinLength, false);
        }

        foreach (string data in _valid)
        {
            Add(data, MinLength, expectedResultFunc(data.Length));
        }
    }
}

internal sealed class MinLengthGenerator() : LengthBaseGenerator(static x => x >= MinLength);

internal sealed class MaxLengthGenerator() : LengthBaseGenerator(static x => x <= MinLength);

internal sealed class BetweenLengthBaseGenerator() : LengthBaseGenerator(static x => x is >= MinLength and <= MaxLength);

internal sealed class ExactLengthBaseGenerator() : LengthBaseGenerator(static x => x == MinLength);