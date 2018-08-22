using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LibOsb.BrewHelper;
using LibOsb.Model.Constants;
using LibOsb.Model.EventClass;
using LibOsb.Model.EventType;
using OpenTK;
using StorybrewCommon.Storyboarding;

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
        public double? DefaultY { get; internal set; }
        public double? DefaultX { get; internal set; }
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

        public int MaxTimeCount { get; internal set; } = 1;
        public int MinTimeCount { get; internal set; } = 1;

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

        #region 折叠：Event function
        public void Move(int startTime, System.Drawing.PointF location) => AddMove(0, startTime, startTime, location.X, location.Y, location.X, location.Y);
        public void Move(int startTime, double x, double y) => AddMove(0, startTime, startTime, x, y, x, y);
        public void Move(int startTime, int endTime, double x, double y) => AddMove(0, startTime, endTime, x, y, x, y);
        public void Move(EasingType easing, int startTime, int endTime, System.Drawing.PointF startLocation, System.Drawing.PointF endLocation) => AddMove(easing, startTime, endTime, startLocation.X, startLocation.Y, endLocation.X, endLocation.Y);
        public void Move(EasingType easing, int startTime, int endTime, double x1, double y1, double x2, double y2) => AddMove(easing, startTime, endTime, x1, y1, x2, y2);

        public void Fade(int startTime, double alpha) => AddFade(0, startTime, startTime, alpha, alpha);
        public void Fade(int startTime, int endTime, double alpha) => AddFade(0, startTime, endTime, alpha, alpha);
        public void Fade(EasingType easing, int startTime, int endTime, double startAlpha, double endAlpha) => AddFade(easing, startTime, endTime, startAlpha, endAlpha);

        public void Scale(int startTime, double scale) => AddScale(0, startTime, startTime, scale, scale);
        public void Scale(int startTime, int endTime, double scale) => AddScale(0, startTime, endTime, scale, scale);
        public void Scale(EasingType easing, int startTime, int endTime, double startScale, double endScale) => AddScale(easing, startTime, endTime, startScale, endScale);

        public void Rotate(int startTime, double rotate) => AddRotate(0, startTime, startTime, rotate, rotate);
        public void Rotate(int startTime, int endTime, double rotate) => AddRotate(0, startTime, endTime, rotate, rotate);
        public void Rotate(EasingType easing, int startTime, int endTime, double startRotate, double endRotate) => AddRotate(easing, startTime, endTime, startRotate, endRotate);

        public void MoveX(int startTime, double x) => AddMoveX(0, startTime, startTime, x, x);
        public void MoveX(int startTime, int endTime, double x) => AddMoveX(0, startTime, endTime, x, x);
        public void MoveX(EasingType easing, int startTime, int endTime, double startX, double endX) => AddMoveX(easing, startTime, endTime, startX, endX);

        public void MoveY(int startTime, double y) => AddMoveY(0, startTime, startTime, y, y);
        public void MoveY(int startTime, int endTime, double y) => AddMoveY(0, startTime, endTime, y, y);
        public void MoveY(EasingType easing, int startTime, int endTime, double startY, double endY) => AddMoveY(easing, startTime, endTime, startY, endY);

        public void Color(int startTime, System.Drawing.Color color) => AddColor(0, startTime, startTime, color.R, color.G, color.B, color.R, color.G, color.B);
        public void Color(int startTime, int endTime, System.Drawing.Color color) => AddColor(0, startTime, endTime, color.R, color.G, color.B, color.R, color.G, color.B);
        public void Color(EasingType easing, int startTime, int endTime, System.Drawing.Color color1, System.Drawing.Color color2) => AddColor(easing, startTime, endTime, color1.R, color1.G, color1.B, color2.R, color2.G, color2.B);
        public void Color(int startTime, int r, int g, int b) => AddColor(0, startTime, startTime, r, g, b, r, g, b);
        public void Color(int startTime, int endTime, int r, int g, int b) => AddColor(0, startTime, endTime, r, g, b, r, g, b);
        public void Color(EasingType easing, int startTime, int endTime, int startR, int startG, int startB, int endR, int endG, int endB) => AddColor(easing, startTime, endTime, startR, startG, startB, endR, endG, endB);

        public void Vector(int startTime, System.Drawing.SizeF zoom) => AddVector(0, startTime, startTime, zoom.Width, zoom.Height, zoom.Width, zoom.Height);
        public void Vector(int startTime, double w, double h) => AddVector(0, startTime, startTime, w, h, w, h);
        public void Vector(int startTime, int endTime, double w, double h) => AddVector(0, startTime, endTime, w, h, w, h);
        public void Vector(EasingType easing, int startTime, int endTime, System.Drawing.SizeF startZoom, System.Drawing.SizeF endZoom) => AddVector(easing, startTime, endTime, startZoom.Width, startZoom.Height, endZoom.Width, endZoom.Height);
        public void Vector(EasingType easing, int startTime, int endTime, double w1, double h1, double w2, double h2) => AddVector(easing, startTime, endTime, w1, h1, w2, h2);

        public void FlipH(int startTime) => AddParam(0, startTime, startTime, "H");
        public void FlipH(int startTime, int endTime) => AddParam(0, startTime, endTime, "H");
        public void FlipV(int startTime) => AddParam(0, startTime, startTime, "V");
        public void FlipV(int startTime, int endTime) => AddParam(0, startTime, endTime, "V");

        public void Lighting(int startTime) => AddParam(0, startTime, startTime, "A");
        public void Lighting(int startTime, int endTime) => AddParam(0, startTime, endTime, "A");

        internal void Parameter(EasingType easing, int startTime, int endTime, string type) => AddParam(easing, startTime, endTime, type);
        #endregion


        public override string ToString()
        {
            if (!IsSignificative) return null;

            var sb = new StringBuilder();
            if (!IsLOrT)
            {
                sb.Append(string.Join(",", Type, Layer, Origin, $"\"{ImagePath}\"", DefaultX, DefaultY));
                if (FrameCount != null)
                    sb.AppendLine("," + string.Join(",", FrameCount, FrameRate, LoopType));
                else
                    sb.AppendLine();
            }
            string index = (IsLOrT) ? "  " : " ";
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

        public Element Clone() => (Element)MemberwiseClone();

        #region non-public member
        private bool _isTriggering, _isLooping;
        internal bool IsLOrT = false;

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

        internal List<Move> MoveList { get; } = new List<Move>();
        internal List<Scale> ScaleList { get; } = new List<Scale>();
        internal List<Fade> FadeList { get; } = new List<Fade>();
        internal List<Rotate> RotateList { get; } = new List<Rotate>();
        internal List<Vector> VectorList { get; } = new List<Vector>();
        internal List<Color> ColorList { get; } = new List<Color>();
        internal List<MoveX> MoveXList { get; } = new List<MoveX>();
        internal List<MoveY> MoveYList { get; } = new List<MoveY>();
        internal List<Parameter> ParameterList { get; } = new List<Parameter>();
        internal List<Loop> LoopList { get; } = new List<Loop>();
        internal List<Trigger> TriggerList { get; } = new List<Trigger>();

        #region 折叠：扩展属性

        internal TimeRange FadeoutList { get; set; } = new TimeRange();
        internal bool HasFade => FadeList.Count != 0;

        #endregion

        /// <summary>
        /// 调整物件参数
        /// </summary>
        internal void Adjust(double offsetX, double offsetY, int offsetTiming)
        {
            if (DefaultX != null) DefaultX += offsetX;
            if (DefaultY != null) DefaultY += offsetY;

            foreach (var t in MoveList)
            {
                t._Adjust(offsetX, offsetY);
                if (IsLOrT)
                    continue;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in MoveXList)
            {
                t._Adjust(offsetX);
                if (IsLOrT)
                    continue;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in MoveYList)
            {
                t._Adjust(offsetY);
                if (IsLOrT)
                    continue;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in ColorList)
            {
                if (IsLOrT)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in FadeList)
            {
                if (IsLOrT)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in ParameterList)
            {
                if (IsLOrT)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in RotateList)
            {
                if (IsLOrT)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in ScaleList)
            {
                if (IsLOrT)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in VectorList)
            {
                if (IsLOrT)
                    break;
                t._AdjustTime(offsetTiming);
            }
            foreach (var t in LoopList)
            {
                t.StartTime += offsetTiming;
                t.Adjust(offsetX, offsetY, offsetTiming);
            }
            foreach (var t in TriggerList)
            {
                t.StartTime += offsetTiming;
                t.EndTime += offsetTiming;
                t.Adjust(offsetX, offsetY, offsetTiming);
            }
        }

        #region 折叠：直接控制Event修改方法
        private void AddEvent<T>(ICollection<T> list, T _event)
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
        private void AddMove(EasingType easing, int startTime, int endTime, double x1, double y1, double x2, double y2)
        {
            var obj = new Move(easing, startTime, endTime, x1, y1, x2, y2);
            if (!_isLooping && !_isTriggering)
                AddEvent(MoveList, obj);
            else if (_isLooping)
                AddEvent(LoopList[LoopList.Count - 1].MoveList, obj);
            else
                AddEvent(TriggerList[TriggerList.Count - 1].MoveList, obj);
        }
        private void AddFade(EasingType easing, int startTime, int endTime, double f1, double f2)
        {
            CheckAlpha(f1);
            CheckAlpha(f2);

            var obj = new Fade(easing, startTime, endTime, f1, f2);
            if (!_isLooping && !_isTriggering)
                AddEvent(FadeList, obj);
            else if (_isLooping)
                AddEvent(LoopList[LoopList.Count - 1].FadeList, obj);
            else
                AddEvent(TriggerList[TriggerList.Count - 1].FadeList, obj);

            void CheckAlpha(double a)
            {
                if (a < 0 || a > 1)
                    Debug.WriteLine("[Warning] Alpha of fade should be between 0 and 1.");
            }
        }
        private void AddScale(EasingType easing, int startTime, int endTime, double s1, double s2)
        {
            var obj = new Scale(easing, startTime, endTime, s1, s2);
            if (!_isLooping && !_isTriggering)
                AddEvent(ScaleList, obj);
            else if (_isLooping)
                AddEvent(LoopList[LoopList.Count - 1].ScaleList, obj);
            else
                AddEvent(TriggerList[TriggerList.Count - 1].ScaleList, obj);
        }
        private void AddRotate(EasingType easing, int startTime, int endTime, double r1, double r2)
        {
            var obj = new Rotate(easing, startTime, endTime, r1, r2);
            if (!_isLooping && !_isTriggering)
                AddEvent(RotateList, obj);
            else if (_isLooping)
                AddEvent(LoopList[LoopList.Count - 1].RotateList, obj);
            else
                AddEvent(TriggerList[TriggerList.Count - 1].RotateList, obj);
        }
        private void AddColor(EasingType easing, int startTime, int endTime, int r1, int g1, int b1, int r2, int g2, int b2)
        {
            var obj = new Color(easing, startTime, endTime, r1, g1, b1, r2, g2, b2);
            if (!_isLooping && !_isTriggering)
                AddEvent(ColorList, obj);
            else if (_isLooping)
                AddEvent(LoopList[LoopList.Count - 1].ColorList, obj);
            else
                AddEvent(TriggerList[TriggerList.Count - 1].ColorList, obj);
        }
        private void AddMoveX(EasingType easing, int startTime, int endTime, double x1, double x2)
        {
            var obj = new MoveX(easing, startTime, endTime, x1, x2);
            if (!_isLooping && !_isTriggering)
                AddEvent(MoveXList, obj);
            else if (_isLooping)
                AddEvent(LoopList[LoopList.Count - 1].MoveXList, obj);
            else
                AddEvent(TriggerList[TriggerList.Count - 1].MoveXList, obj);
        }
        private void AddMoveY(EasingType easing, int startTime, int endTime, double y1, double y2)
        {
            var obj = new MoveY(easing, startTime, endTime, y1, y2);
            if (!_isLooping && !_isTriggering)
                AddEvent(MoveYList, obj);
            else if (_isLooping)
                AddEvent(LoopList[LoopList.Count - 1].MoveYList, obj);
            else
                AddEvent(TriggerList[TriggerList.Count - 1].MoveYList, obj);
        }
        private void AddParam(EasingType easing, int startTime, int endTime, string p)
        {
            var obj = new Parameter(easing, startTime, endTime, p);
            if (!_isLooping && !_isTriggering)
                AddEvent(ParameterList, obj);
            else if (_isLooping)
                AddEvent(LoopList[LoopList.Count - 1].ParameterList, obj);
            else
                AddEvent(TriggerList[TriggerList.Count - 1].ParameterList, obj);
        }
        private void AddVector(EasingType easing, int startTime, int endTime, double vx1, double vy1, double vx2, double vy2)
        {
            var obj = new Vector(easing, startTime, endTime, vx1, vy1, vx2, vy2);
            if (!_isLooping && !_isTriggering)
                AddEvent(VectorList, obj);
            else if (_isLooping)
                AddEvent(LoopList[LoopList.Count - 1].VectorList, obj);
            else
                AddEvent(TriggerList[TriggerList.Count - 1].VectorList, obj);
        }

        #endregion

        #endregion non-public member
    }

}
