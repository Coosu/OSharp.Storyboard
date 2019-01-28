using System.Collections.Generic;
using OSharp.Storyboard.MGLib.Osu.Model.Osb.Commands;

namespace OSharp.Storyboard.MGLib.Osu.Model.Osb.Images
{
    public abstract class Image
    {
        public ImageType ImageType;
        public Layer Layer;
        public Position Position;
        public string FilePath;
        public int X;
        public int Y;
        public IEnumerable<Command> Commands;

        public Image(ImageType type, Layer layer, Position pos, string filePath, int x, int y)
        {
            ImageType = type;
            Layer = layer;
            Position = pos;
            FilePath = filePath;
            X = x;
            Y = y;
        }

        public void ApplyCommand(Command command)
        {

        }

        public void TestApplyCommand()
        {
            this.ApplyCommand(
                new MoveCommand(
                    EasingType.CubicInOut,
                    new Vector2<int> { X = 1000, Y = 2000 },
                    new Vector2<int>[] {
                        new Vector2<int> { X = 10, Y = 20 },
                        new Vector2<int> { X = 50, Y = 50 },
                        new Vector2<int> { X = 100, Y = 100},
                    }
            ));
        }
    }
}
