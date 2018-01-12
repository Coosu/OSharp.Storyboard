using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    public class MoveY : Actions
    {
        public MoveY this[int index] { get => MY[index]; }

        public MoveY() { }
        public MoveY(Easing easing, int starttime, int endtime,
        double Y1, double Y2, int? i, int? j)
        {
            type = "MY";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.y1 = Y1;
            this.y2 = Y2;
            indexL = i;
            indexT = j;
            BuildParams();
        }
        public void Remove(int index)
        {
            MY.Remove(MY[index]);
            starttime_L.RemoveAt(index);
            endtime_L.RemoveAt(index);
        }
        private void BuildParams()
        {
            if (y1 != y2) @params = y1 + "," + y2;
            else @params = y1.ToString();
        }
        private List<MoveY> MY = new List<MoveY>();
        private double y1, y2;
        public double Y1 { get => y1; }
        public double Y2 { get => y2; }
        public void Add(Easing Easing, int StartTime, int EndTime,
            double Location_Y1, double Location_Y2)
        {
             MY.Add(new MoveY(Easing, StartTime, EndTime, Location_Y1, Location_Y2, indexL, indexT));
            starttime_L.Add(StartTime);
            endtime_L.Add(EndTime);
        }
    }

}
