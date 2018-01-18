using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    public class Vector : Action
    {

        public Vector this[int index] { get => V[index]; }

        public Vector() { }
        public void Remove(int index)
        {
            V.Remove(V[index]);
            startTime_L.RemoveAt(index);
            endTime_L.RemoveAt(index);
        }
        public Vector(Easing easing, int starttime, int endtime,
         double VX1, double VY1, double VX2, double VY2, int? i, int? j)
        {
            type = "V";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.vx1 = VX1;
            this.vx2 = VX2;
            this.vy1 = VY1;
            this.vy2 = VY2;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        private void BuildParams()
        {
            if (vx1 == vx2 && vy1 == vy2) scriptParams = vx1 + "," + vy1;
            else scriptParams = vx1 + "," + vy1 + "," + vx2 + "," + vy2;
        }

        private List<Vector> V = new List<Vector>();
        private double vx1, vy1, vx2, vy2;

        public double VX1 { get => vx1; }
        public double VY1 { get => vy1; }
        public double VX2 { get => vx2; }
        public double VY2 { get => vy2; }

        public void Add(Easing Easing, int StartTime, int EndTime,
         double Vector_X1, double Vector_Y1,
         double Vector_X2, double Vector_Y2)
        {
            V.Add(new Vector(Easing, StartTime, EndTime, Vector_X1, Vector_Y1, Vector_X2, Vector_Y2, indexL, indexT));
            startTime_L.Add(StartTime);
            endTime_L.Add(EndTime);
        }

    }

}
