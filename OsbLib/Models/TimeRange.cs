using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milkitic.OsbLib.Models
{
    //todo: 差个排序
    public class TimeRange
    {
        public List<(float startTime, float endTime)> TimingList { get; set; } = new List<(float, float)>();

        public float FirstStartTime => TimingList[0].startTime;
        public float FirstEndTime => TimingList[0].endTime;
        public float LastStartTime => TimingList[TimingList.Count - 1].startTime;
        public float LastEndTime => TimingList[TimingList.Count - 1].endTime;
        public int Count => TimingList.Count;

        public void Add(float startTime, float endTime) =>
            TimingList.Add((startTime, endTime));

        public bool InRange(int time, int offsetStart = 0, int offsetEnd = 0)
        {
            foreach (var (startTime, endTime) in TimingList)
                if (time >= startTime + offsetStart && time <= endTime + offsetEnd)
                    return true;
            return false;
        }

        public bool InRange(out bool isLast, params int[] time)
        {
            for (int i = 0; i < TimingList.Count; i++)
            {
                if (time.All(t => t >= TimingList[i].startTime && t <= TimingList[i].endTime))
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
            foreach (var (startTime, endTime) in TimingList)
                sb.AppendLine(startTime + "," + endTime);
            return sb.ToString();
        }
    }
}
