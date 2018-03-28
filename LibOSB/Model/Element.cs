using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB.Constants;
using System.Diagnostics;
using LibOSB.Model.ActionType;

namespace LibOSB
{
    /// <summary>
    /// Represents a storyboard element. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public class Element
    {
        #region non-public member
        private bool isTriggering = false, isLooping = false;
        protected bool isInnerClass = false;

        internal Move _Move { get; } = new Move();
        internal Scale _Scale { get; } = new Scale();
        internal Fade _Fade { get; } = new Fade();
        internal Rotate _Rotate { get; } = new Rotate();
        internal Vector _Vector { set; get; } = new Vector();
        internal Color _Color { get; } = new Color();
        internal MoveX _MoveX { get; } = new MoveX();
        internal MoveY _MoveY { get; } = new MoveY();
        internal Parameter Parameter { set; get; } = new Parameter();
        internal LoopOld LoopOld { set; get; } = new LoopOld();
        internal TriggerOld TriggerOld { set; get; } = new TriggerOld();

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
        #endregion

        public ElementType Type { get; private set; }
        public LayerType Layer { get; private set; }
        public OriginType Origin { get; private set; }
        public string ImagePath { get; private set; }
        public double DefaultY { get; private set; }
        public double DefaultX { get; private set; }
        public LoopType LoopType { get; private set; }
        public double? FrameCount { get; private set; }
        public double? FrameRate { get; private set; }
        public List<Loop> LoopList { protected set; get; } = new List<Loop>();
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
            LoopList.Add(new Loop(startTime, time));
        }

        public void StartTrigger(int startTime, int time)
        {
            if (isLooping || isTriggering) throw new Exception("You can not start another loop when the previous one isn't end.");
            isTriggering = true;
        }

        public void EndLoop()
        {
            if (!isLooping) throw new Exception("You can not stop a loop when a loop isn't started.");
            isLooping = false;
            isTriggering = false;
        }

        public void Move(int startTime, System.Drawing.Point location)
        {
            _add_move(0, startTime, startTime, location.X, location.Y, location.X, location.Y);
        }
        public void Move(int startTime, double x, double y)
        {
            _add_move(0, startTime, startTime, x, y, x, y);
        }
        public void Move(int startTime, int endTime, double x, double y)
        {
            _add_move(0, startTime, endTime, x, y, x, y);
        }
        public void Move(EasingType easing, int startTime, int endTime, System.Drawing.Point startLocation, System.Drawing.Point endLocation)
        {
            _add_move(easing, startTime, endTime, startLocation.X, startLocation.Y, endLocation.X, endLocation.Y);
        }
        public void Move(EasingType easing, int startTime, int endTime, double x1, double y1, double x2, double y2)
        {
            _add_move(easing, startTime, endTime, x1, y1, x2, y2);
        }

        public void Fade(int startTime, double alpha)
        {
            _add_fade(0, startTime, startTime, alpha, alpha);
        }
        public void Fade(int startTime, int endTime, double alpha)
        {
            _add_fade(0, startTime, endTime, alpha, alpha);
        }
        public void Fade(EasingType easing, int startTime, int endTime, double startAlpha, double endAlpha)
        {
            _add_fade(easing, startTime, endTime, startAlpha, endAlpha);
        }

        public void Scale(int startTime, double scale)
        {
            _Scale.Add(0, startTime, startTime, scale, scale);
        }
        public void Scale(int startTime, int endTime, double scale)
        {
            _Scale.Add(0, startTime, endTime, scale, scale);
        }
        public void Scale(EasingType easing, int startTime, int endTime, double startScale, double endScale)
        {
            _Scale.Add(easing, startTime, endTime, startScale, endScale);
        }

        public void Rotate(int startTime, double rotate)
        {
            _Rotate.Add(0, startTime, startTime, rotate, rotate);
        }
        public void Rotate(int startTime, int endTime, double rotate)
        {
            _Rotate.Add(0, startTime, endTime, rotate, rotate);
        }
        public void Rotate(EasingType easing, int startTime, int endTime, double startRotate, double endRotate)
        {
            _Rotate.Add(easing, startTime, endTime, startRotate, endRotate);
        }

        public void MoveX(int startTime, double x)
        {
            _MoveX.Add(0, startTime, startTime, x, x);
        }
        public void MoveX(int startTime, int endTime, double x)
        {
            _MoveX.Add(0, startTime, endTime, x, x);
        }
        public void MoveX(EasingType easing, int startTime, int endTime, double startX, double endX)
        {
            _MoveX.Add(easing, startTime, endTime, startX, endX);
        }

        public void MoveY(int startTime, double y)
        {
            _MoveY.Add(0, startTime, startTime, y, y);
        }
        public void MoveY(int startTime, int endTime, double y)
        {
            _MoveY.Add(0, startTime, endTime, y, y);
        }
        public void MoveY(EasingType easing, int startTime, int endTime, double startY, double endY)
        {
            _MoveY.Add(easing, startTime, endTime, startY, endY);
        }

        private void _add_move(EasingType easing, int startTime, int endTime, double x1, double y1, double x2, double y2)
        {
            if (!isLooping)
                _Move.Add(easing, startTime, endTime, x1, y1, x2, y2);
            else
                LoopList[LoopList.Count - 1]._Move.Add(easing, startTime, endTime, x1, y1, x2, y2);
        }

        private void _add_fade(EasingType easing, int startTime, int endTime, double startAlpha, double endAlpha)
        {
            startAlpha = CheckAlpha(startAlpha);
            endAlpha = CheckAlpha(endAlpha);
            if (!isLooping)
                _Fade.Add(easing, startTime, endTime, startAlpha, endAlpha);
            else
                LoopList[LoopList.Count - 1]._Fade.Add(easing, startTime, endTime, startAlpha, endAlpha);
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
            for (int i = 1; i <= Parameter.Count; i++) sb.AppendLine(index + Parameter[i - 1].ToString());
            for (int i = 1; i <= LoopList.Count; i++) sb.Append(LoopList[i - 1].ToString());
            //for (int i = 1; i <= Loop.Count; i++)
            //{
            //    sb.AppendLine(Loop[i - 1].ToString());
            //    for (int j = 1; j <= Loop[i - 1]._Move.Count; j++) sb.AppendLine(Loop[i - 1]._Move[j - 1].ToString());
            //    for (int j = 1; j <= Loop[i - 1]._Scale.Count; j++) sb.AppendLine(Loop[i - 1]._Scale[j - 1].ToString());
            //    for (int j = 1; j <= Loop[i - 1]._Fade.Count; j++) sb.AppendLine(Loop[i - 1]._Fade[j - 1].ToString());
            //    for (int j = 1; j <= Loop[i - 1]._Rotate.Count; j++) sb.AppendLine(Loop[i - 1]._Rotate[j - 1].ToString());
            //    for (int j = 1; j <= Loop[i - 1]._Vector.Count; j++) sb.AppendLine(Loop[i - 1]._Vector[j - 1].ToString());
            //    for (int j = 1; j <= Loop[i - 1]._Color.Count; j++) sb.AppendLine(Loop[i - 1]._Color[j - 1].ToString());
            //    for (int j = 1; j <= Loop[i - 1]._MoveX.Count; j++) sb.AppendLine(Loop[i - 1]._MoveX[j - 1].ToString());
            //    for (int j = 1; j <= Loop[i - 1]._MoveY.Count; j++) sb.AppendLine(Loop[i - 1]._MoveY[j - 1].ToString());
            //    for (int j = 1; j <= Loop[i - 1].Parameter.Count; j++) sb.AppendLine(Loop[i - 1].Parameter[j - 1].ToString());
            //}
            //for (int i = 1; i <= Trigger.Count; i++)
            //{
            //    sb.AppendLine(Trigger[i - 1].ToString());
            //    for (int j = 1; j <= Trigger[i - 1].Move.Count; j++) sb.AppendLine(Trigger[i - 1].Move[j - 1].ToString());
            //    for (int j = 1; j <= Trigger[i - 1].Scale.Count; j++) sb.AppendLine(Trigger[i - 1].Scale[j - 1].ToString());
            //    for (int j = 1; j <= Trigger[i - 1].Fade.Count; j++) sb.AppendLine(Trigger[i - 1].Fade[j - 1].ToString());
            //    for (int j = 1; j <= Trigger[i - 1].Rotate.Count; j++) sb.AppendLine(Trigger[i - 1].Rotate[j - 1].ToString());
            //    for (int j = 1; j <= Trigger[i - 1].Vector.Count; j++) sb.AppendLine(Trigger[i - 1].Vector[j - 1].ToString());
            //    for (int j = 1; j <= Trigger[i - 1].Color.Count; j++) sb.AppendLine(Trigger[i - 1].Color[j - 1].ToString());
            //    for (int j = 1; j <= Trigger[i - 1].MoveX.Count; j++) sb.AppendLine(Trigger[i - 1].MoveX[j - 1].ToString());
            //    for (int j = 1; j <= Trigger[i - 1].MoveY.Count; j++) sb.AppendLine(Trigger[i - 1].MoveY[j - 1].ToString());
            //    for (int j = 1; j <= Trigger[i - 1].Parameter.Count; j++) sb.AppendLine(Trigger[i - 1].Parameter[j - 1].ToString());

            //}
            return sb.ToString();
        }
    }

}
