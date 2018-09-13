using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models.EventClass;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Rotate : EventSingle
    {
        public Rotate(EasingType easing, float startTime, float endTime, float r1, float r2)
        => Init("R", easing, startTime, endTime, r1, r2);
    }
}
