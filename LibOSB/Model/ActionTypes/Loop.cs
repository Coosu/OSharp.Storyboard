using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    class Loop : Actions
    {
        public Loop this[int index]
        {
            get
            {
                return L[index];
            }
        }
        public Loop(int starttime, int times, int i = -1)
        {
            type = "L";
            indexL = i;
            this.startTime = starttime;
            this.times = times;

            Move = new Move(); if (i != -1) Move.indexL = i;
            MoveX = new MoveX(); if (i != -1) MoveX.indexL = i;
            MoveY = new MoveY(); if (i != -1) MoveY.indexL = i;
            Scale = new Scale(); if (i != -1) Scale.indexL = i;
            Fade = new Fade(); if (i != -1) Fade.indexL = i;
            Rotate = new Rotate(); if (i != -1) Rotate.indexL = i;
            Vector = new Vector(); if (i != -1) Vector.indexL = i;
            Color = new Color(); if (i != -1) Color.indexL = i;
            Parameter = new Parameter(); if (i != -1) Parameter.indexL = i;
            BuildParams();
        }

        new public string ToString()
        {
            sb = new StringBuilder();
            kg = " ";
            sb.Append(kg);
            sb.Append(Type);
            sb.Append(",");

            sb.Append(StartTime);

            if (@params != null)
            {
                sb.Append(",");
                sb.Append(@params);
            }
            return sb.ToString();
        }
        private void BuildParams()
        {
            @params = times.ToString();

        }

        public Loop() { }

        private List<Loop> L = new List<Loop>();
        private int times;
        public int Times { get => times; }
        public void Add(int StartTime, int Times)
        {
            int Lindex = L.Count;
            L.Add(new Loop(StartTime, Times, Lindex));
            starttime_L.Add(StartTime);
        }
        private List<int?> max = new List<int?>();
        private List<int?> min = new List<int?>();

        new public int? MaxTime()
        {
            if (TmpMaxTime != null) return TmpMaxTime; //缓存

            max.Clear();
            //Debug.WriteLine(L);
            if (Scale.TmpMaxTime != null) max.Add(Scale.TmpMaxTime);
            else if (Scale.MaxTime() != null) max.Add(Scale.MaxTime());

            if (Move.TmpMaxTime != null) max.Add(Move.TmpMaxTime);
            else if (Move.MaxTime() != null) max.Add(Move.MaxTime());

            if (Fade.TmpMaxTime != null) max.Add(Fade.TmpMaxTime);
            else if (Fade.MaxTime() != null) max.Add(Fade.MaxTime());

            if (Rotate.TmpMaxTime != null) max.Add(Rotate.TmpMaxTime);
            else if (Rotate.MaxTime() != null) max.Add(Rotate.MaxTime());

            if (Vector.TmpMaxTime != null) max.Add(Vector.TmpMaxTime);
            else if (Vector.MaxTime() != null) max.Add(Vector.MaxTime());

            if (Color.TmpMaxTime != null) max.Add(Color.TmpMaxTime);
            else if (Color.MaxTime() != null) max.Add(Color.MaxTime());

            if (MoveX.TmpMaxTime != null) max.Add(MoveX.TmpMaxTime);
            else if (MoveX.MaxTime() != null) max.Add(MoveX.MaxTime());

            if (MoveY.TmpMaxTime != null) max.Add(MoveY.TmpMaxTime);
            else if (MoveY.MaxTime() != null) max.Add(MoveY.MaxTime());

            if (Parameter.TmpMaxTime != null) max.Add(Parameter.TmpMaxTime);
            else if (Parameter.MaxTime() != null) max.Add(Parameter.MaxTime());

            if (max.Count < 1) return null;

            TmpMaxTime = max.Max();
            //TmpMaxTime = StartTime + (max.Max() * Times);
            return TmpMaxTime;
        }
        new public int? MinTime()
        {
            //get
            {
                if (TmpMinTime != null) return TmpMinTime; //缓存

                min.Clear();
                if (Scale.TmpMinTime != null) min.Add(Scale.TmpMinTime);
                else if (Scale.MinTime() != null) min.Add(Scale.MinTime());

                if (Move.TmpMinTime != null) min.Add(Move.TmpMinTime);
                else if (Move.MinTime() != null) min.Add(Move.MinTime());

                if (Fade.TmpMinTime != null) min.Add(Fade.TmpMinTime);
                else if (Fade.MinTime() != null) min.Add(Fade.MinTime());

                if (Rotate.TmpMinTime != null) min.Add(Rotate.TmpMinTime);
                else if (Rotate.MinTime() != null) min.Add(Rotate.MinTime());

                if (Vector.TmpMinTime != null) min.Add(Vector.TmpMinTime);
                else if (Vector.MinTime() != null) min.Add(Vector.MinTime());

                if (Color.TmpMinTime != null) min.Add(Color.TmpMinTime);
                else if (Color.MinTime() != null) min.Add(Color.MinTime());

                if (MoveX.TmpMinTime != null) min.Add(MoveX.TmpMinTime);
                else if (MoveX.MinTime() != null) min.Add(MoveX.MinTime());

                if (MoveY.TmpMinTime != null) min.Add(MoveY.TmpMinTime);
                else if (MoveY.MinTime() != null) min.Add(MoveY.MinTime());

                if (Parameter.TmpMinTime != null) min.Add(Parameter.TmpMinTime);
                else if (Parameter.MinTime() != null) min.Add(Parameter.MinTime());

                if (min.Count < 1) return null;

                //TmpMaxTime = max.Max();
                //TmpMaxTime = StartTime + (max.Max() * Times);
                TmpMinTime = min.Min();
                //TmpMaxTime = StartTime + (max.Max() * Times);
                return min.Min();
            }
        }

        public bool TwoMin
        {
            get
            {
                List<int?> min1 = min.FindAll(
                    delegate (int? x)
                    {
                        return x == min.Min();
                    }
                    );
                if (min1.Count > 1) return true;
                else return false;
            }
        }
        public bool TwoMax
        {
            get
            {
                List<int?> max1 = max.FindAll(
                    delegate (int? x)
                    {
                        return x == max.Max();
                    }
                    );
                if (max1.Count > 1) return true;
                else return false;
            }
        }
        /// <summary>
        /// An action that controls the object to move. 
        /// </summary>
        public Move Move { set; get; }
        /// <summary>
        /// An action that controls the object to zoom. 
        /// </summary>
        public Scale Scale { set; get; }
        /// <summary>
        /// An action that controls the object to change the transparency. 
        /// </summary>
        public Fade Fade { set; get; }
        /// <summary>
        /// An action that controls the object to change the degree. 
        /// </summary>
        public Rotate Rotate { set; get; }
        /// <summary>
        /// An action that controls the object to zoom the width and height dividually. 
        /// </summary>
        public Vector Vector { set; get; }
        /// <summary>
        /// An action that controls the object to have addtional color. 
        /// </summary>
        public Color Color { set; get; }
        public MoveX MoveX { set; get; }
        public MoveY MoveY { set; get; }
        public Parameter Parameter { set; get; }
    }
}
