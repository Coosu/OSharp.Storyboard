using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Scale : Event
    {
        public float S1 => Start[0];
        public float S2 => End[0];

        public Scale(EasingType easing, float startTime, float endTime, float s1, float s2)
            : base(easing, startTime, endTime, new[] { s1 }, new[] { s2 }) => EventType = EventEnum.Scale;
    }
}
