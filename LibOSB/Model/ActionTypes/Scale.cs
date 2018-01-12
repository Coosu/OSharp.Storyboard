using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    public class Scale : Actions
    {
        public Scale this[int index] { get => S[index]; }

        public Scale(Easing easing, int starttime, int endtime,
         double S1, double S2, int? i, int? j)
        {
            type = "S";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.s1 = S1;
            this.s2 = S2;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        private void BuildParams()
        {
            if (s1 == s2) @params = s1.ToString();
            else @params = s1 + "," + s2;
        }

        public Scale() { }
        public void Remove(int index)
        {
            S.Remove(S[index]);
            starttime_L.RemoveAt(index);
            endtime_L.RemoveAt(index);
        }

        private List<Scale> S = new List<Scale>();
        private double s1, s2;
        public double S1 { get => s1; }
        public double S2 { get => s2; }

        public void Add(Easing Easing, int StartTime, int EndTime,
         double Scale_1, double Scale_2)
        {
             S.Add(new Scale(Easing, StartTime, EndTime, Scale_1, Scale_2, indexL, indexT));
            starttime_L.Add(StartTime);
            endtime_L.Add(EndTime);
        }
    }

}
