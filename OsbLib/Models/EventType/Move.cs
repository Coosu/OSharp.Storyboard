using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models.EventClass;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Move : EventDouble
    {
        public Move(EasingType easing, float startTime, float endTime, (float x, float y) start, (float x, float y) end)
            : this(easing, startTime, endTime, start.x, start.y, end.x, end.y)
        {
        }

        public Move(EasingType easing, float startTime, float endTime, float x1, float y1, float x2, float y2)
        {
            Init("M", easing, startTime, endTime, x1, y1, x2, y2);
        }

        internal void _Adjust(float x, float y)
        {
            Start = (Start.x + x, Start.y + y);
            End = (End.x + x, End.y + y);
        }
    }
}
