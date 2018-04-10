using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOsb.EventClass
{
    // 差个排序
    public class TimeRange
    {
        public List<TimeList> TimingList { get; set; } = new List<TimeList>();

        public int FirstStartTime => TimingList[0].StartTime;
        public int FirstEndTime => TimingList[0].EndTime;
        public int LastStartTime => TimingList[TimingList.Count - 1].StartTime;
        public int LastEndTime => TimingList[TimingList.Count - 1].EndTime;
        public int Count => TimingList.Count;

        public void Add(int startTime, int endTime) =>
            TimingList.Add(new TimeList { StartTime = startTime, EndTime = endTime });

        public bool InRange(int time)
        {
            foreach (var item in TimingList)
                if (time >= item.StartTime && time <= item.EndTime)
                    return true;
            return false;
        }

        public bool InRange(out bool isLast, params int[] time)
        {
            for (int i = 0; i < TimingList.Count; i++)
            {
                bool flag = true;
                foreach (var t in time)
                {
                    if (t < TimingList[i].StartTime || t > TimingList[i].EndTime)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    isLast = i == TimingList.Count - 1;
                    return true;
                }
            }
            isLast = false;
            return false;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in TimingList)
                sb.AppendLine(item.StartTime + "," + item.EndTime);
            return sb.ToString();
        }

        public class TimeList
        {
            public int StartTime { get; set; }
            public int EndTime { get; set; }
        }
    }
}
