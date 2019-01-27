namespace OSharp.Storyboard.Common
{
    public struct RangeValue<T>
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