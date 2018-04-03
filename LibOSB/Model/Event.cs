using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB.Model.Constants;

namespace LibOSB
{
    /// <summary>
    /// Parent class of all actions. Should not be instantiated directly.
    /// </summary>
    public abstract class Event
    {
        public int StartTime { get; internal set; }
        public int EndTime { get; internal set; }
        public EasingType Easing { get; protected set; }
        public string Type { get; protected set; }
        public string ScriptParams { get; protected set; }
        
        internal abstract void BuildParams();

        public override string ToString()
        {
            string end_time = StartTime == EndTime ? "" : EndTime.ToString();
            return string.Join(",", Type, (int)Enum.Parse(typeof(EasingType), Easing.ToString()), StartTime, end_time, ScriptParams);
        }

        internal void _AdjustTime(int time)
        {
            StartTime += time;
            EndTime += time;
        }
    }
}
