using OSharp.Storyboard.Events;
using OSharp.Storyboard.Events.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSharp.Storyboard.Internal
{
    internal static class StringUtility
    {
        public static void AppendGroupedEvent(this StringBuilder sb, IEnumerable<Event> events, int index)
        {
            var indent = new string(' ', index);
            var groupedEvents = events.GroupBy(k => k.EventType);
            foreach (var grouping in groupedEvents)
                foreach (var e in grouping)
                    sb.AppendLine(indent + e);
        }

        public static void AppendElementEvents(this StringBuilder sb, Element element)
        {
            sb.AppendGroupedEvent(element.EventList, 1);
            foreach (var loop in element.LoopList)
                sb.AppendLoop(loop);
            foreach (var trigger in element.TriggerList)
                sb.AppendTrigger(trigger);
        }

        public static void AppendTrigger(this StringBuilder sb, Trigger trigger)
        {
            var head = string.Join(",", " T", trigger.TriggerName, trigger.StartTime, trigger.EndTime);
            sb.AppendLine(head);
            sb.AppendGroupedEvent(trigger.EventList, 2);
        }

        public static void AppendLoop(this StringBuilder sb, Loop loop)
        {
            var head = string.Join(",", " L", loop.StartTime, loop.LoopCount);
            sb.AppendLine(head);
            sb.AppendGroupedEvent(loop.EventList, 2);
        }
    }
}
