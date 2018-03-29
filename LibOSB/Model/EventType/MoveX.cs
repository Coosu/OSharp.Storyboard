using LibOSB.EventClass;
using LibOSB.Constants;
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
    }

}
