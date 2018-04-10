using LibOsb.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOsb.EventClass;

namespace LibOsb.Model.EventType
{
    class Fade : EventSingle
    {
        public Fade(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        => Init("F", easing, startTime, endTime, preParam, postParam);
    }
}
