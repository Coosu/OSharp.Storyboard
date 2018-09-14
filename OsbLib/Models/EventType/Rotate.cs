using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Rotate : Event
    {
        public float R1 => Start[0];
        public float R2 => End[0];

        public Rotate(EasingType easing, float startTime, float endTime, float r1, float r2)
            : base(easing, startTime, endTime, new[] { r1 }, new[] { r2 }) => EventType = EventEnum.Rotate;
    }
}
