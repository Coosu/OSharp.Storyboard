using LibOSB.ActionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB.Constants;

namespace LibOSB
{
    /// <summary>
    /// Base class of storyboard objects.
    /// </summary>
    [Serializable]
    public class Element
    {
        StringBuilder sb = new StringBuilder();

        private Types type;
        private Layers layer;
        private Origins origin;
        private string filepath;
        private double x, y;
        private double? framecount, framerate;
        private LoopType looptype;

        #region 一堆属性
        /// <summary>
        /// 获取物件的种类。
        /// </summary>
        public Types Type { get => type; }
        /// <summary>
        /// 获取物件的图层。
        /// </summary>
        public Layers Layer { get => layer; }
        /// <summary>
        /// 获取物件的原点位置。
        /// </summary>
        public Origins Origin { get => origin; }
        /// <summary>
        /// 获取物件的相对路径。
        /// </summary>
        public string FilePath { get => filepath; }
        /// <summary>
        /// 获取物件的默认y坐标。
        /// </summary>
        public double Y { get => y; set => y = value; }
        /// <summary>
        /// 获取物件的默认x坐标。
        /// </summary>
        public double X { get => x; set => x = value; }
        public LoopType Looptype { get => looptype; }
        public double? Framecount { get => framecount; }
        public double? Framerate { get => framerate; }
        #endregion

        private int? tmpMaxTime;
        private int? tmpMinTime;
        public int? TmpMaxTime { get => tmpMaxTime; }
        public int? TmpMinTime { get => tmpMinTime; }

        /// <summary>
        /// 获取当前所有动作的最大时间。
        /// </summary>
        public int MaxTime { get => maxTime; }
        private int maxTime;
        /// <summary>
        /// 获取当前所有动作的最小时间。
        /// </summary>
        public int MinTime { get => minTime; }
        private int minTime;

        public override string ToString()
        {
            sb.Clear();
            sb.Append(Type); sb.Append(",");
            sb.Append(Layer); sb.Append(",");
            sb.Append(Origin); sb.Append(",");
            sb.Append("\""); sb.Append(FilePath); sb.Append("\"");
            sb.Append(",");
            sb.Append(X); sb.Append(",");
            sb.Append(Y.ToString());
            if (Framecount != null)
            {
                sb.Append(",");
                sb.Append(Framecount); sb.Append(",");
                sb.Append(Framerate); sb.Append(",");
                sb.Append(Looptype);
            }
            sb.AppendLine();
            for (int i = 1; i <= _Move.Count; i++) sb.AppendLine(_Move[i - 1].ToString());
            for (int i = 1; i <= Scale.Count; i++) sb.AppendLine(Scale[i - 1].ToString());
            for (int i = 1; i <= _Fade.Count; i++) sb.AppendLine(_Fade[i - 1].ToString());
            for (int i = 1; i <= Rotate.Count; i++) sb.AppendLine(Rotate[i - 1].ToString());
            for (int i = 1; i <= Vector.Count; i++) sb.AppendLine(Vector[i - 1].ToString());
            for (int i = 1; i <= _Color.Count; i++) sb.AppendLine(_Color[i - 1].ToString());
            for (int i = 1; i <= MoveX.Count; i++) sb.AppendLine(MoveX[i - 1].ToString());
            for (int i = 1; i <= MoveY.Count; i++) sb.AppendLine(MoveY[i - 1].ToString());
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
        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        public Element(Types Type, Layers Layer, Origins Origin, string FilePath, double X, double Y)
        {
            type = Type;
            layer = Layer;
            origin = Origin;

            filepath = FilePath;
            x = X;
            y = Y;
        }
        /// <summary>
        /// Create a storyboard element by a dynamic image.
        /// </summary>
        public Element(Types Type, Layers Layer, Origins Origin, string FilePath, double X, double Y, double FrameCount, double FrameRate, LoopType LoopType)
        {
            type = Type;
            layer = Layer;
            origin = Origin;

            filepath = FilePath;
            x = X;
            y = Y;

            framecount = FrameCount;
            framerate = FrameRate;
            looptype = LoopType;
        }
        /// <summary>
        /// An action that controls the object to move. 
        /// </summary>
        public Move _Move { get; } = new Move();
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
        /// <summary>
        /// An action that controls the object to zoom. 
        /// </summary>
        public Scale Scale { set; get; } = new Scale();
        /// <summary>
        /// An action that controls the object to change the transparency. 
        /// </summary>
        private Fade _Fade { set; get; } = new Fade();
        public void Fade(int startTime, double alpha)
        {
            if (alpha < 0 || alpha > 1)
                throw new Exception("Value of alpha should be between 0 and 1");
            _Fade.Add(0, startTime, startTime, alpha, alpha);
        }
        public void Fade(int startTime, int endTime, double alpha)
        {
            _Fade.Add(0, startTime, endTime, alpha, alpha);
        }
        public void Fade(Easing easing, int startTime, int endTime, double startAlpha, double endAlpha)
        {
            _Fade.Add(easing, startTime, endTime, startAlpha, endAlpha);
        }
        /// <summary>
        /// An action that controls the object to change the degree. 
        /// </summary>
        public Rotate Rotate { set; get; } = new Rotate();
        /// <summary>
        /// An action that controls the object to zoom the width and height dividually. 
        /// </summary>
        public Vector Vector { set; get; } = new Vector();
        /// <summary>
        /// An action that controls the object to have addtional color. 
        /// </summary>
        public Color _Color { set; get; } = new Color();
        public MoveX MoveX { set; get; } = new MoveX();
        public MoveY MoveY { set; get; } = new MoveY();
        public Parameter Parameter { set; get; } = new Parameter();
        public Loop Loop { set; get; } = new Loop();
        public Trigger Trigger { set; get; } = new Trigger();
    }

}
