using System;
using System.Globalization;

namespace OSharp.Storyboard.Events
{
    public class Rotate : Event
    {
        public override EventType EventType => EventType.Rotate;

        public float R1
        {
            get => Start[0];
            set => Start[0] = value;
        }

        public float R2
        {
            get => End[0];
            set => End[0] = value;
        }

        public Rotate(EasingType easing, float startTime, float endTime, float r1, float r2) :
            base(easing, startTime, endTime, new[] { r1 }, new[] { r2 })
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
