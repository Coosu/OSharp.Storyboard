using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models.EventClass;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Color : EventTriple
    {
        public Color(EasingType easing, float startTime, float endTime, (float r, float g, float b) start,
            (float r, float g, float b) end)
            : this(easing, startTime, endTime, start.r, start.g, start.b, end.r, end.g, end.b)
        {

        }

        public Color(EasingType easing, float startTime, float endTime,
            float preParam1, float preParam2, float preParam3, float postParam1, float postParam2, float postParam3)
        {
            Init("C", easing, startTime, endTime, preParam1, preParam2, preParam3, postParam1, postParam2, postParam3);
        }
    }
}
