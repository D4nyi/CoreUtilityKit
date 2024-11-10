
namespace CoreUtilityKit.EnumAttributionCache.UnitTests.DataGenerators;

internal sealed class EnumAttributeAndTypesData : TheoryData<EnumAttributeValue, Type[]?>
{
    public EnumAttributeAndTypesData()
    {
        IEnumerable<EnumAttributeValue> values = Enum.GetValues<EnumAttributeValue>().Where(x => x != EnumAttributeValue.None);
        Type[]?[] types = [[], null];

        foreach (EnumAttributeValue value in values)
        {
            foreach (Type[]? type in types)
            {
                Add(value, type);
            }
        }
    }
}
