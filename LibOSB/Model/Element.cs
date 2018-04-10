using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using StorybrewCommon.Storyboarding;
using OpenTK;
using LibOsb.BrewHelper;
using LibOsb.EventClass;
using LibOsb.Model.EventType;
using LibOsb.Model.Constants;

namespace LibOsb
{
    /// <summary>
    /// Represents a storyboard element. This class cannot be inherited.
    /// </summary>
    public class Element
    {
        public ElementType Type { get; protected set; }
        public LayerType Layer { get; protected set; }
        public OriginType Origin { get; protected set; }
        public string ImagePath { get; protected set; }
        public double? DefaultY { get; protected set; }
        public double? DefaultX { get; protected set; }
        public LoopType LoopType { get; protected set; }
        public double? FrameCount { get; protected set; }
        public double? FrameRate { get; protected set; }

        public int InnerMaxTime { get; protected set; } = int.MinValue;
        public int InnerMinTime { get; protected set; } = int.MaxValue;

        public bool IsSignificative => MinTime != MaxTime;

        public int MaxTime
        {
            get
            {
                int max = InnerMaxTime;
                foreach (var item in _Loop)
                {
                    int time = item.InnerMaxTime * item.LoopCount + item.StartTime;
                    if (time > max) max = time;
                }
                foreach (var item in _Trigger)
                {
                    int time = item.InnerMaxTime + item.EndTime;
                    if (time > max) max = time;
                }
                return max;
            }
        }
        public int MinTime => InnerMinTime;

        public int MaxTimeCount { get; protected set; } = 1;
        public int MinTimeCount { get; protected set; } = 1;

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="type">Set element type.</param>
        /// <param name="layer">Set element layer.</param>
        /// <param name="origin">Set element origin.</param>
        /// <param name="imagePath">Set image path.</param>
        /// <param name="defaultX">Set default x-coordinate of location.</param>
        /// <param name="defaultY">Set default x-coordinate of location.</param>
        public Element(ElementType type, LayerType layer, OriginType origin, string imagePath, double defaultX, double defaultY)
        {
            Type = type;
            Layer = layer;
            Origin = origin;
            ImagePath = imagePath;
            DefaultX = defaultX;
            DefaultY = defaultY;
        }
        /// <summary>
        /// Create a storyboard element by a dynamic image.
        /// </summary>
        /// <param name="type">Set element type.</param>
        /// <param name="layer">Set element layer.</param>
        /// <param name="origin">Set element origin.</param>
        /// <param name="imagePath">Set image path.</param>
        /// <param name="defaultX">Set default x-coordinate of location.</param>
        /// <param name="defaultY">Set default x-coordinate of location.</param>
        /// <param name="frameCount">Set frame count.</param>
        /// <param name="frameRate">Set frame rate (frame delay).</param>
        /// <param name="loopType">Set loop type.</param>
        /// 
        public Element(ElementType type, LayerType layer, OriginType origin, string imagePath, double defaultX, double defaultY, double frameCount, double frameRate, LoopType loopType)
        {
            Type = type;
            Layer = layer;
            Origin = origin;
            ImagePath = imagePath;
            DefaultX = defaultX;
            DefaultY = defaultY;
            FrameCount = frameCount;
            FrameRate = frameRate;
            LoopType = loopType;
        }
        public Element(string type, string layer, string origin, string imagePath, double defaultX, double defaultY)
        {
            Type = (ElementType)Enum.Parse(typeof(ElementType), type);
            Layer = (LayerType)Enum.Parse(typeof(LayerType), layer);
            Origin = (OriginType)Enum.Parse(typeof(OriginType), origin);
            ImagePath = imagePath;
            DefaultX = defaultX;
            DefaultY = defaultY;
        }
        public Element(string type, string layer, string origin, string imagePath, double defaultX, double defaultY, double frameCount, double frameRate, string loopType)
        {
            Type = (ElementType)Enum.Parse(typeof(ElementType), type);
            Layer = (LayerType)Enum.Parse(typeof(LayerType), layer);
            Origin = (OriginType)Enum.Parse(typeof(OriginType), origin);
            ImagePath = imagePath;
            DefaultX = defaultX;
            DefaultY = defaultY;
            FrameCount = frameCount;
            FrameRate = frameRate;
            LoopType = (LoopType)Enum.Parse(typeof(LoopType), loopType);
        }
        internal Element() { }

        public void StartLoop(int startTime, int time)
        {
            if (isLooping || isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            isLooping = true;
            _Loop.Add(new Loop(startTime, time));
        }

        public void StartTrigger(int startTime, int time, TriggerType[] triggerType, int customSampleSet = -1)
        {
            if (isLooping || isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            isTriggering = true;
            _Trigger.Add(new Trigger(startTime, time, triggerType, customSampleSet));
        }

        public void StartTrigger(int startTime, int time, string triggerType)
        {
            if (isLooping || isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            isTriggering = true;
            _Trigger.Add(new Trigger(startTime, time, triggerType));
        }

        public void EndLoop()
        {
            if (!isLooping && !isTriggering) throw new Exception("You can not stop a loop when a loop isn't started.");
            isLooping = false;
            isTriggering = false;
        }

        #region 折叠：Event function
        public void Move(int startTime, System.Drawing.PointF location) => _Add_Move(0, startTime, startTime, location.X, location.Y, location.X, location.Y);
        public void Move(int startTime, double x, double y) => _Add_Move(0, startTime, startTime, x, y, x, y);
        public void Move(int startTime, int endTime, double x, double y) => _Add_Move(0, startTime, endTime, x, y, x, y);
        public void Move(EasingType easing, int startTime, int endTime, System.Drawing.PointF startLocation, System.Drawing.PointF endLocation) => _Add_Move(easing, startTime, endTime, startLocation.X, startLocation.Y, endLocation.X, endLocation.Y);
        public void Move(EasingType easing, int startTime, int endTime, double x1, double y1, double x2, double y2) => _Add_Move(easing, startTime, endTime, x1, y1, x2, y2);

        public void Fade(int startTime, double alpha) => _Add_Fade(0, startTime, startTime, alpha, alpha);
        public void Fade(int startTime, int endTime, double alpha) => _Add_Fade(0, startTime, endTime, alpha, alpha);
        public void Fade(EasingType easing, int startTime, int endTime, double startAlpha, double endAlpha) => _Add_Fade(easing, startTime, endTime, startAlpha, endAlpha);

        public void Scale(int startTime, double scale) => _Add_Scale(0, startTime, startTime, scale, scale);
        public void Scale(int startTime, int endTime, double scale) => _Add_Scale(0, startTime, endTime, scale, scale);
        public void Scale(EasingType easing, int startTime, int endTime, double startScale, double endScale) => _Add_Scale(easing, startTime, endTime, startScale, endScale);

        public void Rotate(int startTime, double rotate) => _Add_Rotate(0, startTime, startTime, rotate, rotate);
        public void Rotate(int startTime, int endTime, double rotate) => _Add_Rotate(0, startTime, endTime, rotate, rotate);
        public void Rotate(EasingType easing, int startTime, int endTime, double startRotate, double endRotate) => _Add_Rotate(easing, startTime, endTime, startRotate, endRotate);

        public void MoveX(int startTime, double x) => _Add_MoveX(0, startTime, startTime, x, x);
        public void MoveX(int startTime, int endTime, double x) => _Add_MoveX(0, startTime, endTime, x, x);
        public void MoveX(EasingType easing, int startTime, int endTime, double startX, double endX) => _Add_MoveX(easing, startTime, endTime, startX, endX);

        public void MoveY(int startTime, double y) => _Add_MoveY(0, startTime, startTime, y, y);
        public void MoveY(int startTime, int endTime, double y) => _Add_MoveY(0, startTime, endTime, y, y);
        public void MoveY(EasingType easing, int startTime, int endTime, double startY, double endY) => _Add_MoveY(easing, startTime, endTime, startY, endY);

        public void Color(int startTime, System.Drawing.Color color) => _Add_Color(0, startTime, startTime, color.R, color.G, color.B, color.R, color.G, color.B);
        public void Color(int startTime, int endTime, System.Drawing.Color color) => _Add_Color(0, startTime, endTime, color.R, color.G, color.B, color.R, color.G, color.B);
        public void Color(EasingType easing, int startTime, int endTime, System.Drawing.Color color1, System.Drawing.Color color2) => _Add_Color(easing, startTime, endTime, color1.R, color1.G, color1.B, color2.R, color2.G, color2.B);
        public void Color(int startTime, int R, int G, int B) => _Add_Color(0, startTime, startTime, R, G, B, R, G, B);
        public void Color(int startTime, int endTime, int R, int G, int B) => _Add_Color(0, startTime, endTime, R, G, B, R, G, B);
        public void Color(EasingType easing, int startTime, int endTime, int startR, int startG, int startB, int endR, int endG, int endB) => _Add_Color(easing, startTime, endTime, startR, startG, startB, endR, endG, endB);

        public void Vector(int startTime, System.Drawing.SizeF zoom) => _Add_Vector(0, startTime, startTime, zoom.Width, zoom.Height, zoom.Width, zoom.Height);
        public void Vector(int startTime, double w, double h) => _Add_Vector(0, startTime, startTime, w, h, w, h);
        public void Vector(int startTime, int endTime, double w, double h) => _Add_Vector(0, startTime, endTime, w, h, w, h);
        public void Vector(EasingType easing, int startTime, int endTime, System.Drawing.SizeF startZoom, System.Drawing.SizeF endZoom) => _Add_Vector(easing, startTime, endTime, startZoom.Width, startZoom.Height, endZoom.Width, endZoom.Height);
        public void Vector(EasingType easing, int startTime, int endTime, double w1, double h1, double w2, double h2) => _Add_Vector(easing, startTime, endTime, w1, h1, w2, h2);

        public void FlipH(int startTime) => _Add_Param(0, startTime, startTime, "H");
        public void FlipH(int startTime, int endTime) => _Add_Param(0, startTime, endTime, "H");
        public void FlipV(int startTime) => _Add_Param(0, startTime, startTime, "V");
        public void FlipV(int startTime, int endTime) => _Add_Param(0, startTime, endTime, "V");

        public void Lighting(int startTime) => _Add_Param(0, startTime, startTime, "A");
        public void Lighting(int startTime, int endTime) => _Add_Param(0, startTime, endTime, "A");

        internal void Parameter(EasingType easing, int startTime, int endTime, string type) => _Add_Param(easing, startTime, endTime, type);
        #endregion

        public void ExecuteBrew(StoryboardLayer lay_parsed, OsbSprite brewObj = null)
        {
            if (brewObj == null)
            {
                if (Type == ElementType.Sprite)
                    brewObj = lay_parsed.CreateSprite(ImagePath, BrewConvert.CvtOrigin(Origin), new Vector2((float)DefaultX, (float)DefaultY));
                else
                    brewObj = lay_parsed.CreateAnimation(ImagePath, (int)FrameCount, (int)FrameRate,
                        BrewConvert.CvtLoopType(LoopType), BrewConvert.CvtOrigin(Origin), new Vector2((float)DefaultX, (float)DefaultY));
            }

            foreach (var m in this._Move)
                BrewConvert.ExeM(m, brewObj);
            foreach (var s in this._Scale)
                BrewConvert.ExeS(s, brewObj);
            foreach (var f in this._Fade)
                BrewConvert.ExeF(f, brewObj);
            foreach (var r in this._Rotate)
                BrewConvert.ExeR(r, brewObj);
            foreach (var v in this._Vector)
                BrewConvert.ExeV(v, brewObj);
            foreach (var c in this._Color)
                BrewConvert.ExeC(c, brewObj);
            foreach (var mx in this._MoveX)
                BrewConvert.ExeMx(mx, brewObj);
            foreach (var my in this._MoveY)
                BrewConvert.ExeMy(my, brewObj);
            foreach (var p in this._Parameter)
                BrewConvert.ExeP(p, brewObj);
            foreach (var l in this._Loop)
            {
                brewObj.StartLoopGroup(l.StartTime, l.LoopCount);
                l.ExecuteBrew(lay_parsed, brewObj);
                brewObj.EndGroup();
            }
            foreach (var t in this._Trigger)
            {
                brewObj.StartTriggerGroup(t.TriggerType, t.StartTime, t.EndTime);
                t.ExecuteBrew(lay_parsed, brewObj);
                brewObj.EndGroup();
            }
        }

        public override string ToString()
        {
            if (!IsSignificative) return null;

            StringBuilder sb = new StringBuilder();
            if (!isInnerClass)
            {
                sb.Append(string.Join(",", Type, Layer, Origin, $"\"{ImagePath}\"", DefaultX, DefaultY));
                if (FrameCount != null)
                    sb.AppendLine("," + string.Join(",", FrameCount, FrameRate, LoopType));
                else
                    sb.AppendLine();
            }
            string index = (isInnerClass) ? "  " : " ";
            for (int i = 1; i <= _Move.Count; i++) sb.AppendLine(index + _Move[i - 1].ToString());
            for (int i = 1; i <= _Scale.Count; i++) sb.AppendLine(index + _Scale[i - 1].ToString());
            for (int i = 1; i <= _Fade.Count; i++) sb.AppendLine(index + _Fade[i - 1].ToString());
            for (int i = 1; i <= _Rotate.Count; i++) sb.AppendLine(index + _Rotate[i - 1].ToString());
            for (int i = 1; i <= _Vector.Count; i++) sb.AppendLine(index + _Vector[i - 1].ToString());
            for (int i = 1; i <= _Color.Count; i++) sb.AppendLine(index + _Color[i - 1].ToString());
            for (int i = 1; i <= _MoveX.Count; i++) sb.AppendLine(index + _MoveX[i - 1].ToString());
            for (int i = 1; i <= _MoveY.Count; i++) sb.AppendLine(index + _MoveY[i - 1].ToString());
            for (int i = 1; i <= _Parameter.Count; i++) sb.AppendLine(index + _Parameter[i - 1].ToString());
            for (int i = 1; i <= _Loop.Count; i++) sb.Append(_Loop[i - 1].ToString());
            for (int i = 1; i <= _Trigger.Count; i++) sb.Append(_Trigger[i - 1].ToString());
            return sb.ToString();
        }

        public static Element Parse(string osbString)
        {
            return Parse(osbString, 1);
        }

        public void Compress()
        {
            Examine();
            // 简单来说每个类型压缩都有一个共同点：从后往前，1.删除没用的 2.整合能整合的 3.考虑单event情况 4.排除第一行误加的情况（defaultParams）
            PreOptimize();
            NormalOptimize();
        }

        public Element Clone() => (Element)MemberwiseClone();

        #region non-public member
        internal static Element Parse(string osbString, int baseLine)
        {
            Element obj = null;
            int currentLine = baseLine + 0;
            try
            {
                var lines = osbString.Replace("\r", "").Split('\n');
                bool is_looping = false, is_triggring = false, is_blank = false;
                foreach (var line in lines)
                {
                    var pars = line.Split(',');

                    if (pars[0] == "Sprite" || pars[0] == "Animation")
                    {
                        if (obj != null)
                            throw new Exception("Sprite declared repeatly");

                        if (pars.Length == 6)
                            obj = new Element(pars[0], pars[1], pars[2], pars[3].Trim('\"'), double.Parse(pars[4]), double.Parse(pars[5]));
                        else if (pars.Length == 9)
                            obj = new Element(pars[0], pars[1], pars[2], pars[3].Trim('\"'), double.Parse(pars[4]), double.Parse(pars[5]),
                                double.Parse(pars[6]), double.Parse(pars[7]), pars[8]);
                        else
                            throw new Exception("Sprite declared wrongly");
                    }
                    else if (line.Trim() == "")
                    {
                        is_blank = true;
                    }
                    else
                    {
                        if (obj == null)
                            throw new Exception("Sprite need to be declared before using");
                        if (is_blank)
                            throw new Exception("Events shouldn't be declared after blank line");

                        // 验证层次是否合法
                        if (pars[0].Length - pars[0].TrimStart(' ').Length > 2)
                        {
                            throw new Exception("Unknown relation of the event");
                        }
                        else if (pars[0].IndexOf("  ") == 0)
                        {
                            if (!is_looping && !is_triggring)
                                throw new Exception("The event should be looping or triggering");
                        }
                        else if (pars[0].IndexOf(" ") == 0)
                        {
                            if (is_looping || is_triggring)
                            {
                                obj.EndLoop();
                                is_looping = false;
                                is_triggring = false;
                            }
                        }
                        else
                        {
                            throw new Exception("Unknown relation of the event");
                        }

                        // 开始验证event类别
                        pars[0] = pars[0].Trim();

                        //if (pars.Length < 5 || pars.Length > 10)
                        //    throw new Exception("Line :" + currentLine + " (Wrong parameter for all events)");

                        string _event = pars[0];
                        int _easing = -1, _start_time = -1, _end_time = -1;
                        if (_event != "T" && _event != "L")
                        {
                            _easing = int.Parse(pars[1]);
                            if (_easing > 34 || _easing < 0) throw new FormatException("Unknown easing");
                            _start_time = int.Parse(pars[2]);
                            _end_time = pars[3] == "" ? _start_time : int.Parse(pars[3]);
                        }
                        switch (pars[0])
                        {
                            // EventSingle
                            case "F":
                            case "S":
                            case "R":
                            case "MX":
                            case "MY":
                                double p1, p2;

                                // 验证是否存在缺省
                                if (pars.Length == 5)
                                    p1 = p2 = double.Parse(pars[4]);
                                else if (pars.Length == 6)
                                {
                                    p1 = double.Parse(pars[4]);
                                    p2 = double.Parse(pars[5]);
                                }
                                else
                                {
                                    throw new Exception($"Wrong parameter for event: \"{_event}\"");
                                }

                                // 开始添加成员
                                switch (_event)
                                {
                                    case "F":
                                        obj.Fade((EasingType)_easing, _start_time, _end_time, p1, p2);
                                        break;
                                    case "S":
                                        obj.Scale((EasingType)_easing, _start_time, _end_time, p1, p2);
                                        break;
                                    case "R":
                                        obj.Rotate((EasingType)_easing, _start_time, _end_time, p1, p2);
                                        break;
                                    case "MX":
                                        obj.MoveX((EasingType)_easing, _start_time, _end_time, p1, p2);
                                        break;
                                    case "MY":
                                        obj.MoveY((EasingType)_easing, _start_time, _end_time, p1, p2);
                                        break;
                                }
                                break;

                            // EventDouble
                            case "M":
                            case "V":
                                double p1_1, p1_2, p2_1, p2_2;

                                // 验证是否存在缺省
                                if (pars.Length == 6)
                                {
                                    p1_1 = p2_1 = double.Parse(pars[4]);
                                    p1_2 = p2_2 = double.Parse(pars[5]);
                                }
                                else if (pars.Length == 8)
                                {
                                    p1_1 = double.Parse(pars[4]);
                                    p1_2 = double.Parse(pars[5]);
                                    p2_1 = double.Parse(pars[6]);
                                    p2_2 = double.Parse(pars[7]);
                                }
                                else
                                {
                                    throw new Exception($"Wrong parameter for event: \"{_event}\"");
                                }
                                // 开始添加成员
                                switch (_event)
                                {
                                    case "M":
                                        obj.Move((EasingType)_easing, _start_time, _end_time, p1_1, p1_2, p2_1, p2_2);
                                        break;
                                    case "V":
                                        obj.Vector((EasingType)_easing, _start_time, _end_time, p1_1, p1_2, p2_1, p2_2);
                                        break;
                                }
                                break;

                            // EventTriple
                            case "C":
                                int c1_1, c1_2, c1_3, c2_1, c2_2, c2_3;

                                // 验证是否存在缺省
                                if (pars.Length == 7)
                                {
                                    c1_1 = c2_1 = int.Parse(pars[4]);
                                    c1_2 = c2_2 = int.Parse(pars[5]);
                                    c1_3 = c2_3 = int.Parse(pars[6]);
                                }
                                else if (pars.Length == 10)
                                {
                                    c1_1 = int.Parse(pars[4]);
                                    c1_2 = int.Parse(pars[5]);
                                    c1_3 = int.Parse(pars[6]);
                                    c2_1 = int.Parse(pars[7]);
                                    c2_2 = int.Parse(pars[8]);
                                    c2_3 = int.Parse(pars[9]);
                                }
                                else
                                {
                                    throw new Exception($"Wrong parameter for event: \"{_event}\"");
                                }
                                // 开始添加成员
                                switch (_event)
                                {
                                    case "C":
                                        obj.Color((EasingType)_easing, _start_time, _end_time, c1_1, c1_2, c1_3, c2_1, c2_2, c2_3);
                                        break;
                                }
                                break;

                            case "P":
                                string type;
                                if (pars.Length == 5)
                                {
                                    type = pars[4];
                                    obj.Parameter((EasingType)_easing, _start_time, _end_time, type);
                                }
                                else
                                {
                                    throw new Exception($"Wrong parameter for event: \"{_event}\"");
                                }
                                break;

                            case "L":
                                if (pars.Length == 3)
                                {
                                    _start_time = int.Parse(pars[1]);
                                    int loop_count = int.Parse(pars[2]);
                                    obj.StartLoop(_start_time, loop_count);
                                    is_looping = true;
                                }
                                else
                                {
                                    throw new Exception($"Wrong parameter for event: \"{_event}\"");
                                }
                                break;

                            case "T":
                                if (pars.Length == 4)
                                {
                                    string trigger_type = pars[1];
                                    _start_time = int.Parse(pars[2]);
                                    _end_time = int.Parse(pars[3]);
                                    obj.StartTrigger(_start_time, _end_time, trigger_type);
                                    is_triggring = true;
                                }
                                else
                                {
                                    throw new Exception($"Wrong parameter for event: \"{_event}\"");
                                }
                                break;
                            default:
                                throw new Exception($"Unknown event: \"{_event}\"");
                        }
                    }

                    currentLine++;
                }
            }
            catch (Exception ex)
            {
                throw new FormatException("You have an syntax error in your osb code at line: " + currentLine, ex);
            }
            return obj;
        }

        private bool isTriggering = false, isLooping = false;
        protected bool isInnerClass = false;

        /// <summary>
        /// 检查timing是否合法，以及计算透明时间段
        /// </summary>
        private void Examine()
        {
            if (_Move.Count != 0) CheckTiming(ref _move);
            if (_Scale.Count != 0) CheckTiming(ref _scale);
            if (_Fade.Count != 0) CheckTiming(ref _fade);
            if (_Rotate.Count != 0) CheckTiming(ref _rotate);
            if (_Vector.Count != 0) CheckTiming(ref _vector);
            if (_Color.Count != 0) CheckTiming(ref _color);
            if (_MoveX.Count != 0) CheckTiming(ref _moveX);
            if (_MoveY.Count != 0) CheckTiming(ref _moveY);
            if (_Parameter.Count != 0) CheckTiming(ref _parameter);
            foreach (var item in _Loop) item.Examine();
            foreach (var item in _Trigger) item.Examine();

            // 验证物件完全消失的时间段
            int tmpTime = -1;
            bool fadeouting = false;
            for (int j = 0; j < _Fade.Count; j++)
            {
                var nowF = _Fade[j];
                if (j == 0 && nowF.P1_1 == 0 && nowF.StartTime > MinTime)  // 最早的F晚于最小开始时间，默认加这一段
                {
                    _FadeoutList.Add(MinTime, nowF.StartTime);
                }
                else if (nowF.P2_1 == 0 && !fadeouting)  // f2=0，开始计时
                {
                    tmpTime = nowF.EndTime;
                    fadeouting = true;
                }
                else if (fadeouting)
                {
                    if (nowF.P1_1 != 0 || nowF.P2_1 != 0)  // 二者任一不为0则取消状态                       
                    {
                        _FadeoutList.Add(tmpTime, nowF.StartTime);
                        fadeouting = false;
                    }
                }
            }
            if (fadeouting && tmpTime != MaxTime)  // 可能存在Fade后还有别的event
            {
                _FadeoutList.Add(tmpTime, MaxTime);
            }
        }
        /// <summary>
        /// 预压缩
        /// </summary>
        private void PreOptimize()
        {
            bool flag = true;
            foreach (var item in _Loop)
            {
                if (item.HasFade) flag = false;
                item.PreOptimize();
            }
            foreach (var item in _Trigger)
            {
                if (item.HasFade) flag = false;
                item.PreOptimize();
            }
            if (!flag) return;

            if (_Scale.Count != 0) FixAll(ref _scale);
            if (_Rotate.Count != 0) FixAll(ref _rotate);
            if (_MoveX.Count != 0) FixAll(ref _moveX);
            if (_MoveY.Count != 0) FixAll(ref _moveY);
            if (_Fade.Count != 0) FixAll(ref _fade);
            if (_Move.Count != 0) FixAll(ref _move);
            if (_Vector.Count != 0) FixAll(ref _vector);
            if (_Color.Count != 0) FixAll(ref _color);
            if (_Parameter.Count != 0) FixAll(ref _parameter);
            //if (_FadeoutList.Count > 0 && _FadeoutList.LastEndTime == MaxTime) InnerMaxTime = _FadeoutList.LastStartTime;

            //foreach (var item in _Loop) item.PreOptimize();
            //foreach (var item in _Trigger) item.PreOptimize();
        }
        /// <summary>
        /// 正常压缩
        /// </summary>
        private void NormalOptimize()
        {
            if (_Scale.Count != 0) FixSingle(ref _scale);
            if (_Rotate.Count != 0) FixSingle(ref _rotate);
            if (_MoveX.Count != 0) FixSingle(ref _moveX);
            if (_MoveY.Count != 0) FixSingle(ref _moveY);
            if (_Fade.Count != 0) FixSingle(ref _fade);
            if (_Move.Count != 0) FixDouble(ref _move);
            if (_Vector.Count != 0) FixDouble(ref _vector);
            if (_Color.Count != 0) FixTriple(ref _color);

            foreach (var item in _Loop) item.NormalOptimize();
            foreach (var item in _Trigger) item.NormalOptimize();
        }
        /// <summary>
        /// 检查Alpha(意义何在?)
        /// </summary>
        private void CheckAlpha(double a)
        {
            if (a < 0 || a > 1)
            {
                a = (a > 1 ? 1 : 0);
                Debug.WriteLine("[Warning] Alpha of fade should be between 0 and 1.");
            }
        }
        /// <summary>
        /// 检查Timing的泛型方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void CheckTiming<T>(ref List<T> _list)
        {
            _list.Sort(new EventSort<T>());
            for (int i = 1; i < _list.Count; i++)
            {
                dynamic obj_next = _list[i];
                dynamic obj_previous = _list[i - 1];
                if (obj_previous.StartTime > obj_previous.EndTime)
                    throw new ArgumentException("Start time should not be larger than end time.");
                if (obj_next.StartTime < obj_previous.EndTime)
                {
                    //throw new Exception(obj_previous.ToString() + Environment.NewLine + obj_next.ToString());
                }
            }
        }
        /// <summary>
        /// 预压缩的泛型方法
        /// </summary>
        private void FixAll<T>(ref List<T> _list)
        {
            var tType = typeof(T);

            #region 预压缩部分，待检验
            if (tType != typeof(Fade))
            {
                //int max_i = _list.Count - 1;
                for (int i = 0; i < _list.Count; i++)
                {
                    dynamic e = _list[i];
                    dynamic e2 = null;
                    if (i != _list.Count - 1) e2 = _list[i + 1];
                    // 判断当前种类动作是否在某一透明范围内，并且下一个动作的startTime也须在此范围内
                    if (i < _list.Count - 1 && _FadeoutList.InRange(out bool cnm, e.StartTime, e.EndTime, e2.StartTime))
                    {
                        _list.RemoveAt(i);  // 待修改，封装一个方法控制min max的增减
                        i--;
                    }
                    if (i != _list.Count - 1) continue;
                    // 判断当前种类最后一个动作是否正处于物件透明状态，而且此状态最大时间即是obj最大时间
                    else if (_FadeoutList.InRange(out bool isLast, e.StartTime, e.EndTime) &&
                             isLast && _FadeoutList.LastEndTime == this.MaxTime)
                    {
                        _Remove_Event(_list, i);
                        i--;
                    }
                }
            }
            #endregion

            //if (tType == typeof(Scale))
            //  FixSingle(ref _list);
            // todo
        }
        /// <summary>
        /// 正常压缩的泛型方法（EventSingle）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        private void FixSingle<T>(ref List<T> _list)
        {
            double defaultParam = -1;
            var tType = typeof(T);
            if (tType == typeof(Scale))
                defaultParam = 1;
            else if (tType == typeof(Rotate))
                defaultParam = 0;
            else if (!isInnerClass && tType == typeof(MoveX))
                defaultParam = (int)this.DefaultX;
            else if (!isInnerClass && tType == typeof(MoveY))
                defaultParam = (int)this.DefaultY;
            else if (tType == typeof(Fade))
                defaultParam = 1;

            int i = _list.Count - 1;
            while (i >= 0)
            {
                dynamic objNow = _list[i];
                dynamic objPre = null;
                if (i >= 1) objPre = _list[i - 1];
                int now_start = objNow.StartTime, now_end = objNow.EndTime;
                int pre_start = -1, pre_end = -1;
                if (objPre != null)
                {
                    pre_start = objPre.StartTime;
                    pre_end = objPre.EndTime;
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
                    if (isInnerClass) break;
                    /* 当 此event唯一
                     * 且 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                     * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                     * 且 此event的param固定
                     * 且 此event.param=default
                     */
                    if (_list.Count == 1 &&
                    (now_end < this.MaxTime || now_end == this.MaxTime && MaxTimeCount > 1) &&
                    (now_start > this.MinTime || now_start == this.MinTime && MinTimeCount > 1) &&
                    nowP1 == nowP2 &&
                    nowP1 == defaultParam)
                    {
                        // Remove 0
                        _Remove_Event(_list, 0);
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
                if ((now_end < this.MaxTime || now_end == this.MaxTime && MaxTimeCount > 1 || (tType == typeof(Fade) && nowP1 == 0)) &&
                   nowP1 == nowP2 &&
                   nowP1 == preP2)
                {
                    // Remove i
                    _Remove_Event(_list, i);
                    i = _list.Count - 1;
                }
                /* 当 此event与前event一致，且前后param皆固定 （有待考证）
                 */
                else if (nowP1 == nowP2 &&
                  preP1 == preP2 &&
                  preP1 == nowP1)
                {

                    objPre.EndTime = objNow.EndTime;  // 整合至前面
                    if (pre_start == this.MinTime && MinTimeCount > 1)  // ??
                    {
                        objPre.StartTime = objPre.EndTime;
                    }
                    // Remove i
                    _Remove_Event(_list, i);
                    i = _list.Count - 1;
                }
                else i--;

            }
        }
        /// <summary>
        /// 正常压缩的泛型方法（EventDouble）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        private void FixDouble<T>(ref List<T> _list)
        {
            double defParam1 = -1, defParam2 = -1;
            var tType = typeof(T);
            if (tType == typeof(Move))
            {
                defParam1 = 320;
                defParam2 = 240;
                DefaultX = 0;
                DefaultY = 0;
            }
            else if (tType == typeof(Vector))
            {
                defParam1 = 1;
                defParam2 = 1;
            }

            int i = _list.Count - 1;
            while (i >= 0)
            {
                dynamic objNow = _list[i];
                dynamic objPre = null;
                if (i >= 1) objPre = _list[i - 1];
                int now_start = objNow.StartTime, now_end = objNow.EndTime;
                int pre_start = -1, pre_end = -1;
                if (objPre != null)
                {
                    pre_start = objPre.StartTime;
                    pre_end = objPre.EndTime;
                }
                double nowP1_1 = objNow.P1_1, nowP1_2 = objNow.P1_2, nowP2_1 = objNow.P2_1, nowP2_2 = objNow.P2_2;
                double preP1_1 = -1, preP1_2 = -1, preP2_1 = -1, preP2_2 = -1;
                if (objPre != null)
                {
                    preP1_1 = objPre.P1_1;
                    preP1_2 = objPre.P1_2;
                    preP2_1 = objPre.P2_1;
                    preP2_2 = objPre.P2_2;
                }
                if (i == 0)
                {
                    if (isInnerClass) break;

                    /* 当 此event唯一
                     * 且 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                     * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                     * 且 此event的param固定
                     * 且 此event.param=default
                     */
                    if (_list.Count == 1 &&
                    (now_end < this.MaxTime || now_end == this.MaxTime && MaxTimeCount > 1) &&
                    (now_start > this.MinTime || now_start == this.MinTime && MinTimeCount > 1) &&
                    objNow.IsStatic)
                    {

                        // Move特有
                        if (tType == typeof(Move))
                        {
                            if (nowP1_1 == (int)nowP1_1 && nowP1_2 == (int)nowP1_2)
                            {
                                DefaultX = nowP1_1;
                                DefaultY = nowP1_2;
                                _Remove_Event(_list, 0);
                            }
                            else if (nowP1_1 == DefaultX && nowP1_2 == DefaultY)
                            {
                                _Remove_Event(_list, 0);
                            }
                        }
                        else
                        {
                            if (nowP1_1 == defParam1 && nowP1_2 == defParam2)
                            {
                                _Remove_Event(_list, 0);
                            }
                        }
                    }
                    break;
                }

                /* 当 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                * 且 此event的param固定
                * 且 此event当前动作 = 此event上个动作
                */
                if ((now_end < this.MaxTime || now_end == this.MaxTime && MaxTimeCount > 1) &&
                     objNow.IsStatic &&
                     nowP1_1 == preP2_1 && nowP1_2 == preP2_2)
                {
                    _Remove_Event(_list, i);
                    i = _list.Count - 1;
                }
                /* 当 此event与前event一致，且前后param皆固定 （有待考证）
                 */
                else if (objNow.IsStatic && objPre.IsStatic &&
                         nowP1_1 == preP2_1 && nowP1_2 == preP2_2)
                {
                    objPre.EndTime = objNow.EndTime;  // 整合至前面
                    if (pre_start == this.MinTime && MinTimeCount > 1)  // ??
                    {
                        objPre.StartTime = objPre.EndTime;
                    }
                    // Remove i
                    _Remove_Event(_list, i);
                    i = _list.Count - 1;
                }
                else i--;
            }
        }
        /// <summary>
        /// 正常压缩的泛型方法（EventTriple）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        private void FixTriple<T>(ref List<T> _list)
        {
            double defParam1 = -1, defParam2 = -1, defParam3 = -1;
            var tType = typeof(T);
            if (tType == typeof(Color))
            {
                defParam1 = 255;
                defParam2 = 255;
                defParam3 = 255;
            }

            int i = _list.Count - 1;
            while (i >= 0)
            {
                Color objNow = (Color)(object)_list[i];
                Color objPre = null;
                if (i >= 1) objPre = (Color)(object)_list[i - 1];
                int now_start = objNow.StartTime, now_end = objNow.EndTime;
                int pre_start = -1, pre_end = -1;
                if (objPre != null)
                {
                    pre_start = objPre.StartTime;
                    pre_end = objPre.EndTime;
                }
                double nowP1_1 = objNow.P1_1, nowP1_2 = objNow.P1_2, nowP1_3 = objNow.P1_3,
                    nowP2_1 = objNow.P2_1, nowP2_2 = objNow.P2_2, nowP2_3 = objNow.P2_3;
                double preP1_1 = -1, preP1_2 = -1, preP1_3 = -1,
                    preP2_1 = -1, preP2_2 = -1, preP2_3 = -1;
                if (objPre != null)
                {
                    preP1_1 = objPre.P1_1;
                    preP1_2 = objPre.P1_2;
                    preP1_3 = objPre.P1_3;
                    preP2_1 = objPre.P2_1;
                    preP2_2 = objPre.P2_2;
                    preP2_3 = objPre.P2_3;
                }
                if (i == 0)
                {
                    if (isInnerClass) break;
                    /* 当 此event唯一
                     * 且 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                     * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                     * 且 此event的param固定
                     * 且 此event.param=default
                     */
                    if (_list.Count == 1 &&
                    (now_end < this.MaxTime || now_end == this.MaxTime && MaxTimeCount > 1) &&
                    (now_start > this.MinTime || now_start == this.MinTime && MinTimeCount > 1) &&
                    objNow.IsStatic &&
                    nowP1_1 == defParam1 && nowP1_2 == defParam2 && nowP1_3 == defParam3)
                    {
                        _Remove_Event(_list, 0);
                    }
                    break;
                }

                /* 当 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                * 且 此event的param固定
                * 且 此event当前动作 = 此event上个动作
                */
                if ((now_end < this.MaxTime || now_end == this.MaxTime && MaxTimeCount > 1) &&
                     objNow.IsStatic &&
                     nowP1_1 == preP2_1 && nowP1_2 == preP2_2 && nowP1_3 == preP2_3)
                {
                    _Remove_Event(_list, i);
                    i = _list.Count - 1;
                }
                /* 当 此event与前event一致，且前后param皆固定 （有待考证）
                 */
                else if (objNow.IsStatic && objPre.IsStatic &&
                         nowP1_1 == preP2_1 && nowP1_2 == preP2_2 && nowP1_3 == preP2_3)
                {
                    objPre.EndTime = objNow.EndTime;  // 整合至前面
                    if (pre_start == this.MinTime && MinTimeCount > 1)  // ??
                    {
                        objPre.StartTime = objPre.EndTime;
                    }
                    // Remove i
                    _Remove_Event(_list, i);
                    i = _list.Count - 1;
                }
                else i--;
            }
        }

        #region 折叠：所有event的属性
        internal List<Move> _Move { get => _move; set => _move = value; }
        internal List<Scale> _Scale { get => _scale; set => _scale = value; }
        internal List<Fade> _Fade { get => _fade; set => _fade = value; }
        internal List<Rotate> _Rotate { get => _rotate; set => _rotate = value; }
        internal List<Vector> _Vector { get => _vector; set => _vector = value; }
        internal List<Color> _Color { get => _color; set => _color = value; }
        internal List<MoveX> _MoveX { get => _moveX; set => _moveX = value; }
        internal List<MoveY> _MoveY { get => _moveY; set => _moveY = value; }
        internal List<Parameter> _Parameter { get => _parameter; set => _parameter = value; }
        internal List<Loop> _Loop { get => _loop; set => _loop = value; }
        internal List<Trigger> _Trigger { get => _trigger; set => _trigger = value; }
        #endregion

        #region 折叠：扩展属性
        internal TimeRange _FadeoutList { get; set; } = new TimeRange();
        internal bool HasFade => _fade.Count != 0;
        #endregion

        #region 折叠：此字段定义是为了ref传递
        private List<Move> _move = new List<Move>();
        private List<Scale> _scale = new List<Scale>();
        private List<Fade> _fade = new List<Fade>();
        private List<Rotate> _rotate = new List<Rotate>();
        private List<Vector> _vector = new List<Vector>();
        private List<Color> _color = new List<Color>();
        private List<MoveX> _moveX = new List<MoveX>();
        private List<MoveY> _moveY = new List<MoveY>();
        private List<Parameter> _parameter = new List<Parameter>();
        private List<Loop> _loop = new List<Loop>();
        private List<Trigger> _trigger = new List<Trigger>();
        #endregion

        /// <summary>
        /// 调整物件参数
        /// </summary>
        internal void _Adjust(double offsetX, double offsetY, int offsetTiming)
        {
            if (DefaultX != null) DefaultX += offsetX;
            if (DefaultY != null) DefaultY += offsetY;

            for (int i = 0; i < _Move.Count; i++)
            {
                _Move[i]._Adjust(offsetX, offsetY);
                if (isInnerClass)
                    continue;
                _Move[i]._AdjustTime(offsetTiming);
            }
            for (int i = 0; i < _MoveX.Count; i++)
            {
                _MoveX[i]._Adjust(offsetX);
                if (isInnerClass)
                    continue;
                _MoveX[i]._AdjustTime(offsetTiming);
            }
            for (int i = 0; i < _MoveY.Count; i++)
            {
                _MoveY[i]._Adjust(offsetY);
                if (isInnerClass)
                    continue;
                _MoveY[i]._AdjustTime(offsetTiming);
            }
            for (int i = 0; i < _Color.Count; i++)
            {
                if (isInnerClass)
                    break;
                _Color[i]._AdjustTime(offsetTiming);
            }
            for (int i = 0; i < _Fade.Count; i++)
            {
                if (isInnerClass)
                    break;
                _Fade[i]._AdjustTime(offsetTiming);
            }
            for (int i = 0; i < _Parameter.Count; i++)
            {
                if (isInnerClass)
                    break;
                _Parameter[i]._AdjustTime(offsetTiming);
            }
            for (int i = 0; i < _Rotate.Count; i++)
            {
                if (isInnerClass)
                    break;
                _Rotate[i]._AdjustTime(offsetTiming);
            }
            for (int i = 0; i < _Scale.Count; i++)
            {
                if (isInnerClass)
                    break;
                _Scale[i]._AdjustTime(offsetTiming);
            }
            for (int i = 0; i < _Vector.Count; i++)
            {
                if (isInnerClass)
                    break;
                _Vector[i]._AdjustTime(offsetTiming);
            }
            for (int i = 0; i < _Loop.Count; i++)
            {
                _Loop[i].StartTime += offsetTiming;
                _Loop[i]._Adjust(offsetX, offsetY, offsetTiming);
            }
            for (int i = 0; i < _Trigger.Count; i++)
            {
                _Trigger[i].StartTime += offsetTiming;
                _Trigger[i].EndTime += offsetTiming;
                _Trigger[i]._Adjust(offsetX, offsetY, offsetTiming);
            }
        }

        #region 折叠：直接控制Event修改方法
        private void _Add_Event<T>(List<T> _list, T _event)
        {
            var t = typeof(T);
            if (isTriggering || isLooping)
            {
                dynamic E = _event;
                dynamic member = null;

                if (isTriggering) member = _Trigger[_Trigger.Count - 1];
                else if (isLooping) member = _Loop[_Loop.Count - 1];

                if (E.StartTime < member.InnerMinTime)
                {
                    member.InnerMinTime = E.StartTime;
                    member.MinTimeCount = 1;
                }
                else if (E.StartTime == member.InnerMinTime) member.MinTimeCount++;
                if (E.EndTime > member.InnerMaxTime)
                {
                    member.InnerMaxTime = E.EndTime;
                    member.MaxTimeCount = 1;
                }
                else if (E.StartTime == member.InnerMaxTime) member.MaxTimeCount++;
            }
            else
            {
                dynamic E = _event;
                if (E.StartTime < InnerMinTime)
                {
                    InnerMinTime = E.StartTime;
                    MinTimeCount = 1;
                }
                else if (E.StartTime == InnerMinTime) MinTimeCount++;
                if (E.EndTime > InnerMaxTime)
                {
                    InnerMaxTime = E.EndTime;
                    MaxTimeCount = 1;
                }
                else if (E.EndTime == InnerMaxTime) MaxTimeCount++;
            }

            _list.Add(_event);
        }
        private void _Add_Move(EasingType easing, int startTime, int endTime, double x1, double y1, double x2, double y2)
        {
            var obj = new Move(easing, startTime, endTime, x1, y1, x2, y2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Move, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Move, obj);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Move, obj);
        }
        private void _Add_Fade(EasingType easing, int startTime, int endTime, double f1, double f2)
        {
            CheckAlpha(f1);
            CheckAlpha(f2);

            var obj = new Fade(easing, startTime, endTime, f1, f2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Fade, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Fade, obj);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Fade, obj);
        }
        private void _Add_Scale(EasingType easing, int startTime, int endTime, double s1, double s2)
        {
            var obj = new Scale(easing, startTime, endTime, s1, s2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Scale, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Scale, obj);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Scale, obj);
        }
        private void _Add_Rotate(EasingType easing, int startTime, int endTime, double r1, double r2)
        {
            var obj = new Rotate(easing, startTime, endTime, r1, r2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Rotate, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Rotate, obj);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Rotate, obj);
        }
        private void _Add_Color(EasingType easing, int startTime, int endTime, int r1, int g1, int b1, int r2, int g2, int b2)
        {
            var obj = new Color(easing, startTime, endTime, r1, g1, b1, r2, g2, b2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Color, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Color, obj);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Color, obj);
        }
        private void _Add_MoveX(EasingType easing, int startTime, int endTime, double x1, double x2)
        {
            var obj = new MoveX(easing, startTime, endTime, x1, x2);
            if (!isLooping && !isTriggering)
                _Add_Event(_MoveX, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._MoveX, obj);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._MoveX, obj);
        }
        private void _Add_MoveY(EasingType easing, int startTime, int endTime, double y1, double y2)
        {
            var obj = new MoveY(easing, startTime, endTime, y1, y2);
            if (!isLooping && !isTriggering)
                _Add_Event(_MoveY, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._MoveY, obj);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._MoveY, obj);
        }
        private void _Add_Param(EasingType easing, int startTime, int endTime, string p)
        {
            var obj = new Parameter(easing, startTime, endTime, p);
            if (!isLooping && !isTriggering)
                _Add_Event(_Parameter, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Parameter, obj);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Parameter, obj);
        }
        private void _Add_Vector(EasingType easing, int startTime, int endTime, double vx1, double vy1, double vx2, double vy2)
        {
            var obj = new Vector(easing, startTime, endTime, vx1, vy1, vx2, vy2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Vector, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Vector, obj);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Vector, obj);
        }
        private void _Remove_Event<T>(List<T> _list, int index)
        {
            dynamic evt = _list[index];
            if (evt.StartTime == MinTime)
            {
                if (MinTimeCount > 1)
                    MinTimeCount--;
                else throw new NotImplementedException();
            }
            if (evt.EndTime == MaxTime)
            {
                if (MaxTimeCount > 1)
                    MaxTimeCount--;
                //else  // 待验证
            }
            _list.RemoveAt(index);
        }
        #endregion

        /// <summary>
        /// 以timing排序event
        /// </summary>
        class EventSort<T> : IComparer<T>
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
        #endregion
    }

}
