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
    public abstract class Actions
    {
        protected StringBuilder sb = new StringBuilder();

        public int? indexL, indexT;
        protected string kg = " ";
        protected string @params;
        protected int? endTime = null;
        protected Easing easing;
        protected int? startTime = null;

        protected string type;

        public List<int?> starttime_L = new List<int?>();
        public List<int?> endtime_L = new List<int?>();
        
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
        public Easing Easing { get => easing; set => easing = value; }
        /// <summary>
        /// Get one action's end type.
        /// </summary>
        public string Type { get => type; set => type = value; }

        public new string ToString()
        {
            sb = new StringBuilder();
            if (indexL != null)
                kg = "  ";
            else if (indexT != null)
                kg = "  ";
            else kg = " ";
            sb.Append(kg);
            sb.Append(Type);
            sb.Append(",");
            sb.Append((int)Enum.Parse(typeof(Easing), Easing.ToString()));
            sb.Append(",");
            sb.Append(StartTime);
            sb.Append(",");

            if (EndTime > StartTime)
            {
                sb.Append(EndTime);
            }
            else if (EndTime < StartTime)
            {
                throw new Exception("End time should be bigger than start time, or error will be occurs while playing.");
            }
            if (@params != null)
            {
                sb.Append(",");
                sb.Append(@params);
            }
            return sb.ToString();
        }


        /// <summary>
        /// Get number of one action.
        /// </summary>
        public int Count { get => starttime_L.Count(); }

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

                if (starttime_L.Count < 1) return null;
                if (starttime_L.Max() > endtime_L.Max())
                {
                    TmpMaxTime = starttime_L.Max();
                    return starttime_L.Max();
                }
                else
                {
                    TmpMaxTime = endtime_L.Max();
                    return endtime_L.Max();
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

                if (starttime_L.Count < 1) return null;
                if (starttime_L.Min() < endtime_L.Min())
                {
                    TmpMinTime = starttime_L.Min();
                    return starttime_L.Min();
                }
                else
                {
                    TmpMinTime = endtime_L.Min();
                    return endtime_L.Min();
                }
            }
        }


    }
}
