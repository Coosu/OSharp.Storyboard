namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Images
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
