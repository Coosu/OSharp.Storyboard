using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.Model.ActionType
{
    public class Parameter : Action
    {
        public Parameter this[int index] { get => P[index]; }

        public Parameter(EasingType easing, int starttime, int endtime,
         string ptype, int? i, int? j)
        {
            type = "P";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.ptype = ptype;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        private void BuildParams()
        {
            scriptParams = ptype;
        }

        public Parameter() { }

        private List<Parameter> P = new List<Parameter>();
        private string ptype;
        public string Ptype { get => ptype; }
        public void Add(EasingType Easing, int StartTime, int EndTime,
         string PType)
        {
            P.Add(new Parameter(Easing, StartTime, EndTime, PType, indexL, indexT));
            startTime_L.Add(StartTime);
            endTime_L.Add(EndTime);
        }
    }

}
