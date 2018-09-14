using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models;
using Milkitic.OsbLib.Models.EventType;
using System.Collections.Generic;
using System.Linq;

namespace Milkitic.OsbLib.Extension
{
    public static class ElementExtension
    {
        public static void Expand(this ElementGroup eleG)
        {
            foreach (var ele in eleG.ElementList)
                ele.Expand();
        }

        public static void Expand(this EventContainer container)
        {
            if (container is Element element)
            {
                foreach (var l in element.LoopList)
                    l.Expand();
                foreach (var t in element.TriggerList)
                    t.Expand();
                foreach (var l in element.LoopList)
                {
                    var loopCount = l.LoopCount;
                    var startT = l.StartTime;
                    for (int times = 0; times < loopCount; times++)
                    {
                        var additionT = startT + times * l.MaxTime;
                        foreach (var e in l.EventList)
                        {
                            element.AddEvent(e.EventType, e.Easing, additionT + e.StartTime, additionT + e.EndTime,
                                e.Start, e.End);
                        }
                    }
                }

                element.LoopList.Clear();
            }

            var events = container.EventList.GroupBy(k => k.EventType);
            foreach (var kv in events)
            {
                List<Event> list = kv.ToList();
                for (var i = 0; i < list.Count - 1; i++)
                {
                    if (list[i].Start == list[i].End)
                        list[i].EndTime = list[i + 1].StartTime;
                    if (list[i].EndTime != list[i + 1].StartTime)
                    {
                        container.AddEvent(list[i].EventType, EasingType.Linear, list[i].EndTime, list[i + 1].StartTime,
                            list[i].End, list[i].End);
                    }
                }
            }
        }
    }
}
