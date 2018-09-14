using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Move : Event
    {
        public float X1 => Start[0];
        public float Y1 => Start[1];
        public float X2 => End[0];
        public float Y2 => End[1];

        public Move(EasingType easing, float startTime, float endTime, float x1, float y1, float x2, float y2)
        : base(easing, startTime, endTime, new[] { x1, y1 }, new[] { x2, y2 }) => EventType = EventEnum.Move;

        public void Adjust(float x, float y)
        {
            Start[0] += x;
            Start[1] += y;
            End[0] += x;
            End[1] += y;
        }
    }
}
