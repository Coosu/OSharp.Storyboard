using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB.Constants;
using System.Diagnostics;
using LibOSB.Model.EventType;
using LibOSB.Model.Constants;

namespace LibOSB
{
    /// <summary>
    /// Represents a storyboard element. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public class Element
    {
        public ElementType Type { get; private set; }
        public LayerType Layer { get; private set; }
        public OriginType Origin { get; private set; }
        public string ImagePath { get; private set; }
        public double DefaultY { get; private set; }
        public double DefaultX { get; private set; }
        public LoopType LoopType { get; private set; }
        public double? FrameCount { get; private set; }
        public double? FrameRate { get; private set; }
        public List<Loop> Loop { protected set; get; } = new List<Loop>();
        public List<Trigger> Trigger { protected set; get; } = new List<Trigger>();

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

        internal Element() { }

        public void StartLoop(int startTime, int time)
        {
            if (isLooping || isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            isLooping = true;
            Loop.Add(new Loop(startTime, time));
        }

        public void StartTrigger(int startTime, int time, TriggerType[] triggerType, int customSampleSet = -1)
        {
            if (isLooping || isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            isTriggering = true;
            Trigger.Add(new Trigger(startTime, time, triggerType, customSampleSet));
        }

        public void EndLoop()
        {
            if (!isLooping) throw new Exception("You can not stop a loop when a loop isn't started.");
            isLooping = false;
            isTriggering = false;
        }

        public void Move(int startTime, System.Drawing.Point location)
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
        public void Move(EasingType easing, int startTime, int endTime, System.Drawing.Point startLocation, System.Drawing.Point endLocation)
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
            _Add_Scale(0, startTime, startTime, rotate, rotate);
        }
        public void Rotate(int startTime, int endTime, double rotate)
        {
            _Add_Scale(0, startTime, endTime, rotate, rotate);
        }
        public void Rotate(EasingType easing, int startTime, int endTime, double startRotate, double endRotate)
        {
            _Add_Scale(easing, startTime, endTime, startRotate, endRotate);
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
            for (int i = 1; i <= Loop.Count; i++) sb.Append(Loop[i - 1].ToString());
            return sb.ToString();
        }

        #region non-public member
        private bool isTriggering = false, isLooping = false;
        protected bool isInnerClass = false;

        internal List<Move> _Move { get; } = new List<Move>();
        internal List<Scale> _Scale { get; } = new List<Scale>();
        internal List<Fade> _Fade { get; } = new List<Fade>();
        internal List<Rotate> _Rotate { get; } = new List<Rotate>();
        internal List<Vector> _Vector { set; get; } = new List<Vector>();
        internal List<Color> _Color { get; } = new List<Color>();
        internal List<MoveX> _MoveX { get; } = new List<MoveX>();
        internal List<MoveY> _MoveY { get; } = new List<MoveY>();
        internal List<Parameter> _Parameter { set; get; } = new List<Parameter>();

        private double CheckAlpha(double a)
        {
            if (a < 0 || a > 1)
            {
                if (a < 0) a = 0;
                else a = 1;
                Debug.WriteLine("[Warning] Alpha of fade should be between 0 and 1.");
            }

            return a;
        }

        private void _Add_Move(EasingType easing, int startTime, int endTime, double x1, double y1, double x2, double y2)
        {
            var obj = new Move(easing, startTime, endTime, x1, y1, x2, y2);
            if (!isLooping && !isTriggering)
                _Move.Add(obj);
            else if (isLooping)
                Loop[Loop.Count - 1]._Move.Add(obj);
            else
                Trigger[Trigger.Count - 1]._Move.Add(obj);
        }

        private void _Add_Fade(EasingType easing, int startTime, int endTime, double f1, double f2)
        {
            var obj = new Fade(easing, startTime, endTime, f1, f2);
            f1 = CheckAlpha(f1);
            f2 = CheckAlpha(f2);
            if (!isLooping)
                _Fade.Add(obj);
            else if (isLooping)
                Loop[Loop.Count - 1]._Fade.Add(obj);
            else
                Trigger[Trigger.Count - 1]._Fade.Add(obj);
        }

        private void _Add_Scale(EasingType easing, int startTime, int endTime, double s1, double s2)
        {
            var obj = new Scale(easing, startTime, endTime, s1, s2);
            if (!isLooping && !isTriggering)
                _Scale.Add(obj);
            else if (isLooping)
                Loop[Loop.Count - 1]._Scale.Add(obj);
            else
                Trigger[Trigger.Count - 1]._Scale.Add(obj);
        }

        private void _Add_Rotate(EasingType easing, int startTime, int endTime, double r1, double r2)
        {
            var obj = new Rotate(easing, startTime, endTime, r1, r2);
            if (!isLooping && !isTriggering)
                _Rotate.Add(obj);
            else if (isLooping)
                Loop[Loop.Count - 1]._Rotate.Add(obj);
            else
                Trigger[Trigger.Count - 1]._Rotate.Add(obj);
        }

        private void _Add_Color(EasingType easing, int startTime, int endTime, int r1, int g1, int b1, int r2, int g2, int b2)
        {
            var obj = new Color(easing, startTime, endTime, r1, g1, b1, r2, g2, b2);
            if (!isLooping && !isTriggering)
                _Color.Add(obj);
            else if (isLooping)
                Loop[Loop.Count - 1]._Color.Add(obj);
            else
                Trigger[Trigger.Count - 1]._Color.Add(obj);
        }
        private void _Add_MoveX(EasingType easing, int startTime, int endTime, double x1, double x2)
        {
            var obj = new MoveX(easing, startTime, endTime, x1, x2);
            if (!isLooping && !isTriggering)
                _MoveX.Add(obj);
            else if (isLooping)
                Loop[Loop.Count - 1]._MoveX.Add(obj);
            else
                Trigger[Trigger.Count - 1]._MoveX.Add(obj);
        }
        private void _Add_MoveY(EasingType easing, int startTime, int endTime, double y1, double y2)
        {
            var obj = new MoveY(easing, startTime, endTime, y1, y2);
            if (!isLooping && !isTriggering)
                _MoveY.Add(obj);
            else if (isLooping)
                Loop[Loop.Count - 1]._MoveY.Add(obj);
            else
                Trigger[Trigger.Count - 1]._MoveY.Add(obj);
        }
        private void _Add_Param(EasingType easing, int startTime, int endTime, string p)
        {
            var obj = new Parameter(easing, startTime, endTime, p);
            if (!isLooping && !isTriggering)
                _Parameter.Add(obj);
            else if (isLooping)
                Loop[Loop.Count - 1]._Parameter.Add(obj);
            else
                Trigger[Trigger.Count - 1]._Parameter.Add(obj);
        }
        private void _Add_Vector(EasingType easing, int startTime, int endTime, double vx1, double vy1, double vx2, double vy2)
        {
            var obj = new Vector(easing, startTime, endTime, vx1, vy1, vx2, vy2);
            if (!isLooping && !isTriggering)
                _Vector.Add(obj);
            else if (isLooping)
                Loop[Loop.Count - 1]._Vector.Add(obj);
            else
                Trigger[Trigger.Count - 1]._Vector.Add(obj);
        }
        #endregion
    }

}
