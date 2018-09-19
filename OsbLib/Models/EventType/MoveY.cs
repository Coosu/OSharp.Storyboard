using System.Linq;
using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class MoveY : IEvent
    {
        public EventEnum EventType { get; set; }
        public EasingType Easing { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public float[] Start { get; set; }
        public float[] End { get; set; }
        public string Script => Start.SequenceEqual(End) ? string.Join(",", Start) : $"{string.Join(",", Start)},{string.Join(",", End)}";
        public int ParamLength => Start.Length;
        public bool IsStatic => Start.Equals(End);
        public float Y1 => Start[0];
        public float Y2 => End[0];

        public MoveY(EasingType easing, float startTime, float endTime, float y1, float y2)
        {
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = new[] { y1 };
            End = new[] { y2 };
            EventType = EventEnum.MoveY;
        }

        public void Adjust(float y)
        {
            Start[0] += y;
            End[0] += y;
        }

        public void AdjustTime(float time)
        {
            StartTime += time;
            EndTime += time;
        }
    }
}
