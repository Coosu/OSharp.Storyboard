using LibOSB.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOSB.Model.ActionType
{
    public class Trigger : Element
    {
        internal Trigger(int startTime, int endTime, TriggerType[] triggerType, int customSampleSet = -1)
        {
            isInnerClass = false;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
