using LibOsb.Enums;
using LibOsb.Models.EventClass;

namespace LibOsb.Models.EventType
{
    internal class Vector : EventDouble
    {
        public Vector(EasingType easing, int startTime, int endTime, double preParam1, double preParam2, double postParam1, double postParam2)
        => Init("V", easing, startTime, endTime, preParam1, preParam2, postParam1, postParam2);
    }
}
