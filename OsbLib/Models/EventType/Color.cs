using System;
using System.Globalization;
using System.Linq;
using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public struct Color : IEvent
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

        public float R1 => Start[0];
        public float G1 => Start[1];
        public float B1 => Start[2];
        public float R2 => End[0];
        public float G2 => End[1];
        public float B2 => End[2];

        public Color(EasingType easing, float startTime, float endTime, float r1, float g1, float b1, float r2,
            float g2, float b2)
        {
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = new[] { r1, g1, b1 };
            End = new[] { r2, g2, b2 };
            EventType = EventEnum.Color;
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
