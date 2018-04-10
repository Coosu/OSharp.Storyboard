using LibOsb.EventClass;
using LibOsb.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOsb.Model.EventType
{
    class Rotate : EventSingle
    {
        public Rotate(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        => Init("R", easing, startTime, endTime, preParam, postParam);
    }
}
