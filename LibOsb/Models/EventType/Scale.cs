using LibOsb.Enums;
using LibOsb.Models.EventClass;

namespace LibOsb.Models.EventType
{
    internal class Scale : EventSingle
    {
        public Scale(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        => Init("S", easing, startTime, endTime, preParam, postParam);
    }
}
