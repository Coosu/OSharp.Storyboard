using System;

namespace Milkitic.OsbLib.Enums
{
    public enum EventEnum
    {
        Fade, Move, MoveX, MoveY, Scale, Vector, Rotate, Color, Parameter, Loop, Trigger
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

        public static EventEnum ToCommandType(this string shortHand)
        {
            switch (shortHand)
            {
                case "F": return EventEnum.Fade;
                case "M": return EventEnum.Move;
                case "MX": return EventEnum.MoveX;
                case "MY": return EventEnum.MoveY;
                case "S": return EventEnum.Scale;
                case "V": return EventEnum.Vector;
                case "R": return EventEnum.Rotate;
                case "C": return EventEnum.Color;
                case "P": return EventEnum.Parameter;
                case "L": return EventEnum.Loop;
                case "T": return EventEnum.Trigger;
            }
            return EventEnum.Fade;
        }
    }
}
