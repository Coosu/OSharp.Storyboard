using LibOsb.Enums;
using LibOsb.Models.EventClass;

namespace LibOsb.Models.EventType
{
    internal class Rotate : EventSingle
    {
        public Rotate(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        => Init("R", easing, startTime, endTime, preParam, postParam);
    }
}
