using Milkitic.OsbLib.Enums;
using System.Linq;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Scale : IEvent
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
        public float S1 => Start[0];
        public float S2 => End[0];

        public Scale(EasingType easing, float startTime, float endTime, float s1, float s2)
        {
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = new[] { s1 };
            End = new[] { s2 };
            EventType = EventEnum.Scale;
        }

        public void AdjustTime(float time)
        {
            StartTime += time;
            EndTime += time;
        }
    }
}
