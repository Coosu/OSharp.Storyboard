using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB.Constants;

namespace LibOSB
{
    /// <summary>
    /// Parent class of all actions. Should not be instantiated directly.
    /// </summary>
    public abstract class Action
    {
        protected StringBuilder sb = new StringBuilder();

        public int? indexL, indexT;
        protected string kg = "";
        protected string scriptParams;
        protected int? endTime = null;
        protected EasingType easing;
        protected int? startTime = null;
        protected string type;

        public List<int?> startTime_L = new List<int?>();
        public List<int?> endTime_L = new List<int?>();

        /// <summary>
        /// Get one action's start time.
        /// </summary>
        public int? StartTime { get => startTime; set => startTime = value; }
        /// <summary>
        /// Get one action's end time.
        /// </summary>
        public int? EndTime { get => endTime; set => endTime = value; }
        /// <summary>
        /// Get one action's easing.
        /// </summary>
        public EasingType Easing { get => easing; set => easing = value; }
        /// <summary>
        /// Get one action's end type.
        /// </summary>
        public string Type { get => type; set => type = value; }

        public new string ToString()
        {
            sb = new StringBuilder();
            //if (indexL != null)
            //    kg = "  ";
            //else if (indexT != null)
            //    kg = "  ";
            //else kg = " ";
            sb.Append(kg + Type + "," + (int)Enum.Parse(typeof(EasingType), Easing.ToString()) + "," + StartTime + ",");

            if (EndTime > StartTime)
            {
                sb.Append(EndTime);
            }
            else if (EndTime < StartTime)
            {
                throw new Exception("End time should be bigger than start time, or error will be occurs while playing.");
            }
            if (scriptParams != null)
            {
                sb.Append(",");
                sb.Append(scriptParams);
            }
            return sb.ToString();
        }


        /// <summary>
        /// Get number of one action.
        /// </summary>
        public int Count { get => startTime_L.Count(); }

        private int? tmpMaxTime;
        private int? tmpMinTime;
        public int? TmpMaxTime { get => tmpMaxTime; set => tmpMaxTime = value; }
        public int? TmpMinTime { get => tmpMinTime; set => tmpMinTime = value; }

        public void ToNull()
        {
            TmpMaxTime = null; TmpMinTime = null;
        }
        /// <summary>
        /// Get max time of one action.
        /// </summary>
        public int? MaxTime()
        {
            {
                if (TmpMaxTime != null) return TmpMaxTime; //缓存

                if (startTime_L.Count < 1) return null;
                if (startTime_L.Max() > endTime_L.Max())
                {
                    TmpMaxTime = startTime_L.Max();
                    return startTime_L.Max();
                }
                else
                {
                    TmpMaxTime = endTime_L.Max();
                    return endTime_L.Max();
                }
            }
        }
        /// <summary>
        /// Get min time of one action.
        /// </summary>
        public int? MinTime()
        {
            {
                if (TmpMinTime != null) return TmpMinTime; //缓存

                if (startTime_L.Count < 1) return null;
                if (startTime_L.Min() < endTime_L.Min())
                {
                    TmpMinTime = startTime_L.Min();
                    return startTime_L.Min();
                }
                else
                {
                    TmpMinTime = endTime_L.Min();
                    return endTime_L.Min();
                }
            }
        }


    }
}
