using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOSB.Model.ActionType
{
    public class Loop : Element
    {
        StringBuilder sb = new StringBuilder();
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
            sb.Clear();
            sb.AppendLine(" L," + StartTime + "," + LoopCount);
            return sb + base.ToString();
        }
    }
}
