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
            {
                ele.Expand();
                ele.FillFadeoutList();
            }
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

        public static void FillFadeoutList(this Element element)
        {
            // 验证物件完全消失的时间段
            float startTime = -1;
            bool fadeouting = false;
            var fadeList = element.FadeList;
            for (int i = 0; i < fadeList.Count; i++)
            {
                Fade nowF = fadeList[i];
                if (i == 0 && nowF.F1.Equals(0) && nowF.StartTime > element.MinTime)  // 最早的F晚于最小开始时间，默认加这一段
                {
                    startTime = element.MinTime;
                    fadeouting = true;
                }

                if (nowF.F2.Equals(0) && !fadeouting)  // f2=0，开始计时
                {
                    startTime = nowF.EndTime;
                    fadeouting = true;
                }
                else if (fadeouting)
                {
                    if (nowF.F1.Equals(0) && nowF.F2.Equals(0))
                        continue;
                    element.FadeoutList.Add(startTime, nowF.StartTime);
                    fadeouting = false;
                }
            }

            if (fadeouting && startTime != element.MaxTime)  // 可能存在Fade后还有别的event
            {
                element.FadeoutList.Add(startTime, element.MaxTime);
            }

            // only test not optimized
            var scaList = element.ScaleList;
            for (int i = 0; i < scaList.Count; i++)
            {
                Scale nowF = scaList[i];
                if (i == 0 && nowF.S1.Equals(0) && nowF.StartTime > element.MinTime)  // 最早的F晚于最小开始时间，默认加这一段
                {
                    startTime = element.MinTime;
                    fadeouting = true;
                }

                if (nowF.S2.Equals(0) && !fadeouting)  // f2=0，开始计时
                {
                    startTime = nowF.EndTime;
                    fadeouting = true;
                }
                else if (fadeouting)
                {
                    if (nowF.S1.Equals(0) && nowF.S2.Equals(0))
                        continue;
                    element.FadeoutList.Add(startTime, nowF.StartTime);
                    fadeouting = false;
                }
            }

            if (fadeouting && startTime != element.MaxTime)  // 可能存在Fade后还有别的event
            {
                element.FadeoutList.Add(startTime, element.MaxTime);
            }
        }
    }
}
