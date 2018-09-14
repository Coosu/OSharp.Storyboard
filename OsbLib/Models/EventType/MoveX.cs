using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class MoveX : Event
    {
        public float X1 => Start[0];
        public float X2 => End[0];

        public MoveX(EasingType easing, float startTime, float endTime, float x1, float x2)
            : base(easing, startTime, endTime, new[] { x1 }, new[] { x2 }) => EventType = EventEnum.MoveX;

        public void Adjust(float x)
        {
            Start[0] += x;
            End[0] += x;
        }
    }
}
