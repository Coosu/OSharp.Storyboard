using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB.Constants;
namespace LibOSB.ActionTypes
{
    class Move : Actions
    {
        /// <summary>
        /// 访问已存储的Move元素。
        /// </summary>
        /// <param name="index">索引。访问第n个Move元素。</param>
        /// <returns></returns>
        public Move this[int index] { get => M[index]; set => M[index] = value; }

        public Move() { }

        public void Remove(int index)
        {
            M.Remove(M[index]);
            starttime_L.RemoveAt(index);
            endtime_L.RemoveAt(index);
        }

        public Move(Easing easing, int starttime, int endtime,
         double X1, double Y1, double X2, double Y2, int? i, int? j)
        {
            type = "M";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.x1 = X1;
            this.x2 = X2;
            this.y1 = Y1;
            this.y2 = Y2;
            indexL = i;
            indexT = j;
            BuildParams();
        }
        private void BuildParams()
        {
            if (x1 == x2 && y1 == y2) @params = x1 + "," + y1;
            else @params = x1 + "," + y1 + "," + x2 + "," + y2;
        }

        public List<Move> M = new List<Move>();
        private double x1, y1, x2, y2;

        /// <summary>
        /// 获取对应Move元素的首x坐标。
        /// </summary>
        public double X1 { get => x1; set => x1 = value; }
        /// <summary>
        /// 获取对应Move元素的首y坐标。
        /// </summary>
        public double Y1 { get => y1; set => y1 = value; }
        /// <summary>
        /// 获取对应Move元素的末x坐标。
        /// </summary>
        public double X2 { get => x2; set => x2 = value; }
        /// <summary>
        /// 获取对应Move元素的末y坐标。
        /// </summary>
        public double Y2 { get => y2; set => y2 = value; }
        /// <summary>
        /// 添加一个Move动作。
        /// </summary>
        /// <param name="Easing"></param>
        /// <param name="StartTime">动作的开始时间。</param>
        /// <param name="EndTime">动作的结束时间。</param>
        /// <param name="Location_X1">动作的首x坐标</param>
        /// <param name="Location_Y1">动作的首y坐标<</param>
        /// <param name="Location_X2">动作的末x坐标<</param>
        /// <param name="Location_Y2">动作的末y坐标<</param>
        public void Add(Easing Easing, int StartTime, int EndTime,
         double Location_X1, double Location_Y1,
         double Location_X2, double Location_Y2)
        {

            //Console.WriteLine();
            M.Add(new Move(Easing, StartTime, EndTime, Location_X1, Location_Y1, Location_X2, Location_Y2, indexL, indexT));

            //checkTwoMinMax(StartTime, EndTime);

            starttime_L.Add(StartTime);
            endtime_L.Add(EndTime);
        }
    }
}
