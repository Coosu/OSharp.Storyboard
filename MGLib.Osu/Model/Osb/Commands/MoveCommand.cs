using System.Collections.Generic;

namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
{
    public class MoveCommand : NormalizeCommand<Vector2<int>>
    {
        public MoveCommand(EasingType easing, Vector2<int> time,
            IEnumerable<Vector2<int>> param,
            CommandType commandType = CommandType.Move) : base(commandType, easing, time, param)
        {
        }
    }
}
