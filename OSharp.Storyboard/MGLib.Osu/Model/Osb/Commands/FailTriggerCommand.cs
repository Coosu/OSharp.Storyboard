using System;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb.Commands
{
    public class FailTriggerCommand : TriggerCommand
    {
        public FailTriggerCommand(Vector2<int> time) : base(TriggerSourceType.Failing, time)
        {
        }
    }
}
