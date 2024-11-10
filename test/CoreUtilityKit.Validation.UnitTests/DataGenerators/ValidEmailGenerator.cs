namespace CoreUtilityKit.Validation.UnitTests.DataGenerators;

internal sealed class ValidEmailGenerator : TheoryData<string>
{
    public ValidEmailGenerator()
    {
        Add("email@example.com");
        Add("email@example.co.uk");
        Add("email@하나.kr");
    }
}
