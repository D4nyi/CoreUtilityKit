using System.Runtime.CompilerServices;

namespace CoreUtilityKit.Helpers;

public static class DateTimeHelper
{
    #region NextYear

    public static int NextYear(this DateOnly dateOnly) => dateOnly.Year + 1;

    public static int NextYear(this DateTime dateTime) => dateTime.Year + 1;

    public static int NextYear(this DateTimeOffset dateTimeOffset) => dateTimeOffset.Year + 1;

    public static int NextYear(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return NextYear(timeProvider.GetUtcNow());
    }

    #endregion

    #region PreviousYear

    public static int PreviousYear(this DateOnly dateOnly) => dateOnly.Year - 1;

    public static int PreviousYear(this DateTime dateTime) => dateTime.Year - 1;

    public static int PreviousYear(this DateTimeOffset dateTimeOffset) => dateTimeOffset.Year - 1;

    public static int PreviousYear(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return PreviousYear(timeProvider.GetUtcNow());
    }

    #endregion

    #region NextMonth

    public static int NextMonth(this DateOnly dateOnly) => (dateOnly.Month % 12) + 1;

    public static int NextMonth(this DateTime dateTime) => (dateTime.Month % 12) + 1;

    public static int NextMonth(this DateTimeOffset dateTimeOffset) => (dateTimeOffset.Month % 12) + 1;

    public static int NextMonth(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return NextMonth(timeProvider.GetUtcNow());
    }

    #endregion

    #region PreviousMonth

    public static int PreviousMonth(this DateOnly dateOnly) => PreviousMonthCore(dateOnly.Month);

    public static int PreviousMonth(this DateTime dateTime) => PreviousMonthCore(dateTime.Month);

    public static int PreviousMonth(this DateTimeOffset dateTimeOffset) => PreviousMonthCore(dateTimeOffset.Month);

    public static int PreviousMonth(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return PreviousMonth(timeProvider.GetUtcNow());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int PreviousMonthCore(int month) => month == 1 ? 12 : month - 1;

    #endregion

    #region NextDay

    public static int NextDay(this DateOnly dateOnly)
    {
        int daysInCurrentMonth = DateTime.DaysInMonth(dateOnly.Year, dateOnly.Month);

        return (dateOnly.Day % daysInCurrentMonth) + 1;
    }

    public static int NextDay(this DateTime dateTime)
    {
        int daysInCurrentMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

        return (dateTime.Day % daysInCurrentMonth) + 1;
    }

    public static int NextDay(this DateTimeOffset dateTimeOffset)
    {
        int daysInCurrentMonth = DateTime.DaysInMonth(dateTimeOffset.Year, dateTimeOffset.Month);

        return (dateTimeOffset.Day % daysInCurrentMonth) + 1;
    }

    public static int NextDay(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return NextDay(timeProvider.GetUtcNow());
    }

    #endregion

    #region PreviousDay

    public static int PreviousDay(this DateOnly dateOnly) => PreviousDayCore(dateOnly.Year, dateOnly.Month, dateOnly.Day);

    public static int PreviousDay(this DateTime dateTime) => PreviousDayCore(dateTime.Year, dateTime.Month, dateTime.Day);

    public static int PreviousDay(this DateTimeOffset dateTimeOffset) => PreviousDayCore(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day);

    public static int PreviousDay(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return PreviousDay(timeProvider.GetUtcNow());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int PreviousDayCore(int year, int month, int day) => day == 1 ? DateTime.DaysInMonth(year, PreviousMonthCore(month)) : day - 1;

    #endregion

    #region NextDayOfWeek

    public static DayOfWeek NextDayOfWeek(this DateOnly dateOnly) => (DayOfWeek)(((int)dateOnly.DayOfWeek + 1) % 7);

    public static DayOfWeek NextDayOfWeek(this DateTime dateTime) => (DayOfWeek)(((int)dateTime.DayOfWeek + 1) % 7);

    public static DayOfWeek NextDayOfWeek(this DateTimeOffset dateTimeOffset) => (DayOfWeek)(((int)dateTimeOffset.DayOfWeek + 1) % 7);

    public static DayOfWeek NextDayOfWeek(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return NextDayOfWeek(timeProvider.GetUtcNow());
    }

    public static DayOfWeek NextDayOfWeek(this DayOfWeek dayOfWeek) => (DayOfWeek)(((int)dayOfWeek + 1) % 7);

    #endregion

    #region PreviousDayOfWeek

    public static DayOfWeek PreviousDayOfWeek(this DateOnly dateOnly) => PreviousDayOfWeek(dateOnly.DayOfWeek);

    public static DayOfWeek PreviousDayOfWeek(this DateTime dateTime) => PreviousDayOfWeek(dateTime.DayOfWeek);

    public static DayOfWeek PreviousDayOfWeek(this DateTimeOffset dateTimeOffset) => PreviousDayOfWeek(dateTimeOffset.DayOfWeek);

    public static DayOfWeek PreviousDayOfWeek(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return PreviousDayOfWeek(timeProvider.GetUtcNow());
    }

    public static DayOfWeek PreviousDayOfWeek(this DayOfWeek dayOfWeek) => (DayOfWeek)(((int)dayOfWeek + 6) % 7);

    #endregion

    #region EndOfMonth

    public static DateTime EndOfMonth(this DateTime dateTime)
    {
        int daysInCurrentMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

        long ticks = CalcEndOfMonthTicks((ulong)dateTime.Ticks, daysInCurrentMonth);

        return new DateTime(ticks, dateTime.Kind);
    }

    public static DateTimeOffset EndOfMonth(this DateTimeOffset dateTimeOffset)
    {
        int daysInCurrentMonth = DateTime.DaysInMonth(dateTimeOffset.Year, dateTimeOffset.Month);

        long ticks = CalcEndOfMonthTicks((ulong)dateTimeOffset.Ticks, daysInCurrentMonth);

        return new DateTimeOffset(ticks, dateTimeOffset.Offset);
    }

    public static DateTime EndOfMonth(this DateTime dateTime, int months)
    {
        DateTime newDate = dateTime.AddMonths(months);

        int daysInMonth = DateTime.DaysInMonth(newDate.Year, newDate.Month);

        long ticks = CalcEndOfMonthTicks((ulong)newDate.Ticks, daysInMonth);

        return new DateTime(ticks, dateTime.Kind);
    }

    public static DateTimeOffset EndOfMonth(this DateTimeOffset dateTimeOffset, int months)
    {
        DateTimeOffset newDate = dateTimeOffset.AddMonths(months);

        int daysInMonth = DateTime.DaysInMonth(newDate.Year, newDate.Month);

        long ticks = CalcEndOfMonthTicks((ulong)newDate.Ticks, daysInMonth);

        return new DateTimeOffset(ticks, dateTimeOffset.Offset);
    }

    public static DateTimeOffset EndOfMonth(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return EndOfMonth(timeProvider.GetUtcNow());
    }

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

    public static DateTime BeginningOfMonth(this DateTime dateTime)
    {
        long ticks = CalcBeginningOfMonthTicks((ulong)dateTime.Ticks);

        return new DateTime(ticks, dateTime.Kind);
    }

    public static DateTimeOffset BeginningOfMonth(this DateTimeOffset dateTimeOffset)
    {
        long ticks = CalcBeginningOfMonthTicks((ulong)dateTimeOffset.Ticks);

        return new DateTimeOffset(ticks, dateTimeOffset.Offset);
    }

    public static DateTime BeginningOfMonth(this DateTime dateTime, int months)
    {
        DateTime newDate = dateTime.AddMonths(months);

        long ticks = CalcBeginningOfMonthTicks((ulong)newDate.Ticks);

        return new DateTime(ticks, dateTime.Kind);
    }

    public static DateTimeOffset BeginningOfMonth(this DateTimeOffset dateTimeOffset, int months)
    {
        DateTimeOffset newDate = dateTimeOffset.AddMonths(months);

        long ticks = CalcBeginningOfMonthTicks((ulong)newDate.Ticks);

        return new DateTimeOffset(ticks, dateTimeOffset.Offset);
    }

    public static DateTimeOffset BeginningOfMonth(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return BeginningOfMonth(timeProvider.GetUtcNow());
    }

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

        // Constants used for fast calculation of following subexpressions
        //      x / DaysPer4Years
        //      x % DaysPer4Years / 4
        // EafMultiplier = (uint)(((1UL << 32) + DaysPer4Years - 1) / DaysPer4Years); // 2,939,745
        // EafDivider    = EafMultiplier * 4;                                         // 11,758,980
        const uint EafMultiplier = 2_939_745;
        const uint EafDivider    = 11_758_980;
        #endregion

        ulong uTicks = ticks & TicksMask;

        // r1 = (day number within 100-year period) * 4
        uint r1 = (((uint)(uTicks / TicksPer6Hours) | 3U) + 1224) % DaysPer400Years;
        uint u2 = EafMultiplier * (r1 | 3u);
        ushort daySinceMarch1 = (ushort)(u2 / EafDivider);
        int n3 = (2141 * daySinceMarch1) + 197_913;

        // Return 1-based day-of-month
        return ((ushort)n3 / 2141) + 1;
    }
}
