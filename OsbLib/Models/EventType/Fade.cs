using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models.EventClass;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Fade : EventSingle
    {
        public Fade(EasingType easing, float startTime, float endTime, float f1, float f2)
        => Init("F", easing, startTime, endTime, f1, f2);
    }
}
