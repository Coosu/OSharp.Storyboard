using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB.Model.Constants;
using System.Diagnostics;
using LibOSB.Model.EventType;
using StorybrewCommon.Storyboarding;
using LibOSB.Function;
using OpenTK;

namespace LibOSB
{
    /// <summary>
    /// Represents a storyboard element. This class cannot be inherited.
    /// </summary>
    [Serializable]
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
        public int MinTime { get => InnerMinTime; }

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

        #region Event function
        public void Move(int startTime, System.Drawing.PointF location)
        {
            _Add_Move(0, startTime, startTime, location.X, location.Y, location.X, location.Y);
        }
        public void Move(int startTime, double x, double y)
        {
            _Add_Move(0, startTime, startTime, x, y, x, y);
        }
        public void Move(int startTime, int endTime, double x, double y)
        {
            _Add_Move(0, startTime, endTime, x, y, x, y);
        }
        public void Move(EasingType easing, int startTime, int endTime, System.Drawing.PointF startLocation, System.Drawing.PointF endLocation)
        {
            _Add_Move(easing, startTime, endTime, startLocation.X, startLocation.Y, endLocation.X, endLocation.Y);
        }
        public void Move(EasingType easing, int startTime, int endTime, double x1, double y1, double x2, double y2)
        {
            _Add_Move(easing, startTime, endTime, x1, y1, x2, y2);
        }

        public void Fade(int startTime, double alpha)
        {
            _Add_Fade(0, startTime, startTime, alpha, alpha);
        }
        public void Fade(int startTime, int endTime, double alpha)
        {
            _Add_Fade(0, startTime, endTime, alpha, alpha);
        }
        public void Fade(EasingType easing, int startTime, int endTime, double startAlpha, double endAlpha)
        {
            _Add_Fade(easing, startTime, endTime, startAlpha, endAlpha);
        }

        public void Scale(int startTime, double scale)
        {
            _Add_Scale(0, startTime, startTime, scale, scale);
        }
        public void Scale(int startTime, int endTime, double scale)
        {
            _Add_Scale(0, startTime, endTime, scale, scale);
        }
        public void Scale(EasingType easing, int startTime, int endTime, double startScale, double endScale)
        {
            _Add_Scale(easing, startTime, endTime, startScale, endScale);
        }

        public void Rotate(int startTime, double rotate)
        {
            _Add_Rotate(0, startTime, startTime, rotate, rotate);
        }
        public void Rotate(int startTime, int endTime, double rotate)
        {
            _Add_Rotate(0, startTime, endTime, rotate, rotate);
        }
        public void Rotate(EasingType easing, int startTime, int endTime, double startRotate, double endRotate)
        {
            _Add_Rotate(easing, startTime, endTime, startRotate, endRotate);
        }

        public void MoveX(int startTime, double x)
        {
            _Add_MoveX(0, startTime, startTime, x, x);
        }
        public void MoveX(int startTime, int endTime, double x)
        {
            _Add_MoveX(0, startTime, endTime, x, x);
        }
        public void MoveX(EasingType easing, int startTime, int endTime, double startX, double endX)
        {
            _Add_MoveX(easing, startTime, endTime, startX, endX);
        }

        public void MoveY(int startTime, double y)
        {
            _Add_MoveY(0, startTime, startTime, y, y);
        }
        public void MoveY(int startTime, int endTime, double y)
        {
            _Add_MoveY(0, startTime, endTime, y, y);
        }
        public void MoveY(EasingType easing, int startTime, int endTime, double startY, double endY)
        {
            _Add_MoveY(easing, startTime, endTime, startY, endY);
        }

        public void Color(int startTime, System.Drawing.Color color)
        {
            _Add_Color(0, startTime, startTime, color.R, color.G, color.B, color.R, color.G, color.B);
        }
        public void Color(int startTime, int endTime, System.Drawing.Color color)
        {
            _Add_Color(0, startTime, endTime, color.R, color.G, color.B, color.R, color.G, color.B);
        }
        public void Color(EasingType easing, int startTime, int endTime, System.Drawing.Color color1, System.Drawing.Color color2)
        {
            _Add_Color(easing, startTime, endTime, color1.R, color1.G, color1.B, color2.R, color2.G, color2.B);
        }
        public void Color(int startTime, int R, int G, int B)
        {
            _Add_Color(0, startTime, startTime, R, G, B, R, G, B);
        }
        public void Color(int startTime, int endTime, int R, int G, int B)
        {
            _Add_Color(0, startTime, endTime, R, G, B, R, G, B);
        }
        public void Color(EasingType easing, int startTime, int endTime, int startR, int startG, int startB, int endR, int endG, int endB)
        {
            _Add_Color(easing, startTime, endTime, startR, startG, startB, endR, endG, endB);
        }

        public void Vector(int startTime, System.Drawing.SizeF zoom)
        {
            _Add_Vector(0, startTime, startTime, zoom.Width, zoom.Height, zoom.Width, zoom.Height);
        }
        public void Vector(int startTime, double w, double h)
        {
            _Add_Vector(0, startTime, startTime, w, h, w, h);
        }
        public void Vector(int startTime, int endTime, double w, double h)
        {
            _Add_Vector(0, startTime, endTime, w, h, w, h);
        }
        public void Vector(EasingType easing, int startTime, int endTime, System.Drawing.SizeF startZoom, System.Drawing.SizeF endZoom)
        {
            _Add_Vector(easing, startTime, endTime, startZoom.Width, startZoom.Height, endZoom.Width, endZoom.Height);
        }
        public void Vector(EasingType easing, int startTime, int endTime, double w1, double h1, double w2, double h2)
        {
            _Add_Vector(easing, startTime, endTime, w1, h1, w2, h2);
        }

        public void FlipH(int startTime)
        {
            _Add_Param(0, startTime, startTime, "H");
        }
        public void FlipH(int startTime, int endTime)
        {
            _Add_Param(0, startTime, endTime, "H");
        }

        public void FlipV(int startTime)
        {
            _Add_Param(0, startTime, startTime, "V");
        }
        public void FlipV(int startTime, int endTime)
        {
            _Add_Param(0, startTime, endTime, "V");
        }

        public void Lighting(int startTime)
        {
            _Add_Param(0, startTime, startTime, "A");
        }
        public void Lighting(int startTime, int endTime)
        {
            _Add_Param(0, startTime, endTime, "A");
        }

        internal void Parameter(EasingType easing, int startTime, int endTime, string type)
        {
            _Add_Param(easing, startTime, endTime, type);
        }
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
            StringBuilder sb = new StringBuilder();
            if (!isInnerClass)
            {
                sb.Append(Type + "," + Layer + "," + Origin + "," + "\"" + ImagePath + "\"," + DefaultX + "," + DefaultY);
                if (FrameCount != null)
                    sb.AppendLine("," + FrameCount + "," + FrameRate + "," + LoopType);
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

        #region non-public member
        private bool isTriggering = false, isLooping = false;
        protected bool isInnerClass = false;

        internal List<Move> _Move { get; set; } = new List<Move>();
        internal List<Scale> _Scale { get; set; } = new List<Scale>();
        internal List<Fade> _Fade { get; set; } = new List<Fade>();
        internal List<Rotate> _Rotate { get; set; } = new List<Rotate>();
        internal List<Vector> _Vector { get; set; } = new List<Vector>();
        internal List<Color> _Color { get; set; } = new List<Color>();
        internal List<MoveX> _MoveX { get; set; } = new List<MoveX>();
        internal List<MoveY> _MoveY { get; set; } = new List<MoveY>();
        internal List<Parameter> _Parameter { set; get; } = new List<Parameter>();
        internal List<Loop> _Loop { get; set; } = new List<Loop>();
        internal List<Trigger> _Trigger { get; set; } = new List<Trigger>();

        private void CheckAlpha(double a)
        {
            if (a < 0 || a > 1)
            {
                a = (a > 1 ? 1 : 0);
                Debug.WriteLine("[Warning] Alpha of fade should be between 0 and 1.");
            }
        }
        /// <summary>
        /// 调整
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

        private void _Add_Event<T>(List<T> _list, T _event, bool isLoop = false, bool isTrigger = false)
        {
            var t = typeof(T);
            if (isTrigger || isLoop)
            {
                dynamic E = _event;
                dynamic member = null;

                if (isTrigger) member = _Trigger[_Trigger.Count - 1];
                else if (isLoop) member = _Loop[_Loop.Count - 1];

                if (E.StartTime < member.InnerMinTime) member.InnerMinTime = E.StartTime;
                else if (E.StartTime == member.InnerMinTime) member.MinTimeCount++;
                if (E.EndTime > member.InnerMaxTime) member.InnerMaxTime = E.EndTime;
                else if (E.StartTime == member.InnerMaxTime) member.MaxTimeCount++;
            }
            else
            {
                dynamic E = _event;
                if (E.StartTime < InnerMinTime) InnerMinTime = E.StartTime;
                else if (E.StartTime == InnerMinTime) MinTimeCount++;
                if (E.EndTime > InnerMaxTime) InnerMaxTime = E.EndTime;
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
                _Add_Event(_Loop[_Loop.Count - 1]._Move, obj, isLoop: true);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Move, obj, isTrigger: true);
        }
        private void _Add_Fade(EasingType easing, int startTime, int endTime, double f1, double f2)
        {
            CheckAlpha(f1);
            CheckAlpha(f2);

            var obj = new Fade(easing, startTime, endTime, f1, f2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Fade, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Fade, obj, isLoop: true);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Fade, obj, isTrigger: true);
        }
        private void _Add_Scale(EasingType easing, int startTime, int endTime, double s1, double s2)
        {
            var obj = new Scale(easing, startTime, endTime, s1, s2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Scale, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Scale, obj, isLoop: true);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Scale, obj, isTrigger: true);
        }
        private void _Add_Rotate(EasingType easing, int startTime, int endTime, double r1, double r2)
        {
            var obj = new Rotate(easing, startTime, endTime, r1, r2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Rotate, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Rotate, obj, isLoop: true);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Rotate, obj, isTrigger: true);
        }
        private void _Add_Color(EasingType easing, int startTime, int endTime, int r1, int g1, int b1, int r2, int g2, int b2)
        {
            var obj = new Color(easing, startTime, endTime, r1, g1, b1, r2, g2, b2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Color, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Color, obj, isLoop: true);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Color, obj, isTrigger: true);
        }
        private void _Add_MoveX(EasingType easing, int startTime, int endTime, double x1, double x2)
        {
            var obj = new MoveX(easing, startTime, endTime, x1, x2);
            if (!isLooping && !isTriggering)
                _Add_Event(_MoveX, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._MoveX, obj, isLoop: true);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._MoveX, obj, isTrigger: true);
        }
        private void _Add_MoveY(EasingType easing, int startTime, int endTime, double y1, double y2)
        {
            var obj = new MoveY(easing, startTime, endTime, y1, y2);
            if (!isLooping && !isTriggering)
                _Add_Event(_MoveY, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._MoveY, obj, isLoop: true);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._MoveY, obj, isTrigger: true);
        }
        private void _Add_Param(EasingType easing, int startTime, int endTime, string p)
        {
            var obj = new Parameter(easing, startTime, endTime, p);
            if (!isLooping && !isTriggering)
                _Add_Event(_Parameter, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Parameter, obj, isLoop: true);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Parameter, obj, isTrigger: true);
        }
        private void _Add_Vector(EasingType easing, int startTime, int endTime, double vx1, double vy1, double vx2, double vy2)
        {
            var obj = new Vector(easing, startTime, endTime, vx1, vy1, vx2, vy2);
            if (!isLooping && !isTriggering)
                _Add_Event(_Vector, obj);
            else if (isLooping)
                _Add_Event(_Loop[_Loop.Count - 1]._Vector, obj, isLoop: true);
            else
                _Add_Event(_Trigger[_Trigger.Count - 1]._Vector, obj, isTrigger: true);
        }
        #endregion
    }

}
