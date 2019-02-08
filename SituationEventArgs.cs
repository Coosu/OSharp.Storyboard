using System;
using System.Collections.Generic;
using OSharp.Storyboard.Events;

namespace OSharp.Storyboard
{
    public class SituationEventArgs : EventArgs
    {
        public Element Element { get; set; }
        public List<CommonEvent> Events { get; set; }
        public string Message { get; set; }
        public bool Continue { get; set; } = true;
    }
}