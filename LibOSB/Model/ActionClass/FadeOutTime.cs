using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionClass
{
    public class FadeOutTime
    {
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }

        public FadeOutTime(int? startTime, int? endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
