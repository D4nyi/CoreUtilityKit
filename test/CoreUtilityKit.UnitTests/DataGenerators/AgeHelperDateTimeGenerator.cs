using Microsoft.Extensions.Time.Testing;

namespace CoreUtilityKit.UnitTests.DataGenerators;

internal static class Constants
{
    internal const int Age = 24;
    internal const int AgeLeapDay = 25;

    internal static readonly DateTimeOffset DateTimeOffsetUtc = new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
    internal static readonly DateTimeOffset DateTimeOffsetUtcLeapDay = DateTimeOffsetUtc.AddMonths(1).AddDays(28);
    internal static readonly DateTimeOffset DateTimeOffset = new (2000, 1, 1, 0, 0, 0, TimeSpan.FromHours(2));

    internal static readonly DateTime DateTimeUtc = DateTimeOffsetUtc.UtcDateTime;
    internal static readonly DateTime DateTimeUtcLeapDay = DateTimeUtc.AddMonths(1).AddDays(28);
    internal static readonly DateTime DateTime = DateTimeOffsetUtc.DateTime;

    internal static readonly TimeProvider TimeProvider = new FakeTimeProvider(DateTimeOffsetUtc.AddYears(Age));

    private static readonly DateTimeOffset _leapDayTime = DateTimeOffsetUtc.AddYears(AgeLeapDay).AddMonths(1).AddDays(27);
    internal static TimeProvider CreateTimeProviderForLeapDay()
    {
        return new FakeTimeProvider(_leapDayTime)
        {
            AutoAdvanceAmount = TimeSpan.FromDays(1)
        };
    }
}

internal sealed class AgeHelperDateTimeGenerator : TheoryData<DateTime, int>
{
    public AgeHelperDateTimeGenerator()
    {
        Add(Constants.DateTimeUtc, Constants.Age);
        Add(Constants.DateTime, Constants.Age);
    }
}

internal sealed class AgeHelperDateTimeOffsetGenerator : TheoryData<DateTimeOffset, int>
{
    public AgeHelperDateTimeOffsetGenerator()
    {
        Add(Constants.DateTimeOffsetUtc, Constants.Age);
        Add(Constants.DateTimeOffset, Constants.Age);
    }
}

internal sealed class AgeHelperDateOnlyGenerator : TheoryData<DateOnly, int>
{
    public AgeHelperDateOnlyGenerator()
    {
        Add(DateOnly.FromDateTime(Constants.DateTimeUtc.AddDays(-1)), Constants.Age);
        Add(DateOnly.FromDateTime(Constants.DateTime), Constants.Age);
    }
}