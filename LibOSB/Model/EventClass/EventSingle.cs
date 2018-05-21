using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using LibOsb.Model.Constants;

namespace LibOsb.EventClass
{
    class EventSingle : Event
    {
        public double P11 { get; internal set; }
        public double P21 { get; internal set; }

        public override bool IsStatic => P11 == P21;

        protected void Init(string type, EasingType easing, int startTime, int endTime, double preParam, double postParam)
        {
            Type = type;
            Easing = easing;
            StartTime = startTime;
            EndTime = endTime;
            P11 = preParam;
            P21 = postParam;

            BuildParams();
        }

        internal override void BuildParams()
        {
            if (P11 == P21)
                ScriptParams = P11.ToString(CultureInfo.InvariantCulture);
            else
                ScriptParams = P11 + "," + P21;
        }
    }
}
