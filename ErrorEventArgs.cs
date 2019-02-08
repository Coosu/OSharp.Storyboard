using System;

namespace OSharp.Storyboard
{
    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; set; }
        public bool Continue { get; set; } = false;
    }
}