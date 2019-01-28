using System.Collections.Generic;
using System.Linq;

namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
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
