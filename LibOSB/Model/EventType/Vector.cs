using LibOSB.EventClass;
using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.Model.EventType
{
    class Vector : EventDouble
    {
        public Vector(EasingType easing, int startTime, int endTime, double preParam1, double preParam2, double postParam1, double postParam2)
        {
            Init("V", easing, startTime, endTime, preParam1, preParam2, postParam1, postParam2);
        }
    }
}
