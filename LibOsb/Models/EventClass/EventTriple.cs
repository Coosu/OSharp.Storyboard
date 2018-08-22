using LibOsb.Enums;

namespace LibOsb.Models.EventClass
{
    class EventTriple : Event
    {
        public (double x, double y, double z) Start { get; internal set; }
        public (double x, double y, double z) End { get; internal set; }

        public override string ScriptParams =>
            Start.Equals(End)
                ? string.Join(",", Start.x, Start.y, Start.z)
                : string.Join(",", Start.x, Start.y, Start.z, End.x, End.y, End.z);

        public override bool IsStatic => Start.Equals(End);

        public void Init(string type, EasingType easing, int startTime, int endTime,
            double x1, double y1, double z1, double x2, double y2, double z2)
        {
            Type = type;
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            Start = (x1, y1, z1);
            End = (x2, y2, z2);
        }
    }
}
