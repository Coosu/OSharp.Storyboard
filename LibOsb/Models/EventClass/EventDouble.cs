using LibOsb.Enums;

namespace LibOsb.Models.EventClass
{
    class EventDouble : Event
    {
        public (double x, double y) Start { get; internal set; }
        public (double x, double y) End { get; internal set; }

        public override string ScriptParams =>
            Start.Equals(End)
            ? Start.x + "," + Start.y
            : Start.x + "," + Start.y + "," + End.x + "," + End.y;

        public override bool IsStatic => Start.Equals(End);

        protected void Init(string type, EasingType easing, int startTime, int endTime, double x1, double y1, double x2, double y2)
        {
            Type = type;
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = (x1, y1);
            End = (x2, y2);
        }
    }
}
