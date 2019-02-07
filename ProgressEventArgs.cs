using System;

namespace OSharp.Storyboard
{
    public class ProgressEventArgs : EventArgs
    {
        public int Progress { get; set; }
        public int TotalCount { get; set; }
    }
}