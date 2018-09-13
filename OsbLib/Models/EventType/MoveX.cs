using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models.EventClass;

namespace Milkitic.OsbLib.Models.EventType
{
    public class MoveX : EventSingle
    {
        public MoveX(EasingType easing, float startTime, float endTime, float x1, float x2)
        => Init("MX", easing, startTime, endTime, x1, x2);

        internal void _Adjust(float x)
        {
            Start += x;
            End += x;
        }
    }

}
