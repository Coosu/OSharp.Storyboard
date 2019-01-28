using System;

namespace OSharp.Storyboard.Common
{
    public struct RangeValue<T> where T : IComparable
    {
        public T StartTime { get; }
        public T EndTime { get; }

        public RangeValue(T startTime, T endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}