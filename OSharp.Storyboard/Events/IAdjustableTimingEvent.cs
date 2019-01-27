using System;
using System.Collections.Generic;
using System.Text;

namespace OSharp.Storyboard.Events
{
    public interface IAdjustableTimingEvent
    {
        void AdjustTiming(float time);
    }
}
