using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOsb.Model.Constants;

namespace LibOsb.EventClass
{
    class EventSingle : Event
    {
        public double P1_1 { get; internal set; }
        public double P2_1 { get; internal set; }

        public override bool IsStatic => P1_1 == P2_1;

        protected void Init(string type, EasingType easing, int startTime, int endTime, double preParam, double postParam)
        {
            Type = type;
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            P1_1 = preParam;
            P2_1 = postParam;

            BuildParams();
        }

        internal override void BuildParams()
        {
            if (P1_1 == P2_1)
                ScriptParams = P1_1.ToString();
            else
                ScriptParams = P1_1 + "," + P2_1;
        }
    }
}
