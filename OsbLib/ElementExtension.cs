using Milkitic.OsbLib.Compress;
using Milkitic.OsbLib.Models.EventType;

namespace Milkitic.OsbLib
{
    public static class ElementExtension
    {
        public static void Expand(this ElementGroup eleG)
        {
            foreach (var ele in eleG.ElementList)
                ele.Expand();
        }

        public static void Expand(this Element ele)
        {
            foreach (var t in ele.LoopList)
                t.Expand();
            foreach (var t in ele.TriggerList)
                t.Expand();

            foreach (var loop in ele.LoopList)
            {
                var loopCount = loop.LoopCount;
                var startT = loop.StartTime;
                for (int times = 0; times < loopCount; times++)
                {
                    var additionT = startT + times * loop.InnerMaxTime;
                    foreach (var f in loop.FadeList)
                        ele.FadeList.Add(new Fade(f.Easing, additionT + f.StartTime, additionT + f.EndTime, f.Start, f.End));
                    foreach (var r in loop.RotateList)
                        ele.RotateList.Add(new Rotate(r.Easing, additionT + r.StartTime, additionT + r.EndTime, r.Start, r.End));
                    foreach (var s in loop.ScaleList)
                        ele.ScaleList.Add(new Scale(s.Easing, additionT + s.StartTime, additionT + s.EndTime, s.Start, s.End));
                    foreach (var mx in loop.MoveXList)
                        ele.MoveXList.Add(new MoveX(mx.Easing, additionT + mx.StartTime, additionT + mx.EndTime, mx.Start, mx.End));
                    foreach (var my in loop.MoveYList)
                        ele.MoveYList.Add(new MoveY(my.Easing, additionT + my.StartTime, additionT + my.EndTime, my.Start, my.End));
                    foreach (var m in loop.MoveList)
                        ele.MoveList.Add(new Move(m.Easing, additionT + m.StartTime, additionT + m.EndTime, m.Start, m.End));
                    foreach (var v in loop.VectorList)
                        ele.VectorList.Add(new Vector(v.Easing, additionT + v.StartTime, additionT + v.EndTime, v.Start, v.End));
                    foreach (var c in loop.ColorList)
                        ele.ColorList.Add(new Color(c.Easing, additionT + c.StartTime, additionT + c.EndTime, c.Start, c.End));
                    foreach (var p in loop.ParameterList)
                        ele.ParameterList.Add(new Parameter(p.Easing, additionT + p.StartTime, additionT + p.EndTime, p.PType));
                }
            }
            ele.Examine();


            for (var i = 0; i < ele.MoveList.Count - 1; i++)
            {
                if (ele.MoveList[i].Start == ele.MoveList[i].End)
                    ele.MoveList[i].EndTime = ele.MoveList[i + 1].StartTime;
                if (ele.MoveList[i].EndTime != ele.MoveList[i + 1].StartTime)
                {
                    ele.MoveList.Insert(i + 1,
                        new Move(Enums.EasingType.Linear, (int)ele.MoveList[i].EndTime,
                            (int)ele.MoveList[i + 1].StartTime, ele.MoveList[i].End, ele.MoveList[i].End));
                }
            }

            for (var i = 0; i < ele.MoveXList.Count - 1; i++)
            {
                if (ele.MoveXList[i].Start == ele.MoveXList[i].End)
                    ele.MoveXList[i].EndTime = ele.MoveXList[i + 1].StartTime;
                if (ele.MoveXList[i].EndTime != ele.MoveXList[i + 1].StartTime)
                {
                    ele.MoveXList.Insert(i + 1,
                        new MoveX(Enums.EasingType.Linear, (int)ele.MoveXList[i].EndTime,
                            (int)ele.MoveXList[i + 1].StartTime, ele.MoveXList[i].End, ele.MoveXList[i].End));
                }
            }

            for (var i = 0; i < ele.MoveYList.Count - 1; i++)
            {
                if (ele.MoveYList[i].Start == ele.MoveYList[i].End)
                    ele.MoveYList[i].EndTime = ele.MoveYList[i + 1].StartTime;
                if (ele.MoveYList[i].EndTime != ele.MoveYList[i + 1].StartTime)
                {
                    ele.MoveYList.Insert(i + 1,
                        new MoveY(Enums.EasingType.Linear, (int)ele.MoveYList[i].EndTime,
                            (int)ele.MoveYList[i + 1].StartTime, ele.MoveYList[i].End, ele.MoveYList[i].End));
                }
            }

            for (var i = 0; i < ele.ColorList.Count - 1; i++)
            {
                if (ele.ColorList[i].Start == ele.ColorList[i].End)
                    ele.ColorList[i].EndTime = ele.ColorList[i + 1].StartTime;
                if (ele.ColorList[i].EndTime != ele.ColorList[i + 1].StartTime)
                {
                    ele.ColorList.Insert(i + 1,
                        new Color(Enums.EasingType.Linear, (int)ele.ColorList[i].EndTime,
                            (int)ele.ColorList[i + 1].StartTime, ele.ColorList[i].End, ele.ColorList[i].End));
                }
            }

            for (var i = 0; i < ele.FadeList.Count - 1; i++)
            {
                if (ele.FadeList[i].Start == ele.FadeList[i].End)
                    ele.FadeList[i].EndTime = ele.FadeList[i + 1].StartTime;
                if (ele.FadeList[i].EndTime != ele.FadeList[i + 1].StartTime)
                {
                    ele.FadeList.Insert(i + 1,
                        new Fade(Enums.EasingType.Linear, (int)ele.FadeList[i].EndTime,
                            (int)ele.FadeList[i + 1].StartTime, ele.FadeList[i].End, ele.FadeList[i].End));
                }
            }

            for (var i = 0; i < ele.ParameterList.Count - 1; i++)
            {
                if (ele.ParameterList[i].PType == ele.ParameterList[i].PType)
                    ele.ParameterList[i].EndTime = ele.ParameterList[i + 1].StartTime;
                if (ele.ParameterList[i].EndTime != ele.ParameterList[i + 1].StartTime)
                {
                    ele.ParameterList.Insert(i + 1,
                        new Parameter(Enums.EasingType.Linear, (int)ele.ParameterList[i].EndTime,
                            (int)ele.ParameterList[i + 1].StartTime, ele.ParameterList[i].PType));
                }
            }

            for (var i = 0; i < ele.RotateList.Count - 1; i++)
            {
                if (ele.RotateList[i].Start == ele.RotateList[i].End)
                    ele.RotateList[i].EndTime = ele.RotateList[i + 1].StartTime;
                if (ele.RotateList[i].EndTime != ele.RotateList[i + 1].StartTime)
                {
                    ele.RotateList.Insert(i + 1,
                        new Rotate(Enums.EasingType.Linear, (int)ele.RotateList[i].EndTime,
                            (int)ele.RotateList[i + 1].StartTime, ele.RotateList[i].End, ele.RotateList[i].End));
                }
            }

            for (var i = 0; i < ele.ScaleList.Count - 1; i++)
            {
                if (ele.ScaleList[i].Start == ele.ScaleList[i].End)
                    ele.ScaleList[i].EndTime = ele.ScaleList[i + 1].StartTime;
                if (ele.ScaleList[i].EndTime != ele.ScaleList[i + 1].StartTime)
                {
                    ele.ScaleList.Insert(i + 1,
                        new Scale(Enums.EasingType.Linear, (int)ele.ScaleList[i].EndTime,
                            (int)ele.ScaleList[i + 1].StartTime, ele.ScaleList[i].End, ele.ScaleList[i].End));
                }
            }

            for (var i = 0; i < ele.VectorList.Count - 1; i++)
            {
                if (ele.VectorList[i].Start == ele.VectorList[i].End)
                    ele.VectorList[i].EndTime = ele.VectorList[i + 1].StartTime;
                if (ele.VectorList[i].EndTime != ele.VectorList[i + 1].StartTime)
                {
                    ele.VectorList.Insert(i + 1,
                        new Vector(Enums.EasingType.Linear, (int)ele.VectorList[i].EndTime,
                            (int)ele.VectorList[i + 1].StartTime, ele.VectorList[i].End, ele.VectorList[i].End));
                }
            }
        }
    }
}
