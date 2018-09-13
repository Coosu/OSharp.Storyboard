using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models.EventClass;

namespace Milkitic.OsbLib.Models.EventType
{
    public class MoveY : EventSingle
    {
        public MoveY(EasingType easing, float startTime, float endTime, float y1, float y2)
        => Init("MY", easing, startTime, endTime, y1, y2);

        internal void Adjust(float y)
        {
            Start += y;
            End += y;
        }
    }

}
