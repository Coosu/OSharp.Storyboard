using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB.Constants;

namespace LibOSB.ActionClass
{
    class ActionSingle : Action
    {
        public ActionSingle this[int index]
        {
            get => listSingle[index];
        }

        List<ActionSingle> listSingle = new List<ActionSingle>();
        double preParam, postParam;

        private void BuildParams()
        {
            if (preParam == postParam) scriptParams = preParam.ToString();
            else scriptParams = preParam + "," + postParam;
        }

        public double PreParam { get => preParam; }
        public double PostParam { get => postParam; }

        public ActionSingle() { }
        public ActionSingle(EasingType easing, int startTime, int endTime, double preParam, double postParam, int? i, int? j)
        {
            type = "F";
            this.easing = easing;
            this.startTime = startTime;
            this.endTime = endTime;
            this.preParam = preParam;
            this.postParam = postParam;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        public void Add(EasingType easing, int startTime, int endTime, double preParam, double postParam)
        {
            //if (easing < 0 || easing > 34) throw new Exception("Unknown Easing.");
            listSingle.Add(new ActionSingle(easing, startTime, endTime, preParam, postParam, indexL, indexT));
            startTime_L.Add(startTime);
            endTime_L.Add(endTime);
        }

        public void Remove(int index)
        {
            listSingle.Remove(listSingle[index]);
            startTime_L.RemoveAt(index);
            endTime_L.RemoveAt(index);
        }
    }
}
