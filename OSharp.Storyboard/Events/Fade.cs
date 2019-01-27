using System;
using System.Globalization;

namespace OSharp.Storyboard.Events
{
    public sealed class Fade : Event
    {
        public override EventType EventType => EventType.Fade;

        public float F1
        {
            get => Start[0];
            set => Start[0] = value;
        }

        public float F2
        {
            get => End[0];
            set => End[0] = value;
        }

        public Fade(EasingType easing, float startTime, float endTime, float f1, float f2)
            : base(easing, startTime, endTime, new[] { f1 }, new[] { f2 })
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
