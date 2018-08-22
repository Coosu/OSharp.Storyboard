using LibOsb.Enums;
using LibOsb.Models.EventClass;

namespace LibOsb.Models.EventType
{
    public class Parameter : Event
    {
        public string PType { get; }

        public override string ScriptParams => PType;
        public override bool IsStatic => true;

        public Parameter(EasingType easing, int starttime, int endtime, string ptype)
        {
            Type = "P";
            Easing = easing;
            StartTime = starttime;
            EndTime = endtime;
            PType = ptype;
        }
    }
}
