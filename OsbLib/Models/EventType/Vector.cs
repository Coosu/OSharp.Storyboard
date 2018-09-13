using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models.EventClass;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Vector : EventDouble
    {
        public Vector(EasingType easing, float startTime, float endTime, (float vx, float vy) start, (float vx, float vy) end)
            : this(easing, startTime, endTime, start.vx, start.vy, end.vx, end.vy)
        {
        }

        public Vector(EasingType easing, float startTime, float endTime, float vx1, float vy1, float vx2, float vy2)
        {
            Init("V", easing, startTime, endTime, vx1, vy1, vx2, vy2);
        }
    }
}
