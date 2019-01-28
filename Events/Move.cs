using System;
using System.Globalization;

namespace OSharp.Storyboard.Events
{
    public sealed class Move : Event, IAdjustablePositionEvent
    {
        public override EventType EventType => EventType.Move;

        public float X1
        {
            get => Start[0];
            set => Start[0] = value;
        }

        public float Y1
        {
            get => Start[1];
            set => Start[1] = value;
        }

        public float X2
        {
            get => End[0];
            set => End[0] = value;
        }

        public float Y2
        {
            get => End[1];
            set => End[1] = value;
        }

        public Move(EasingType easing, float startTime, float endTime, float x1, float y1, float x2, float y2) :
            base(easing, startTime, endTime, new[] { x1, y1 }, new[] { x2, y2 })
        {
        }

        public void AdjustPosition(float x, float y)
        {
            Start[0] += x;
            Start[1] += y;
            End[0] += x;
            End[1] += y;
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
