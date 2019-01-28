namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
{
    public class FailTriggerCommand : TriggerCommand
    {
        public FailTriggerCommand(Vector2<int> time) : base(TriggerSourceType.Failing, time)
        {
        }
    }
}
