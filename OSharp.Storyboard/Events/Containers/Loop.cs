using OSharp.Storyboard.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSharp.Storyboard.Events.Containers
{
    public sealed class Loop : EventContainer
    {
        public int StartTime { get; set; }
        public int LoopCount { get; set; }

        public float OuterMaxTime => StartTime + MaxTime * LoopCount;
        public float OuterMinTime => StartTime + MinTime;
        public override float MaxTime => EventList.Count > 0 ? EventList.Max(k => k.EndTime) : 0;
        public override float MinTime => EventList.Count > 0 ? EventList.Min(k => k.StartTime) : 0;
        public override float MaxStartTime => EventList.Count > 0 ? EventList.Max(k => k.StartTime) : 0;
        public override float MinEndTime => EventList.Count > 0 ? EventList.Min(k => k.EndTime) : 0;
        //public bool HasFade => EventList.Any(k => k.EventType == EventType.Fade);

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
            var sb = new StringBuilder();
            sb.AppendLoop(this);
            return sb.ToString();
        }
    }
}
