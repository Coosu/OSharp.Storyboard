using System;
using System.Collections.Generic;
using System.Linq;
using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models
{
    public class Trigger : EventContainer
    {
        public override List<IEvent> EventList { get; set; } = new List<IEvent>();
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string TriggerType { get; set; }

        public override float MaxTime => EndTime + EventList.Count > 0 ? EventList.Max(k => k.EndTime) : 0;
        public override float MinTime => StartTime;
        public override float MaxStartTime => EndTime + EventList.Count > 0 ? EventList.Max(k => k.StartTime) : 0; //if hitsound played at end time
        public override float MinEndTime => StartTime; // if no hitsound here

        public bool HasFade => EventList.Any(k => k.EventType == EventEnum.Fade);

        public Trigger(int startTime, int endTime, IEnumerable<TriggerType> triggerType, int customSampleSet = -1)
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
        }

        public Trigger(int startTime, int endTime, string triggerName)
        {
            StartTime = startTime;
            EndTime = endTime;
            TriggerType = triggerName;
        }

        public override void Adjust(float offsetX, float offsetY, int offsetTiming)
        {
            StartTime += offsetTiming;
            EndTime += offsetTiming;
            base.Adjust(offsetX, offsetY, offsetTiming);
        }

        public override string ToString() => string.Join(",", " T", TriggerType, StartTime, EndTime) + "\r\n" + base.ToString();
    }
}
