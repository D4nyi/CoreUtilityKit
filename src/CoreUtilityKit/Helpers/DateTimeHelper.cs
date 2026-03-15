using System.Runtime.CompilerServices;

namespace CoreUtilityKit.Helpers;

/// <summary>
/// Provides extension methods for date and time calculations.
/// </summary>
public static class DateTimeHelper
{
    #region NextYear

    /// <summary>
    /// Gets the next year from the specified <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="dateOnly">The date.</param>
    /// <returns>The year after the specified date's year.</returns>
    public static int NextYear(this DateOnly dateOnly) => dateOnly.Year + 1;

    /// <summary>
    /// Gets the next year from the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <returns>The year after the specified date's year.</returns>
    public static int NextYear(this DateTime dateTime) => dateTime.Year + 1;

    /// <summary>
    /// Gets the next year from the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <returns>The year after the specified date's year.</returns>
    public static int NextYear(this DateTimeOffset dateTimeOffset) => dateTimeOffset.Year + 1;

    /// <summary>
    /// Gets the next year from the current time provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The year after the current year.</returns>
    public static int NextYear(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return NextYear(timeProvider.GetUtcNow());
    }

    #endregion

    #region PreviousYear

    /// <summary>
    /// Gets the previous year from the specified <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="dateOnly">The date.</param>
    /// <returns>The year before the specified date's year.</returns>
    public static int PreviousYear(this DateOnly dateOnly) => dateOnly.Year - 1;

    /// <summary>
    /// Gets the previous year from the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <returns>The year before the specified date's year.</returns>
    public static int PreviousYear(this DateTime dateTime) => dateTime.Year - 1;

    /// <summary>
    /// Gets the previous year from the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <returns>The year before the specified date's year.</returns>
    public static int PreviousYear(this DateTimeOffset dateTimeOffset) => dateTimeOffset.Year - 1;

    /// <summary>
    /// Gets the previous year from the current time provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The year before the current year.</returns>
    public static int PreviousYear(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return PreviousYear(timeProvider.GetUtcNow());
    }

    #endregion

    #region NextMonth

    /// <summary>
    /// Gets the next month from the specified <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="dateOnly">The date.</param>
    /// <returns>The month after the specified date's month (1-12).</returns>
    public static int NextMonth(this DateOnly dateOnly) => (dateOnly.Month % 12) + 1;

    /// <summary>
    /// Gets the next month from the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <returns>The month after the specified date's month (1-12).</returns>
    public static int NextMonth(this DateTime dateTime) => (dateTime.Month % 12) + 1;

    /// <summary>
    /// Gets the next month from the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <returns>The month after the specified date's month (1-12).</returns>
    public static int NextMonth(this DateTimeOffset dateTimeOffset) => (dateTimeOffset.Month % 12) + 1;

    /// <summary>
    /// Gets the next month from the current time provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The month after the current month (1-12).</returns>
    public static int NextMonth(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return NextMonth(timeProvider.GetUtcNow());
    }

    #endregion

    #region PreviousMonth

    /// <summary>
    /// Gets the previous month from the specified <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="dateOnly">The date.</param>
    /// <returns>The month before the specified date's month (1-12).</returns>
    public static int PreviousMonth(this DateOnly dateOnly) => PreviousMonthCore(dateOnly.Month);

    /// <summary>
    /// Gets the previous month from the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <returns>The month before the specified date's month (1-12).</returns>
    public static int PreviousMonth(this DateTime dateTime) => PreviousMonthCore(dateTime.Month);

    /// <summary>
    /// Gets the previous month from the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <returns>The month before the specified date's month (1-12).</returns>
    public static int PreviousMonth(this DateTimeOffset dateTimeOffset) => PreviousMonthCore(dateTimeOffset.Month);

    /// <summary>
    /// Gets the previous month from the current time provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The month before the current month (1-12).</returns>
    public static int PreviousMonth(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return PreviousMonth(timeProvider.GetUtcNow());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int PreviousMonthCore(int month) => month == 1 ? 12 : month - 1;

    #endregion

    #region NextDay

    /// <summary>
    /// Gets the next day from the specified <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="dateOnly">The date.</param>
    /// <returns>The day after the specified date's day of the month.</returns>
    public static int NextDay(this DateOnly dateOnly)
    {
        int daysInCurrentMonth = DateTime.DaysInMonth(dateOnly.Year, dateOnly.Month);

        return (dateOnly.Day % daysInCurrentMonth) + 1;
    }

    /// <summary>
    /// Gets the next day from the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <returns>The day after the specified date's day of the month.</returns>
    public static int NextDay(this DateTime dateTime)
    {
        int daysInCurrentMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

        return (dateTime.Day % daysInCurrentMonth) + 1;
    }

    /// <summary>
    /// Gets the next day from the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <returns>The day after the specified date's day of the month.</returns>
    public static int NextDay(this DateTimeOffset dateTimeOffset)
    {
        int daysInCurrentMonth = DateTime.DaysInMonth(dateTimeOffset.Year, dateTimeOffset.Month);

        return (dateTimeOffset.Day % daysInCurrentMonth) + 1;
    }

    /// <summary>
    /// Gets the next day from the current time provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The day after the current day of the month.</returns>
    public static int NextDay(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return NextDay(timeProvider.GetUtcNow());
    }

    #endregion

    #region PreviousDay

    /// <summary>
    /// Gets the previous day from the specified <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="dateOnly">The date.</param>
    /// <returns>The day before the specified date's day of the month.</returns>
    public static int PreviousDay(this DateOnly dateOnly) => PreviousDayCore(dateOnly.Year, dateOnly.Month, dateOnly.Day);

    /// <summary>
    /// Gets the previous day from the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <returns>The day before the specified date's day of the month.</returns>
    public static int PreviousDay(this DateTime dateTime) => PreviousDayCore(dateTime.Year, dateTime.Month, dateTime.Day);

    /// <summary>
    /// Gets the previous day from the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <returns>The day before the specified date's day of the month.</returns>
    public static int PreviousDay(this DateTimeOffset dateTimeOffset) => PreviousDayCore(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day);

    /// <summary>
    /// Gets the previous day from the current time provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The day before the current day of the month.</returns>
    public static int PreviousDay(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return PreviousDay(timeProvider.GetUtcNow());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int PreviousDayCore(int year, int month, int day) => day == 1 ? DateTime.DaysInMonth(year, PreviousMonthCore(month)) : day - 1;

    #endregion

    #region NextDayOfWeek

    /// <summary>
    /// Gets the next day of the week from the specified <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="dateOnly">The date.</param>
    /// <returns>The <see cref="DayOfWeek"/> after the specified date's day of the week.</returns>
    public static DayOfWeek NextDayOfWeek(this DateOnly dateOnly) => (DayOfWeek)(((int)dateOnly.DayOfWeek + 1) % 7);

    /// <summary>
    /// Gets the next day of the week from the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <returns>The <see cref="DayOfWeek"/> after the specified date's day of the week.</returns>
    public static DayOfWeek NextDayOfWeek(this DateTime dateTime) => (DayOfWeek)(((int)dateTime.DayOfWeek + 1) % 7);

    /// <summary>
    /// Gets the next day of the week from the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <returns>The <see cref="DayOfWeek"/> after the specified date's day of the week.</returns>
    public static DayOfWeek NextDayOfWeek(this DateTimeOffset dateTimeOffset) => (DayOfWeek)(((int)dateTimeOffset.DayOfWeek + 1) % 7);

    /// <summary>
    /// Gets the next day of the week from the current time provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The <see cref="DayOfWeek"/> after the current day of the week.</returns>
    public static DayOfWeek NextDayOfWeek(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return NextDayOfWeek(timeProvider.GetUtcNow());
    }

    /// <summary>
    /// Gets the next day of the week from the specified <see cref="DayOfWeek"/>.
    /// </summary>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <returns>The <see cref="DayOfWeek"/> after the specified day of the week.</returns>
    public static DayOfWeek NextDayOfWeek(this DayOfWeek dayOfWeek) => (DayOfWeek)(((int)dayOfWeek + 1) % 7);

    #endregion

    #region PreviousDayOfWeek

    /// <summary>
    /// Gets the previous day of the week from the specified <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="dateOnly">The date.</param>
    /// <returns>The <see cref="DayOfWeek"/> before the specified date's day of the week.</returns>
    public static DayOfWeek PreviousDayOfWeek(this DateOnly dateOnly) => PreviousDayOfWeek(dateOnly.DayOfWeek);

    /// <summary>
    /// Gets the previous day of the week from the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <returns>The <see cref="DayOfWeek"/> before the specified date's day of the week.</returns>
    public static DayOfWeek PreviousDayOfWeek(this DateTime dateTime) => PreviousDayOfWeek(dateTime.DayOfWeek);

    /// <summary>
    /// Gets the previous day of the week from the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <returns>The <see cref="DayOfWeek"/> before the specified date's day of the week.</returns>
    public static DayOfWeek PreviousDayOfWeek(this DateTimeOffset dateTimeOffset) => PreviousDayOfWeek(dateTimeOffset.DayOfWeek);

    /// <summary>
    /// Gets the previous day of the week from the current time provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The <see cref="DayOfWeek"/> before the current day of the week.</returns>
    public static DayOfWeek PreviousDayOfWeek(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return PreviousDayOfWeek(timeProvider.GetUtcNow());
    }

    /// <summary>
    /// Gets the previous day of the week from the specified <see cref="DayOfWeek"/>.
    /// </summary>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <returns>The <see cref="DayOfWeek"/> before the specified day of the week.</returns>
    public static DayOfWeek PreviousDayOfWeek(this DayOfWeek dayOfWeek) => (DayOfWeek)(((int)dayOfWeek + 6) % 7);

    #endregion

    #region EndOfMonth

    /// <summary>
    /// Gets the end of the month for the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <returns>A <see cref="DateTime"/> representing the last tick of the month.</returns>
    public static DateTime EndOfMonth(this DateTime dateTime)
    {
        int daysInCurrentMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

        long ticks = CalcEndOfMonthTicks((ulong)dateTime.Ticks, daysInCurrentMonth);

        return new DateTime(ticks, dateTime.Kind);
    }

    /// <summary>
    /// Gets the end of the month for the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the last tick of the month.</returns>
    public static DateTimeOffset EndOfMonth(this DateTimeOffset dateTimeOffset)
    {
        int daysInCurrentMonth = DateTime.DaysInMonth(dateTimeOffset.Year, dateTimeOffset.Month);

        long ticks = CalcEndOfMonthTicks((ulong)dateTimeOffset.Ticks, daysInCurrentMonth);

        return new DateTimeOffset(ticks, dateTimeOffset.Offset);
    }

    /// <summary>
    /// Gets the end of the month after adding the specified number of months to the <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="months">The number of months to add.</param>
    /// <returns>A <see cref="DateTime"/> representing the last tick of the calculated month.</returns>
    public static DateTime EndOfMonth(this DateTime dateTime, int months)
    {
        DateTime newDate = dateTime.AddMonths(months);

        int daysInMonth = DateTime.DaysInMonth(newDate.Year, newDate.Month);

        long ticks = CalcEndOfMonthTicks((ulong)newDate.Ticks, daysInMonth);

        return new DateTime(ticks, dateTime.Kind);
    }

    /// <summary>
    /// Gets the end of the month after adding the specified number of months to the <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <param name="months">The number of months to add.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the last tick of the calculated month.</returns>
    public static DateTimeOffset EndOfMonth(this DateTimeOffset dateTimeOffset, int months)
    {
        DateTimeOffset newDate = dateTimeOffset.AddMonths(months);

        int daysInMonth = DateTime.DaysInMonth(newDate.Year, newDate.Month);

        long ticks = CalcEndOfMonthTicks((ulong)newDate.Ticks, daysInMonth);

        return new DateTimeOffset(ticks, dateTimeOffset.Offset);
    }

    /// <summary>
    /// Gets the end of the current month provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the last tick of the current month.</returns>
    public static DateTimeOffset EndOfMonth(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return EndOfMonth(timeProvider.GetUtcNow());
    }

    /// <summary>
    /// Gets the end of the month after adding the specified number of months to the current time provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="months">The number of months to add.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the last tick of the calculated month.</returns>
    public static DateTimeOffset EndOfMonth(this TimeProvider timeProvider, int months)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return EndOfMonth(timeProvider.GetUtcNow(), months);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long CalcEndOfMonthTicks(ulong ticks, int daysInCurrentMonth)
    {
        #region Constants
        const ulong FlagsMask = 0xC000000000000000UL;

        // MicrosecondsPerMillisecond = 1000
        // TicksPerMicrosecond        = 10
        // TicksPerMillisecond        = TicksPerMicrosecond * MicrosecondsPerMillisecond
        // HoursPerDay                = 24
        // TicksPerSecond             = TicksPerMillisecond * 1000
        // TicksPerMinute             = TicksPerSecond * 60
        // TicksPerHour               = TicksPerMinute * 60
        // TicksPerDay                = TicksPerHour * HoursPerDay
        const ulong TicksPerDay = 864_000_000_000UL;
        #endregion

        int currentDayInMonth = GetCurrentDayInMonth(ticks);

        ulong forwardDays = (ulong)(daysInCurrentMonth - currentDayInMonth + 1);

        // (date part calc) + (forward to beginning of next month) - 1tick
        ulong endOfCurrentMonth = ticks - (ticks % TicksPerDay) + (forwardDays * TicksPerDay) - 1UL;

        ulong internalKind = ticks & FlagsMask;

        return (long)(endOfCurrentMonth | internalKind);
    }

    #endregion

    #region BeginningOfMonth

    /// <summary>
    /// Gets the beginning of the month for the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <returns>A <see cref="DateTime"/> representing the first tick of the month.</returns>
    public static DateTime BeginningOfMonth(this DateTime dateTime)
    {
        long ticks = CalcBeginningOfMonthTicks((ulong)dateTime.Ticks);

        return new DateTime(ticks, dateTime.Kind);
    }

    /// <summary>
    /// Gets the beginning of the month for the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the first tick of the month.</returns>
    public static DateTimeOffset BeginningOfMonth(this DateTimeOffset dateTimeOffset)
    {
        long ticks = CalcBeginningOfMonthTicks((ulong)dateTimeOffset.Ticks);

        return new DateTimeOffset(ticks, dateTimeOffset.Offset);
    }

    /// <summary>
    /// Gets the beginning of the month after adding the specified number of months to the <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="months">The number of months to add.</param>
    /// <returns>A <see cref="DateTime"/> representing the first tick of the calculated month.</returns>
    public static DateTime BeginningOfMonth(this DateTime dateTime, int months)
    {
        DateTime newDate = dateTime.AddMonths(months);

        long ticks = CalcBeginningOfMonthTicks((ulong)newDate.Ticks);

        return new DateTime(ticks, dateTime.Kind);
    }

    /// <summary>
    /// Gets the beginning of the month after adding the specified number of months to the <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time offset.</param>
    /// <param name="months">The number of months to add.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the first tick of the calculated month.</returns>
    public static DateTimeOffset BeginningOfMonth(this DateTimeOffset dateTimeOffset, int months)
    {
        DateTimeOffset newDate = dateTimeOffset.AddMonths(months);

        long ticks = CalcBeginningOfMonthTicks((ulong)newDate.Ticks);

        return new DateTimeOffset(ticks, dateTimeOffset.Offset);
    }

    /// <summary>
    /// Gets the beginning of the current month provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the first tick of the current month.</returns>
    public static DateTimeOffset BeginningOfMonth(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return BeginningOfMonth(timeProvider.GetUtcNow());
    }

    /// <summary>
    /// Gets the beginning of the month after adding the specified number of months to the current time provided by <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="months">The number of months to add.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the first tick of the calculated month.</returns>
    public static DateTimeOffset BeginningOfMonth(this TimeProvider timeProvider, int months)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return BeginningOfMonth(timeProvider.GetUtcNow(), months);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long CalcBeginningOfMonthTicks(ulong ticks)
    {
        #region Constants
        const ulong FlagsMask = 0xC000000000000000UL;

        // MicrosecondsPerMillisecond = 1000
        // TicksPerMicrosecond        = 10
        // TicksPerMillisecond        = TicksPerMicrosecond * MicrosecondsPerMillisecond
        // HoursPerDay                = 24
        // TicksPerSecond             = TicksPerMillisecond * 1000
        // TicksPerMinute             = TicksPerSecond * 60
        // TicksPerHour               = TicksPerMinute * 60
        // TicksPerDay                = TicksPerHour * HoursPerDay
        const ulong TicksPerDay = 864_000_000_000UL;

        #endregion

        int currentDayInMonth = GetCurrentDayInMonth(ticks);

        // (date part calc) - (rewind to beginning of month)
        ulong beginningOfCurrentMonth = ticks - (ticks % TicksPerDay) - ((ulong)(currentDayInMonth - 1) * TicksPerDay);

        ulong internalKind = ticks & FlagsMask;

        return (long)(beginningOfCurrentMonth | internalKind);
    }

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetCurrentDayInMonth(ulong ticks)
    {
        #region Constants
        const ulong TicksMask = 0x3FFFFFFFFFFFFFFF;

        // MicrosecondsPerMillisecond = 1000
        // TicksPerMicrosecond        = 10
        // TicksPerMillisecond        = TicksPerMicrosecond * MicrosecondsPerMillisecond
        // TicksPerSecond             = TicksPerMillisecond * 1000
        // TicksPerMinute             = TicksPerSecond * 60
        // TicksPerHour               = TicksPerMinute * 60
        // TicksPer6Hours             = TicksPerHour * 6
        const ulong TicksPer6Hours = 216_000_000_000UL;

        // DaysPerYear     = 365;                     // non-leap year
        // DaysPer4Years   = DaysPerYear * 4 + 1;     // 1461
        // DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
        // DaysPer400Years = DaysPer100Years * 4 + 1; // 146097
        const int DaysPer400Years = 146_097;

        // Constants used for fast calculation of the following subexpressions
        //      x / DaysPer4Years
        //      x % DaysPer4Years / 4
        // EafMultiplier = (uint)(((1UL << 32) + DaysPer4Years - 1) / DaysPer4Years); // 2,939,745
        // EafDivider    = EafMultiplier * 4;                                         // 11,758,980
        const uint EafMultiplier = 2_939_745;
        const uint EafDivider    = 11_758_980;
        #endregion

        ulong uTicks = ticks & TicksMask;

        // r1 = (day number within a 100-year period) * 4
        uint r1 = (((uint)(uTicks / TicksPer6Hours) | 3U) + 1224) % DaysPer400Years;
        uint u2 = EafMultiplier * (r1 | 3u);
        ushort daySinceMarch1 = (ushort)(u2 / EafDivider);
        int n3 = (2141 * daySinceMarch1) + 197_913;

        // Return 1-based day-of-month
        return ((ushort)n3 / 2141) + 1;
    }
}
