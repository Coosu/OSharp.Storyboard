namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
{
    public enum CommandType
    {
        Fade,
        Move,
        MoveX,
        MoveY,
        Scale,
        VectorScale,
        Rotate,
        Color,
        Parameter,
        Loop,
        Trigger,
    }

    public static class CommandTypeHelper
    {
        public static string GetShortHand(this CommandType command)
        {
            switch (command)
            {   
                case CommandType.MoveX:
                    return "MX";
                case CommandType.MoveY:
                    return "MY";
                default:
                    return command.ToString().Substring(0, 1);
            }
        }

        public static CommandType ToCommandType(this string shortHand)
        {
            switch (shortHand)
            {
                case "F": return CommandType.Fade;
                case "M": return CommandType.Move;
                case "MX": return CommandType.MoveX;
                case "MY": return CommandType.MoveY;
                case "S": return CommandType.Scale;
                case "V": return CommandType.VectorScale;
                case "R": return CommandType.Rotate;
                case "C": return CommandType.Color;
                case "P": return CommandType.Parameter;
                case "L": return CommandType.Loop;
                case "T": return CommandType.Trigger;
            }
            return CommandType.Fade;
        }
    }
}
