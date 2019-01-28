using System;
using System.Globalization;

namespace OSharp.Storyboard.Events
{
    public sealed class Scale : Event
    {
        public override EventType EventType => EventType.Scale; 
       
        public float S1
        {
            get => Start[0];
            set => Start[0] = value;
        }

        public float S2
        {
            get => End[0];
            set => End[0] = value;
        }

        public Scale(EasingType easing, float startTime, float endTime, float s1, float s2):
            base(easing, startTime, endTime, new[] { s1 }, new[] { s2 })
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
