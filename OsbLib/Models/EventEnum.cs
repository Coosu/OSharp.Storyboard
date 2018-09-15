using System;

namespace Milkitic.OsbLib.Models
{
    public enum EventEnum
    {
        Fade, Move, MoveX, MoveY, Scale, Vector, Rotate, Color, Parameter
    }

    public static class EventEnumExtension
    {
        public static string ToShortString(this EventEnum eventEnum)
        {
            switch (eventEnum)
            {
                case EventEnum.Fade:
                    return "F";
                case EventEnum.Move:
                    return "M";
                case EventEnum.MoveX:
                    return "MX";
                case EventEnum.MoveY:
                    return "MY";
                case EventEnum.Scale:
                    return "S";
                case EventEnum.Vector:
                    return "V";
                case EventEnum.Rotate:
                    return "R";
                case EventEnum.Color:
                    return "C";
                case EventEnum.Parameter:
                    return "P";
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventEnum), eventEnum, null);
            }
            
        }
    }
}
