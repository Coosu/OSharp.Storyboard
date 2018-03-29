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
    public abstract class Event
    {
        public int StartTime { get; protected set; }
        public int EndTime { get; protected set; }
        public EasingType Easing { get; protected set; }
        public string Type { get; protected set; }
        public string ScriptParams { get; protected set; }

        public override string ToString()
        {
            string end_time = StartTime == EndTime ? "" : EndTime.ToString();
            return string.Join(",", Type, (int)Enum.Parse(typeof(EasingType), Easing.ToString()), StartTime, end_time, ScriptParams);
        }

        /// <summary>
        /// Get max time of one action.
        /// </summary>
        public int MaxTime()
        {
            return -1;
        }
        /// <summary>
        /// Get min time of one action.
        /// </summary>
        public int MinTime()
        {
            return -1;
        }
    }
}
