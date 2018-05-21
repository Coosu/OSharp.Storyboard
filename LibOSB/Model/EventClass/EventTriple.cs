using LibOsb.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOsb.EventClass
{
    class EventTriple : Event
    {
        public double P11 { get; private set; }
        public double P12 { get; private set; }
        public double P13 { get; private set; }
        public double P21 { get; private set; }
        public double P22 { get; private set; }
        public double P23 { get; private set; }

        public override bool IsStatic => P11 == P21 && P12 == P22 && P13 == P23;

        public void Init(string type, EasingType easing, int startTime, int endTime,
            double preParam1, double preParam2, double preParam3, double postParam1, double postParam2, double postParam3)
        {
            Type = type;
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            P11 = preParam1;
            P12 = preParam2;
            P13 = preParam3;
            P21 = postParam1;
            P22 = postParam2;
            P23 = postParam3;

            BuildParams();
        }

        internal override void BuildParams()
        {
            if (P11 == P21 && P12 == P22 && P13 == P23)
                ScriptParams = string.Join(",", P11, P12, P13);
            else
                ScriptParams = string.Join(",", P11, P12, P13, P21, P22, P23);
        }
    }
}
