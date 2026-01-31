using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;

namespace CoreUtilityKit.EnumAttributionCache;

public enum EnumAttributeValue
{
    None = 0,
    Description,
    EnumMemberValue,
    DisplayName,
    DisplayDescription,
}

internal static class EnumAttributeReaderFactory
{
    private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Static;

    public static Dictionary<Enum, string> GenerateDictionary(EnumAttributeValue attributeValue, Type[] enums)
    {
        ArgumentNullException.ThrowIfNull(enums);
        ArgumentOutOfRangeException.ThrowIfZero(enums.Length);

        return attributeValue switch
        {
            EnumAttributeValue.Description => CreateDictionary<DescriptionAttribute>(enums, static x => x.Description),
            EnumAttributeValue.EnumMemberValue => CreateDictionary<EnumMemberAttribute>(enums, static x => x.Value!),
            EnumAttributeValue.DisplayName => CreateDictionary<DisplayAttribute>(enums, static x => x.Name!),
            EnumAttributeValue.DisplayDescription => CreateDictionary<DisplayAttribute>(enums, static x => x.Description!),
            _ => throw new UnreachableException($"Value {attributeValue} is not supported!"),
        };
    }

    public static Func<Enum, string?> GetSingleReader(EnumAttributeValue attributeValue)
    {
        return attributeValue switch
        {
            EnumAttributeValue.Description => GetDescription,
            EnumAttributeValue.EnumMemberValue => GetEnumMemberValue,
            EnumAttributeValue.DisplayName => GetDisplayName,
            EnumAttributeValue.DisplayDescription => GetDisplayDescription,
            _ => throw new UnreachableException($"Value {attributeValue} is not supported!"),
        };
    }

    private static Dictionary<Enum, string> CreateDictionary<T>(Type[] enums, Func<T, string> accessor) where T : Attribute
    {
        Dictionary<Enum, string> dict = [];

        foreach (FieldInfo fi in enums.SelectMany(([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] x) => x.GetFields(Flags)))
        {
            if (fi.GetCustomAttributes(typeof(T), false) is T[] { Length: > 0 } attributes)
            {
                string value = accessor(attributes[0]);
                if (!String.IsNullOrEmpty(value))
                {
                    dict.Add((Enum)fi.GetValue(null)!, value);
                }
            }
        }

        return dict;
    }

    private static string? GetDescription(Enum key) => GetAttribute<DescriptionAttribute>(key);

    private static string? GetEnumMemberValue(Enum key) => GetAttribute<EnumMemberAttribute>(key);

    private static string? GetDisplayName(Enum key) => GetAttribute<DisplayAttribute>(key, true);

    private static string? GetDisplayDescription(Enum key) => GetAttribute<DisplayAttribute>(key);

    private static string? GetAttribute<T>(Enum key, bool displayName = false) where T : Attribute
    {
        ArgumentNullException.ThrowIfNull(key);

        if (key
                .GetType()
                .GetField(key.ToString())
                !.GetCustomAttributes(typeof(T), false) is not T?[] { Length: > 0 } attributes)
        {
            return null;
        }

        T? attribute = attributes[0];

        return attribute switch
        {
            DescriptionAttribute description => description.Description,
            EnumMemberAttribute enumMember => enumMember.Value,
            DisplayAttribute displayAttribute => displayName ? displayAttribute.Name : displayAttribute.Description,
            _ => throw new UnreachableException($"Unknown attribute provided! ({typeof(T).FullName})")
        };
    }
}
