using LibOsb.Enums;
using LibOsb.Models.EventClass;

namespace LibOsb.Models.EventType
{
    internal class Fade : EventSingle
    {
        public Fade(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        => Init("F", easing, startTime, endTime, preParam, postParam);
    }
}
