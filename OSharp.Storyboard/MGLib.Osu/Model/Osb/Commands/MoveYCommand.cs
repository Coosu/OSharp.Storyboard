using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGLib.Osu.Model.Osb.Commands
{
    public class MoveYCommand : MoveCommand
    {
        public MoveYCommand(EasingType easing, Vector2<int> time,
            IEnumerable<int> y) 
            : base(easing, time, 
                  y.Select(vec => new Vector2<int> { Y = vec } ),
                  CommandType.MoveY)
        {
        }
    }
}
