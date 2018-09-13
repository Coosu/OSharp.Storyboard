using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models.EventClass;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Parameter : Event
    {
        public string PType { get; }

        public override string ScriptParams => PType;
        public override bool IsStatic => true;

        public Parameter(EasingType easing, float starttime, float endtime, string pType)
        {
            Type = "P";
            Easing = easing;
            StartTime = starttime;
            EndTime = endtime;
            PType = pType;
        }
    }
}
