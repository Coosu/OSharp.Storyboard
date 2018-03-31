using LibOSB.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOSB.Model.EventType
{
    public class Trigger : Element
    {
        public int StartTime { get; internal set; }
        public int EndTime { get; internal set; }
        public string TriggerType { get; internal set; }

        internal Trigger(int startTime, int endTime, TriggerType[] triggerType, int customSampleSet = -1)
        {
            throw new NotImplementedException("TODO HERE");
            isInnerClass = true;
        }
        internal Trigger(int startTime, int endTime, string triggerType)
        {
            StartTime = startTime;
            EndTime = endTime;
            TriggerType = triggerType;

            isInnerClass = true;
        }

        public override string ToString()
        {
            return string.Join(",", " T", TriggerType, StartTime, EndTime) + "\r\n" + base.ToString();
        }
    }
}
