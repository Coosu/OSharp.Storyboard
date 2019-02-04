using System;
using System.Globalization;
using System.Linq;

namespace OSharp.Storyboard.Events
{
    public abstract class CommonEvent : ICommonEvent, IAdjustableTimingEvent, IComparable<CommonEvent>
    {
        public abstract EventType EventType { get; }
        public EasingType Easing { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public float[] Start { get; }
        public float[] End { get; }

        protected virtual string Script => Start.SequenceEqual(End)
            ? string.Join(",", Start)
            : $"{string.Join(",", Start)},{string.Join(",", End)}";

        public virtual int ParamLength => Start.Length;
        public virtual bool IsStatic => Start.Equals(End);

        protected CommonEvent(EasingType easing, float startTime, float endTime, float[] start, float[] end)
        {
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = start;
            End = end;
        }

        public int CompareTo(CommonEvent other)
        {
            if (other == null)
                return 1;

            if (StartTime > other.StartTime)
                return 1;

            if (StartTime.Equals(other.StartTime))
                return 0;

            if (StartTime < other.StartTime)
                return -1;

            throw new ArgumentOutOfRangeException(nameof(other));
        }

        public override string ToString()
        {
            return ToOsbString();
        }

        public virtual string ToOsbString()
        {
            return string.Join(",",
                EventType.ToShortString(),
                (int)Easing,
                Math.Round(StartTime).ToString(CultureInfo.InvariantCulture),
                StartTime.Equals(EndTime)
                    ? ""
                    : Math.Round(EndTime).ToString(CultureInfo.InvariantCulture),
                Script);
        }

        public void AdjustTiming(float time)
        {
            StartTime += time;
            EndTime += time;
        }
    }
}
