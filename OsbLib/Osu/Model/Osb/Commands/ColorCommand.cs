using System;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb.Commands
{
    public class ColorCommand : Command
    {
        public IEnumerable<Range<Vector3<int>>> ColorRanges;
        public ColorCommand(CommandType type, EasingType easing, Vector2<int> time,
            IEnumerable<Range<Vector3<int>>> colorRanges) : base(type, easing, time)
        {
            ColorRanges = colorRanges;
        }
    }
}
