using System;
using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Trigger : Element
    {
        public int StartTime { get; internal set; }
        public int EndTime { get; internal set; }
        public string TriggerType { get; internal set; }

        internal Trigger(int startTime, int endTime, TriggerType[] triggerType, int customSampleSet = -1)
        {
            const string hitSound = "HitSound";
            string sampleSet = "", additionsSampleSet = "", addition = "", custom = "";
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
            IsLorT = true;
        }
        internal Trigger(int startTime, int endTime, string triggerName)
        {
            StartTime = startTime;
            EndTime = endTime;
            TriggerType = triggerName;
            IsLorT = true;
        }

        public override string ToString() => string.Join(",", " T", TriggerType, StartTime, EndTime) + "\r\n" + base.ToString();
    }
}
