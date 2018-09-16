using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milkitic.OsbLib.Models
{
    //todo: 差个排序
    public class TimeRange
    {
        public List<Range> TimingList { get; set; } = new List<Range>();

        public float FirstStartTime => TimingList[0].StartTime;
        public float FirstEndTime => TimingList[0].EndTime;
        public float LastStartTime => TimingList[TimingList.Count - 1].StartTime;
        public float LastEndTime => TimingList[TimingList.Count - 1].EndTime;
        public int Count => TimingList.Count;

        public void Add(float startTime, float endTime) =>
            TimingList.Add(new Range(startTime, endTime));

        public bool InRange(int time, int offsetStart = 0, int offsetEnd = 0)
        {
            foreach (var range in TimingList)
                if (time >= range.StartTime + offsetStart && time <= range.EndTime + offsetEnd)
                    return true;
            return false;
        }

        public bool InRange(out bool isLast, params int[] time)
        {
            for (int i = 0; i < TimingList.Count; i++)
            {
                if (time.All(t => t >= TimingList[i].StartTime && t <= TimingList[i].EndTime))
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
            var sb = new StringBuilder();
            foreach (var range in TimingList)
                sb.AppendLine(range.StartTime + "," + range.EndTime);
            return sb.ToString();
        }

        public struct Range
        {
            public readonly float StartTime;
            public readonly float EndTime;

            public Range(float startTime, float endTime)
            {
                StartTime = startTime;
                EndTime = endTime;
            }
        }
    }
}
