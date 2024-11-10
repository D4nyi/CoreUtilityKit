
namespace CoreUtilityKit.Helpers;

public interface IAgeHelper
{
    DateTime CalculateDateTimeBirthYear(int age);
    int CalculateBirthYear(int age);
    int CalculateAge(DateOnly birthDate);
    int CalculateAge(DateTime birthDate);
    int CalculateAge(DateTimeOffset birthDate);
}