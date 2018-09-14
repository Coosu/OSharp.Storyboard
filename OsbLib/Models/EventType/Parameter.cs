using Milkitic.OsbLib.Enums;

namespace Milkitic.OsbLib.Models.EventType
{
    public class Parameter : Event
    {
        public ParameterEnum Type => (ParameterEnum)(int)Start[0];

        public Parameter(EasingType easing, float startTime, float endTime, ParameterEnum type)
            : base(easing, startTime, endTime, new[] { (float)(int)type }, new[] { (float)(int)type }) => 
            EventType = EventEnum.Parameter;
    }
}
