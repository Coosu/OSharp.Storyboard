using System;
using System.Collections.Generic;
using System.Text;

namespace OSharp.Storyboard.Events
{
    public interface IAdjustablePositionEvent
    {
        void AdjustPosition(float x, float y);
    }
}
