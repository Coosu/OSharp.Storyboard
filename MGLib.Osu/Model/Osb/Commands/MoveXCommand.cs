using System.Collections.Generic;
using System.Linq;

namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
{
    public class MoveXCommand : MoveCommand
    {
        public MoveXCommand(EasingType easing, Vector2<int> time,
            IEnumerable<int> x)
            : base(easing, time,
                  x.Select(vec => new Vector2<int> { X = vec }),
                  CommandType.MoveX)
        {
        }
    }
}
