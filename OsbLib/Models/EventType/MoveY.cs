using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class MoveY : Event
    {
        public float Y1 => Start[0];
        public float Y2 => End[0];

        public MoveY(EasingType easing, float startTime, float endTime, float y1, float y2)
            : base(easing, startTime, endTime, new[] { y1 }, new[] { y2 }) => EventType = EventEnum.MoveY;

        public void Adjust(float y)
        {
            Start[0] += y;
            End[0] += y;
        }
    }
}
