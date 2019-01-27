using System;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb
{
    public class AnimationImage : Image
    {
        public int FrameCount;
        public int FrameDelay;
        public AnimationLoop AnimationLoop;

        public AnimationImage(Layer layer, Position pos, string filePath, int x, int y, int frameCount, int frameDelay, AnimationLoop loopType) : base(ImageType.Animation, layer, pos, filePath, x, y)
        {
            FrameCount = frameCount;
            FrameDelay = frameDelay;
            AnimationLoop = loopType;
        }
    } 
}
