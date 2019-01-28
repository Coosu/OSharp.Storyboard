using System;
using System.Collections.Generic;

namespace OSharp.Storyboard.Events
{
    public class EventComparer : IComparer<Event>
    {
        public int Compare(Event x, Event y)
        {
            if (y == null && x == null)
                return 0;
            if (y == null)
                return 1;
            if (x == null)
                return -1;
            if (x.StartTime > y.StartTime)
                return 1;
            if (x.StartTime < y.StartTime)
                return -1;
            if (x.EndTime > y.EndTime)
                return 1;
            if (x.EndTime < y.EndTime)
                return -1;
            return 0;
        }
    }
}