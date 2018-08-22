using LibOsb.Enums;
using LibOsb.Models.EventClass;

namespace LibOsb.Models.EventType
{
    internal class MoveY : EventSingle
    {
        public MoveY(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        => Init("MY", easing, startTime, endTime, preParam, postParam);

        internal void Adjust(double y)
        {
            Start += y;
            End += y;
        }
    }

}
