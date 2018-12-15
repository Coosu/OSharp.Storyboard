using System;
using System.Globalization;
using Milkitic.OsbLib.Enums;
using System.Linq;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Move : IEvent
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
        public float Y1 => Start[1];
        public float X2 => End[0];
        public float Y2 => End[1];

        public Move(EasingType easing, float startTime, float endTime, float x1, float y1, float x2, float y2)
        {
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = new[] { x1, y1 };
            End = new[] { x2, y2 };
            EventType = EventEnum.Move;
        }

        public void Adjust(float x, float y)
        {
            Start[0] += x;
            Start[1] += y;
            End[0] += x;
            End[1] += y;
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
