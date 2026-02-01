using CoreUtilityKit.UnitTests.DataGenerators.Models;

namespace CoreUtilityKit.UnitTests.DataGenerators;

internal sealed class UrlHelpers3ParamGenerator : TheoryData<string, string, string>
{
    public UrlHelpers3ParamGenerator()
    {
        foreach (string[] parts in UrlHelpersGenerator.GenerateParts(3))
        {
            Add(parts[0], parts[1], parts[2]);
        }
    }
}

internal sealed class UrlHelpers3ParamThrowsGenerator : TheoryData<string?, string?, string?>
{
    public UrlHelpers3ParamThrowsGenerator()
    {
        foreach (string?[] parts in UrlHelpersGenerator.GenerateThrowsParts(3))
        {
            Add(parts[0], parts[1], parts[2]);
        }
    }
}
