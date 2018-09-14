using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Fade : Event
    {
        public float F1 => Start[0];
        public float F2 => End[0];

        public Fade(EasingType easing, float startTime, float endTime, float f1, float f2)
            : base(easing, startTime, endTime, new[] { f1 }, new[] { f2 }) => EventType = EventEnum.Fade;
    }
}
