using LibOsb.Enums;
using LibOsb.Models.EventClass;

namespace LibOsb.Models.EventType
{
    internal class MoveX : EventSingle
    {
        public MoveX(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        => Init("MX", easing, startTime, endTime, preParam, postParam);

        internal void _Adjust(double x)
        {
            Start += x;
            End += x;
        }
    }

}
