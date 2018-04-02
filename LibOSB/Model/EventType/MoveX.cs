using LibOSB.EventClass;
using LibOSB.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.Model.EventType
{
    class MoveX : EventSingle
    {
        public MoveX(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        {
            Init("MX", easing, startTime, endTime, preParam, postParam);
        }
        internal void _Adjust(double x)
        {
            P1_1 += x;
            P2_1 += x;
            BuildParams();
        }
    }

}
