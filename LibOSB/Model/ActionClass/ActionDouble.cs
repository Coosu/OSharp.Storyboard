using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionClass
{
    class ActionDouble : Action
    {
        public ActionDouble this[int index]
        {
            get => listDouble[index];
        }

        internal List<ActionDouble> listDouble = new List<ActionDouble>();
        double preParam1, preParam2, postParam1, postParam2;

        private void BuildParams()
        {
            if (preParam1 == postParam1 && preParam2 == postParam2) scriptParams = preParam1 + "," + preParam2;
            else scriptParams = preParam1 + "," + preParam2 + "," + postParam1 + "," + postParam2;
        }

        public double PreParam1 { get => preParam1; }
        public double PreParam2 { get => preParam2; }
        public double PostParam1 { get => postParam1; }
        public double PostParam2 { get => postParam2; }

        public ActionDouble() { }
        public ActionDouble(Easing easing, int startTime, int endTime, double preParam1, double preParam2, double postParam1, double postParam2, int? i, int? j)
        {
            type = "F";
            this.easing = easing;
            this.startTime = startTime;
            this.endTime = endTime;
            this.preParam1 = preParam1;
            this.preParam2 = preParam2;
            this.postParam1 = postParam1;
            this.postParam2 = postParam2;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        public void Add(Easing easing, int startTime, int endTime, double preParam1, double preParam2, double postParam1, double postParam2)
        {
            listDouble.Add(new ActionDouble(easing, startTime, endTime, preParam1, preParam2, postParam1, postParam2, indexL, indexT));
            startTime_L.Add(startTime);
            endTime_L.Add(endTime);
        }

        public void Remove(int index)
        {
            listDouble.Remove(listDouble[index]);
            startTime_L.RemoveAt(index);
            endTime_L.RemoveAt(index);
        }
    }
}
