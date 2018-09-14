using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Vector : Event
    {
        public float Vx1 => Start[0];
        public float Vy1 => Start[1];
        public float Vx2 => End[0];
        public float Vy2 => End[1];

        public Vector(EasingType easing, float startTime, float endTime, float vx1, float vy1, float vx2, float vy2)
        : base(easing, startTime, endTime, new[] { vx1, vy1 }, new[] { vx2, vy2 }) => EventType = EventEnum.Vector;

    }
}
