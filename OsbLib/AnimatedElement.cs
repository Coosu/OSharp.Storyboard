using Milkitic.OsbLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkitic.OsbLib
{
    public class AnimatedElement : Element
    {
        public float FrameCount { get; set; }
        public float FrameRate { get; set; }
        public LoopType LoopType { get; set; }

        /// <summary>
        /// Create a storyboard element by a dynamic image.
        /// </summary>
        /// <param name="type">Set element type.</param>
        /// <param name="layer">Set element layer.</param>
        /// <param name="origin">Set element origin.</param>
        /// <param name="imagePath">Set image path.</param>
        /// <param name="defaultX">Set default x-coordinate of location.</param>
        /// <param name="defaultY">Set default x-coordinate of location.</param>
        /// <param name="frameCount">Set frame count.</param>
        /// <param name="frameRate">Set frame rate (frame delay).</param>
        /// <param name="loopType">Set loop type.</param>
        public AnimatedElement(ElementType type, LayerType layer, OriginType origin, string imagePath, float defaultX,
            float defaultY, float frameCount, float frameRate, LoopType loopType) : base(type, layer, origin, imagePath, defaultX, defaultY)
        {
            FrameCount = frameCount;
            FrameRate = frameRate;
            LoopType = loopType;
        }
        public AnimatedElement(string type, string layer, string origin, string imagePath, float defaultX,
            float defaultY, float frameCount, float frameRate, string loopType) : base(type, layer, origin, imagePath, defaultX, defaultY)
        {
            FrameCount = frameCount;
            FrameRate = frameRate;
            LoopType = (LoopType)Enum.Parse(typeof(LoopType), loopType);
        }

        public override string ToString()
        {
            var head =
                $"{string.Join(",", Type, Layer, Origin, $"\"{ImagePath}\"", DefaultX, DefaultY, FrameCount, FrameRate, LoopType)}\r\n";
            return head + GetStringBody();
        }
    }
}
