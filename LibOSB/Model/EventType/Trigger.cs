using LibOSB.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOSB.Model.EventType
{
    public class Trigger : Element
    {
        public int StartTime { get; internal set; }
        public int EndTime { get; internal set; }
        public string TriggerType { get; internal set; }

        internal Trigger(int startTime, int endTime, TriggerType[] triggerType, int customSampleSet = -1)
        {
            int length = triggerType.Length;
            string hitSound = "HitSound", sampleSet = "", additionsSampleSet = "", addition = "", custom = "";
            foreach (var item in triggerType)
            {
                var s = item.ToString();
                
                if (sampleSet == "" &&
                    (s.IndexOf("Drum") != -1 || s.IndexOf("Soft") != -1 || s.IndexOf("Normal") != -1))
                {
                    if (s.IndexOf("Drum") != -1) sampleSet = "Drum";
                    else if (s.IndexOf("Soft") != -1) sampleSet = "Soft";
                    else sampleSet = "Normal";
                }
                else if (additionsSampleSet == "" &&
                     (s.IndexOf("Drum") != -1 || s.IndexOf("Soft") != -1 || s.IndexOf("Normal") != -1))
                {
                    if (s.IndexOf("Drum") != -1) additionsSampleSet = "Drum";
                    else if (s.IndexOf("Soft") != -1) additionsSampleSet = "Soft";
                    else additionsSampleSet = "Normal";
                }
                else if (sampleSet != "" &&
                  (s.IndexOf("Drum") != -1 || s.IndexOf("Soft") != -1 || s.IndexOf("Normal") != -1))
                    throw new Exception("SampleSet is conflict.");
                else if (additionsSampleSet != "" &&
                    (s.IndexOf("Drum") != -1 || s.IndexOf("Soft") != -1 || s.IndexOf("Normal") != -1))
                    throw new Exception("AdditionsSampleSet is conflict.");

                if (addition == "" &&
                     (s.IndexOf("Clap") != -1 || s.IndexOf("Finish") != -1 || s.IndexOf("Whistle") != -1))
                {
                    if (s.IndexOf("Clap") != -1) addition = "Clap";
                    else if (s.IndexOf("Finish") != -1) addition = "Finish";
                    else addition = "Whistle";
                }
                else if (addition != "" &&
                    (s.IndexOf("Clap") != -1 || s.IndexOf("Finish") != -1 || s.IndexOf("Whistle") != -1))
                    throw new Exception("Addtion is conflict.");

                if (customSampleSet != -1) custom = customSampleSet.ToString();
            }
            string triggerName = hitSound + sampleSet + additionsSampleSet + addition + custom;

            StartTime = startTime;
            EndTime = endTime;
            TriggerType = triggerName;

            isInnerClass = true;
        }
        internal Trigger(int startTime, int endTime, string triggerType)
        {
            StartTime = startTime;
            EndTime = endTime;
            TriggerType = triggerType;

            isInnerClass = true;
        }

        public override string ToString()
        {
            return string.Join(",", " T", TriggerType, StartTime, EndTime) + "\r\n" + base.ToString();
        }
    }
}
