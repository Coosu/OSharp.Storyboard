using System;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb.Commands
{
    public class PassTriggerCommand : TriggerCommand
    {
        public PassTriggerCommand(Vector2<int> time) : base(TriggerSourceType.Passing, time)
        {
        }
    }
}
