using OSharp.Storyboard.Events;
using OSharp.Storyboard.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSharp.Storyboard.Management
{
    public static class ElementCompress
    {
        public static void Compress(this Element element)
        {
            element.Examine();
            element.FillObsoleteList();
            // 每个类型压缩从后往前
            // 1.删除没用的
            // 2.整合能整合的
            // 3.考虑单event情况
            // 4.排除第一行误加的情况（defaultParams）
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
                RemoveObsoletedEvents(container, container.EventList.ToList());

            //if (container.ScaleList.Count != 0) FixAll(container, container.ScaleList);
            //if (container.RotateList.Count != 0) FixAll(container, container.RotateList);
            //if (container.MoveXList.Count != 0) FixAll(container, container.MoveXList);
            //if (container.MoveYList.Count != 0) FixAll(container, container.MoveYList);
            //if (container.FadeList.Count != 0) FixAll(container, container.FadeList);
            //if (container.MoveList.Count != 0) FixAll(container, container.MoveList);
            //if (container.VectorList.Count != 0) FixAll(container, container.VectorList);
            //if (container.ColorList.Count != 0) FixAll(container, container.ColorList);
            //if (container.ParameterList.Count != 0) FixAll(container, container.ParameterList);
            //if (_FadeoutList.Count > 0 && _FadeoutList.LastEndTime == MaxTime) InnerMaxTime = _FadeoutList.LastStartTime;

            //foreach (var item in LoopList) item.PreOptimize();
            //foreach (var item in TriggerList) item.PreOptimize();
        }

        /// <summary>
        /// 预压缩
        /// </summary>
        private static void RemoveObsoletedEvents(EventContainer container, List<Event> eventList)
        {
            #region 预压缩部分，待检验

            var groups = eventList.GroupBy(k => k.EventType).Where(k => k.Key != EventType.Fade);
            foreach (var group in groups)
            {
                var list = group.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    Event nowE = list[i];
                    Event nextE =
                        i == list.Count
                            ? null
                            : list[i + 1];
                    // 判断当前种类动作是否在某一透明范围内，并且下一个动作的startTime也须在此范围内
                    var b = nextE == null
                        ? container.ObsoleteList.ContainsTimingPoint(nowE.StartTime, nowE.EndTime)
                        : container.ObsoleteList.ContainsTimingPoint(nowE.StartTime, nowE.EndTime, nextE.StartTime);

                    if (!b) continue;
                    container.EventList.Remove(nowE);
                    list.Remove(nowE);
                    i--;

                    // 判断当前种类最后一个动作是否正处于物件透明状态，而且此状态最大时间即是obj最大时间
                }
            }

            #endregion

            // if (tType == typeof(Scale))
            // FixSingle(ref _list);
            // todo
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
                    Fix(item, container.EventList.ToList());
                }

                foreach (var item in ele.TriggerList)
                {
                    Fix(item, container.EventList.ToList());
                }
            }

            if (container.EventList.Any())
            {
                Fix(container, container.EventList.ToList());
            }

            //if (container.ScaleList.Count() != 0) FixSingle(container, container.ScaleList);
            //if (container.RotateList.Count() != 0) FixSingle(container, container.RotateList);
            //if (container.MoveXList.Count() != 0) FixSingle(container, container.MoveXList);
            //if (container.MoveYList.Count() != 0) FixSingle(container, container.MoveYList);
            //if (container.FadeList.Count() != 0) FixSingle(container, container.FadeList);
            //if (container.MoveList.Count() != 0) FixDouble(container, container.MoveList);
            //if (container.VectorList.Count() != 0) FixDouble(container, container.VectorList);
            //if (container.ColorList.Count() != 0) FixTriple(container, container.ColorList);

            //foreach (var item in container.LoopList) NormalOptimize(item);
            //foreach (var item in container.TriggerList) NormalOptimize(item);
        }

        private static void Fix(EventContainer container, List<Event> eventList)
        {
            //float defaultParam = -1;
            //var tType = typeof(T);
            //if (tType == typeof(Scale))
            //    defaultParam = 1;
            //else if (tType == typeof(Rotate))
            //    defaultParam = 0;
            //else if (!element.IsLorT && tType == typeof(MoveX))
            //    defaultParam = (int)element.DefaultX;
            //else if (!element.IsLorT && tType == typeof(MoveY))
            //    defaultParam = (int)element.DefaultY;
            //else if (tType == typeof(Fade))
            //    defaultParam = 1;
            var groups = eventList.GroupBy(k => k.EventType);
            foreach (var group in groups)
            {
                EventType type = group.Key;
                var list = group.ToList();

                int index = list.Count - 1;
                while (index >= 0)
                {
                    Event nowE = list[index];

                    Event preE;
                    if (index >= 1)
                    {
                        preE = list[index - 1];
                    }

                    float nowStartT = nowE.StartTime, nowEndT = nowE.EndTime;
                    float preStartT = float.MinValue, preEndT = float.MinValue;
                    if (preE != null)
                    {
                        preStartT = preE.StartTime;
                        preEndT = preE.EndTime;
                    }

                    float[] nowStartP = nowE.Start, nowEndP = nowE.End;
                    float[] preStartP, preEndP;
                    if (preE != null)
                    {
                        preStartP = preE.Start;
                        preEndP = preE.End;
                    }

                    // 首个event     
                    if (index == 0)
                    {
                        //S,0,300,,1
                        //S,0,400,500,0.5
                        /* 当 此event唯一 *unnecessary
                         * 且 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                         * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                         * 且 此event的param固定
                         * 且 此event.param=default
                         */
                        if (/*list.Count == 1
                            && */(nowEndT < container.MaxTime ||
                                nowEndT == container.MaxTime && container.MaxTimeCount > 1
                            )
                            && (nowStartT > container.MinTime ||
                                nowStartT == container.MinTime && container.MinTimeCount > 1
                            )
                            && nowStartP.SequenceEqual(nowEndP)
                            && nowStartP.SequenceEqual(EventExtension.UnworthyDictionary[type]))
                        {
                            // Move特有
                            if (type == EventType.Move && container is Element element)
                            {
                                //var @event = (Move)nowE;
                                if (nowStartP.SequenceEqual(nowStartP.Select(k => (float)(int)k))) //若为小数，不归并
                                {
                                    element.DefaultX = nowE.Start[0];
                                    element.DefaultY = nowE.Start[1];
                                    RemoveEvent(element, list, 0);
                                }
                                else if (nowP11.Equals(element.DefaultX) && nowP12.Equals(element.DefaultY))
                                {
                                    RemoveEvent(element, list, 0);
                                }
                            }
                            else
                            {
                                // Remove
                                container.EventList.Remove(nowE);
                                list.Remove(nowE);
                            }

                        }



                        break;
                    }
                    else
                    {
                        // 优先进行合并，若不符合再进行删除。
                        /*
                         * 当 此event与前event一致，且前后param皆固定
                        */
                        if (nowStartP.SequenceEqual(nowEndP)
                            && preStartP.SequenceEqual(preEndP)
                            && preStartP.SequenceEqual(nowStartP))
                        {

                            preE.EndTime = nowE.EndTime;  // 整合至前面: 前一个命令的结束时间延伸

                            //if (preStartT == container.MinTime && container.MinTimeCount > 1) // todo: optimize: ?
                            //{
                            //    //preE.StartTime = preE.EndTime; // old
                            //    preE.EndTime = preE.StartTime;
                            //}

                            // Remove
                            container.EventList.Remove(nowE);
                            list.Remove(nowE);
                            //index = list.Count - 1; // todo: optimize: ?
                            index--;
                        }
                        /*
                         * 当 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                         * 且 此event的param固定
                         * 且 此event当前动作 = 此event上个动作
                         * (包含一个F的特例) todo: optimize: ?
                        */
                        else if ((nowEndT < container.MaxTime ||
                                  nowEndT == container.MaxTime && container.MaxTimeCount > 1 /*||
                         type == EventType.Fade && nowStartP.SequenceEqual(EventExtension.UnworthyDictionary[EventType.Fade]) */
                                 )
                                 && nowStartP.SequenceEqual(nowEndP)
                                 && nowStartP.SequenceEqual(preEndP))
                        {
                            // Remove
                            container.EventList.Remove(nowE);
                            list.Remove(nowE);
                            //index = list.Count - 1; // todo: optimize: ?
                            index--;
                        }
                        else index--;
                    }
                }
            }


        }

        /// <summary>
        /// 正常压缩的泛型方法（EventSingle）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="list"></param>
        private static void FixSingle<T>(Element element, List<T> list)
        {
            float defaultParam = -1;
            var tType = typeof(T);
            if (tType == typeof(Scale))
                defaultParam = 1;
            else if (tType == typeof(Rotate))
                defaultParam = 0;
            else if (!element.IsLorT && tType == typeof(MoveX))
                defaultParam = (int)element.DefaultX;
            else if (!element.IsLorT && tType == typeof(MoveY))
                defaultParam = (int)element.DefaultY;
            else if (tType == typeof(Fade))
                defaultParam = 1;

            int i = list.Count - 1;
            while (i >= 0)
            {
                dynamic objNow = list[i];
                dynamic objPre = null;
                if (i >= 1) objPre = list[i - 1];
                int nowStart = objNow.StartTime, nowEnd = objNow.EndTime;
                int preStart = -1, preEnd = -1;
                if (objPre != null)
                {
                    preStart = objPre.StartTime;
                    preEnd = objPre.EndTime;
                }
                float nowP1 = objNow.P1_1, nowP2 = objNow.P2_1;
                float preP1 = -1, preP2 = -1;
                if (objPre != null)
                {
                    preP1 = objPre.P1_1;
                    preP2 = objPre.P2_1;
                }
                if (i == 0)
                {
                    if (element.IsLorT) break;
                    /* 当 此event唯一
                     * 且 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                     * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                     * 且 此event的param固定
                     * 且 此event.param=default
                     */
                    if (list.Count == 1 &&
                    (nowEnd < element.MaxTime || nowEnd == element.MaxTime && element.MaxTimeCount > 1) &&
                    (nowStart > element.MinTime || nowStart == element.MinTime && element.MinTimeCount > 1) &&
                    nowP1.Equals(nowP2) &&
                    nowP1.Equals(defaultParam))
                    {
                        // Remove 0
                        RemoveEvent(element, list, 0);
                    }

                    //// 加个条件 对第一行再判断，因为经常可能会出现误加了一个默认值的event
                    ////S,0,300,,1
                    ////S,0,400,500,0.5
                    //dynamic objNext = null;
                    //if (_list.Count > 1 ) objNext = _list[1];

                    //else if (_list.Count > 1 &&
                    //    (now_end < this.MaxTime || now_end == this.MaxTime && MaxTimeCount > 1) &&
                    //    (now_start > this.MinTime || now_start == this.MinTime && MinTimeCount > 1) &&
                    //    objNow.IsStatic &&
                    //    objNow.P2_1 == objNext.P1_1 &&
                    //    objNow.P1_1 == defaultParam)
                    //{
                    //    // Remove 0
                    //    _Remove_Event(obj0, 0);
                    //}

                    break;
                }

                /* 当 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                * 且 此event的param固定
                * 且 此event当前动作 = 此event上个动作
                * (包含一个F的特例)
                */
                if ((nowEnd < element.MaxTime || nowEnd == element.MaxTime && element.MaxTimeCount > 1 ||
                     tType == typeof(Fade) && nowP1.Equals(0)) && nowP1.Equals(nowP2) && nowP1.Equals(preP2))
                {
                    // Remove i
                    RemoveEvent(element, list, i);
                    i = list.Count - 1;
                }
                /* 当 此event与前event一致，且前后param皆固定 （有待考证）
                 */
                else if (nowP1.Equals(nowP2) &&
                  preP1.Equals(preP2) &&
                  preP1.Equals(nowP1))
                {

                    objPre.EndTime = objNow.EndTime;  // 整合至前面
                    if (preStart == element.MinTime && element.MinTimeCount > 1)  // ??
                    {
                        objPre.StartTime = objPre.EndTime;
                    }
                    // Remove i
                    RemoveEvent(element, list, i);
                    i = list.Count - 1;
                }
                else i--;

            }
        }

        /// <summary>
        /// 正常压缩的泛型方法（EventDouble）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="list"></param>
        private static void FixDouble<T>(Element element, List<T> list)
        {
            float defParam1 = -1, defParam2 = -1;
            var tType = typeof(T);
            if (tType == typeof(Move))
            {
                defParam1 = 320;
                defParam2 = 240;
                element.DefaultX = 0;
                element.DefaultY = 0;
            }
            else if (tType == typeof(Vector))
            {
                defParam1 = 1;
                defParam2 = 1;
            }

            int i = list.Count - 1;
            while (i >= 0)
            {
                dynamic objNow = list[i];
                dynamic objPre = null;
                if (i >= 1) objPre = list[i - 1];
                int nowStart = objNow.StartTime, nowEnd = objNow.EndTime;
                int preStart = -1, preEnd = -1;
                if (objPre != null)
                {
                    preStart = objPre.StartTime;
                    preEnd = objPre.EndTime;
                }
                float nowP11 = objNow.P1_1, nowP12 = objNow.P1_2, nowP21 = objNow.P2_1, nowP22 = objNow.P2_2;
                float preP11 = -1, preP12 = -1, preP21 = -1, preP22 = -1;
                if (objPre != null)
                {
                    preP11 = objPre.P1_1;
                    preP12 = objPre.P1_2;
                    preP21 = objPre.P2_1;
                    preP22 = objPre.P2_2;
                }
                if (i == 0)
                {
                    if (element.IsLorT) break;

                    /* 当 此event唯一
                     * 且 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                     * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                     * 且 此event的param固定
                     * 且 此event.param=default
                     */
                    if (list.Count == 1 &&
                    (nowEnd < element.MaxTime || nowEnd == element.MaxTime && element.MaxTimeCount > 1) &&
                    (nowStart > element.MinTime || nowStart == element.MinTime && element.MinTimeCount > 1) &&
                    objNow.IsStatic)
                    {

                        // Move特有
                        if (tType == typeof(Move))
                        {
                            if (nowP11.Equals((int)nowP11) && nowP12.Equals((int)nowP12))
                            {
                                element.DefaultX = nowP11;
                                element.DefaultY = nowP12;
                                RemoveEvent(element, list, 0);
                            }
                            else if (nowP11.Equals(element.DefaultX) && nowP12.Equals(element.DefaultY))
                            {
                                RemoveEvent(element, list, 0);
                            }
                        }
                        else
                        {
                            if (nowP11.Equals(defParam1) && nowP12.Equals(defParam2))
                            {
                                RemoveEvent(element, list, 0);
                            }
                        }
                    }
                    break;
                }

                /* 当 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                * 且 此event的param固定
                * 且 此event当前动作 = 此event上个动作
                */
                if ((nowEnd < element.MaxTime || nowEnd == element.MaxTime && element.MaxTimeCount > 1) &&
                     objNow.IsStatic &&
                     nowP11.Equals(preP21) && nowP12.Equals(preP22))
                {
                    RemoveEvent(element, list, i);
                    i = list.Count - 1;
                }
                /* 当 此event与前event一致，且前后param皆固定 （有待考证）
                 */
                else if (objNow.IsStatic && objPre.IsStatic &&
                         nowP11.Equals(preP21) && nowP12.Equals(preP22))
                {
                    objPre.EndTime = objNow.EndTime;  // 整合至前面
                    if (preStart == element.MinTime && element.MinTimeCount > 1)  // ??
                    {
                        objPre.StartTime = objPre.EndTime;
                    }
                    // Remove i
                    RemoveEvent(element, list, i);
                    i = list.Count - 1;
                }
                else i--;
            }
        }

        /// <summary>
        /// 正常压缩的泛型方法（EventTriple）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="list"></param>
        private static void FixTriple<T>(Element element, List<T> list)
        {
            float defParam1 = -1, defParam2 = -1, defParam3 = -1;
            var tType = typeof(T);
            if (tType == typeof(Color))
            {
                defParam1 = 255;
                defParam2 = 255;
                defParam3 = 255;
            }

            int i = list.Count - 1;
            while (i >= 0)
            {
                Color objNow = (Color)(object)list[i];
                Color objPre = null;
                if (i >= 1) objPre = (Color)(object)list[i - 1];
                float nowStart = objNow.StartTime, nowEnd = objNow.EndTime;
                float preStart = -1, preEnd = -1;
                if (objPre != null)
                {
                    preStart = objPre.StartTime;
                    preEnd = objPre.EndTime;
                }
                float nowP11 = objNow.Start.x, nowP12 = objNow.Start.y, nowP13 = objNow.Start.z,
                    nowP21 = objNow.End.x, nowP22 = objNow.End.y, nowP23 = objNow.End.z;
                float preP11 = -1, preP12 = -1, preP13 = -1,
                    preP21 = -1, preP22 = -1, preP23 = -1;
                if (objPre != null)
                {
                    preP11 = objPre.Start.x;
                    preP12 = objPre.Start.y;
                    preP13 = objPre.Start.z;
                    preP21 = objPre.End.x;
                    preP22 = objPre.End.y;
                    preP23 = objPre.End.z;
                }
                if (i == 0)
                {
                    if (element.IsLorT) break;
                    /* 当 此event唯一
                     * 且 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                     * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                     * 且 此event的param固定
                     * 且 此event.param=default
                     */
                    if (list.Count == 1 &&
                    (nowEnd < element.MaxTime || nowEnd == element.MaxTime && element.MaxTimeCount > 1) &&
                    (nowStart > element.MinTime || nowStart == element.MinTime && element.MinTimeCount > 1) &&
                    objNow.IsStatic &&
                    nowP11.Equals(defParam1) && nowP12.Equals(defParam2) && nowP13.Equals(defParam3))
                    {
                        RemoveEvent(element, list, 0);
                    }
                    break;
                }

                /* 当 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                * 且 此event的param固定
                * 且 此event当前动作 = 此event上个动作
                */
                if ((nowEnd < element.MaxTime || nowEnd == element.MaxTime && element.MaxTimeCount > 1) &&
                     objNow.IsStatic &&
                     nowP11.Equals(preP21) && nowP12.Equals(preP22) && nowP13.Equals(preP23))
                {
                    RemoveEvent(element, list, i);
                    i = list.Count - 1;
                }
                /* 当 此event与前event一致，且前后param皆固定 （有待考证）
                 */
                else if (objNow.IsStatic && objPre.IsStatic && nowP11.Equals(preP21) && nowP12.Equals(preP22) &&
                         nowP13.Equals(preP23))
                {
                    objPre.EndTime = objNow.EndTime;  // 整合至前面
                    if (preStart == element.MinTime && element.MinTimeCount > 1)  // ??
                    {
                        objPre.StartTime = objPre.EndTime;
                    }
                    // Remove i
                    RemoveEvent(element, list, i);
                    i = list.Count - 1;
                }
                else i--;
            }

        }

        private static void RemoveEvent(Element element, IList<T> list, int index)
        {
            var evt = list[index];
            if (evt.StartTime == element.MinTime)
            {
                if (element.MinTimeCount > 1)
                {
                    //element.MinTimeCount--;
                }
                else throw new NotImplementedException();
            }
            if (evt.EndTime == element.MaxTime)
            {
                if (element.MaxTimeCount > 1)
                    element.MaxTimeCount--;
                //else  // 待验证
            }
            list.RemoveAt(index);
        }
    }
}