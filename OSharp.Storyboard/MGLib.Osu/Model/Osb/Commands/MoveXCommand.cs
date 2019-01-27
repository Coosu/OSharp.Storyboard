using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb.Commands
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
