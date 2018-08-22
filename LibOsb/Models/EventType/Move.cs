using LibOsb.Enums;
using LibOsb.Models.EventClass;

namespace LibOsb.Models.EventType
{
    internal class Move : EventDouble
    {
        public Move(EasingType easing, int startTime, int endTime, double preParam1, double preParam2, double postParam1, double postParam2)
        => Init("M", easing, startTime, endTime, preParam1, preParam2, postParam1, postParam2);

        internal void _Adjust(double x, double y)
        {
            Start = (Start.x + x, Start.y + y);
            End = (End.x + x, End.y + y);
        }
    }
}
