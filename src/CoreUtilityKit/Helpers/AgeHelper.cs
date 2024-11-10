namespace CoreUtilityKit.Helpers;

internal sealed class AgeHelper : IAgeHelper
{
    private readonly TimeProvider _timeProvider;

    public AgeHelper(TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        _timeProvider = timeProvider;
    }

    public DateTime CalculateDateTimeBirthYear(int age)
    {
        int birthYear = DateTime.UtcNow.AddYears(-age).Year;

        return new DateTime(birthYear, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }

    public int CalculateBirthYear(int age) => DateTime.UtcNow.AddYears(-age).Year;

    public int CalculateAge(DateTime birthDate) => CalculateAgeCore(birthDate.ToUniversalTime());

    public int CalculateAge(DateTimeOffset birthDate) => CalculateAgeCore(birthDate.UtcDateTime);

    public int CalculateAge(DateOnly birthDate) => CalculateAgeCore(new(birthDate.DayNumber * TimeSpan.TicksPerDay));

    private int CalculateAgeCore(DateTime birthDate)
    {
        DateTime utcNow = _timeProvider.GetUtcNow().UtcDateTime;

        int age = utcNow.Year - birthDate.Year;

        // For leap years we need this
        if (birthDate > utcNow.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}
