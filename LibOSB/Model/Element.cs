using LibOSB.ActionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB.Constants;
using System.Diagnostics;

namespace LibOSB
{
    /// <summary>
    /// Represents a storyboard element. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public class Element
    {
        #region non-public member
        private Types type;
        private Layers layer;
        private Origins origin;
        private string imagePath;
        private double defaultX, defaultY;
        private double? frameCount, frameRate;
        private LoopType loopType;

        internal Move _Move { get; } = new Move();
        internal Scale _Scale { get; } = new Scale();
        internal Fade _Fade { get; } = new Fade();
        internal Rotate _Rotate { get; } = new Rotate();
        internal Vector Vector { set; get; } = new Vector();
        internal Color _Color { get; } = new Color();
        internal MoveX _MoveX { get; } = new MoveX();
        internal MoveY _MoveY { get; } = new MoveY();
        internal Parameter Parameter { set; get; } = new Parameter();
        internal Loop Loop { set; get; } = new Loop();
        internal Trigger Trigger { set; get; } = new Trigger();

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

        public Types Type { get => type; }
        public Layers Layer { get => layer; }
        public Origins Origin { get => origin; }
        public string ImagePath { get => imagePath; }
        public double DefaultY { get => defaultY; }
        public double DefaultX { get => defaultX; }
        public LoopType LoopType { get => loopType; }
        public double? FrameCount { get => frameCount; }
        public double? FrameRate { get => frameRate; }

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="type">Set element type.</param>
        /// <param name="layer">Set element layer.</param>
        /// <param name="origin">Set element origin.</param>
        /// <param name="imagePath">Set image path.</param>
        /// <param name="defaultX">Set default x-coordinate of location.</param>
        /// <param name="defaultY">Set default x-coordinate of location.</param>
        public Element(Types type, Layers layer, Origins origin, string imagePath, double defaultX, double defaultY)
        {
            this.type = type;
            this.layer = layer;
            this.origin = origin;
            this.imagePath = imagePath;
            this.defaultX = defaultX;
            this.defaultY = defaultY;
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
        public Element(Types type, Layers layer, Origins origin, string imagePath, double defaultX, double defaultY, double frameCount, double frameRate, LoopType loopType)
        {
            this.type = type;
            this.layer = layer;
            this.origin = origin;
            this.imagePath = imagePath;
            this.defaultX = defaultX;
            this.defaultY = defaultY;
            this.frameCount = frameCount;
            this.frameRate = frameRate;
            this.loopType = loopType;
        }

        public void Move(int startTime, System.Drawing.Point location)
        {
            _Move.Add(0, startTime, startTime, location.X, location.Y, location.X, location.Y);
        }
        public void Move(int startTime, double x, double y)
        {
            _Move.Add(0, startTime, startTime, x, y, x, y);
        }
        public void Move(int startTime, int endTime, double x, double y)
        {
            _Move.Add(0, startTime, endTime, x, y, x, y);
        }
        public void Move(Easing easing, int startTime, int endTime, System.Drawing.Point startLocation, System.Drawing.Point endLocation)
        {
            _Move.Add(easing, startTime, endTime, startLocation.X, startLocation.Y, endLocation.X, endLocation.Y);
        }
        public void Move(Easing easing, int startTime, int endTime, double x1, double y1, double x2, double y2)
        {
            _Move.Add(easing, startTime, endTime, x1, y1, x2, y2);
        }

        public void Fade(int startTime, double alpha)
        {
            alpha = CheckAlpha(alpha);
            _Fade.Add(0, startTime, startTime, alpha, alpha);
        }
        public void Fade(int startTime, int endTime, double alpha)
        {
            alpha = CheckAlpha(alpha);
            _Fade.Add(0, startTime, endTime, alpha, alpha);
        }
        public void Fade(Easing easing, int startTime, int endTime, double startAlpha, double endAlpha)
        {
            startAlpha = CheckAlpha(startAlpha);
            endAlpha = CheckAlpha(endAlpha);
            _Fade.Add(easing, startTime, endTime, startAlpha, endAlpha);
        }

        public void Scale(int startTime, double scale)
        {
            _Scale.Add(0, startTime, startTime, scale, scale);
        }
        public void Scale(int startTime, int endTime, double scale)
        {
            _Scale.Add(0, startTime, endTime, scale, scale);
        }
        public void Scale(Easing easing, int startTime, int endTime, double startScale, double endScale)
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
        public void Rotate(Easing easing, int startTime, int endTime, double startRotate, double endRotate)
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
        public void MoveX(Easing easing, int startTime, int endTime, double startX, double endX)
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
        public void MoveY(Easing easing, int startTime, int endTime, double startY, double endY)
        {
            _MoveY.Add(easing, startTime, endTime, startY, endY);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(type + "," + layer + "," + origin + "," + "\"" + imagePath + "\"," + defaultX + "," + defaultY);

            if (FrameCount != null)
            {
                sb.Append("," + FrameCount + "," + frameRate + "," + loopType);
            }

            sb.AppendLine();
            for (int i = 1; i <= _Move.Count; i++) sb.AppendLine(_Move[i - 1].ToString());
            for (int i = 1; i <= _Scale.Count; i++) sb.AppendLine(_Scale[i - 1].ToString());
            for (int i = 1; i <= _Fade.Count; i++) sb.AppendLine(_Fade[i - 1].ToString());
            for (int i = 1; i <= _Rotate.Count; i++) sb.AppendLine(_Rotate[i - 1].ToString());
            for (int i = 1; i <= Vector.Count; i++) sb.AppendLine(Vector[i - 1].ToString());
            for (int i = 1; i <= _Color.Count; i++) sb.AppendLine(_Color[i - 1].ToString());
            for (int i = 1; i <= _MoveX.Count; i++) sb.AppendLine(_MoveX[i - 1].ToString());
            for (int i = 1; i <= _MoveY.Count; i++) sb.AppendLine(_MoveY[i - 1].ToString());
            for (int i = 1; i <= Parameter.Count; i++) sb.AppendLine(Parameter[i - 1].ToString());
            for (int i = 1; i <= Loop.Count; i++)
            {
                sb.AppendLine(Loop[i - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Move.Count; j++) sb.AppendLine(Loop[i - 1].Move[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Scale.Count; j++) sb.AppendLine(Loop[i - 1].Scale[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Fade.Count; j++) sb.AppendLine(Loop[i - 1].Fade[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Rotate.Count; j++) sb.AppendLine(Loop[i - 1].Rotate[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Vector.Count; j++) sb.AppendLine(Loop[i - 1].Vector[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Color.Count; j++) sb.AppendLine(Loop[i - 1].Color[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].MoveX.Count; j++) sb.AppendLine(Loop[i - 1].MoveX[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].MoveY.Count; j++) sb.AppendLine(Loop[i - 1].MoveY[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Parameter.Count; j++) sb.AppendLine(Loop[i - 1].Parameter[j - 1].ToString());
            }
            for (int i = 1; i <= Trigger.Count; i++)
            {
                sb.AppendLine(Trigger[i - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Move.Count; j++) sb.AppendLine(Trigger[i - 1].Move[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Scale.Count; j++) sb.AppendLine(Trigger[i - 1].Scale[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Fade.Count; j++) sb.AppendLine(Trigger[i - 1].Fade[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Rotate.Count; j++) sb.AppendLine(Trigger[i - 1].Rotate[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Vector.Count; j++) sb.AppendLine(Trigger[i - 1].Vector[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Color.Count; j++) sb.AppendLine(Trigger[i - 1].Color[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].MoveX.Count; j++) sb.AppendLine(Trigger[i - 1].MoveX[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].MoveY.Count; j++) sb.AppendLine(Trigger[i - 1].MoveY[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Parameter.Count; j++) sb.AppendLine(Trigger[i - 1].Parameter[j - 1].ToString());

            }
            return sb.ToString();
        }
    }

}
