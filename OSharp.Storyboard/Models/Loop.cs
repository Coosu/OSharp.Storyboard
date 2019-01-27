using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSharp.Storyboard.Enums;

namespace OSharp.Storyboard.Models
{
    public class Loop : EventContainer
    {
        public override List<IEvent> EventList { get; set; } = new List<IEvent>();

        public int StartTime { get; set; }
        public int LoopCount { get; set; }

        public float OutterMaxTime => StartTime + MaxTime * LoopCount;
        public float OutterMinTime => StartTime + MinTime;
        public override float MaxTime => EventList.Count > 0 ? EventList.Max(k => k.EndTime) : 0;
        public override float MinTime => EventList.Count > 0 ? EventList.Min(k => k.StartTime) : 0;
        public override float MaxStartTime => EventList.Count > 0 ? EventList.Max(k => k.StartTime) : 0;
        public override float MinEndTime => EventList.Count > 0 ? EventList.Min(k => k.EndTime) : 0;
        public bool HasFade => EventList.Any(k => k.EventType == EventEnum.Fade);

        public Loop(int startTime, int loopCount)
        {
            StartTime = startTime;
            LoopCount = loopCount;
        }

        public override void Adjust(float offsetX, float offsetY, int offsetTiming)
        {
            StartTime += offsetTiming;
            base.Adjust(offsetX, offsetY, offsetTiming);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{string.Join(",", " L", StartTime, LoopCount)}\r\n");
            const string index = "  ";
            var events = EventList.GroupBy(k => k.EventType);
            foreach (var kv in events)
                foreach (var e in kv)
                    sb.AppendLine(index + e);
            return sb.ToString();
        }
    }
}
