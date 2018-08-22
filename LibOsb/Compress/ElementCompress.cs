using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibOsb.Model.EventType;

namespace LibOsb.Compress
{
    public static class ElementCompress
    {
        public static void Compress(this Element element)
        {
            Examine(element);
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
        private static void Examine(Element sbObj)
        {
            if (sbObj.MoveList.Count != 0) CheckTiming(sbObj.MoveList);
            if (sbObj.ScaleList.Count != 0) CheckTiming(sbObj.ScaleList);
            if (sbObj.FadeList.Count != 0) CheckTiming(sbObj.FadeList);
            if (sbObj.RotateList.Count != 0) CheckTiming(sbObj.RotateList);
            if (sbObj.VectorList.Count != 0) CheckTiming(sbObj.VectorList);
            if (sbObj.ColorList.Count != 0) CheckTiming(sbObj.ColorList);
            if (sbObj.MoveXList.Count != 0) CheckTiming(sbObj.MoveXList);
            if (sbObj.MoveYList.Count != 0) CheckTiming(sbObj.MoveYList);
            if (sbObj.ParameterList.Count != 0) CheckTiming(sbObj.ParameterList);
            foreach (var item in sbObj.LoopList) Examine(item);
            foreach (var item in sbObj.TriggerList) Examine(item);

            // 验证物件完全消失的时间段
            int tmpTime = -1;
            bool fadeouting = false;
            for (int j = 0; j < sbObj.FadeList.Count; j++)
            {
                var nowF = sbObj.FadeList[j];
                if (j == 0 && nowF.P11.Equals(0) && nowF.StartTime > sbObj.MinTime)  // 最早的F晚于最小开始时间，默认加这一段
                {
                    sbObj.FadeoutList.Add(sbObj.MinTime, nowF.StartTime);
                }
                else if (nowF.P21.Equals(0) && !fadeouting)  // f2=0，开始计时
                {
                    tmpTime = nowF.EndTime;
                    fadeouting = true;
                }
                else if (fadeouting)
                {
                    if (nowF.P11.Equals(0) && nowF.P21.Equals(0))
                        continue;
                    sbObj.FadeoutList.Add(tmpTime, nowF.StartTime);
                    fadeouting = false;
                }
            }
            if (fadeouting && tmpTime != sbObj.MaxTime)  // 可能存在Fade后还有别的event
            {
                sbObj.FadeoutList.Add(tmpTime, sbObj.MaxTime);
            }
        }

        /// <summary>
        /// 预压缩
        /// </summary>
        private static void PreOptimize(Element element)
        {
            bool flag = true;
            foreach (var item in element.LoopList)
            {
                if (item.HasFade) flag = false;
                PreOptimize(item);
            }
            foreach (var item in element.TriggerList)
            {
                if (item.HasFade) flag = false;
                PreOptimize(item);
            }
            if (!flag) return;

            if (element.ScaleList.Count != 0) FixAll(element, element.ScaleList);
            if (element.RotateList.Count != 0) FixAll(element, element.RotateList);
            if (element.MoveXList.Count != 0) FixAll(element, element.MoveXList);
            if (element.MoveYList.Count != 0) FixAll(element, element.MoveYList);
            if (element.FadeList.Count != 0) FixAll(element, element.FadeList);
            if (element.MoveList.Count != 0) FixAll(element, element.MoveList);
            if (element.VectorList.Count != 0) FixAll(element, element.VectorList);
            if (element.ColorList.Count != 0) FixAll(element, element.ColorList);
            if (element.ParameterList.Count != 0) FixAll(element, element.ParameterList);
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
        /// 检查Timing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static void CheckTiming<T>(List<T> list)
        {
            list.Sort(new EventSort<T>());
            for (int i = 1; i < list.Count; i++)
            {
                dynamic objNext = list[i];
                dynamic objPrevious = list[i - 1];
                if (objPrevious.StartTime > objPrevious.EndTime)
                    throw new ArgumentException("Start time should not be larger than end time.");
                if (objNext.StartTime < objPrevious.EndTime)
                {
                    //throw new Exception(obj_previous.ToString() + Environment.NewLine + obj_next.ToString());
                }
            }
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
            double defaultParam = -1;
            var tType = typeof(T);
            if (tType == typeof(Scale))
                defaultParam = 1;
            else if (tType == typeof(Rotate))
                defaultParam = 0;
            else if (!element.IsLOrT && tType == typeof(MoveX))
                defaultParam = (int)element.DefaultX;
            else if (!element.IsLOrT && tType == typeof(MoveY))
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
                double nowP1 = objNow.P1_1, nowP2 = objNow.P2_1;
                double preP1 = -1, preP2 = -1;
                if (objPre != null)
                {
                    preP1 = objPre.P1_1;
                    preP2 = objPre.P2_1;
                }
                if (i == 0)
                {
                    if (element.IsLOrT) break;
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
            double defParam1 = -1, defParam2 = -1;
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
                double nowP11 = objNow.P1_1, nowP12 = objNow.P1_2, nowP21 = objNow.P2_1, nowP22 = objNow.P2_2;
                double preP11 = -1, preP12 = -1, preP21 = -1, preP22 = -1;
                if (objPre != null)
                {
                    preP11 = objPre.P1_1;
                    preP12 = objPre.P1_2;
                    preP21 = objPre.P2_1;
                    preP22 = objPre.P2_2;
                }
                if (i == 0)
                {
                    if (element.IsLOrT) break;

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
            double defParam1 = -1, defParam2 = -1, defParam3 = -1;
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
                int nowStart = objNow.StartTime, nowEnd = objNow.EndTime;
                int preStart = -1, preEnd = -1;
                if (objPre != null)
                {
                    preStart = objPre.StartTime;
                    preEnd = objPre.EndTime;
                }
                double nowP11 = objNow.P11, nowP12 = objNow.P12, nowP13 = objNow.P13,
                    nowP21 = objNow.P21, nowP22 = objNow.P22, nowP23 = objNow.P23;
                double preP11 = -1, preP12 = -1, preP13 = -1,
                    preP21 = -1, preP22 = -1, preP23 = -1;
                if (objPre != null)
                {
                    preP11 = objPre.P11;
                    preP12 = objPre.P12;
                    preP13 = objPre.P13;
                    preP21 = objPre.P21;
                    preP22 = objPre.P22;
                    preP23 = objPre.P23;
                }
                if (i == 0)
                {
                    if (element.IsLOrT) break;
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
        private class EventSort<T> : IComparer<T>
        {
            public int Compare(T event1, T event2)
            {
                if (event1 == null || event2 == null)
                    throw new NullReferenceException();
                dynamic d1 = event1, d2 = event2;
                if (d1.StartTime >= d2.StartTime) return 1;
                return -1;
            }
        }
    }
}
