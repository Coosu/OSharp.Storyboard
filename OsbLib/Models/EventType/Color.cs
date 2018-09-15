using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Color : Event
    {
        public float R1 => Start[0];
        public float G1 => Start[1];
        public float B1 => Start[2];
        public float R2 => End[0];
        public float G2 => End[1];
        public float B2 => End[2];

        public Color(EasingType easing, float startTime, float endTime, float r1, float g1, float b1, float r2, float g2, float b2)
        : base(easing, startTime, endTime, new[] { r1, g1, b1 }, new[] { r2, g2, b2 }) => EventType = EventEnum.Color;

    }
}
