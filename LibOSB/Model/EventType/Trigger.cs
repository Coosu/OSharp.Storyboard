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
        public int StartTime { get; private set; }
        public int EndTime { get; private set; }
        public string TriggerType { get; private set; }

        internal Trigger(int startTime, int endTime, TriggerType[] triggerType, int customSampleSet = -1)
        {
            throw new NotImplementedException("TODO HERE");
            isInnerClass = false;
        }
        internal Trigger(int startTime, int endTime, string triggerType)
        {
            startTime = StartTime;
            endTime = EndTime;
            TriggerType = triggerType;

            isInnerClass = false;
        }

        public override string ToString()
        {
            return string.Join(",", " T", StartTime, EndTime, TriggerType) + base.ToString();
        }
    }
}
