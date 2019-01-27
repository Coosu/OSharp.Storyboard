using System;
using System.Globalization;
using System.Linq;
using OSharp.Storyboard.Enums;

namespace OSharp.Storyboard.Models.EventType
{
    public class MoveX : IEvent
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

        public float X1 => Start[0];
        public float X2 => End[0];

        public MoveX(EasingType easing, float startTime, float endTime, float x1, float x2)
        {
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = new[] { x1 };
            End = new[] { x2 };
            EventType = EventEnum.MoveX;
        }

        public void Adjust(float x)
        {
            Start[0] += x;
            End[0] += x;
        }

        public void AdjustTime(float time)
        {
            StartTime += time;
            EndTime += time;
        }

        public override string ToString()
        {
            return string.Join(",",
                EventType.ToShortString(),
                (int)Easing,
                Math.Round(StartTime).ToString(CultureInfo.InvariantCulture),
                StartTime.Equals(EndTime) ? "" : Math.Round(EndTime).ToString(CultureInfo.InvariantCulture),
                Script);
        }
    }
}
