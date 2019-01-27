using System;

namespace OSharp.Storyboard.Models
{
    public enum ParameterEnum
    {
        Horizontal, Vertical, Additive
    }

    public static class PEnumExtension
    {
        public static string ToShortString(this ParameterEnum pEnum)
        {
            switch (pEnum)
            {
                case ParameterEnum.Horizontal:
                    return "H";
                case ParameterEnum.Vertical:
                    return "V";
                case ParameterEnum.Additive:
                    return "A";
                default:
                    throw new ArgumentOutOfRangeException(nameof(pEnum), pEnum, null);
            }
        }

        public static ParameterEnum ToEnum(this string str)
        {
            switch (str)
            {
                case "H":
                    return ParameterEnum.Horizontal;
                case "V":
                    return ParameterEnum.Vertical;
                case "A":
                    return ParameterEnum.Additive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(str), str, null);
            }
        }
    }
}
