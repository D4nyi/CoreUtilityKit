using CoreUtilityKit.UnitTests.DataGenerators.Models;

namespace CoreUtilityKit.UnitTests.DataGenerators;

internal sealed class UrlHelpers4ParamGenerator : TheoryData<string, string, string, string>
{
    public UrlHelpers4ParamGenerator()
    {
        foreach (string[] parts in UrlHelpersGenerator.GenerateParts(4))
        {
            Add(parts[0], parts[1], parts[2], parts[3]);
        }
    }
}

internal sealed class UrlHelpers4ParamThrowsGenerator : TheoryData<string?, string?, string?, string?>
{
    public UrlHelpers4ParamThrowsGenerator()
    {
        foreach (string?[] parts in UrlHelpersGenerator.GenerateThrowsParts(4))
        {
            Add(parts[0], parts[1], parts[2], parts[3]);
        }
    }
}
