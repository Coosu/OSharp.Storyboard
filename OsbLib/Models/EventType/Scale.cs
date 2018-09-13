using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models.EventClass;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Scale : EventSingle
    {
        public Scale(EasingType easing, float startTime, float endTime, float s1, float s2)
        => Init("S", easing, startTime, endTime, s1, s2);
    }
}
