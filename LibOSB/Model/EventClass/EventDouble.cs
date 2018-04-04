using LibOsb.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOsb.EventClass
{
    class EventDouble : Event
    {
        public double P1_1 { get; internal set; }
        public double P1_2 { get; internal set; }
        public double P2_1 { get; internal set; }
        public double P2_2 { get; internal set; }

        protected void Init(string type, EasingType easing, int startTime, int endTime, double preParam1, double preParam2, double postParam1, double postParam2)
        {
            Type = type;
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            P1_1 = preParam1;
            P1_2 = preParam2;
            P2_1 = postParam1;
            P2_2 = postParam2;

            BuildParams();
        }

        internal override void BuildParams()
        {
            if (P1_1 == P2_1 && P1_2 == P2_2)
                ScriptParams = P1_1 + "," + P1_2;
            else
                ScriptParams = P1_1 + "," + P1_2 + "," + P2_1 + "," + P2_2;
        }
    }
}
