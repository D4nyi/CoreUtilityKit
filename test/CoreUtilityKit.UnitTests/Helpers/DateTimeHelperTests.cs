using Microsoft.Extensions.Time.Testing;

namespace CoreUtilityKit.UnitTests.Helpers;

public sealed class DateTimeHelperTests
{
    #region Test data

    public static readonly TheoryData<TimeProvider, TimeProvider> EndOfMonthData = new()
    {
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            new FakeTimeProvider(new DateTimeOffset(2024, 6, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 15, 0, 0, 0, TimeSpan.Zero)),
            new FakeTimeProvider(new DateTimeOffset(2024, 6, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 31, 0, 0, 0, TimeSpan.Zero)),
            new FakeTimeProvider(new DateTimeOffset(2024, 6, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 6, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1)),
            new FakeTimeProvider(new DateTimeOffset(2024, 6, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1))
        },
    };

    public static readonly TheoryData<TimeProvider, int, TimeProvider> EndOfMonthAddData = new()
    {
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            -5,
            new FakeTimeProvider(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 15, 0, 0, 0, TimeSpan.Zero)),
            -4,
            new FakeTimeProvider(new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 31, 0, 0, 0, TimeSpan.Zero)),
            1,
            new FakeTimeProvider(new DateTimeOffset(2024, 7, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 31, 23, 59, 59, 999, 999, TimeSpan.Zero)),
            6,
            new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 31, 23, 59, 59, 999, 999, TimeSpan.Zero)),
            7,
            new FakeTimeProvider(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 31, 23, 59, 59, 999, 999, TimeSpan.Zero)),
            8,
            new FakeTimeProvider(new DateTimeOffset(2025, 2, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1))
        },
    };

    public static readonly TheoryData<TimeProvider, TimeProvider> BeginningOfMonthData = new()
    {
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, 0, 0, TimeSpan.Zero))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 15, 0, 0, 0, TimeSpan.Zero)),
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, 0, 0, TimeSpan.Zero))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 31, 0, 0, 0, TimeSpan.Zero)),
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, 0, 0, TimeSpan.Zero))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 6, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1)),
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, 0, 0, TimeSpan.Zero))
        },
    };

    public static readonly TheoryData<TimeProvider, int, TimeProvider> BeginningOfMonthAddData = new()
    {
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            -5,
            new FakeTimeProvider(new DateTimeOffset(2023, 12, 1, 0, 0, 0, TimeSpan.Zero))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            -4,
            new FakeTimeProvider(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            1,
            new FakeTimeProvider(new DateTimeOffset(2024, 6, 1, 0, 0, 0, TimeSpan.Zero))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 15, 0, 0, 0, TimeSpan.Zero)),
            6,
            new FakeTimeProvider(new DateTimeOffset(2024, 11, 1, 0, 0, 0, TimeSpan.Zero))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 31, 0, 0, 0, TimeSpan.Zero)),
            7,
            new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero))
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 6, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1)),
            8,
            new FakeTimeProvider(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero))
        },
    };

    public static readonly TheoryData<TimeProvider, int> PreviousMonthData = new()
    {
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            4
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero)),
            12
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 12, 31, 0, 0, 0, TimeSpan.Zero)),
            11
        }
    };

    public static readonly TheoryData<TimeProvider, int> NextMonthData = new()
    {
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            6
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero)),
            2
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 12, 31, 0, 0, 0, TimeSpan.Zero)),
            1
        }
    };

    public static readonly TheoryData<TimeProvider, int> PreviousDayData = new()
    {
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            30
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero)),
            14
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 12, 31, 0, 0, 0, TimeSpan.Zero)),
            30
        }
    };

    public static readonly TheoryData<TimeProvider, int> NextDayData = new()
    {
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            2
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero)),
            16
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 12, 31, 0, 0, 0, TimeSpan.Zero)),
            1
        }
    };

    public static readonly TheoryData<TimeProvider, DayOfWeek> PreviousDayOfWeekData = new()
    {
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 12, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Sunday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 13, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Monday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 14, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Tuesday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 15, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Wednesday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 16, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Thursday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 17, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Friday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 18, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Saturday
        }
    };

    public static readonly TheoryData<TimeProvider, DayOfWeek> NextDayOfWeekData = new()
    {
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 12, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Tuesday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 13, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Wednesday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 14, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Thursday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 15, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Friday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 16, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Saturday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 17, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Sunday
        },
        {
            new FakeTimeProvider(new DateTimeOffset(2024, 8, 18, 0, 0, 0, TimeSpan.Zero)),
            DayOfWeek.Monday
        }
    };

    private static readonly TimeProvider _timeProvider = new FakeTimeProvider();

    #endregion

    #region Next

    [Fact]
    public void NextYear_HappyCase()
    {
        const int ExpectedNextYear = 2001;

        DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime).NextYear().ShouldBe(ExpectedNextYear);

        _timeProvider.GetUtcNow().DateTime.NextYear().ShouldBe(ExpectedNextYear);

        _timeProvider.GetUtcNow().NextYear().ShouldBe(ExpectedNextYear);

        _timeProvider.NextYear().ShouldBe(ExpectedNextYear);
    }

    [Fact]
    public void NextYear_ShouldThrow_WhenTimeProviderIsNull()
    {
        // Arrange
        TimeProvider? timeProvider = null;

        Action action = () => timeProvider!.NextYear();

        // Act && Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [MemberData(nameof(NextMonthData))]
    public void NextMonth_HappyCase(TimeProvider timeProvider, int expectedNextMonth)
    {
        DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime).NextMonth().ShouldBe(expectedNextMonth);

        timeProvider.GetUtcNow().DateTime.NextMonth().ShouldBe(expectedNextMonth);

        timeProvider.GetUtcNow().NextMonth().ShouldBe(expectedNextMonth);

        timeProvider.NextMonth().ShouldBe(expectedNextMonth);
    }

    [Fact]
    public void NextMonth_ShouldThrow_WhenTimeProviderIsNull()
    {
        // Arrange
        TimeProvider? timeProvider = null;

        Action action = () => timeProvider!.NextMonth();

        // Act && Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [MemberData(nameof(NextDayData))]
    public void NextDay_HappyCase(TimeProvider timeProvider, int expectedNextDay)
    {
        DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime).NextDay().ShouldBe(expectedNextDay);

        timeProvider.GetUtcNow().DateTime.NextDay().ShouldBe(expectedNextDay);

        timeProvider.GetUtcNow().NextDay().ShouldBe(expectedNextDay);

        timeProvider.NextDay().ShouldBe(expectedNextDay);
    }

    [Fact]
    public void NextDay_ShouldThrow_WhenTimeProviderIsNull()
    {
        // Arrange
        TimeProvider? timeProvider = null;

        Action action = () => timeProvider!.NextDay();

        // Act && Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [MemberData(nameof(NextDayOfWeekData))]
    public static void NextDayOfWeek_HappyCase(TimeProvider timeProvider, DayOfWeek expectedNextDayOfWeek)
    {
        DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime).NextDayOfWeek().ShouldBe(expectedNextDayOfWeek);

        timeProvider.GetUtcNow().DateTime.NextDayOfWeek().ShouldBe(expectedNextDayOfWeek);

        timeProvider.GetUtcNow().NextDayOfWeek().ShouldBe(expectedNextDayOfWeek);

        timeProvider.GetUtcNow().DayOfWeek.NextDayOfWeek().ShouldBe(expectedNextDayOfWeek);

        timeProvider.NextDayOfWeek().ShouldBe(expectedNextDayOfWeek);

    }

    [Fact]
    public void NextDayOfWeek_ShouldThrow_WhenTimeProviderIsNull()
    {
        // Arrange
        TimeProvider? timeProvider = null;

        Action action = () => timeProvider!.NextDayOfWeek();

        // Act && Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    #endregion

    #region Previous

    [Fact]
    public void PreviousYear_HappyCase()
    {
        const int ExpectedPreviousYear = 1999;

        DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime).PreviousYear().ShouldBe(ExpectedPreviousYear);

        _timeProvider.GetUtcNow().DateTime.PreviousYear().ShouldBe(ExpectedPreviousYear);

        _timeProvider.GetUtcNow().PreviousYear().ShouldBe(ExpectedPreviousYear);

        _timeProvider.PreviousYear().ShouldBe(ExpectedPreviousYear);
    }

    [Fact]
    public void PreviousYear_ShouldThrow_WhenTimeProviderIsNull()
    {
        // Arrange
        TimeProvider? timeProvider = null;

        Action action = () => timeProvider!.PreviousYear();

        // Act && Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [MemberData(nameof(PreviousMonthData))]
    public void PreviousMonth_HappyCase(TimeProvider timeProvider, int expectedPreviousMonth)
    {
        DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime).PreviousMonth().ShouldBe(expectedPreviousMonth);

        timeProvider.GetUtcNow().DateTime.PreviousMonth().ShouldBe(expectedPreviousMonth);

        timeProvider.GetUtcNow().PreviousMonth().ShouldBe(expectedPreviousMonth);

        timeProvider.PreviousMonth().ShouldBe(expectedPreviousMonth);
    }

    [Fact]
    public void PreviousMonth_ShouldThrow_WhenTimeProviderIsNull()
    {
        // Arrange
        TimeProvider? timeProvider = null;

        Action action = () => timeProvider!.PreviousMonth();

        // Act && Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [MemberData(nameof(PreviousDayData))]
    public void PreviousDay_HappyCase(TimeProvider timeProvider, int expectedPreviousDay)
    {
        DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime).PreviousDay().ShouldBe(expectedPreviousDay);

        timeProvider.GetUtcNow().DateTime.PreviousDay().ShouldBe(expectedPreviousDay);

        timeProvider.GetUtcNow().PreviousDay().ShouldBe(expectedPreviousDay);

        timeProvider.PreviousDay().ShouldBe(expectedPreviousDay);
    }

    [Fact]
    public void PreviousDay_ShouldThrow_WhenTimeProviderIsNull()
    {
        // Arrange
        TimeProvider? timeProvider = null;

        Action action = () => timeProvider!.PreviousDay();

        // Act && Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [MemberData(nameof(PreviousDayOfWeekData))]
    public static void PreviousDayOfWeek_HappyCase(TimeProvider timeProvider, DayOfWeek expectedPreviousDayOfWeek)
    {
        DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime).PreviousDayOfWeek().ShouldBe(expectedPreviousDayOfWeek);

        timeProvider.GetUtcNow().DateTime.PreviousDayOfWeek().ShouldBe(expectedPreviousDayOfWeek);

        timeProvider.GetUtcNow().PreviousDayOfWeek().ShouldBe(expectedPreviousDayOfWeek);

        timeProvider.GetUtcNow().DayOfWeek.PreviousDayOfWeek().ShouldBe(expectedPreviousDayOfWeek);

        timeProvider.PreviousDayOfWeek().ShouldBe(expectedPreviousDayOfWeek);
    }

    [Fact]
    public void PreviousDayOfWeek_ShouldThrow_WhenTimeProviderIsNull()
    {
        // Arrange
        TimeProvider? timeProvider = null;

        Action action = () => timeProvider!.PreviousDayOfWeek();

        // Act && Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    #endregion

    #region EndOfMonth

    [Theory]
    [MemberData(nameof(EndOfMonthData))]
    public void EndOfMonth_HappyCase(TimeProvider timeProvider, TimeProvider expected)
    {
        timeProvider.GetUtcNow().EndOfMonth().ShouldBe(expected.GetUtcNow());

        timeProvider.GetUtcNow().DateTime.EndOfMonth().ShouldBe(expected.GetUtcNow().DateTime);

        timeProvider.EndOfMonth().ShouldBe(expected.GetUtcNow());
    }

    [Theory]
    [MemberData(nameof(EndOfMonthAddData))]
    public void EndOfMonth_Add_HappyCase(TimeProvider timeProvider, int addMonth, TimeProvider expected)
    {
        timeProvider.GetUtcNow().EndOfMonth(addMonth).ShouldBe(expected.GetUtcNow());

        timeProvider.GetUtcNow().DateTime.EndOfMonth(addMonth).ShouldBe(expected.GetUtcNow().DateTime);

        timeProvider.EndOfMonth(addMonth).ShouldBe(expected.GetUtcNow());
    }

    #endregion

    #region BeginningOfMonth

    [Theory]
    [MemberData(nameof(BeginningOfMonthData))]
    public void BeginningOfMonth_HappyCase(TimeProvider timeProvider, TimeProvider expected)
    {
        timeProvider.GetUtcNow().BeginningOfMonth().ShouldBe(expected.GetUtcNow());

        timeProvider.GetUtcNow().DateTime.BeginningOfMonth().ShouldBe(expected.GetUtcNow().DateTime);

        timeProvider.BeginningOfMonth().ShouldBe(expected.GetUtcNow());
    }

    [Theory]
    [MemberData(nameof(BeginningOfMonthAddData))]
    public void BeginningOfMonth_Add_HappyCase(TimeProvider timeProvider, int addMonth, TimeProvider expected)
    {
        timeProvider.GetUtcNow().BeginningOfMonth(addMonth).ShouldBe(expected.GetUtcNow());

        timeProvider.GetUtcNow().DateTime.BeginningOfMonth(addMonth).ShouldBe(expected.GetUtcNow().DateTime);

        timeProvider.BeginningOfMonth(addMonth).ShouldBe(expected.GetUtcNow());
    }

    #endregion
}
