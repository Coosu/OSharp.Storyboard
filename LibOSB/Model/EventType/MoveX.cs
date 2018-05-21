using LibOsb.EventClass;
using LibOsb.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOsb.Model.EventType
{
    internal class MoveX : EventSingle
    {
        public MoveX(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        => Init("MX", easing, startTime, endTime, preParam, postParam);

        internal void _Adjust(double x)
        {
            P11 += x;
            P21 += x;
            BuildParams();
        }
    }

}
