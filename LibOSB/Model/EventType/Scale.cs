using LibOsb.EventClass;
using LibOsb.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOsb.Model.EventType
{
    internal class Scale : EventSingle
    {
        public Scale(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        => Init("S", easing, startTime, endTime, preParam, postParam);
    }
}
