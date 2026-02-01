using CoreUtilityKit.UnitTests.DataGenerators.Models;

namespace CoreUtilityKit.UnitTests.DataGenerators;

internal sealed class UrlHelpersNParamGenerator : TheoryData<string[]>
{
    public UrlHelpersNParamGenerator()
    {
        foreach (string[] parts in UrlHelpersGenerator.GenerateParts(5))
        {
            Add(parts);
        }
    }
}

internal sealed class UrlHelpersNParamThrowsGenerator : TheoryData<string?[]>
{
    public UrlHelpersNParamThrowsGenerator()
    {
        foreach (string?[] parts in UrlHelpersGenerator.GenerateThrowsParts(5))
        {
            Add(parts);
        }
    }
}
