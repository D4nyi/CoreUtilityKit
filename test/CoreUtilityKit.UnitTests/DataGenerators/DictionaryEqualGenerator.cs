namespace CoreUtilityKit.UnitTests.DataGenerators;

internal sealed class DictionaryEqualGenerator : TheoryData<Dictionary<int, int>?, Dictionary<int, int>?, bool>
{
    public DictionaryEqualGenerator()
    {
        Dictionary<int, int> first = new() { { 1, 1 }, { 2, 2 } };
        Dictionary<int, int> second = new() { { 1, 1 }, { 2, 2 } };
        Dictionary<int, int> third = new() { { 2, 2 }, { 1, 1 } };
        Dictionary<int, int> fourth = new() { { 3, 3 }, { 4, 4 } };

        Add(null, null, true);
        Add(first, first, true);
        Add(first, second, true);
        Add(first, third, true);
        Add(second, third, true);

        Add(first, null, false);
        Add(null, first, false);
        Add(first, fourth, false);
        Add(third, fourth, false);
    }
}
