using System;

namespace OSharp.Storyboard.Events
{
    public interface IEvent
    {
        EventType EventType { get; }
        EasingType Easing { get; set; }
        float StartTime { get; }
        float EndTime { get; }
        float[] Start { get; }
        float[] End { get; }
        int ParamLength { get; }
        bool IsStatic { get; }
    }
}
