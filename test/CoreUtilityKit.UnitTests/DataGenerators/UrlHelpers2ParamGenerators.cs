using CoreUtilityKit.UnitTests.DataGenerators.Models;

namespace CoreUtilityKit.UnitTests.DataGenerators;

internal sealed class UrlHelpers2ParamGenerator : TheoryData<string, string>
{
    public UrlHelpers2ParamGenerator()
    {
        foreach (string[] parts in UrlHelpersGenerator.GenerateParts(2))
        {
            Add(parts[0], parts[1]);
        }
    }
}

internal sealed class UrlHelpers2ParamThrowsGenerator : TheoryData<string?, string?>
{
    public UrlHelpers2ParamThrowsGenerator()
    {
        foreach (string?[] parts in UrlHelpersGenerator.GenerateThrowsParts(2))
        {
            Add(parts[0], parts[1]);
        }
    }
}
