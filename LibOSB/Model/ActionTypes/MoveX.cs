using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    class MoveX : Actions
    {
        public MoveX this[int index] { get => MX[index]; }

        public MoveX() { }
        public MoveX(Easing easing, int starttime, int endtime,
        double X1, double X2, int? i, int? j)
        {
            type = "MX";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.x1 = X1;
            this.x2 = X2;
            indexL = i;
            indexT = j;
            BuildParams();
        }
        public void Remove(int index)
        {
            MX.Remove(MX[index]);
            starttime_L.RemoveAt(index);
            endtime_L.RemoveAt(index);
        }
        private void BuildParams()
        {
            if (x1 != x2) @params = x1 + "," + x2;
            else @params = x1.ToString();
        }

        private List<MoveX> MX = new List<MoveX>();
        private double x1, x2;
        public double X1 { get => x1; }
        public double X2 { get => x2; }

        public void Add(Easing Easing, int StartTime, int EndTime,
        double Location_X1, double Location_X2)
        {
             MX.Add(new MoveX(Easing, StartTime, EndTime, Location_X1, Location_X2, indexL, indexT));
            starttime_L.Add(StartTime);
            endtime_L.Add(EndTime);
        }
    }

}
