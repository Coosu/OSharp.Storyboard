using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventClass
{
    public abstract class EventTriple : Event
    {
        public (float x, float y, float z) Start { get; internal set; }
        public (float x, float y, float z) End { get; internal set; }

        public override string ScriptParams =>
            Start.Equals(End)
                ? string.Join(",", Start.x, Start.y, Start.z)
                : string.Join(",", Start.x, Start.y, Start.z, End.x, End.y, End.z);

        public override bool IsStatic => Start.Equals(End);

        public void Init(string type, EasingType easing, float startTime, float endTime,
            float x1, float y1, float z1, float x2, float y2, float z2)
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
