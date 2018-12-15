using System;
using System.Globalization;
using Milkitic.OsbLib.Enums;
using System.Linq;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Parameter : IEvent
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
        public ParameterEnum Type => (ParameterEnum)(int)Start[0];

        public Parameter(EasingType easing, float startTime, float endTime, ParameterEnum type)
        {
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = new[] { (float)(int)type };
            End = new[] { (float)(int)type };
            EventType = EventEnum.Parameter;
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
