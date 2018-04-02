using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB.EventClass;
using LibOSB.Model.Constants;

namespace LibOSB.Model.EventType
{
    class Move : EventDouble
    {
        public Move(EasingType easing, int startTime, int endTime, double preParam1, double preParam2, double postParam1, double postParam2)
        {
            Init("M", easing, startTime, endTime, preParam1, preParam2, postParam1, postParam2);
        }

        internal void _Adjust(double x, double y)
        {
            P1_1 += x;
            P1_2 += y;
            P2_1 += x;
            P2_2 += y;
            BuildParams();
        }
    }
}
