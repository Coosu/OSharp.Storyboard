using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOsb.Model.EventType
{
    public class Loop : Element
    {
        public int StartTime { get; internal set; }
        public int LoopCount { get; internal set; }

        internal Loop(int startTime, int loopCount)
        {
            StartTime = startTime;
            LoopCount = loopCount;
            IsInnerClass = true;
        }
        public override string ToString()
        => string.Join(",", " L", StartTime, LoopCount) + "\r\n" + base.ToString();

    }
}
