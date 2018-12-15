using System;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb.Commands
{
    public class HitSoundTriggerCommand : TriggerCommand
    {
        public HitSoundSampleSetType SampleSet;
        public HitSoundSampleSetType AdditionalSampleSet;
        public HitSoundAdditionSampeType Addition;
        public int CustomSampleSet;
        public bool IsCustomSampleSet { get; private set; }
        public HitSoundTriggerCommand(Vector2<int> time,
            HitSoundSampleSetType sampleSet,
            HitSoundSampleSetType addSampleSet = HitSoundSampleSetType.None,
            HitSoundAdditionSampeType addition = HitSoundAdditionSampeType.None,
            int customSampleSet = -1
            ) : base(TriggerSourceType.HitSound, time)
        {
            if (customSampleSet > -1)
            {
                IsCustomSampleSet = true;
                CustomSampleSet = -1;
            }
            else
            {
                CustomSampleSet = customSampleSet;
            }
            SampleSet = sampleSet;
            AdditionalSampleSet = addSampleSet;
            Addition = addition;
        }
    }
}
