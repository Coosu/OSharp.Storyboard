using System.Collections.Generic;

namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
{
    public class LoopCommand : Command
    {
        public int StartTime, LoopCount;
        public IEnumerable<Command> Commands;
        public LoopCommand(int startTime, int loopCount, IEnumerable<Command> commands) : base(CommandType.Loop, EasingType.Linear, new Vector2<int> { X = startTime })
        {
            Commands = commands;
            StartTime = startTime;
            LoopCount = loopCount;
        }
    }
}
