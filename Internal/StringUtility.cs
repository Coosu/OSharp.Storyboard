using OSharp.Storyboard.Events;
using OSharp.Storyboard.Events.Containers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSharp.Storyboard.Internal
{
    internal static class StringUtility
    {
        public static void AppendGroupedEvent(this StringBuilder sb, IEnumerable<CommonEvent> events, int index)
        {
            var indent = new string(' ', index);
            var groupedEvents = events.GroupBy(k => k.EventType);
            foreach (var grouping in groupedEvents)
                foreach (CommonEvent e in grouping)
                    sb.AppendLine(indent + e);
        }

        public static void AppendSequentialEvent(this StringBuilder sb, IEnumerable<CommonEvent> events, int index)
        {
            var indent = new string(' ', index);
            foreach (CommonEvent e in events)
                sb.AppendLine(indent + e);
        }

        public static void AppendElementEvents(this StringBuilder sb, Element element, bool group)
        {
            if (group)
                sb.AppendGroupedEvent(element.EventList, 1);
            else
                sb.AppendSequentialEvent(element.EventList, 1);

            foreach (var loop in element.LoopList)
                sb.AppendLoop(loop, group);
            foreach (var trigger in element.TriggerList)
                sb.AppendTrigger(trigger, group);
        }

        public static void AppendTrigger(this StringBuilder sb, Trigger trigger, bool group)
        {
            var head = string.Join(",", " T", trigger.TriggerName, trigger.StartTime, trigger.EndTime);
            sb.AppendLine(head);

            if (group)
                sb.AppendGroupedEvent(trigger.EventList, 2);
            else
                sb.AppendSequentialEvent(trigger.EventList, 2);
        }

        public static void AppendLoop(this StringBuilder sb, Loop loop, bool group)
        {
            var head = string.Join(",", " L", loop.StartTime, loop.LoopCount);
            sb.AppendLine(head);

            if (group)
                sb.AppendGroupedEvent(loop.EventList, 2);
            else
                sb.AppendSequentialEvent(loop.EventList, 2);
        }
    }
}
