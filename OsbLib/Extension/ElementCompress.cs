#if false
using Milkitic.OsbLib.Models;
using Milkitic.OsbLib.Models.EventType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Milkitic.OsbLib.Extension
{
    public static class ElementCompress
    {
        public static void Compress(this Element element)
        {
            Sort(element);
            Examine(element);
            FillFadeoutList(element);
            // 每个类型压缩从后往前
            // 1.删除没用的
            // 2.整合能整合的
            // 3.考虑单event情况
            // 4.排除第一行误加的情况（defaultParams）

            PreOptimize(element);
            NormalOptimize(element);
        }

        /// <summary>
        /// 检查timing是否合法，以及计算透明时间段
        /// </summary>
        public static void Examine(this IEventContainer sbObj)
        {
            var events = sbObj.EventList.GroupBy(k => k.EventType);
            foreach (var kv in events)
            {
                var list = kv.ToArray();
                for (var i = 0; i < list.Length - 1; i++)
                {
                    Event objNext = list[i + 1];
                    Event objNow = list[i];
                    if (objNow.StartTime > objNow.EndTime)
                    {
                        //throw new ArgumentException("Start time should not be larger than end time.");
                    }
                    if (objNext.StartTime < objNow.EndTime)
                    {
                        //throw new Exception(obj_previous.ToString() + Environment.NewLine + obj_next.ToString());
                    }
                }
            }

            if (!(sbObj is Element e))
                return;
            foreach (var item in e.LoopList) Examine(item);
            foreach (var item in e.TriggerList) Examine(item);
        }

        private static void Sort(IEventContainer container)
        {
            container.EventList.Sort(new EventSort<Event>());
            if (!(container is Element e))
                return;
            foreach (var item in e.LoopList) Sort(item);
            foreach (var item in e.TriggerList) Sort(item);
        }

       

        /// <summary>
        /// 预压缩
        /// </summary>
        private static void PreOptimize(IEventContainer container)
        {
            if (container is Element ele)
            {
                bool flag = true;
                foreach (var item in ele.LoopList)
                {
                    if (item.HasFade)
                    {
                        flag = false;
                        break;
                    }
                    PreOptimize(item);
                }
                foreach (var item in ele.TriggerList)
                {
                    if (item.HasFade)
                    {
                        flag = false;
                        break;
                    }
                    PreOptimize(item);
                }
                if (!flag) return;
            }

            if (container.ScaleList.Count != 0) FixAll(container, container.ScaleList);
            if (container.RotateList.Count != 0) FixAll(container, container.RotateList);
            if (container.MoveXList.Count != 0) FixAll(container, container.MoveXList);
            if (container.MoveYList.Count != 0) FixAll(container, container.MoveYList);
            if (container.FadeList.Count != 0) FixAll(container, container.FadeList);
            if (container.MoveList.Count != 0) FixAll(container, container.MoveList);
            if (container.VectorList.Count != 0) FixAll(container, container.VectorList);
            if (container.ColorList.Count != 0) FixAll(container, container.ColorList);
            if (container.ParameterList.Count != 0) FixAll(container, container.ParameterList);
            //if (_FadeoutList.Count > 0 && _FadeoutList.LastEndTime == MaxTime) InnerMaxTime = _FadeoutList.LastStartTime;

            //foreach (var item in LoopList) item.PreOptimize();
            //foreach (var item in TriggerList) item.PreOptimize();
        }

        /// <summary>
        /// 正常压缩
        /// </summary>
        private static void NormalOptimize(Element element)
        {
            if (element.ScaleList.Count != 0) FixSingle(element, element.ScaleList);
            if (element.RotateList.Count != 0) FixSingle(element, element.RotateList);
            if (element.MoveXList.Count != 0) FixSingle(element, element.MoveXList);
            if (element.MoveYList.Count != 0) FixSingle(element, element.MoveYList);
            if (element.FadeList.Count != 0) FixSingle(element, element.FadeList);
            if (element.MoveList.Count != 0) FixDouble(element, element.MoveList);
            if (element.VectorList.Count != 0) FixDouble(element, element.VectorList);
            if (element.ColorList.Count != 0) FixTriple(element, element.ColorList);

            foreach (var item in element.LoopList) NormalOptimize(item);
            foreach (var item in element.TriggerList) NormalOptimize(item);
        }



        /// <summary>
        /// 预压缩
        /// </summary>
        private static void FixAll<T>(Element element, List<T> list)
        {
            var tType = typeof(T);

#region 预压缩部分，待检验
            if (tType != typeof(Fade))
            {
                //int max_i = _list.Count - 1;
                for (int i = 0; i < list.Count; i++)
                {
                    dynamic e = list[i];
                    dynamic e2 = null;
                    if (i != list.Count - 1) e2 = list[i + 1];
                    // 判断当前种类动作是否在某一透明范围内，并且下一个动作的startTime也须在此范围内
                    if (i < list.Count - 1 && element.FadeoutList.InRange(out bool _, e.StartTime, e.EndTime, e2.StartTime))
                    {
                        list.RemoveAt(i);  // 待修改，封装一个方法控制min max的增减
                        i--;
                    }

                    if (i != list.Count - 1) continue;
                    // 判断当前种类最后一个动作是否正处于物件透明状态，而且此状态最大时间即是obj最大时间
                    if (element.FadeoutList.InRange(out bool isLast, e.StartTime, e.EndTime) &&
                       isLast && element.FadeoutList.LastEndTime == element.MaxTime)
                    {
                        RemoveEvent(element, list, i);
                        i--;
                    }
                }
            }
#endregion

            // if (tType == typeof(Scale))
            // FixSingle(ref _list);
            // todo
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

        private static void RemoveEvent<T>(Element element, IList<T> list, int index)
        {
            dynamic evt = list[index];
            if (evt.StartTime == element.MinTime)
            {
                if (element.MinTimeCount > 1)
                    element.MinTimeCount--;
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
        /// <inheritdoc />
        /// <summary>
        /// 以timing排序event
        /// </summary>
        private class EventSort<T> : IComparer<T> where T : Event
        {
            public int Compare(T e1, T e2)
            {
                if (e1 == null || e2 == null)
                    throw new NullReferenceException();

                if (e1.StartTime >= e2.StartTime) return 1;
                return -1;
            }
        }
    }
}
#endif