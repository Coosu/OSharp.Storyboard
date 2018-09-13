using System.Globalization;
using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventClass
{
    public abstract class EventSingle : Event
    {
        public float Start { get; internal set; }
        public float End { get; internal set; }
        public override string ScriptParams =>
            Start.Equals(End)
                ? Start.ToString(CultureInfo.InvariantCulture)
                : $"{Start.ToString(CultureInfo.InvariantCulture)},{End.ToString(CultureInfo.InvariantCulture)}";

        public override bool IsStatic => Start.Equals(End);

        protected void Init(string type, EasingType easing, float startTime, float endTime, float x1, float x2)
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
