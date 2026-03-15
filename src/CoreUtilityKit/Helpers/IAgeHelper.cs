
namespace CoreUtilityKit.Helpers;

/// <summary>
/// Provides methods for age-related calculations.
/// </summary>
public interface IAgeHelper
{
    /// <summary>
    /// Calculates the birth date as <see cref="DateTime"/> for the specified age.
    /// </summary>
    /// <param name="age">The age in years.</param>
    /// <returns>A <see cref="DateTime"/> representing the first day of the calculated birth year.</returns>
    DateTime CalculateDateTimeBirthYear(int age);

    /// <summary>
    /// Calculates the birth year for the specified age.
    /// </summary>
    /// <param name="age">The age in years.</param>
    /// <returns>The birth year.</returns>
    int CalculateBirthYear(int age);

    /// <summary>
    /// Calculates the current age for a given birth date.
    /// </summary>
    /// <param name="birthDate">The birth date.</param>
    /// <returns>The calculated age.</returns>
    int CalculateAge(DateOnly birthDate);

    /// <summary>
    /// Calculates the current age for a given birth date.
    /// </summary>
    /// <param name="birthDate">The birth date.</param>
    /// <returns>The calculated age.</returns>
    int CalculateAge(DateTime birthDate);

    /// <summary>
    /// Calculates the current age for a given birth date.
    /// </summary>
    /// <param name="birthDate">The birth date.</param>
    /// <returns>The calculated age.</returns>
    int CalculateAge(DateTimeOffset birthDate);
}
