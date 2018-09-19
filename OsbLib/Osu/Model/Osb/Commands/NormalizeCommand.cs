using System;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb.Commands
{
    public class NormalizeCommand<T> : Command where T : struct
    {
        public IEnumerable<T> Params;
        public NormalizeCommand(CommandType type, EasingType easing, Vector2<int> time, IEnumerable<T> param) : base(type, easing, time)
        {
            Params = param;
        }
    }
}
