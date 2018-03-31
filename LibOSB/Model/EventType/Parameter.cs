using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.Model.EventType
{
    public class Parameter : Event
    {
        public string PType { get; private set; }

        public Parameter(EasingType easing, int starttime, int endtime, string ptype)
        {
            Type = "P";
            Easing = easing;
            StartTime = starttime;
            EndTime = endtime;
            PType = ptype;
            BuildParams();
        }
        internal override void BuildParams()
        {
            ScriptParams = PType;
        }
    }
}
