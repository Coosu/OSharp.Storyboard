using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LibOsb.BrewHelper;
using LibOsb.EventClass;
using LibOsb.Model.Constants;
using LibOsb.Model.EventClass;
using LibOsb.Model.EventType;
using OpenTK;
using StorybrewCommon.Storyboarding;

namespace LibOsb.Model
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

        public string PrivateDiff { get; private set; }


        public int InnerMaxTime { get; protected set; } = int.MinValue;
        public int InnerMinTime { get; protected set; } = int.MaxValue;

        public bool IsSignificative => MinTime != MaxTime;

        public Element Backup { get; private set; }

        public int MaxTime
        {
            get
            {
                int max = InnerMaxTime;
                foreach (var item in LoopList)
                {
                    int time = item.InnerMaxTime * item.LoopCount + item.StartTime;
                    if (time > max) max = time;
                }
                foreach (var item in TriggerList)
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
            _isLooping = false;
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
            _isLooping = false;
        }
        public Element(string type, string layer, string origin, string imagePath, double defaultX, double defaultY)
        {
            Type = (ElementType)Enum.Parse(typeof(ElementType), type);
            Layer = (LayerType)Enum.Parse(typeof(LayerType), layer);
            Origin = (OriginType)Enum.Parse(typeof(OriginType), origin);
            ImagePath = imagePath;
            DefaultX = defaultX;
            DefaultY = defaultY;
            _isLooping = false;
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
            _isLooping = false;
        }
        internal Element()
        {
            _isLooping = false;
        }

        public void StartLoop(int startTime, int time)
        {
            if (_isLooping || _isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            _isLooping = true;
            LoopList.Add(new Loop(startTime, time));
        }

        public void StartTrigger(int startTime, int time, TriggerType[] triggerType, int customSampleSet = -1)
        {
            if (_isLooping || _isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            _isTriggering = true;
            TriggerList.Add(new Trigger(startTime, time, triggerType, customSampleSet));
        }

        public void StartTrigger(int startTime, int time, string triggerType)
        {
            if (_isLooping || _isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            _isTriggering = true;
            TriggerList.Add(new Trigger(startTime, time, triggerType));
        }

        public void EndLoop()
        {
            if (!_isLooping && !_isTriggering) throw new Exception("You can not stop a loop when a loop isn't started.");
            _isLooping = false;
            _isTriggering = false;
        }

        public void SetPrivateDiff(string filePath)
        {
            PrivateDiff = filePath;
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
        public void Color(int startTime, int r, int g, int b) => _Add_Color(0, startTime, startTime, r, g, b, r, g, b);
        public void Color(int startTime, int endTime, int r, int g, int b) => _Add_Color(0, startTime, endTime, r, g, b, r, g, b);
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

        public void ExecuteBrew(StoryboardLayer layParsed, OsbSprite brewObj = null)
        {
            if (brewObj == null)
            {
                if (Type == ElementType.Sprite)
                    brewObj = layParsed.CreateSprite(ImagePath, BrewConvert.CvtOrigin(Origin), new Vector2((float)DefaultX, (float)DefaultY));
                else
                    brewObj = layParsed.CreateAnimation(ImagePath, (int)FrameCount, (int)FrameRate,
                        BrewConvert.CvtLoopType(LoopType), BrewConvert.CvtOrigin(Origin), new Vector2((float)DefaultX, (float)DefaultY));
            }

            foreach (var m in MoveList)
                BrewConvert.ExeM(m, brewObj);
            foreach (var s in ScaleList)
                BrewConvert.ExeS(s, brewObj);
            foreach (var f in FadeList)
                BrewConvert.ExeF(f, brewObj);
            foreach (var r in RotateList)
                BrewConvert.ExeR(r, brewObj);
            foreach (var v in VectorList)
                BrewConvert.ExeV(v, brewObj);
            foreach (var c in ColorList)
                BrewConvert.ExeC(c, brewObj);
            foreach (var mx in MoveXList)
                BrewConvert.ExeMx(mx, brewObj);
            foreach (var my in MoveYList)
                BrewConvert.ExeMy(my, brewObj);
            foreach (var p in ParameterList)
                BrewConvert.ExeP(p, brewObj);
            foreach (var l in LoopList)
            {
                brewObj.StartLoopGroup(l.StartTime, l.LoopCount);
                l.ExecuteBrew(layParsed, brewObj);
                brewObj.EndGroup();
            }
            foreach (var t in TriggerList)
            {
                brewObj.StartTriggerGroup(t.TriggerType, t.StartTime, t.EndTime);
                t.ExecuteBrew(layParsed, brewObj);
                brewObj.EndGroup();
            }
        }

        public override string ToString()
        {
            if (!IsSignificative) return null;

            var sb = new StringBuilder();
            if (!IsInnerClass)
            {
                sb.Append(string.Join(",", Type, Layer, Origin, $"\"{ImagePath}\"", DefaultX, DefaultY));
                if (FrameCount != null)
                    sb.AppendLine("," + string.Join(",", FrameCount, FrameRate, LoopType));
                else
                    sb.AppendLine();
            }
            string index = (IsInnerClass) ? "  " : " ";
            for (int i = 1; i <= MoveList.Count; i++) sb.AppendLine(index + MoveList[i - 1]);
            for (int i = 1; i <= ScaleList.Count; i++) sb.AppendLine(index + ScaleList[i - 1]);
            for (int i = 1; i <= FadeList.Count; i++) sb.AppendLine(index + FadeList[i - 1]);
            for (int i = 1; i <= RotateList.Count; i++) sb.AppendLine(index + RotateList[i - 1]);
            for (int i = 1; i <= VectorList.Count; i++) sb.AppendLine(index + VectorList[i - 1]);
            for (int i = 1; i <= ColorList.Count; i++) sb.AppendLine(index + ColorList[i - 1]);
            for (int i = 1; i <= MoveXList.Count; i++) sb.AppendLine(index + MoveXList[i - 1]);
            for (int i = 1; i <= MoveYList.Count; i++) sb.AppendLine(index + MoveYList[i - 1]);
            for (int i = 1; i <= ParameterList.Count; i++) sb.AppendLine(index + ParameterList[i - 1]);
            for (int i = 1; i <= LoopList.Count; i++) sb.Append(LoopList[i - 1]);
            for (int i = 1; i <= TriggerList.Count; i++) sb.Append(TriggerList[i - 1]);
            return sb.ToString();
        }

        public static Element Parse(string osbString)
        {
            return Parse(osbString, 1);
        }

        public void Compress(bool backup = false)
        {
            if (backup) Backup = Clone();
            Examine();
            // 简单来说每个类型压缩都有一个共同点：从后往前，1.删除没用的 2.整合能整合的 3.考虑单event情况 4.排除第一行误加的情况（defaultParams）
            PreCompress();
            NormalOptimize();
        }

        public Element Clone() => (Element)MemberwiseClone();

<<<<<<< HEAD
        #region non-public member
        private bool _isTriggering, _isLooping;
        protected bool IsInnerClass = false;

=======
        #region 折叠：非公共成员
>>>>>>> 54f644f1489c737e9e0254084878e3da8e13ef97
        internal static Element Parse(string osbString, int baseLine)
        {
            Element obj = null;
            int currentLine = baseLine + 0;
            try
            {
                var lines = osbString.Replace("\r", "").Split('\n');
                bool isLooping = false, isTriggring = false, isBlank = false;
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
                        isBlank = true;
                    }
                    else
                    {
                        if (obj == null)
                            throw new Exception("Sprite need to be declared before using");
                        if (isBlank)
                            throw new Exception("Events shouldn't be declared after blank line");

                        // 验证层次是否合法
                        if (pars[0].Length - pars[0].TrimStart(' ').Length > 2)
                        {
                            throw new Exception("Unknown relation of the event");
                        }
                        else if (pars[0].IndexOf("  ", StringComparison.Ordinal) == 0)
                        {
                            if (!isLooping && !isTriggring)
                                throw new Exception("The event should be looping or triggering");
                        }
                        else if (pars[0].IndexOf(" ", StringComparison.Ordinal) == 0)
                        {
                            if (isLooping || isTriggring)
                            {
                                obj.EndLoop();
                                isLooping = false;
                                isTriggring = false;
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
                        int easing = -1, startTime = -1, endTime = -1;
                        if (_event != "T" && _event != "L")
                        {
                            easing = int.Parse(pars[1]);
                            if (easing > 34 || easing < 0) throw new FormatException("Unknown easing");
                            startTime = int.Parse(pars[2]);
                            endTime = pars[3] == "" ? startTime : int.Parse(pars[3]);
                        }
                        switch (pars[0])
                        {
                            // EventSingle
                            case "F":
                            case "S":
                            case "r":
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
                                        obj.Fade((EasingType)easing, startTime, endTime, p1, p2);
                                        break;
                                    case "S":
                                        obj.Scale((EasingType)easing, startTime, endTime, p1, p2);
                                        break;
                                    case "r":
                                        obj.Rotate((EasingType)easing, startTime, endTime, p1, p2);
                                        break;
                                    case "MX":
                                        obj.MoveX((EasingType)easing, startTime, endTime, p1, p2);
                                        break;
                                    case "MY":
                                        obj.MoveY((EasingType)easing, startTime, endTime, p1, p2);
                                        break;
                                }
                                break;

                            // EventDouble
                            case "M":
                            case "V":
                                double p11, p12, p21, p22;

                                // 验证是否存在缺省
                                if (pars.Length == 6)
                                {
                                    p11 = p21 = double.Parse(pars[4]);
                                    p12 = p22 = double.Parse(pars[5]);
                                }
                                else if (pars.Length == 8)
                                {
                                    p11 = double.Parse(pars[4]);
                                    p12 = double.Parse(pars[5]);
                                    p21 = double.Parse(pars[6]);
                                    p22 = double.Parse(pars[7]);
                                }
                                else
                                {
                                    throw new Exception($"Wrong parameter for event: \"{_event}\"");
                                }
                                // 开始添加成员
                                switch (_event)
                                {
                                    case "M":
                                        obj.Move((EasingType)easing, startTime, endTime, p11, p12, p21, p22);
                                        break;
                                    case "V":
                                        obj.Vector((EasingType)easing, startTime, endTime, p11, p12, p21, p22);
                                        break;
                                }
                                break;

                            // EventTriple
                            case "C":
                                int c11, c12, c13, c21, c22, c23;

                                // 验证是否存在缺省
                                if (pars.Length == 7)
                                {
                                    c11 = c21 = int.Parse(pars[4]);
                                    c12 = c22 = int.Parse(pars[5]);
                                    c13 = c23 = int.Parse(pars[6]);
                                }
                                else if (pars.Length == 10)
                                {
                                    c11 = int.Parse(pars[4]);
                                    c12 = int.Parse(pars[5]);
                                    c13 = int.Parse(pars[6]);
                                    c21 = int.Parse(pars[7]);
                                    c22 = int.Parse(pars[8]);
                                    c23 = int.Parse(pars[9]);
                                }
                                else
                                {
                                    throw new Exception($"Wrong parameter for event: \"{_event}\"");
                                }
                                // 开始添加成员
                                switch (_event)
                                {
                                    case "C":
                                        obj.Color((EasingType)easing, startTime, endTime, c11, c12, c13, c21, c22, c23);
                                        break;
                                }
                                break;

                            case "P":
                                string type;
                                if (pars.Length == 5)
                                {
                                    type = pars[4];
                                    obj.Parameter((EasingType)easing, startTime, endTime, type);
                                }
                                else
                                {
                                    throw new Exception($"Wrong parameter for event: \"{_event}\"");
                                }
                                break;

                            case "L":
                                if (pars.Length == 3)
                                {
                                    startTime = int.Parse(pars[1]);
                                    int loopCount = int.Parse(pars[2]);
                                    obj.StartLoop(startTime, loopCount);
                                    isLooping = true;
                                }
                                else
                                {
                                    throw new Exception($"Wrong parameter for event: \"{_event}\"");
                                }
                                break;

                            case "T":
                                if (pars.Length == 4)
                                {
                                    string triggerType = pars[1];
                                    startTime = int.Parse(pars[2]);
                                    endTime = int.Parse(pars[3]);
                                    obj.StartTrigger(startTime, endTime, triggerType);
                                    isTriggring = true;
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

        /// <summary>
        /// 检查timing是否合法，以及计算透明时间段
        /// </summary>
        private void Examine()
        {
<<<<<<< HEAD
            if (MoveList.Count != 0) CheckTiming(ref _moveList);
            if (ScaleList.Count != 0) CheckTiming(ref _scaleList);
            if (FadeList.Count != 0) CheckTiming(ref _fadeList);
            if (RotateList.Count != 0) CheckTiming(ref _rotateList);
            if (VectorList.Count != 0) CheckTiming(ref _vectorList);
            if (ColorList.Count != 0) CheckTiming(ref _colorList);
            if (MoveXList.Count != 0) CheckTiming(ref _moveXList);
            if (MoveYList.Count != 0) CheckTiming(ref _moveYList);
            if (ParameterList.Count != 0) CheckTiming(ref _parameterList);
            foreach (var item in LoopList) item.Examine();
            foreach (var item in TriggerList) item.Examine();
=======
            if (_Move.Count != 0)
            {
                if (_MoveX.Count != 0 || _MoveY.Count != 0)
                    throw new InvalidOperationException("MX(MY) and M can't exist at the same time.");
                CheckTiming(ref _move);
            }
            if (_Scale.Count != 0)
            {
                if (_Vector.Count != 0)
                    throw new InvalidOperationException("S and V can't exist at the same time.");
                CheckTiming(ref _scale);
            }
            if (_Fade.Count != 0) CheckTiming(ref _fade);
            if (_Rotate.Count != 0) CheckTiming(ref _rotate);
            if (_Vector.Count != 0) CheckTiming(ref _vector);
            if (_Color.Count != 0) CheckTiming(ref _color);
            if (_MoveX.Count != 0) CheckTiming(ref _moveX);
            if (_MoveY.Count != 0) CheckTiming(ref _moveY);
            if (_Parameter.Count != 0) CheckTiming(ref _parameter);
            foreach (var item in _Loop) item.Examine();
            foreach (var item in _Trigger) item.Examine();
>>>>>>> 54f644f1489c737e9e0254084878e3da8e13ef97

            // 验证物件完全消失的时间段
            int tmpTime = -1;
            bool fadeouting = false;
            for (int j = 0; j < FadeList.Count; j++)
            {
                var nowF = FadeList[j];
                if (j == 0 && nowF.P11 == 0 && nowF.StartTime > MinTime)  // 最早的F晚于最小开始时间，默认加这一段
                {
                    FadeoutList.Add(MinTime, nowF.StartTime);
                }
                else if (nowF.P21 == 0 && !fadeouting)  // f2=0，开始计时
                {
                    tmpTime = nowF.EndTime;
                    fadeouting = true;
                }
                else if (fadeouting)
                {
                    if (nowF.P11 == 0 && nowF.P21 == 0)
                        continue;
                    FadeoutList.Add(tmpTime, nowF.StartTime);
                    fadeouting = false;
                }
            }
            if (fadeouting && tmpTime != MaxTime)  // 可能存在Fade后还有别的event
            {
                FadeoutList.Add(tmpTime, MaxTime);
            }
        }

        /// <summary>
        /// 预压缩
        /// </summary>
        private void PreCompress()
        {
            bool flag = true;
            foreach (var item in LoopList)
            {
                if (item.HasFade) flag = false;
                item.PreCompress();
            }
            foreach (var item in TriggerList)
            {
                if (item.HasFade) flag = false;
                item.PreCompress();
            }
            if (!flag) return;

            if (ScaleList.Count != 0) FixAll(ref _scaleList);
            if (RotateList.Count != 0) FixAll(ref _rotateList);
            if (MoveXList.Count != 0) FixAll(ref _moveXList);
            if (MoveYList.Count != 0) FixAll(ref _moveYList);
            if (FadeList.Count != 0) FixAll(ref _fadeList);
            if (MoveList.Count != 0) FixAll(ref _moveList);
            if (VectorList.Count != 0) FixAll(ref _vectorList);
            if (ColorList.Count != 0) FixAll(ref _colorList);
            if (ParameterList.Count != 0) FixAll(ref _parameterList);
            //if (_FadeoutList.Count > 0 && _FadeoutList.LastEndTime == MaxTime) InnerMaxTime = _FadeoutList.LastStartTime;

            //foreach (var item in LoopList) item.PreOptimize();
            //foreach (var item in TriggerList) item.PreOptimize();
        }

        /// <summary>
        /// 正常压缩
        /// </summary>
        private void NormalOptimize()
        {
            if (ScaleList.Count != 0) FixSingle(ref _scaleList);
            if (RotateList.Count != 0) FixSingle(ref _rotateList);
            if (MoveXList.Count != 0) FixSingle(ref _moveXList);
            if (MoveYList.Count != 0) FixSingle(ref _moveYList);
            if (FadeList.Count != 0) FixSingle(ref _fadeList);
            if (MoveList.Count != 0) FixDouble(ref _moveList);
            if (VectorList.Count != 0) FixDouble(ref _vectorList);
            if (ColorList.Count != 0) FixTriple(ref _colorList);

            foreach (var item in LoopList) item.NormalOptimize();
            foreach (var item in TriggerList) item.NormalOptimize();
        }

        /// <summary>
        /// 检查Alpha(意义何在?)
        /// </summary>
        private static void CheckAlpha(double a)
        {
            if (a < 0 || a > 1)
<<<<<<< HEAD
                Debug.WriteLine("[Warning] Alpha of fade should be between 0 and 1.");
=======
            {
                a = (a > 1 ? 1 : 0);
                throw new InvalidOperationException("Alpha should be between 0 and 1.");
            }
>>>>>>> 54f644f1489c737e9e0254084878e3da8e13ef97
        }

        /// <summary>
        /// 检查Timing的泛型方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static void CheckTiming<T>(ref List<T> list)
        {
            list.Sort(new EventSort<T>());
            for (int i = 1; i < list.Count; i++)
            {
<<<<<<< HEAD
                dynamic objNext = list[i];
                dynamic objPrevious = list[i - 1];
                if (objPrevious.StartTime > objPrevious.EndTime)
                    throw new ArgumentException("Start time should not be larger than end time.");
                if (objNext.StartTime < objPrevious.EndTime)
=======
                dynamic obj_next = _list[i];
                dynamic obj_previous = _list[i - 1];
                if (obj_previous.StartTime > obj_previous.EndTime)
                    throw new InvalidOperationException("Start time should not be larger than end time.");
                if (obj_next.StartTime < obj_previous.EndTime)
>>>>>>> 54f644f1489c737e9e0254084878e3da8e13ef97
                {
                    throw new InvalidOperationException("Conflict timing exsits.");
                }
            }
        }

        /// <summary>
        /// 预压缩的泛型方法
        /// </summary>
        private void FixAll<T>(ref List<T> list)
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
                    if (i < list.Count - 1 && FadeoutList.InRange(out bool _, e.StartTime, e.EndTime, e2.StartTime))
                    {
                        list.RemoveAt(i);  // 待修改，封装一个方法控制min max的增减
                        i--;
                    }

                    if (i != list.Count - 1) continue;
                    // 判断当前种类最后一个动作是否正处于物件透明状态，而且此状态最大时间即是obj最大时间
                    if (FadeoutList.InRange(out bool isLast, e.StartTime, e.EndTime) &&
                       isLast && FadeoutList.LastEndTime == MaxTime)
                    {
                        _Remove_Event(list, i);
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
        /// <param name="list"></param>
        private void FixSingle<T>(ref List<T> list)
        {
            double defaultParam = -1;
            var tType = typeof(T);
            if (tType == typeof(Scale))
                defaultParam = 1;
            else if (tType == typeof(Rotate))
                defaultParam = 0;
            else if (!IsInnerClass && tType == typeof(MoveX))
                defaultParam = (int)DefaultX;
            else if (!IsInnerClass && tType == typeof(MoveY))
                defaultParam = (int)DefaultY;
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
                    if (IsInnerClass) break;
                    /* 当 此event唯一
                     * 且 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                     * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                     * 且 此event的param固定
                     * 且 此event.param=default
                     */
                    if (list.Count == 1 &&
                    (nowEnd < MaxTime || nowEnd == MaxTime && MaxTimeCount > 1) &&
                    (nowStart > MinTime || nowStart == MinTime && MinTimeCount > 1) &&
                    nowP1 == nowP2 &&
                    nowP1 == defaultParam)
                    {
                        // Remove 0
                        _Remove_Event(list, 0);
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
                if ((nowEnd < MaxTime || nowEnd == MaxTime && MaxTimeCount > 1 || (tType == typeof(Fade) && nowP1 == 0)) &&
                   nowP1 == nowP2 &&
                   nowP1 == preP2)
                {
                    // Remove i
                    _Remove_Event(list, i);
                    i = list.Count - 1;
                }
                /* 当 此event与前event一致，且前后param皆固定 （有待考证）
                 */
                else if (nowP1 == nowP2 &&
                  preP1 == preP2 &&
                  preP1 == nowP1)
                {

                    objPre.EndTime = objNow.EndTime;  // 整合至前面
                    if (preStart == MinTime && MinTimeCount > 1)  // ??
                    {
                        objPre.StartTime = objPre.EndTime;
                    }
                    // Remove i
                    _Remove_Event(list, i);
                    i = list.Count - 1;
                }
                else i--;

            }
        }
        /// <summary>
        /// 正常压缩的泛型方法（EventDouble）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private void FixDouble<T>(ref List<T> list)
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
                    if (IsInnerClass) break;

                    /* 当 此event唯一
                     * 且 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                     * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                     * 且 此event的param固定
                     * 且 此event.param=default
                     */
                    if (list.Count == 1 &&
                    (nowEnd < MaxTime || nowEnd == MaxTime && MaxTimeCount > 1) &&
                    (nowStart > MinTime || nowStart == MinTime && MinTimeCount > 1) &&
                    objNow.IsStatic)
                    {

                        // Move特有
                        if (tType == typeof(Move))
                        {
                            if (nowP11 == (int)nowP11 && nowP12 == (int)nowP12)
                            {
                                DefaultX = nowP11;
                                DefaultY = nowP12;
                                _Remove_Event(list, 0);
                            }
                            else if (nowP11 == DefaultX && nowP12 == DefaultY)
                            {
                                _Remove_Event(list, 0);
                            }
                        }
                        else
                        {
                            if (nowP11 == defParam1 && nowP12 == defParam2)
                            {
                                _Remove_Event(list, 0);
                            }
                        }
                    }
                    break;
                }

                /* 当 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                * 且 此event的param固定
                * 且 此event当前动作 = 此event上个动作
                */
                if ((nowEnd < MaxTime || nowEnd == MaxTime && MaxTimeCount > 1) &&
                     objNow.IsStatic &&
                     nowP11 == preP21 && nowP12 == preP22)
                {
                    _Remove_Event(list, i);
                    i = list.Count - 1;
                }
                /* 当 此event与前event一致，且前后param皆固定 （有待考证）
                 */
                else if (objNow.IsStatic && objPre.IsStatic &&
                         nowP11 == preP21 && nowP12 == preP22)
                {
                    objPre.EndTime = objNow.EndTime;  // 整合至前面
                    if (preStart == MinTime && MinTimeCount > 1)  // ??
                    {
                        objPre.StartTime = objPre.EndTime;
                    }
                    // Remove i
                    _Remove_Event(list, i);
                    i = list.Count - 1;
                }
                else i--;
            }
        }
        /// <summary>
        /// 正常压缩的泛型方法（EventTriple）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private void FixTriple<T>(ref List<T> list)
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
                    if (IsInnerClass) break;
                    /* 当 此event唯一
                     * 且 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                     * 且 此event开始时间 > obj最小时间 (或包括此event有两个以上的最小时间)
                     * 且 此event的param固定
                     * 且 此event.param=default
                     */
                    if (list.Count == 1 &&
                    (nowEnd < MaxTime || nowEnd == MaxTime && MaxTimeCount > 1) &&
                    (nowStart > MinTime || nowStart == MinTime && MinTimeCount > 1) &&
                    objNow.IsStatic &&
                    nowP11 == defParam1 && nowP12 == defParam2 && nowP13 == defParam3)
                    {
                        _Remove_Event(list, 0);
                    }
                    break;
                }

                /* 当 此event结束时间 < obj最大时间 (或包括此event有两个以上的最大时间)
                * 且 此event的param固定
                * 且 此event当前动作 = 此event上个动作
                */
                if ((nowEnd < MaxTime || nowEnd == MaxTime && MaxTimeCount > 1) &&
                     objNow.IsStatic &&
                     nowP11 == preP21 && nowP12 == preP22 && nowP13 == preP23)
                {
                    _Remove_Event(list, i);
                    i = list.Count - 1;
                }
                /* 当 此event与前event一致，且前后param皆固定 （有待考证）
                 */
                else if (objNow.IsStatic && objPre.IsStatic &&
                         nowP11 == preP21 && nowP12 == preP22 && nowP13 == preP23)
                {
                    objPre.EndTime = objNow.EndTime;  // 整合至前面
                    if (preStart == MinTime && MinTimeCount > 1)  // ??
                    {
                        objPre.StartTime = objPre.EndTime;
                    }
                    // Remove i
                    _Remove_Event(list, i);
                    i = list.Count - 1;
                }
                else i--;
            }
        }

        #region 折叠：所有event的属性
        internal List<Move> MoveList { get => _moveList; set => _moveList = value; }
        internal List<Scale> ScaleList { get => _scaleList; set => _scaleList = value; }
        internal List<Fade> FadeList { get => _fadeList; set => _fadeList = value; }
        internal List<Rotate> RotateList { get => _rotateList; set => _rotateList = value; }
        internal List<Vector> VectorList { get => _vectorList; set => _vectorList = value; }
        internal List<Color> ColorList { get => _colorList; set => _colorList = value; }
        internal List<MoveX> MoveXList { get => _moveXList; set => _moveXList = value; }
        internal List<MoveY> MoveYList { get => _moveYList; set => _moveYList = value; }
        internal List<Parameter> ParameterList { get => _parameterList; set => _parameterList = value; }
        internal List<Loop> LoopList { get; set; } = new List<Loop>();
        internal List<Trigger> TriggerList { get; set; } = new List<Trigger>();
        #endregion

        #region 折叠：扩展属性
        internal TimeRange FadeoutList { get; set; } = new TimeRange();
        internal bool HasFade => _fadeList.Count != 0;
        #endregion

        #region 折叠：此字段定义是为了ref传递
        private List<Move> _moveList = new List<Move>();
        private List<Scale> _scaleList = new List<Scale>();
        private List<Fade> _fadeList = new List<Fade>();
        private List<Rotate> _rotateList = new List<Rotate>();
        private List<Vector> _vectorList = new List<Vector>();
        private List<Color> _colorList = new List<Color>();
        private List<MoveX> _moveXList = new List<MoveX>();
        private List<MoveY> _moveYList = new List<MoveY>();
        private List<Parameter> _parameterList = new List<Parameter>();

        #endregion

        /// <summary>
        /// 调整物件参数
        /// </summary>
        internal void _Adjust(double offsetX, double offsetY, int offsetTiming)
        {
            if (DefaultX != null) DefaultX += offsetX;
            if (DefaultY != null) DefaultY += offsetY;

            foreach (var t in MoveList)
            {
                t._Adjust(offsetX, offsetY);
                if (IsInnerClass)
                    continue;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in MoveXList)
            {
                t._Adjust(offsetX);
                if (IsInnerClass)
                    continue;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in MoveYList)
            {
                t._Adjust(offsetY);
                if (IsInnerClass)
                    continue;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in ColorList)
            {
                if (IsInnerClass)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in FadeList)
            {
                if (IsInnerClass)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in ParameterList)
            {
                if (IsInnerClass)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in RotateList)
            {
                if (IsInnerClass)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in ScaleList)
            {
                if (IsInnerClass)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in VectorList)
            {
                if (IsInnerClass)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in LoopList)
            {
                t.StartTime += offsetTiming;
                t._Adjust(offsetX, offsetY, offsetTiming);
            }
            foreach (var t in TriggerList)
            {
                t.StartTime += offsetTiming;
                t.EndTime += offsetTiming;
                t._Adjust(offsetX, offsetY, offsetTiming);
            }
        }

        #region 折叠：直接控制Event修改方法
        private void _Add_Event<T>(ICollection<T> list, T _event)
        {
            var t = typeof(T);
            if (_isTriggering || _isLooping)
            {
                dynamic e = _event;
                dynamic member = null;

                if (_isTriggering) member = TriggerList[TriggerList.Count - 1];
                else if (_isLooping) member = LoopList[LoopList.Count - 1];

                if (e.StartTime < member.InnerMinTime)
                {
                    member.InnerMinTime = e.StartTime;
                    member.MinTimeCount = 1;
                }
                else if (e.StartTime == member.InnerMinTime) member.MinTimeCount++;
                if (e.EndTime > member.InnerMaxTime)
                {
                    member.InnerMaxTime = e.EndTime;
                    member.MaxTimeCount = 1;
                }
                else if (e.StartTime == member.InnerMaxTime) member.MaxTimeCount++;
            }
            else
            {
                dynamic e = _event;
                if (e.StartTime < InnerMinTime)
                {
                    InnerMinTime = e.StartTime;
                    MinTimeCount = 1;
                }
                else if (e.StartTime == InnerMinTime) MinTimeCount++;
                if (e.EndTime > InnerMaxTime)
                {
                    InnerMaxTime = e.EndTime;
                    MaxTimeCount = 1;
                }
                else if (e.EndTime == InnerMaxTime) MaxTimeCount++;
            }

            list.Add(_event);
        }
        private void _Add_Move(EasingType easing, int startTime, int endTime, double x1, double y1, double x2, double y2)
        {
            var obj = new Move(easing, startTime, endTime, x1, y1, x2, y2);
            if (!_isLooping && !_isTriggering)
                _Add_Event(MoveList, obj);
            else if (_isLooping)
                _Add_Event(LoopList[LoopList.Count - 1].MoveList, obj);
            else
                _Add_Event(TriggerList[TriggerList.Count - 1].MoveList, obj);
        }
        private void _Add_Fade(EasingType easing, int startTime, int endTime, double f1, double f2)
        {
            CheckAlpha(f1);
            CheckAlpha(f2);

            var obj = new Fade(easing, startTime, endTime, f1, f2);
            if (!_isLooping && !_isTriggering)
                _Add_Event(FadeList, obj);
            else if (_isLooping)
                _Add_Event(LoopList[LoopList.Count - 1].FadeList, obj);
            else
                _Add_Event(TriggerList[TriggerList.Count - 1].FadeList, obj);
        }
        private void _Add_Scale(EasingType easing, int startTime, int endTime, double s1, double s2)
        {
            var obj = new Scale(easing, startTime, endTime, s1, s2);
            if (!_isLooping && !_isTriggering)
                _Add_Event(ScaleList, obj);
            else if (_isLooping)
                _Add_Event(LoopList[LoopList.Count - 1].ScaleList, obj);
            else
                _Add_Event(TriggerList[TriggerList.Count - 1].ScaleList, obj);
        }
        private void _Add_Rotate(EasingType easing, int startTime, int endTime, double r1, double r2)
        {
            var obj = new Rotate(easing, startTime, endTime, r1, r2);
            if (!_isLooping && !_isTriggering)
                _Add_Event(RotateList, obj);
            else if (_isLooping)
                _Add_Event(LoopList[LoopList.Count - 1].RotateList, obj);
            else
                _Add_Event(TriggerList[TriggerList.Count - 1].RotateList, obj);
        }
        private void _Add_Color(EasingType easing, int startTime, int endTime, int r1, int g1, int b1, int r2, int g2, int b2)
        {
            var obj = new Color(easing, startTime, endTime, r1, g1, b1, r2, g2, b2);
            if (!_isLooping && !_isTriggering)
                _Add_Event(ColorList, obj);
            else if (_isLooping)
                _Add_Event(LoopList[LoopList.Count - 1].ColorList, obj);
            else
                _Add_Event(TriggerList[TriggerList.Count - 1].ColorList, obj);
        }
        private void _Add_MoveX(EasingType easing, int startTime, int endTime, double x1, double x2)
        {
            var obj = new MoveX(easing, startTime, endTime, x1, x2);
            if (!_isLooping && !_isTriggering)
                _Add_Event(MoveXList, obj);
            else if (_isLooping)
                _Add_Event(LoopList[LoopList.Count - 1].MoveXList, obj);
            else
                _Add_Event(TriggerList[TriggerList.Count - 1].MoveXList, obj);
        }
        private void _Add_MoveY(EasingType easing, int startTime, int endTime, double y1, double y2)
        {
            var obj = new MoveY(easing, startTime, endTime, y1, y2);
            if (!_isLooping && !_isTriggering)
                _Add_Event(MoveYList, obj);
            else if (_isLooping)
                _Add_Event(LoopList[LoopList.Count - 1].MoveYList, obj);
            else
                _Add_Event(TriggerList[TriggerList.Count - 1].MoveYList, obj);
        }
        private void _Add_Param(EasingType easing, int startTime, int endTime, string p)
        {
            var obj = new Parameter(easing, startTime, endTime, p);
            if (!_isLooping && !_isTriggering)
                _Add_Event(ParameterList, obj);
            else if (_isLooping)
                _Add_Event(LoopList[LoopList.Count - 1].ParameterList, obj);
            else
                _Add_Event(TriggerList[TriggerList.Count - 1].ParameterList, obj);
        }
        private void _Add_Vector(EasingType easing, int startTime, int endTime, double vx1, double vy1, double vx2, double vy2)
        {
            var obj = new Vector(easing, startTime, endTime, vx1, vy1, vx2, vy2);
            if (!_isLooping && !_isTriggering)
                _Add_Event(VectorList, obj);
            else if (_isLooping)
                _Add_Event(LoopList[LoopList.Count - 1].VectorList, obj);
            else
                _Add_Event(TriggerList[TriggerList.Count - 1].VectorList, obj);
        }
        private void _Remove_Event<T>(IList<T> list, int index)
        {
            dynamic evt = list[index];
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
            list.RemoveAt(index);
        }
        #endregion

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
        #endregion
    }

}
