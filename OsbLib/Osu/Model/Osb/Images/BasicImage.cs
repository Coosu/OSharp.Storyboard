using System;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb
{
    public class BasicImage : Image
    {
        public BasicImage(Layer layer, Position pos, string filePath, int x, int y) : base(ImageType.Basic, layer, pos, filePath, x, y) { }
    }
}    
