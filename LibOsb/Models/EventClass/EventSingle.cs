using System.Globalization;
using LibOsb.Enums;

namespace LibOsb.Models.EventClass
{
    class EventSingle : Event
    {
        public double Start { get; internal set; }
        public double End { get; internal set; }
        public override string ScriptParams =>
            Start.Equals(End)
                ? Start.ToString(CultureInfo.InvariantCulture)
                : $"{Start.ToString(CultureInfo.InvariantCulture)},{End.ToString(CultureInfo.InvariantCulture)}";

        public override bool IsStatic => Start.Equals(End);

        protected void Init(string type, EasingType easing, int startTime, int endTime, double x1, double x2)
        {
            Type = type;
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = x1;
            End = x2;
        }
    }
}
