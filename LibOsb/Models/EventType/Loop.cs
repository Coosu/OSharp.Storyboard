using System;

namespace LibOsb.Models.EventType
{
    public class Loop : Element
    {
        public int StartTime { get; internal set; }
        public int LoopCount { get; internal set; }

        internal Loop(int startTime, int loopCount)
        {
            StartTime = startTime;
            LoopCount = loopCount;
            IsLorT = true;
        }
        public override string ToString()
        => $"{string.Join(",", " L", StartTime, LoopCount)}{Environment.NewLine}{base.ToString()}";

    }
}
