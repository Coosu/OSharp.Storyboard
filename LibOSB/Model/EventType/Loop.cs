using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOSB.Model.EventType
{
    public class Loop : Element
    {
        public int StartTime { get; private set; }
        public int LoopCount { get; private set; }

        internal Loop(int startTime, int loopCount)
        {
            StartTime = startTime;
            LoopCount = loopCount;
            isInnerClass = true;
        }
        public override string ToString()
        {
            return string.Join(",", " L", StartTime, LoopCount) + "\r\n" + base.ToString();
        }
    }
}
