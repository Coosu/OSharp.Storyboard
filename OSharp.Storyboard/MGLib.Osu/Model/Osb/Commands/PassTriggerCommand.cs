namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
{
    public class PassTriggerCommand : TriggerCommand
    {
        public PassTriggerCommand(Vector2<int> time) : base(TriggerSourceType.Passing, time)
        {
        }
    }
}
