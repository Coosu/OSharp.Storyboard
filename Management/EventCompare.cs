using OSharp.Storyboard.Events;
using OSharp.Storyboard.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSharp.Storyboard.Management
{
    public static class EventCompare
    {

        public static bool InObsoleteTimingRange(this Event e, EventContainer container, out Common.RangeValue<float> range)
        {
            return container.ObsoleteList.ContainsTimingPoint(out range, e.StartTime, e.EndTime);
        }

        public static bool OnObsoleteTimingRange(this Event e, EventContainer container)
        {
            return container.ObsoleteList.OnTimingRange(out _, e.StartTime) ||
                   container.ObsoleteList.OnTimingRange(out _, e.EndTime);
        }

        public static bool IsEventSequent(Event previous, Event next)
        {
            return previous.End.SequenceEqual(next.Start);
        }

        public static bool EndsWithUnworthy(this Event e)
        {
            return EventExtension.UnworthyDictionary.ContainsKey(e.EventType) &&
                   EventExtension.UnworthyDictionary[e.EventType].SequenceEqual(e.End);
        }

        public static bool IsStaticAndDefault(this Event e)
        {
            return e.IsDefault() &&
                   e.IsStatic();
        }

        private static bool IsDefault(this Event e)
        {
            return EventExtension.DefaultDictionary.ContainsKey(e.EventType) &&
                   e.Start.SequenceEqual(EventExtension.DefaultDictionary[e.EventType]);
        }

        public static bool IsStatic(this Event e)
        {
            return e.Start.SequenceEqual(e.End);
        }

        public static bool EqualsInitialPosition(this Move move, Element element)
        {
            return move.StartX.Equals(element.DefaultX) &&
                   move.StartY.Equals(element.DefaultY);
        }

        public static bool IsTimeInRange(this Event e, EventContainer container)
        {
            return e.IsSmallerThenMaxTime(container) && e.IsLargerThanMinTime(container);
        }

        public static bool IsSmallerThenMaxTime(this Event e, EventContainer container)
        {
            return e.EndTime < container.MaxTime ||
                   e.EqualsMultiMaxTime(container);
        }

        public static bool IsLargerThanMinTime(this Event e, EventContainer container)
        {
            return e.StartTime > container.MinTime ||
                   e.EqualsMultiMinTime(container);
        }

        public static bool EqualsMultiMaxTime(this Event e, EventContainer container)
        {
            return e.EqualsMaxTime(container) && container.MaxTimeCount > 1;
        }

        public static bool EqualsMultiMinTime(this Event e, EventContainer container)
        {
            return e.EqualsMinTime(container) && container.MinTimeCount > 1;
        }

        public static bool EqualsMaxTime(this Event e, EventContainer container)
        {
            return e.EndTime == container.MaxTime;
        }

        public static bool EqualsMinTime(this Event e, EventContainer container)
        {
            return e.StartTime == container.MinTime;
        }
    }
}