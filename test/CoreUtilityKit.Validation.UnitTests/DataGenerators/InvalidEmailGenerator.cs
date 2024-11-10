namespace CoreUtilityKit.Validation.UnitTests.DataGenerators;

internal sealed class InvalidEmailGenerator : TheoryData<string?>
{
    public InvalidEmailGenerator()
    {
        Add(null);
        Add("");
        Add("\r \t \n \r\n");
        Add("example.com");
        Add("@example.com");
        Add("email@example.");
        Add("email@example");
        Add("email@.com");
        Add("toolonglocal.asdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasda@example.com");
        Add("asd@toolongdomainsdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasd.com");
        Add("toolongsdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasd@asdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasd.com");
    }
}
