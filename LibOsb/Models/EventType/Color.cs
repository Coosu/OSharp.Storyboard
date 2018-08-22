using LibOsb.Enums;
using LibOsb.Models.EventClass;

namespace LibOsb.Models.EventType
{
    internal class Color : EventTriple
    {
        public Color(EasingType easing, int startTime, int endTime,
            double preParam1, double preParam2, double preParam3, double postParam1, double postParam2, double postParam3)
         => Init("C", easing, startTime, endTime, preParam1, preParam2, preParam3, postParam1, postParam2, postParam3);
    }
}
