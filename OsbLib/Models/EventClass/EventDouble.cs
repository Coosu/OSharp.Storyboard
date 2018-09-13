using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventClass
{
    public abstract class EventDouble : Event
    {
        public (float x, float y) Start { get; internal set; }
        public (float x, float y) End { get; internal set; }

        public override string ScriptParams =>
            Start.Equals(End)
            ? Start.x + "," + Start.y
            : Start.x + "," + Start.y + "," + End.x + "," + End.y;

        public override bool IsStatic => Start.Equals(End);

        protected void Init(string type, EasingType easing, float startTime, float endTime, float x1, float y1, float x2, float y2)
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
