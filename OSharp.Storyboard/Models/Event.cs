using System;
using System.Globalization;
using System.Linq;
using OSharp.Storyboard.Enums;

namespace OSharp.Storyboard.Models
{
    public class Event
    {
        public EventEnum EventType { get; set; }
        public EasingType Easing { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public float[] Start { get; set; }
        public float[] End { get; set; }
        public string Script => Start.SequenceEqual(End) ? string.Join(",", Start) : $"{string.Join(",", Start)},{string.Join(",", End)}";

        // 扩展
        public int ParamLength => Start.Length;
        public bool IsStatic => Start.Equals(End);

        protected Event(EasingType easing, float startTime, float endTime, float[] start, float[] end)
        {
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = start;
            End = end;
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

        internal void AdjustTime(float time)
        {
            StartTime += time;
            EndTime += time;
        }
    }
}
