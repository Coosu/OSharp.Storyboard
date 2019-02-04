using OSharp.Storyboard.Events;
using OSharp.Storyboard.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OSharp.Storyboard.Management
{
    public class ElementCompressor
    {
        private readonly IEnumerable<Element> _elements;
        public EventHandler<ErrorEventArgs> OnErrorOccured;
        private int _threadCount = 1;

        public ElementCompressor(IEnumerable<Element> elements)
        {
            this._elements = elements;
        }

        public ElementCompressor(ElementGroup elementGroup)
        {
            _elements = elementGroup.ElementList;
        }

        public string BackgroundPath { get; set; }

        public int ThreadCount
        {
            get => _threadCount;
            set => _threadCount =
                value < 1
                    ? 1
                    : value > 4
                        ? 4
                        : value;
        }

        public bool IsRunning { get; private set; }

        public async Task CompressAsync()
        {
            IsRunning = true;
            var tasks = new Task[ThreadCount];
            object lockObj = new object();
            ConcurrentQueue<Element> queue = new ConcurrentQueue<Element>();
            var cts = new CancellationTokenSource();
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        Element element;
                        //lock (lockObj)
                        {
                            if (!queue.IsEmpty)
                            {
                                if (!queue.TryDequeue(out element))
                                {
                                    continue;
                                }
                            }
                            else
                                continue;
                        }

                        InnerCompress(element);
                    }
                }, cts.Token);
            }

            var enqueueTask = new Task(() =>
            {
                foreach (var element in _elements)
                {
                    //lock (lockObj)
                    {
                        queue.Enqueue(element);
                    }
                }

                while (!queue.IsEmpty)
                {
                    Thread.Sleep(1);
                }

                cts.Cancel();
            }, cts.Token);

            enqueueTask.Start();

            await Task.WhenAll(tasks);
            IsRunning = false;
        }

        private void InnerCompress(Element element)
        {
            if (element.ImagePath == BackgroundPath &&
                element.Layer == LayerType.Background)
            {
                element.IsBackground = true;
            }

            // 每个类型压缩从后往前
            // 1.删除没用的
            // 2.整合能整合的
            // 3.考虑单event情况
            // 4.排除第一行误加的情况 (defaultParams)
            int b = 0;
            element.OnErrorOccured += (sender, args) =>
            {
                OnErrorOccured?.Invoke(sender, args);
                b++;
            };
            element.Examine();
            element.OnErrorOccured = null;

            if (b > 0)
            {
                var arg = new ErrorEventArgs
                {
                    Message = $"Examine failed. Found {b} error(s)."
                };
                OnErrorOccured?.Invoke(this, arg);
                //if (!arg.TryToContinue)
                //    continue;
            }

            element.FillObsoleteList();
            PreOptimize(element);
            NormalOptimize(element);
        }

        /// <summary>
        /// 预压缩
        /// </summary>
        private static void PreOptimize(EventContainer container)
        {
            if (container is Element ele)
            {
                foreach (var item in ele.LoopList)
                {
                    PreOptimize(item);
                }

                foreach (var item in ele.TriggerList)
                {
                    PreOptimize(item);
                }
            }

            if (container.EventList.Any())
                RemoveByObsoletedList(container, container.EventList.ToList());
        }

        /// <summary>
        /// 正常压缩
        /// </summary>
        private static void NormalOptimize(EventContainer container)
        {
            if (container is Element ele)
            {
                foreach (var item in ele.LoopList)
                {
                    NormalOptimize(item);
                }

                foreach (var item in ele.TriggerList)
                {
                    NormalOptimize(item);
                }
            }

            if (container.EventList.Any())
            {
                RemoveByLogic(container, container.EventList.ToList());
            }
        }

        /// <summary>
        /// 根据ObsoletedList，移除不必要的命令。
        /// </summary>
        private static void RemoveByObsoletedList(EventContainer container, List<CommonEvent> eventList)
        {
            if (container.ObsoleteList.TimingList.Count == 0) return;
            var groups = eventList.GroupBy(k => k.EventType).Where(k => k.Key != EventType.Fade);
            foreach (var group in groups)
            {
                var list = group.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    CommonEvent nowE = list[i];
                    CommonEvent nextE =
                        i == list.Count - 1
                            ? null
                            : list[i + 1];

                    /*
                     * 若当前Event在某Obsolete Range内，且下一Event的StartTime也在此Obsolete Range内，则删除。
                     * 若当前Event是此种类最后一个（无下一个Event），那么需要此Event在某Obsolete Range内，且此Obsolete Range持续到Container结束。
                     * 另注意：若此Event为控制Obsolete Range的Event，则将其过滤。（判断是否正好在某段Obsolete Range的StartTime或EndTime上）
                    */

                    // 判断是否此Event为控制Obsolete Range的Event。
                    if (!(nowE.OnObsoleteTimingRange(container) &&
                          EventExtension.UnworthyDictionary.ContainsKey(nowE.EventType)))
                    {
                        bool canRemove;

                        // 若当前Event是此种类最后一个（无下一个Event)。
                        if (nextE == null)
                        {
                            // 判断是否此Event在某Obsolete Range内，且此Obsolete Range持续到Container结束。
                            canRemove = nowE.InObsoleteTimingRange(container, out var range) &&
                                        range.EndTime == container.MaxTime;
                        }
                        else
                        {
                            // 判断是否此Event在某Obsolete Range内，且下一Event的StartTime也在此Obsolete Range内。
                            canRemove = container.ObsoleteList.ContainsTimingPoint(out _,
                                nowE.StartTime, nowE.EndTime, nextE.StartTime);
                        }

                        if (canRemove)
                        {
                            RemoveEvent(container, list, nowE);
                            i--;
                        }
                    }

                    // 判断当前种类最后一个动作是否正处于物件透明状态，而且此状态最大时间即是obj最大时间
                }
            }
        }

        /// <summary>
        /// 根据逻辑，进行命令优化。
        /// </summary>
        /// <param name="container"></param>
        /// <param name="eventList"></param>
        private static void RemoveByLogic(EventContainer container, List<CommonEvent> eventList)
        {
            var groups = eventList.GroupBy(k => k.EventType);
            foreach (var group in groups)
            {
                EventType type = group.Key;
                var list = group.ToList();

                int index = list.Count - 1;
                while (index >= 0)
                {
                    CommonEvent nowE = list[index];

                    if (container is Element ele &&
                        ele.TriggerList.Any(k => nowE.EndTime >= k.StartTime || nowE.StartTime <= k.EndTime) &&
                        ele.LoopList.Any(k => nowE.EndTime >= k.StartTime || nowE.StartTime <= k.EndTime))
                    {
                        index--;
                        continue;
                    }
                    // 首个event     
                    if (index == 0)
                    {
                        if (eventList.Count <= 1) return;
                        //S,0,300,,1
                        //S,0,400,500,0.5
                        /* 
                         * 当 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                         * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                         * 且 此event的param固定
                         * 且 此event.param=default
                         * 且 唯一
                         */
                        if (nowE.IsTimeInRange(container) && nowE.IsStaticAndDefault())
                        {
                            // Remove
                            RemoveEvent(container, list, nowE);
                        }
                        /*
                         * 当 此event为move，param固定，且唯一时
                         */
                        else if (type == EventType.Move
                                 && container is Element element)
                        {
                            if (list.Count == 1 && nowE.IsStatic()
                                                && nowE.IsTimeInRange(container)
                                                && eventList.Count > 1)
                            {
                                var move = (Move)nowE;
                                if (nowE.Start.All(k => k == (int)k)) //若为小数，不归并
                                {
                                    element.DefaultX = move.StartX;
                                    element.DefaultY = move.StartY;

                                    // Remove
                                    RemoveEvent(container, list, nowE);
                                }
                                else if (move.EqualsInitialPosition(element))
                                {
                                    // Remove
                                    RemoveEvent(container, list, nowE);
                                }
                                else
                                {
                                    element.DefaultX = 0;
                                    element.DefaultY = 0;
                                }
                            }
                            else
                            {
                                element.DefaultX = 0;
                                element.DefaultY = 0;
                            }
                        }
                        break;
                    }
                    else
                    {
                        CommonEvent preE = list[index - 1];
                        //if (container is Element ele2 &&
                        //    ele2.TriggerList.Any(k => nowE.EndTime >= k.StartTime && nowE.StartTime <= k.EndTime) &&
                        //    ele2.LoopList.Any(k => nowE.EndTime >= k.StartTime && nowE.StartTime <= k.EndTime))
                        //{
                        //    index--;
                        //    continue;
                        //}
                        // 优先进行合并，若不符合再进行删除。
                        /*
                         * 当 此event与前event一致，且前后param皆固定
                        */
                        if (nowE.IsStatic()
                            && preE.IsStatic()
                            && EventCompare.IsEventSequent(preE, nowE))
                        {
                            preE.EndTime = nowE.EndTime;  // 整合至前面: 前一个命令的结束时间延伸

                            // Remove
                            RemoveEvent(container, list, nowE);
                            index--;
                        }
                        /*
                         * 当 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                         * 且 此event的param固定
                         * 且 此event当前动作 = 此event上个动作
                        */
                        else if (nowE.IsSmallerThenMaxTime(container) /*||
                                 type == EventType.Fade && nowStartP.SequenceEqual(EventExtension.UnworthyDictionary[EventType.Fade]) */
                                 && nowE.IsStatic()
                                 && EventCompare.IsEventSequent(preE, nowE))
                        {
                            // Remove
                            RemoveEvent(container, list, nowE);
                            index--;
                        }
                        // 存在一种非正常的无效情况，例如：
                        // F,0,0,,0
                        // F,0,0,5000,1
                        // S,0,0,,0.5,0.8
                        // 此时，第一行的F可被删除。或者：
                        // F,0,0,,1
                        // F,0,1000,,0
                        // F,0,1000,5000,1
                        // S,0,0,,0.5,0.8
                        // 此时，第二行的F可被删除。
                        else if (nowE.StartTime == preE.EndTime &&
                                 preE.StartTime == preE.EndTime)
                        {
                            if (index > 1)
                            {
                                // Remove
                                RemoveEvent(container, list, preE);
                                index--;
                            }
                            else if (preE.EqualsMultiMinTime(container))
                            {
                                // Remove
                                RemoveEvent(container, list, preE);
                                index--;
                            }
                            else if (preE.IsStatic() && EventCompare.IsEventSequent(preE, nowE))
                            {
                                // Remove
                                RemoveEvent(container, list, preE);
                                index--;
                            }
                            else
                                index--;
                        }
                        else index--;
                    }
                }
            }
        }

        private static void RemoveEvent(EventContainer sourceContainer, ICollection<CommonEvent> eventList, CommonEvent e)
        {
            sourceContainer.EventList.Remove(e);
            eventList.Remove(e);
        }
    }
}