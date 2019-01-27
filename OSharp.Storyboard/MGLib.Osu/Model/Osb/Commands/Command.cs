namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands
{
    public class Command
    {
        public CommandType CommandType;
        public EasingType Easing;
        public Vector2<int> Time;

        public Command(CommandType type, EasingType easing, Vector2<int> time)
        {
            CommandType = type;
            Easing = easing;
            Time = time;
        }
        
    }
}
