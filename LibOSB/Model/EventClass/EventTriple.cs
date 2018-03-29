using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.EventClass
{
    abstract class EventTriple : Event
    {
        public double P1_1 { get; private set; }
        public double P1_2 { get; private set; }
        public double P1_3 { get; private set; }
        public double P2_1 { get; private set; }
        public double P2_2 { get; private set; }
        public double P2_3 { get; private set; }


        public void Init(EasingType easing, int startTime, int endTime,
            double preParam1, double preParam2, double preParam3, double postParam1, double postParam2, double postParam3)
        {
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            P1_1 = preParam1;
            P1_2 = preParam2;
            P1_3 = preParam3;
            P2_1 = postParam1;
            P2_1 = postParam2;
            P2_1 = postParam3;
            if (P1_1 == P2_1) ScriptParams = P1_1.ToString();
            else ScriptParams = P1_1 + "," + P2_1;
        }

    }
}
