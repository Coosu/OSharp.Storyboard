using System;
using System.Globalization;

namespace OSharp.Storyboard.Events
{
    public sealed class Color : Event
    {
        public override EventType EventType => EventType.Color;

        public float R1
        {
            get => Start[0];
            set => Start[0] = value;
        }

        public float G1
        {
            get => Start[1];
            set => Start[1] = value;
        }

        public float B1
        {
            get => Start[2];
            set => Start[2] = value;
        }

        public float R2
        {
            get => End[0];
            set => End[0] = value;
        }

        public float G2
        {
            get => End[1];
            set => End[1] = value;
        }

        public float B2
        {
            get => End[2];
            set => End[2] = value;
        }

        public Color(EasingType easing, float startTime, float endTime, float r1, float g1, float b1, float r2,
            float g2, float b2):base(easing,startTime, endTime,new[] { r1, g1, b1 }, new[] { r2, g2, b2 })
        {
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
