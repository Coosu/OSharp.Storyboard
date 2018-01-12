using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    class Rotate : Actions
    {
        public Rotate this[int index] { get => R[index]; }

        public Rotate(Easing easing, int starttime, int endtime,
         double R1, double R2, int? i, int? j)
        {
            type = "R";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.r1 = R1;
            this.r2 = R2;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        private void BuildParams()
        {
            if (r1 == r2) @params = r1.ToString();
            else @params = r1 + "," + r2;
        }

        public Rotate() { }
        public void Remove(int index)
        {
            R.Remove(R[index]);
            starttime_L.RemoveAt(index);
            endtime_L.RemoveAt(index);
        }

        private List<Rotate> R = new List<Rotate>();
        private double r1, r2;
        public double R1 { get => r1; }
        public double R2 { get => r2; }
        public void Add(Easing Easing, int StartTime, int EndTime,
         double Rotate_1, double Rotate_2)
        {
            R.Add(new Rotate(Easing, StartTime, EndTime, Rotate_1, Rotate_2, indexL, indexT));
            starttime_L.Add(StartTime);
            endtime_L.Add(EndTime);
        }
    }

}
