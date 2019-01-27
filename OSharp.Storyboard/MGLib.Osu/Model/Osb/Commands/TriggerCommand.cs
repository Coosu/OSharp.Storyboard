namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
{
    public class TriggerCommand : Command
    {
        public TriggerSourceType TiggerSourceType;
        public TriggerCommand(TriggerSourceType tiggerType, Vector2<int> time) : base(CommandType.Trigger, EasingType.Linear, time)
        {
            TiggerSourceType = tiggerType;
        }
    }
}
