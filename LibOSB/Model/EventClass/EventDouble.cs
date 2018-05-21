using LibOsb.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOsb.EventClass
{
    class EventDouble : Event
    {
        public double P11 { get; internal set; }
        public double P12 { get; internal set; }
        public double P21 { get; internal set; }
        public double P22 { get; internal set; }

        public override bool IsStatic => P11 == P21 && P12 == P22;

        protected void Init(string type, EasingType easing, int startTime, int endTime, double preParam1, double preParam2, double postParam1, double postParam2)
        {
            Type = type;
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            P11 = preParam1;
            P12 = preParam2;
            P21 = postParam1;
            P22 = postParam2;

            BuildParams();
        }

        internal override void BuildParams()
        {
            if (P11 == P21 && P12 == P22)
                ScriptParams = P11 + "," + P12;
            else
                ScriptParams = P11 + "," + P12 + "," + P21 + "," + P22;
        }
    }
}
