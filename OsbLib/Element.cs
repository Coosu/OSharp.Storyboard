using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models;
using Milkitic.OsbLib.Models.EventType;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Milkitic.OsbLib
{
    /// <summary>
    /// Represents a storyboard element. This class cannot be inherited.
    /// </summary>
    public class Element : EventContainer
    {
        public ElementType Type { get; protected set; }
        public LayerType Layer { get; protected set; }
        public OriginType Origin { get; protected set; }
        public string ImagePath { get; protected set; }
        public float DefaultY { get; internal set; }
        public float DefaultX { get; internal set; }

        //扩展
        public override List<IEvent> EventList { get; set; } = new List<IEvent>();
        public List<Loop> LoopList { get; } = new List<Loop>();
        public List<Trigger> TriggerList { get; } = new List<Trigger>();

        public List<Fade> FadeList =>
            EventList.Where(k => k.EventType == EventEnum.Fade).Select(k => (Fade)k).ToList();
        public List<Color> ColorList =>
            EventList.Where(k => k.EventType == EventEnum.Color).Select(k => (Color)k).ToList();
        public List<Move> MoveList =>
            EventList.Where(k => k.EventType == EventEnum.Move).Select(k => (Move)k).ToList();
        public List<MoveX> MoveXList =>
            EventList.Where(k => k.EventType == EventEnum.MoveX).Select(k => (MoveX)k).ToList();
        public List<MoveY> MoveYList =>
            EventList.Where(k => k.EventType == EventEnum.MoveY).Select(k => (MoveY)k).ToList();
        public List<Parameter> ParameterList =>
            EventList.Where(k => k.EventType == EventEnum.Parameter).Select(k => (Parameter)k).ToList();
        public List<Rotate> RotateList =>
            EventList.Where(k => k.EventType == EventEnum.Rotate).Select(k => (Rotate)k).ToList();
        public List<Scale> ScaleList =>
            EventList.Where(k => k.EventType == EventEnum.Scale).Select(k => (Scale)k).ToList();
        public List<Vector> VectorList =>
            EventList.Where(k => k.EventType == EventEnum.Vector).Select(k => (Vector)k).ToList();

        public override float MaxTime
        {
            get
            {
                List<float> max = new List<float>();
                if (EventList.Count > 0)
                    max.Add(EventList.Max(k => k.EndTime));
                if (LoopList.Count > 0)
                    max.Add(LoopList.Max(k => k.OutterMaxTime));
                if (TriggerList.Count > 0)
                    max.Add(TriggerList.Max(k => k.MaxTime));
                return max.Count == 0 ? 0 : max.Max();
            }
        }

        public override float MinTime
        {
            get
            {
                List<float> min = new List<float>();
                if (EventList.Count > 0)
                    min.Add(EventList.Min(k => k.StartTime));
                if (LoopList.Count > 0)
                    min.Add(LoopList.Min(k => k.OutterMinTime));
                if (TriggerList.Count > 0)
                    min.Add(TriggerList.Min(k => k.MinTime));
                return min.Count == 0 ? 0 : min.Min();
            }
        }

        public override float MaxStartTime
        {
            get
            {
                List<float> max = new List<float>();
                if (EventList.Count > 0)
                    max.Add(EventList.Max(k => k.StartTime));
                if (LoopList.Count > 0)
                    max.Add(LoopList.Max(k => k.OutterMinTime));
                if (TriggerList.Count > 0)
                    max.Add(TriggerList.Max(k => k.MinTime));
                return max.Count == 0 ? 0 : max.Max();
            }
        }

        public override float MinEndTime
        {
            get
            {
                List<float> min = new List<float>();
                if (EventList.Count > 0)
                    min.Add(EventList.Min(k => k.EndTime));
                if (LoopList.Count > 0)
                    min.Add(LoopList.Min(k => k.OutterMaxTime));
                if (TriggerList.Count > 0)
                    min.Add(TriggerList.Min(k => k.MaxTime));
                return min.Count == 0 ? 0 : min.Min();
            }
        }

        public bool IsValid => MinTime != MaxTime;

        public int MaxTimeCount { get; internal set; } = 1;
        public int MinTimeCount { get; internal set; } = 1;

        public TimeRange FadeoutList { get; internal set; } = new TimeRange();

        // Loop control
        private bool _isTriggering, _isLooping;

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="type">Set element type.</param>
        /// <param name="layer">Set element layer.</param>
        /// <param name="origin">Set element origin.</param>
        /// <param name="imagePath">Set image path.</param>
        /// <param name="defaultX">Set default x-coordinate of location.</param>
        /// <param name="defaultY">Set default x-coordinate of location.</param>
        public Element(ElementType type, LayerType layer, OriginType origin, string imagePath, float defaultX, float defaultY)
        {
            Type = type;
            Layer = layer;
            Origin = origin;
            ImagePath = imagePath;
            DefaultX = defaultX;
            DefaultY = defaultY;
            _isLooping = false;
        }

        public Element(string type, string layer, string origin, string imagePath, float defaultX, float defaultY)
        {
            Type = (ElementType)Enum.Parse(typeof(ElementType), type);
            Layer = (LayerType)Enum.Parse(typeof(LayerType), layer);
            Origin = (OriginType)Enum.Parse(typeof(OriginType), origin);
            ImagePath = imagePath;
            DefaultX = defaultX;
            DefaultY = defaultY;
            _isLooping = false;
        }

        public void StartLoop(int startTime, int loopCount)
        {
            if (_isLooping || _isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            _isLooping = true;
            LoopList.Add(new Loop(startTime, loopCount));
        }

        public void StartTrigger(int startTime, int endTime, TriggerType[] triggerType, int customSampleSet = -1)
        {
            if (_isLooping || _isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            _isTriggering = true;
            TriggerList.Add(new Trigger(startTime, endTime, triggerType, customSampleSet));
        }

        public void StartTrigger(int startTime, int endTime, string triggerType)
        {
            if (_isLooping || _isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            _isTriggering = true;
            TriggerList.Add(new Trigger(startTime, endTime, triggerType));
        }

        public void EndLoop()
        {
            if (!_isLooping && !_isTriggering) throw new Exception("You can not stop a loop when a loop isn't started.");
            TryEndLoop();
        }

        private void TryEndLoop()
        {
            _isLooping = false;
            _isTriggering = false;
        }

        #region 折叠：Event function

        // Move
        public void Move(int startTime, Vector2 point) =>
            AddEvent(EventEnum.Move, 0, startTime, startTime, point.X, point.Y, point.X, point.Y);
        public void Move(int startTime, float x, float y) =>
            AddEvent(EventEnum.Move, 0, startTime, startTime, x, y, x, y);
        public void Move(int startTime, int endTime, float x, float y) =>
            AddEvent(EventEnum.Move, 0, startTime, endTime, x, y, x, y);
        public void Move(EasingType easing, int startTime, int endTime, Vector2 startPoint, Vector2 endPoint) =>
            AddEvent(EventEnum.Move, easing, startTime, endTime, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
        public void Move(EasingType easing, int startTime, int endTime, float x1, float y1, float x2, float y2) =>
            AddEvent(EventEnum.Move, easing, startTime, endTime, x1, y1, x2, y2);

        // Fade
        public void Fade(int startTime, float opacity) =>
            AddEvent(EventEnum.Fade, 0, startTime, startTime, opacity, opacity);
        public void Fade(int startTime, int endTime, float opacity) =>
            AddEvent(EventEnum.Fade, 0, startTime, endTime, opacity, opacity);
        public void Fade(EasingType easing, int startTime, int endTime, float startOpacity, float endOpacity) =>
            AddEvent(EventEnum.Fade, 0, startTime, endTime, startOpacity, endOpacity);

        // Scale
        public void Scale(int startTime, float scale) =>
            AddEvent(EventEnum.Scale, 0, startTime, startTime, scale, scale);
        public void Scale(int startTime, int endTime, float scale) =>
            AddEvent(EventEnum.Scale, 0, startTime, endTime, scale, scale);
        public void Scale(EasingType easing, int startTime, int endTime, float startScale, float endScale) =>
            AddEvent(EventEnum.Scale, easing, startTime, endTime, startScale, endScale);

        // Rotate
        public void Rotate(int startTime, float rotate) =>
            AddEvent(EventEnum.Rotate, 0, startTime, startTime, rotate, rotate);
        public void Rotate(int startTime, int endTime, float rotate) =>
            AddEvent(EventEnum.Rotate, 0, startTime, endTime, rotate, rotate);
        public void Rotate(EasingType easing, int startTime, int endTime, float startRotate, float endRotate) =>
            AddEvent(EventEnum.Rotate, easing, startTime, endTime, startRotate, endRotate);

        // MoveX
        public void MoveX(int startTime, float x) =>
            AddEvent(EventEnum.MoveX, 0, startTime, startTime, x, x);
        public void MoveX(int startTime, int endTime, float x) =>
            AddEvent(EventEnum.MoveX, 0, startTime, endTime, x, x);
        public void MoveX(EasingType easing, int startTime, int endTime, float startX, float endX) =>
            AddEvent(EventEnum.MoveX, easing, startTime, endTime, startX, endX);

        // MoveY
        public void MoveY(int startTime, float y) =>
            AddEvent(EventEnum.MoveY, 0, startTime, startTime, y, y);
        public void MoveY(int startTime, int endTime, float y) =>
            AddEvent(EventEnum.MoveY, 0, startTime, endTime, y, y);
        public void MoveY(EasingType easing, int startTime, int endTime, float startY, float endY) =>
            AddEvent(EventEnum.MoveY, easing, startTime, endTime, startY, endY);

        // Color
        public void Color(int startTime, Vector3 color) =>
            AddEvent(EventEnum.Color, 0, startTime, startTime, color.X, color.Y, color.Z, color.X, color.Y, color.Z);
        public void Color(int startTime, int endTime, Vector3 color) =>
            AddEvent(EventEnum.Color, 0, startTime, endTime, color.X, color.Y, color.Z, color.X, color.Y, color.Z);
        public void Color(EasingType easing, int startTime, int endTime, Vector3 color1, Vector3 color2) =>
            AddEvent(EventEnum.Color, easing, startTime, endTime, color1.X, color1.Y, color1.Z, color2.X, color2.Y, color2.Z);
        public void Color(int startTime, int r, int g, int b) =>
            AddEvent(EventEnum.Color, 0, startTime, startTime, r, g, b, r, g, b);
        public void Color(int startTime, int endTime, int r, int g, int b) =>
            AddEvent(EventEnum.Color, 0, startTime, endTime, r, g, b, r, g, b);
        public void Color(EasingType easing, int startTime, int endTime, int startR, int startG, int startB, int endR, int endG, int endB) =>
            AddEvent(EventEnum.Color, easing, startTime, endTime, startR, startG, startB, endR, endG, endB);

        // Vector
        public void Vector(int startTime, Vector2 vector) =>
            AddEvent(EventEnum.Vector, 0, startTime, startTime, vector.X, vector.Y, vector.X, vector.Y);
        public void Vector(int startTime, float w, float h) =>
            AddEvent(EventEnum.Vector, 0, startTime, startTime, w, h, w, h);
        public void Vector(int startTime, int endTime, float w, float h) =>
            AddEvent(EventEnum.Vector, 0, startTime, endTime, w, h, w, h);
        public void Vector(EasingType easing, int startTime, int endTime, Vector2 startZoom, Vector2 endZoom) =>
            AddEvent(EventEnum.Vector, easing, startTime, endTime, startZoom.X, startZoom.Y, endZoom.X, endZoom.Y);
        public void Vector(EasingType easing, int startTime, int endTime, float w1, float h1, float w2, float h2) =>
            AddEvent(EventEnum.Vector, easing, startTime, endTime, w1, h1, w2, h2);

        //Extra
        public void FlipH(int startTime) => AddEvent(0, startTime, startTime, ParameterEnum.Horizontal);
        public void FlipH(int startTime, int endTime) => AddEvent(0, startTime, endTime, ParameterEnum.Horizontal);

        public void FlipV(int startTime) => AddEvent(0, startTime, startTime, ParameterEnum.Vertical);
        public void FlipV(int startTime, int endTime) => AddEvent(0, startTime, endTime, ParameterEnum.Vertical);

        public void Additive(int startTime) =>
            AddEvent(0, startTime, startTime, ParameterEnum.Additive);
        public void Additive(int startTime, int endTime) =>
            AddEvent(0, startTime, endTime, ParameterEnum.Additive);

        public void Parameter(EasingType easing, int startTime, int endTime, ParameterEnum p) =>
            AddEvent(easing, startTime, endTime, p);
        #endregion

        public override string ToString()
        {
            if (!IsValid) return "";

            var head = $"{string.Join(",", Type, Layer, Origin, $"\"{ImagePath}\"", DefaultX, DefaultY)}\r\n";
            return head + GetStringBody();
        }

        protected string GetStringBody()
        {
            var sb = new StringBuilder();
            const string index = " ";
            var events = EventList.GroupBy(k => k.EventType);
            foreach (var kv in events)
                foreach (var e in kv)
                    sb.AppendLine(index + e);

            for (int i = 1; i <= LoopList.Count; i++) sb.Append(LoopList[i - 1]);
            for (int i = 1; i <= TriggerList.Count; i++) sb.Append(TriggerList[i - 1]);
            var body = sb.ToString();
            return body;
        }

        public static Element Parse(string osbString)
        {
            return Parse(osbString, 1);
        }

        public Element Clone() => (Element)MemberwiseClone();

        /// <summary>
        /// 调整物件参数
        /// </summary>
        public override void Adjust(float offsetX, float offsetY, int offsetTiming)
        {
            DefaultX += offsetX;
            DefaultY += offsetY;

            foreach (var loop in LoopList)
                loop.Adjust(offsetX, offsetY, offsetTiming);
            foreach (var trigger in TriggerList)
                trigger.Adjust(offsetX, offsetY, offsetTiming);

            base.Adjust(offsetX, offsetY, offsetTiming);
        }

        public void AddEvent(EventEnum e, EasingType easing, float startTime, float endTime, float x1, float x2) =>
            AddEvent(e, easing, startTime, endTime, new[] { x1 }, new[] { x2 });
        public void AddEvent(EventEnum e, EasingType easing, float startTime, float endTime, float x1, float y1, float x2, float y2) =>
            AddEvent(e, easing, startTime, endTime, new[] { x1, y1 }, new[] { x2, y2 });
        public void AddEvent(EventEnum e, EasingType easing, float startTime, float endTime,
            float x1, float y1, float z1, float x2, float y2, float z2) =>
            AddEvent(e, easing, startTime, endTime, new[] { x1, y1, z1 }, new[] { x2, y2, z2 });
        public void AddEvent(EasingType easing, float startTime, float endTime, ParameterEnum p) =>
            AddEvent(EventEnum.Parameter, easing, startTime, endTime, new[] { (float)(int)p }, new[] { (float)(int)p });
        public override void AddEvent(EventEnum e, EasingType easing, float startTime, float endTime, float[] start, float[] end, bool sequential = true)
        {
            if (_isLooping)
                LoopList[LoopList.Count - 1].AddEvent(e, easing, startTime, endTime, start, end);
            else if (_isTriggering)
                TriggerList[TriggerList.Count - 1].AddEvent(e, easing, startTime, endTime, start, end);
            else
                base.AddEvent(e, easing, startTime, endTime, start, end, sequential);
        }

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
                            obj = new Element(pars[0], pars[1], pars[2], pars[3].Trim('\"'), float.Parse(pars[4]), float.Parse(pars[5]));
                        else if (pars.Length == 9)
                            obj = new AnimatedElement(pars[0], pars[1], pars[2], pars[3].Trim('\"'), float.Parse(pars[4]), float.Parse(pars[5]),
                                int.Parse(pars[6]), float.Parse(pars[7]), pars[8]);
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
                            case "R":
                            case "MX":
                            case "MY":
                                float p1, p2;

                                // 验证是否存在缺省
                                if (pars.Length == 5)
                                    p1 = p2 = float.Parse(pars[4]);
                                else if (pars.Length == 6)
                                {
                                    p1 = float.Parse(pars[4]);
                                    p2 = float.Parse(pars[5]);
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
                                    case "R":
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
                                float p11, p12, p21, p22;

                                // 验证是否存在缺省
                                if (pars.Length == 6)
                                {
                                    p11 = p21 = float.Parse(pars[4]);
                                    p12 = p22 = float.Parse(pars[5]);
                                }
                                else if (pars.Length == 8)
                                {
                                    p11 = float.Parse(pars[4]);
                                    p12 = float.Parse(pars[5]);
                                    p21 = float.Parse(pars[6]);
                                    p22 = float.Parse(pars[7]);
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
                                if (pars.Length == 5)
                                {
                                    string type = pars[4];
                                    obj.Parameter((EasingType)easing, startTime, endTime, type.ToEnum());
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

                obj.TryEndLoop();
            }
            catch (Exception ex)
            {
                throw new FormatException("You have an syntax error in your osb code at line: " + currentLine, ex);
            }
            return obj;
        }
    }
}
