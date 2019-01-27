using System;

namespace OSharp.Storyboard
{
    public sealed class AnimatedElement : Element
    {
        public int FrameCount { get; set; }
        public float FrameDelay { get; set; }
        public LoopType LoopType { get; set; }

        /// <summary>
        /// Create a storyboard element by dynamic images.
        /// </summary>
        /// <param name="type">Set element type.</param>
        /// <param name="layer">Set element layer.</param>
        /// <param name="origin">Set element origin.</param>
        /// <param name="imagePath">Set image path.</param>
        /// <param name="defaultX">Set default x-coordinate of location.</param>
        /// <param name="defaultY">Set default x-coordinate of location.</param>
        /// <param name="frameCount">Set frame count.</param>
        /// <param name="frameDelay">Set frame rate (frame delay).</param>
        /// <param name="loopType">Set loop type.</param>
        public AnimatedElement(ElementType type, LayerType layer, OriginType origin, string imagePath, float defaultX,
            float defaultY, int frameCount, float frameDelay, LoopType loopType) : base(type, layer, origin, imagePath, defaultX, defaultY)
        {
            FrameCount = frameCount;
            FrameDelay = frameDelay;
            LoopType = loopType;
        }

        /// <summary>
        /// Create a storyboard element by dynamic images.
        /// </summary>
        /// <param name="type">Set element type.</param>
        /// <param name="layer">Set element layer.</param>
        /// <param name="origin">Set element origin.</param>
        /// <param name="imagePath">Set image path.</param>
        /// <param name="defaultX">Set default x-coordinate of location.</param>
        /// <param name="defaultY">Set default x-coordinate of location.</param>
        /// <param name="frameCount">Set frame count.</param>
        /// <param name="frameDelay">Set frame rate (frame delay).</param>
        /// <param name="loopType">Set loop type.</param>
        public AnimatedElement(string type, string layer, string origin, string imagePath, float defaultX,
            float defaultY, int frameCount, float frameDelay, string loopType) : base(type, layer, origin, imagePath, defaultX, defaultY)
        {
            FrameCount = frameCount;
            FrameDelay = frameDelay;
            LoopType = (LoopType)Enum.Parse(typeof(LoopType), loopType);
        }

        public override string ToString()
        {
            var head =
                $"{string.Join(",", Type, Layer, Origin, $"\"{ImagePath}\"", DefaultX, DefaultY, FrameCount, FrameDelay, LoopType)}\r\n";
            return head + GetStringBody();
        }
    }
}
