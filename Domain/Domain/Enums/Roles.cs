using System.ComponentModel;
using System.Reflection;

namespace Domain;
public enum EnumUserGroup
{
    [Description(EnumUserGroupSettings.None)]
    None,
    [Description(EnumUserGroupSettings.Grupa1)]
    Grupa1,
    [Description(EnumUserGroupSettings.Grupa2)]
    Grupa2,
    [Description(EnumUserGroupSettings.Grupa3)]
    Grupa3,
    [Description(EnumUserGroupSettings.Grupa4)]
    Grupa4,
    [Description(EnumUserGroupSettings.Grupa5)]
    Grupa5,

}

public static class EnumUserGroupSettings
{
    internal const string None = "None";
    internal const string Grupa1 = "Grupa1";
    internal const string Grupa2 = "Grupa2";
    internal const string Grupa3 = "Grupa3";
    internal const string Grupa4 = "Grupa4";
    internal const string Grupa5 = "Grupa5";
}

public static class EnumConverters
{
    public static string GetDescription(this Enum value)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo == null) return "";
        var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
        if (attribute == null) return "";

        return attribute.Description;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}