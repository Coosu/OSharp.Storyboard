using LibOSB.EventClass;
using LibOSB.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.Model.EventType
{
    class Scale : EventSingle
    {
        public Scale(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        {
            Init("S", easing, startTime, endTime, preParam, postParam);
        }
    }
}
