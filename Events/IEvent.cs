using System;

namespace OSharp.Storyboard.Events
{
    public interface IEvent
    {
        EventType EventType { get; }
        EasingType Easing { get; set; }
        float StartTime { get; }
        float EndTime { get; }
        object RawStart { get; }
        object RawEnd { get; }
        int ParamLength { get; }
        bool IsStatic { get; }
    }
}
