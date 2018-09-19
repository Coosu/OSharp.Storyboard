using MGLib.Osu.Model.Osb.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb
{
    public class Command
    {
        public CommandType CommandType;
        public EasingType Easing;
        public Vector2<int> Time;

        public Command(CommandType type, EasingType easing, Vector2<int> time)
        {
            CommandType = type;
            Easing = easing;
            Time = time;
        }
        
    }
}
