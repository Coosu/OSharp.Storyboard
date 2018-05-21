using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOsb.EventClass;
using LibOsb.Model.Constants;

namespace LibOsb.Model.EventType
{
    internal class Move : EventDouble
    {
        public Move(EasingType easing, int startTime, int endTime, double preParam1, double preParam2, double postParam1, double postParam2)
        => Init("M", easing, startTime, endTime, preParam1, preParam2, postParam1, postParam2);
        
        internal void _Adjust(double x, double y)
        {
            P11 += x;
            P12 += y;
            P21 += x;
            P22 += y;
            BuildParams();
        }
    }
}
