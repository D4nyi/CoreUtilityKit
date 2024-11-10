using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CoreUtilityKit.EnumAttributionCache.UnitTests;

[Flags]
public enum Color
{
    [Description(ColorNames.Red), EnumMember(Value = ColorNames.Red), Display(Name = ColorNames.Red, Description = ColorNames.Red)]
    Red = 8,
    [Description(ColorNames.Green), EnumMember(Value = ColorNames.Green), Display(Name = ColorNames.Green, Description = ColorNames.Green)]
    Green = 4,
    [Description(ColorNames.Black), EnumMember(Value = ColorNames.Black), Display(Name = ColorNames.Black, Description = ColorNames.Black)]
    Black = 1,
    [Description(ColorNames.Blue), EnumMember(Value = ColorNames.Blue), Display(Name = ColorNames.Blue, Description = ColorNames.Blue)]
    Blue = 2,
    None = 0
}

public static class ColorNames
{
    public const string Red   = "Crimson red";
    public const string Green = "Pine";
    public const string Black = "Vantablack";
    public const string Blue  = "Sky";

    public static readonly IReadOnlyDictionary<Color, string> Lookup = new Dictionary<Color, string>
    {
        { Color.Red,   Red },
        { Color.Green, Green },
        { Color.Black, Black },
        { Color.Blue,  Blue }
    };
}