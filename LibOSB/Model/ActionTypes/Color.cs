using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    public class Color : Actions
    {
        public Color this[int index] { get => C[index]; }

        public Color() { }
        public Color(Easing easing, int starttime, int endtime,
        byte R1, byte G1, byte B1, byte R2, byte G2, byte B2, int? i, int? j)
        {
            type = "C";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.r1 = R1;
            this.g1 = G1;
            this.b1 = B1;
            this.r2 = R2;
            this.g2 = G2;
            this.b2 = B2;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        private void BuildParams()
        {
            if (r1 == r2 && g1 == g2 && b1 == b2) @params = r1 + "," + g1 + "," + b1;
            else @params = r1 + "," + g1 + "," + b1 +
                        "," + r2 + "," + g2 + "," + b2;
        }
        public void Remove(int index)
        {
            C.Remove(C[index]);
            starttime_L.RemoveAt(index);
            endtime_L.RemoveAt(index);
        }

        private List<Color> C = new List<Color>();
        private byte? r1, g1, b1,
              r2, g2, b2;
        public byte? G1 { get => g1; }
        public byte? B1 { get => b1; }
        public byte? R2 { get => r2; }
        public byte? G2 { get => g2; }
        public byte? B2 { get => b2; }
        public byte? R1 { get => r1; }


        public void Add(Easing Easing, int StartTime, int EndTime,
      byte R1, byte G1, byte B1, byte R2, byte G2, byte B2)
        {
            C.Add(new Color(Easing, StartTime, EndTime, R1, G1, B1, R2, G2, B2, indexL, indexT));
            starttime_L.Add(StartTime);
            endtime_L.Add(EndTime);
        }

    }

}
