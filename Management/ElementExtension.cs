using OSharp.Storyboard.Events;
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

        public static void FillFadeoutList(this Element element)
        {
            // 验证物件完全消失的时间段
            float startTime = float.MinValue;
            bool fadeouting = false;
            var fadeList = element.FadeList;
            if (fadeList != null)
            {
                bool isFirst = true;
                foreach (var nowF in fadeList)
                {
                    if (isFirst && nowF.StartOpacity.Equals(0) && nowF.StartTime > element.MinTime)  // 最早的F晚于最小开始时间，默认加这一段
                    {
                        startTime = element.MinTime;
                        fadeouting = true;
                        isFirst = false;
                    }

                    if (nowF.EndOpacity.Equals(0) && !fadeouting)  // f2=0，开始计时
                    {
                        startTime = nowF.EndTime;
                        fadeouting = true;
                    }
                    else if (fadeouting)
                    {
                        if (nowF.StartOpacity.Equals(0) && nowF.EndOpacity.Equals(0))
                            continue;
                        element.FadeoutList.Add(startTime, nowF.StartTime);
                        fadeouting = false;
                    }
                }
            }

            if (fadeouting && startTime != element.MaxTime)  // 可能存在Fade后还有别的event
            {
                element.FadeoutList.Add(startTime, element.MaxTime);
            }

            // only test not optimized
            var scaList = element.ScaleList;
            if (scaList != null)
            {
                bool isFirst = true;
                foreach (Scale nowF in scaList)
                {
                    if (isFirst && nowF.StartScale.Equals(0) && nowF.StartTime > element.MinTime)  // 最早的F晚于最小开始时间，默认加这一段
                    {
                        startTime = element.MinTime;
                        fadeouting = true;
                        isFirst = false;
                    }

                    if (nowF.EndScale.Equals(0) && !fadeouting)  // f2=0，开始计时
                    {
                        startTime = nowF.EndTime;
                        fadeouting = true;
                    }
                    else if (fadeouting)
                    {
                        if (nowF.StartScale.Equals(0) && nowF.EndScale.Equals(0))
                            continue;
                        element.FadeoutList.Add(startTime, nowF.StartTime);
                        fadeouting = false;
                    }
                }
            }

            if (fadeouting && startTime != element.MaxTime)  // 可能存在Fade后还有别的event
            {
                element.FadeoutList.Add(startTime, element.MaxTime);
            }
        }
    }
}
