using OSharp.Storyboard.Events;
using OSharp.Storyboard.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSharp.Storyboard
{
    public abstract class EventContainer
    {
        public SortedSet<Event> EventList { get; } = new SortedSet<Event>(new EventComparer());
        //public List<Event> EventList { get; } = new List<Event>();
        public abstract float MaxTime { get; }
        public abstract float MinTime { get; }
        public abstract float MaxStartTime { get; }
        public abstract float MinEndTime { get; }

        public virtual void Adjust(float offsetX, float offsetY, int offsetTiming)
        {
            var events = EventList.GroupBy(k => k.EventType);
            foreach (var kv in events)
            {
                foreach (var e in kv)
                {
                    if (e is IAdjustablePositionEvent adjustable)
                        adjustable.AdjustPosition(offsetX, offsetY);

                    e.AdjustTiming(offsetTiming);
                }
            }
        }

        internal virtual void AddEvent(EventType e, EasingType easing, float startTime, float endTime, float[] start, float[] end)
        {
            Event newEvent;
            if (end == null || end.Length == 0)
                end = start;

            switch (e)
            {
                case EventType.Fade:
                    newEvent = new Fade(easing, startTime, endTime, start[0], end[0]);
                    break;
                case EventType.Move:
                    newEvent = new Move(easing, startTime, endTime, start[0], start[1], end[0], end[1]);
                    break;
                case EventType.MoveX:
                    newEvent = new MoveX(easing, startTime, endTime, start[0], end[0]);
                    break;
                case EventType.MoveY:
                    newEvent = new MoveY(easing, startTime, endTime, start[0], end[0]);
                    break;
                case EventType.Scale:
                    newEvent = new Scale(easing, startTime, endTime, start[0], end[0]);
                    break;
                case EventType.Vector:
                    newEvent = new Vector(easing, startTime, endTime, start[0], start[1], end[0], end[1]);
                    break;
                case EventType.Rotate:
                    newEvent = new Rotate(easing, startTime, endTime, start[0], end[0]);
                    break;
                case EventType.Color:
                    newEvent = new Color(easing, startTime, endTime, start[0], start[1], start[2], end[0], end[1], end[2]);
                    break;
                case EventType.Parameter:
                    newEvent = new Parameter(easing, startTime, endTime, (ParameterType)(int)start[0]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }

            //List
            //if (sequential)
            //    EventList.AddSorted(newEvent);
            //else
            //    EventList.Add(newEvent);

            //SortedSet
            EventList.Add(newEvent);
        }
    }
}
