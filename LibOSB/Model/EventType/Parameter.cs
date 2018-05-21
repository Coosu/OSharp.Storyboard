using LibOsb.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOsb.Model.EventType
{
    public class Parameter : Event
    {
        public string PType { get; }

        public override bool IsStatic => true;

        public Parameter(EasingType easing, int starttime, int endtime, string ptype)
        {
            Type = "P";
            Easing = easing;
            StartTime = starttime;
            EndTime = endtime;
            PType = ptype;
            BuildParams();
        }
        internal sealed override void BuildParams() => ScriptParams = PType;
    }
}
