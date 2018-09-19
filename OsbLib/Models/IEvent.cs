using Milkitic.OsbLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkitic.OsbLib.Models
{
    public interface IEvent
    {
        EventEnum EventType { get; set; }
        EasingType Easing { get; set; }
        float StartTime { get; set; }
        float EndTime { get; set; }
        float[] Start { get; set; }
        float[] End { get; set; }
        string Script { get; }

        // 扩展
        int ParamLength { get; }
        bool IsStatic { get; }

        void AdjustTime(float time);
    }
}
