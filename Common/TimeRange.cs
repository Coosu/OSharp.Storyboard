using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSharp.Storyboard.Common
{
    public class TimeRange
    {
        public SortedSet<RangeValue<float>> TimingList { get; } =
            new SortedSet<RangeValue<float>>(new RangeComparer<float>());

        //public float MinStartTime => NumericUtility.GetMinValue(TimingList.Select(k => k.StartTime));
        //public float MinEndTime => NumericUtility.GetMinValue(TimingList.Select(k => k.EndTime));
        //public float MaxStartTime => NumericUtility.GetMaxValue(TimingList.Select(k => k.StartTime));
        //public float MaxEndTime => NumericUtility.GetMaxValue(TimingList.Select(k => k.EndTime));
        public float MinStartTime => TimingList.First().StartTime;
        public float MinEndTime => TimingList.First().EndTime;
        public float MaxStartTime => TimingList.Last().StartTime;
        public float MaxEndTime => TimingList.Last().EndTime;

        public int Count => TimingList.Count;

        public void Add(float startTime, float endTime) =>
            TimingList.Add(new RangeValue<float>(startTime, endTime));

        public bool ContainsTimingPoint(int time, int offsetStart = 0, int offsetEnd = 0)
        {
            foreach (var range in TimingList)
                if (time >= range.StartTime + offsetStart && time <= range.EndTime + offsetEnd)
                    return true;
            return false;
        }

        public bool ContainsTimingPoint(out bool isLast, params int[] time)
        {
            int i = 0;
            foreach (var sb in TimingList)
            {
                if (time.All(t => t >= sb.StartTime && t <= sb.EndTime))
                {
                    isLast = i == TimingList.Count - 1;
                    return true;
                }

                i++;
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
    }
}
