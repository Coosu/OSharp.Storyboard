using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    class FadeOutTime
    {
        private int? startTime, endTime;

        public int? StartTime { get => startTime; set => startTime = value; }
        public int? EndTime { get => endTime; set => endTime = value; }

        public FadeOutTime(int? startTime, int? endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }

    class Fade : Actions
    {
        public Fade this[int index]
        {
            get => F[index];
        }
        private List<FadeOutTime> fadeOutList = new List<FadeOutTime>();

        public Fade(Easing easing, int starttime, int endtime,
         double F1, double F2, int? i, int? j)
        {
            type = "F";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.f1 = F1;
            this.f2 = F2;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        private void BuildParams()
        {
            if (f1 == f2) @params = f1.ToString();
            else @params = f1 + "," + f2;
        }

        public Fade() { }
        public void Remove(int index)
        {
            F.Remove(F[index]);
            starttime_L.RemoveAt(index);
            endtime_L.RemoveAt(index);
        }

        private List<Fade> F = new List<Fade>();
        private double f1, f2;
        public double F1 { get => f1; }
        public double F2 { get => f2; }
        internal List<FadeOutTime> FadeOutList { get => fadeOutList; set => fadeOutList = value; }

        public void Add(Easing Easing, int StartTime, int EndTime,
         double Fade_1, double Fade_2)
        {
            F.Add(new Fade(Easing, StartTime, EndTime, Fade_1, Fade_2, indexL, indexT));
            starttime_L.Add(StartTime);
            endtime_L.Add(EndTime);
        }
    }

}
