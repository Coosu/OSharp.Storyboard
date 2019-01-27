using System.Collections.Generic;

namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
{
    public class FadeCommand : NormalizeCommand<Vector2<int>>
    {
        public FadeCommand(EasingType easing, Vector2<int> time, IEnumerable<Vector2<int>> param) : base(CommandType.Fade, easing, time, param)
        {
        }
    }
}
