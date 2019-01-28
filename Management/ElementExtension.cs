using OSharp.Storyboard.Events;
using OSharp.Storyboard.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSharp.Storyboard.Management
{
    public static class ElementExtension
    {
        public static async Task ExpandAsync(this ElementGroup eleG)
        {
            await Task.Run(() => { Expand(eleG); });
        }

        public static void ExpandAndFillFadeout(this ElementGroup eleG)
        {
            eleG.InnerFix(true, true);
        }

        public static void Expand(this ElementGroup eleG)
        {
            eleG.InnerFix(true, false);
        }

        public static void FillFadeout(this ElementGroup eleG)
        {
            eleG.InnerFix(false, true);
        }

        private static void InnerFix(this ElementGroup eleG, bool expand, bool fillFadeout)
        {
            if (!expand && !fillFadeout)
                return;
            foreach (var ele in eleG.ElementList)
            {
                if (expand) ele.Expand();
                if (fillFadeout) ele.FillFadeoutList();
            }
        }

        public static void Expand(this EventContainer container)
        {
            if (container is Element element)
            {
                if (element.TriggerList.Any())
                {
                    foreach (var t in element.TriggerList)
                        t.Expand();
                }

                if (element.LoopList.Any())
                {
                    foreach (var loop in element.LoopList)
                    {
                        loop.Expand();
                        var loopCount = loop.LoopCount;
                        var startTime = loop.StartTime;
                        for (int count = 0; count < loopCount; count++)
                        {
                            var fixedStartTime = startTime + count * loop.MaxTime;
                            foreach (var e in loop.EventList)
                            {
                                element.AddEvent(
                                    e.EventType,
                                    e.Easing,
                                    fixedStartTime + e.StartTime, fixedStartTime + e.EndTime,
                                    e.Start, e.End
                                );
                            }
                        }
                    }

                    element.LoopList.Clear();
                }
            }

            var events = container.EventList?.GroupBy(k => k.EventType);
            if (events == null) return;
            foreach (var kv in events)
            {
                List<Event> list = kv.ToList();
                for (var i = 0; i < list.Count - 1; i++)
                {
                    if (list[i].Start == list[i].End) // case 1
                    {
                        list[i].EndTime = list[i + 1].StartTime;
                    }

                    if (!list[i].EndTime.Equals(list[i + 1].StartTime)) // case 2
                    {
                        container.AddEvent(
                            list[i].EventType,
                            EasingType.Linear,
                            list[i].EndTime, list[i + 1].StartTime,
                            list[i].End, list[i].End
                        );
                    }
                }
            }
        }

        public class EventSettings
        {
            public int Count { get; set; } = -1;
            public bool IsFadingOut { get; set; } = false;
            public float StartTime { get; set; } = float.MinValue;
        }
        public static void FillFadeoutList(this Element element)
        {
            var possibleList = element.EventList
                .Where(k => k.EventType == EventType.Fade ||
                            k.EventType == EventType.Scale ||
                            k.EventType == EventType.Vector);

            if (possibleList.Any())
            {
                var dic = new Dictionary<EventType, EventSettings>
                {
                    [EventType.Fade] = new EventSettings(),
                    [EventType.Scale] = new EventSettings(),
                    [EventType.Vector] = new EventSettings()
                };
                foreach (var @event in possibleList)
                {
                    dic[@event.EventType].Count++;
                    // 最早的event晚于最小开始时间，默认加这一段
                    if (dic[@event.EventType].Count == 0 &&
                        @event.Start.SequenceEqual(@event.GetUnworthyValue()) &&
                        @event.StartTime > element.MinTime)
                    {
                        dic[@event.EventType].StartTime = element.MinTime;
                        dic[@event.EventType].IsFadingOut = true;
                    }

                    // event.End为无用值时，开始计时
                    if (@event.End.SequenceEqual(@event.GetUnworthyValue()) &&
                        dic[@event.EventType].IsFadingOut == false)
                    {
                        dic[@event.EventType].StartTime = @event.EndTime;
                        dic[@event.EventType].IsFadingOut = true;
                    }

                    else if (dic[@event.EventType].IsFadingOut)
                    {
                        if (@event.Start.SequenceEqual(@event.GetUnworthyValue()) &&
                            @event.End.SequenceEqual(@event.GetUnworthyValue()))
                            continue;
                        element.FadeoutList.Add(dic[@event.EventType].StartTime, @event.StartTime);
                        dic[@event.EventType].IsFadingOut = false;
                    }
                }

                // 可能存在遍历完后所有event后，仍存在某一项>0（后面还有别的event，算无用）
                foreach (var pair in dic
                    .Where(k => k.Value.IsFadingOut && !k.Value.StartTime.Equals(element.MaxTime))
                    .OrderBy(k => k.Value.StartTime))
                {
                    element.FadeoutList.Add(pair.Value.StartTime, element.MaxTime);
                    break;
                }
            }



            //// only test not optimized
            //var scaList = element.ScaleList;
            //if (scaList.Any())
            //{
            //    var i = -1;
            //    foreach (Scale nowF in scaList)
            //    {
            //        i++;
            //        if (i == 0 && nowF.StartScale.Equals(0) && nowF.StartTime > element.MinTime)  // 最早的F晚于最小开始时间，默认加这一段
            //        {
            //            startTime = element.MinTime;
            //            fadeouting = true;
            //        }

            //        if (nowF.EndScale.Equals(0) && !fadeouting)  // f2=0，开始计时
            //        {
            //            startTime = nowF.EndTime;
            //            fadeouting = true;
            //        }
            //        else if (fadeouting)
            //        {
            //            if (nowF.StartScale.Equals(0) && nowF.EndScale.Equals(0))
            //                continue;
            //            element.FadeoutList.Add(startTime, nowF.StartTime);
            //            fadeouting = false;
            //        }
            //    }
            //}

            //if (fadeouting && startTime != element.MaxTime)  // 可能存在Fade后还有别的event
            //{
            //    element.FadeoutList.Add(startTime, element.MaxTime);
            //}
        }
    }
}
