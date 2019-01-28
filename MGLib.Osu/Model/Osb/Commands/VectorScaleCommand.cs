using System.Collections.Generic;

namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
{
    public class VectorScaleCommand : NormalizeCommand<Range<Vector2<double>>>
    {
        public VectorScaleCommand(EasingType easing, Vector2<int> time,
            IEnumerable<Range<Vector2<double>>> param) : base(CommandType.VectorScale, easing, time, param)
        {
        }
    }
}
